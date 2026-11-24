/*
  Roboterarm AM-R1 – Automatischer Ablauf
  Board:   Freenove ESP32-S3 WROOM (Kit FNK0082)
           Arduino IDE: Board "ESP32S3 Dev Module", USB CDC On Boot: Enabled
  Bibliotheken (Bibliotheksverwalter):  ESP32Servo  (Kevin Harrington)
                                         AccelStepper (Mike McCauley)

  Achsen:
    Basis     28BYJ-48 + ULN2003, Zahnräder 15:75 (1:5), Bereich +-200°
    Schulter  MG996R   (Oberarm)
    Ellbogen  MG996R   (Kurbel -> Parallelogramm -> Unterarm)
    Greifer   SG90     (Fin-Ray-Finger aus TPU)

  Serieller Monitor (115200 Baud), Befehle:
    k = Kalibrierstellung (alle Servos 90°, Basis steht)  -> zum Montieren der Arme
    l = Lichtschranke anzeigen (Wert + blockiert ja/nein)
    h = Referenzfahrt Basis
    p = Pause / weiter        (auch über den Taster)
    s = Ablauf starten
    t = Testfahrt Basis +30° / -30° (Drehrichtung prüfen)
*/

#include <ESP32Servo.h>
#include <AccelStepper.h>

// ---------------- Pins (Freenove ESP32-S3) ----------------
const int PIN_SCHULTER = 4;
const int PIN_ELLBOGEN = 5;
const int PIN_GREIFER  = 6;
const int PIN_IN1 = 15, PIN_IN2 = 16, PIN_IN3 = 17, PIN_IN4 = 18;   // ULN2003
const int PIN_LSK_LED = 7;     // LED der Lichtschranke (über 220 Ohm)
const int PIN_LSK_LDR = 1;     // Fotowiderstand (Spannungsteiler mit 10 kOhm)
const int PIN_TASTER  = 14;    // Taster nach GND: Pause / weiter

// ---------------- Mechanik (wie in roboterarm.scad) ----------------
const float L1        = 120.0;   // Schulter -> Ellbogen
const float L2_GRIFF  = 159.0;   // Ellbogen -> Greifpunkt (Fingermitte), entlang Unterarm
const float GRIFF_V   = 12.0;    // Greifpunkt seitlich (senkrecht) zum Unterarm
const float SCHULTER_HOEHE = 155.0;  // Schulterachse über dem Tisch

// ---------------- Kalibrierung ----------------
// Montage: Befehl "k" -> Servos stehen auf 90°.
//   Oberarm senkrecht nach oben aufsetzen (alpha = 90°).
//   Kurbel so aufsetzen, dass der Unterarm 30° schräg nach unten-vorne zeigt (beta = -30°).
int   SCHULTER_RICHTUNG = 1;    // falls der Arm falsch herum fährt: -1
int   ELLBOGEN_RICHTUNG = 1;    // falls der Unterarm falsch herum fährt: -1
float SCHULTER_TRIMM = 0;       // Feinkorrektur in Grad
float ELLBOGEN_TRIMM = 0;

// Greifer: Servowinkel bei dem sich die Finger gerade berühren
int   GREIFER_BERUEHRT = 90;
int   GREIFER_OFFEN    = 130;   // ganz offen
int   GREIFER_DRUCK    = 8;     // so viele Grad weiter zu = fester Griff

// Basis
const float SCHRITTE_PRO_UMDREHUNG = 4096.0;   // 28BYJ-48 im Halbschritt (genau: 4076)
const float UEBERSETZUNG = 75.0 / 15.0;
const float SCHRITTE_PRO_GRAD = SCHRITTE_PRO_UMDREHUNG * UEBERSETZUNG / 360.0;
int   BASIS_RICHTUNG = 1;       // falls "t" falsch herum dreht: -1
const float BASIS_MIN = -200, BASIS_MAX = 200;
int   LSK_SCHWELLE = 1500;      // unter diesem Wert = Lichtstrahl blockiert (Befehl "l")

// Geschwindigkeiten
const float SERVO_GRAD_PRO_S = 60;     // ruhig und kräftig
const float BASIS_MAX_SPEED  = 800;    // Schritte/s
const float BASIS_BESCHL     = 500;

// ---------------- Ablauf ----------------
const int AUF = 100, ZU = 0;
struct Punkt { float basis, reichweite, hoehe; int greifer; int pauseMs; };
// basis in Grad (0 = vorne), reichweite = Abstand Greifpunkt zur Drehachse (mm),
// hoehe = Greifpunkt über dem Tisch (mm), greifer 0..100 % offen
Punkt ablauf[] = {
  {   0, 140, 140, AUF, 500 },   // Startstellung
  {  60, 150,  90, AUF,   0 },   // über Platz A
  {  60, 150,  40, AUF, 300 },   // runter
  {  60, 150,  40, ZU,  600 },   // greifen
  {  60, 150, 110, ZU,    0 },   // anheben
  {-120, 150, 110, ZU,    0 },   // halbe Drehung zu Platz B
  {-120, 150,  45, ZU,  300 },   // absetzen
  {-120, 150,  45, AUF, 500 },   // loslassen
  {-120, 150, 110, AUF,   0 },
  { 190, 120, 160, AUF, 800 },   // zeigen: fast ganz herum
  {-120, 150,  90, AUF,   0 },   // zurück, Teil wieder holen
  {-120, 150,  40, AUF, 300 },
  {-120, 150,  40, ZU,  600 },
  {-120, 150, 110, ZU,    0 },
  {  60, 150, 110, ZU,    0 },
  {  60, 150,  45, ZU,  300 },
  {  60, 150,  45, AUF, 500 },
  {  60, 150, 110, AUF,   0 },
  {   0, 140, 140, AUF, 1500 },  // Parken bei 0° (wichtig für die Referenz)
};
const int ANZAHL = sizeof(ablauf) / sizeof(ablauf[0]);

// ---------------- Objekte / Zustand ----------------
Servo sSchulter, sEllbogen, sGreifer;
AccelStepper basis(AccelStepper::HALF4WIRE, PIN_IN1, PIN_IN3, PIN_IN2, PIN_IN4);

float aktA = 90, aktB = 150, aktG = 100;   // alpha, Kurbelwinkel theta, Greifer %
bool  pausiert = false, laeuft = false, referenziert = false;
int   schritt = 0;

// ---------------- Hilfsfunktionen ----------------
float rad(float g) { return g * PI / 180.0; }
float grad(float r) { return r * 180.0 / PI; }

void schreibeServos(float alpha, float theta, float greifer) {
  float sA = 90 + SCHULTER_RICHTUNG * (alpha - 90) + SCHULTER_TRIMM;
  float sB = 90 + ELLBOGEN_RICHTUNG * (theta - 150) + ELLBOGEN_TRIMM;
  float sG = (GREIFER_BERUEHRT - GREIFER_DRUCK) +
             (GREIFER_OFFEN - (GREIFER_BERUEHRT - GREIFER_DRUCK)) * greifer / 100.0;
  sSchulter.write(constrain((int)lround(sA), 0, 180));
  sEllbogen.write(constrain((int)lround(sB), 0, 180));
  sGreifer.write(constrain((int)lround(sG), 0, 180));
}

// Rückwärtsrechnung: Reichweite/Höhe -> alpha (Oberarm), theta (Kurbel)
bool rechne(float r, float h, float &alpha, float &theta) {
  float dx = r, dz = h - SCHULTER_HOEHE;
  float L2 = sqrt(L2_GRIFF * L2_GRIFF + GRIFF_V * GRIFF_V);
  float delta = grad(atan2(GRIFF_V, L2_GRIFF));
  float d = sqrt(dx * dx + dz * dz);
  if (d > L1 + L2 - 1 || d < fabsf(L1 - L2) + 1) return false;
  float a = grad(atan2(dz, dx)) + grad(acos((L1 * L1 + d * d - L2 * L2) / (2 * L1 * d)));
  float ex = L1 * cos(rad(a)), ez = L1 * sin(rad(a));
  float b = grad(atan2(dz - ez, dx - ex)) - delta;     // Unterarmwinkel absolut
  float knick = a - b;
  if (a < 0 || a > 170) return false;
  if (knick < 25 || knick > 150) return false;          // Parallelogramm nicht strecken/falten
  alpha = a; theta = b + 180;
  float sB = 90 + (theta - 150);
  if (sB < 0 || sB > 180) return false;
  return true;
}

bool blockiert() {
  long s = 0;
  for (int i = 0; i < 4; i++) s += analogRead(PIN_LSK_LDR);
  return (s / 4) < LSK_SCHWELLE;
}

void pruefeEingaben();
float aktT() { return aktB; }

// Basis drehen bis die Lichtschranke den gewünschten Zustand hat
bool dreheBis(bool zielBlockiert, int richtung, float maxGrad, float speed) {
  long start = basis.currentPosition();
  long maxSchritte = maxGrad * SCHRITTE_PRO_GRAD;
  basis.setSpeed(richtung * BASIS_RICHTUNG * speed);
  while (labs(basis.currentPosition() - start) < maxSchritte) {
    if (basis.runSpeed() && blockiert() == zielBlockiert) return true;
  }
  return false;
}

void dreheUmGrad(float g) {
  basis.move(g * SCHRITTE_PRO_GRAD * BASIS_RICHTUNG);
  basis.runToPosition();
}

bool referenzfahrt() {
  Serial.println("Referenzfahrt Basis ...");
  basis.enableOutputs();
  if (!blockiert()) {
    // Basis steht zwischen 0° und +180° -> nach minus drehen bis Blende kommt
    if (!dreheBis(true, -1, 200, 500)) { Serial.println("FEHLER: Blende nicht gefunden"); return false; }
    dreheUmGrad(-5);
  }
  // langsam nach plus, bis der Strahl frei wird = genau 0°
  if (!dreheBis(false, +1, 200, 200)) { Serial.println("FEHLER: Kante nicht gefunden"); return false; }
  basis.setCurrentPosition(0);
  basis.disableOutputs();
  Serial.println("Referenz OK, Basis = 0°");
  return true;
}

// Alle Achsen gleichzeitig, weich (Servos interpoliert, Basis mit Rampe)
bool fahre(const Punkt &p) {
  float zA, zT;
  if (!rechne(p.reichweite, p.hoehe, zA, zT)) {
    Serial.printf("Punkt nicht erreichbar: r=%.0f h=%.0f\n", p.reichweite, p.hoehe);
    return false;
  }
  float b = constrain(p.basis, BASIS_MIN, BASIS_MAX);
  long ziel = lround(b * SCHRITTE_PRO_GRAD) * BASIS_RICHTUNG;

  float sA = aktA, sT = aktT(), sG = aktG;
  float maxD = fmaxf(fmaxf(fabsf(zA - sA), fabsf(zT - sT)), fabsf(p.greifer - sG) * 0.6f);
  unsigned long dauer = (unsigned long)(maxD / SERVO_GRAD_PRO_S * 1000);
  if (dauer < 400) dauer = 400;

  basis.enableOutputs();
  basis.moveTo(ziel);
  unsigned long t0 = millis(), tLetzt = 0;
  while (true) {
    basis.run();
    unsigned long t = millis() - t0;
    if (millis() - tLetzt >= 15) {
      tLetzt = millis();
      float k = fminf(1.0f, (float)t / dauer);
      k = k * k * (3 - 2 * k);                          // sanft anfahren und bremsen
      aktA = sA + (zA - sA) * k;
      aktB = sT + (zT - sT) * k;
      aktG = sG + (p.greifer - sG) * k;
      schreibeServos(aktA, aktB, aktG);
    }
    if (t >= dauer && basis.distanceToGo() == 0) break;
  }
  basis.disableOutputs();                               // Motor kühl halten
  return true;
}

void pruefeEingaben() {
  static bool tasterAlt = true;
  bool taster = digitalRead(PIN_TASTER);
  if (tasterAlt && !taster) { pausiert = !pausiert; Serial.println(pausiert ? "Pause" : "Weiter"); delay(30); }
  tasterAlt = taster;

  if (!Serial.available()) return;
  char c = Serial.read();
  switch (c) {
    case 'k':
      laeuft = false;
      sSchulter.write(90); sEllbogen.write(90); sGreifer.write(GREIFER_BERUEHRT);
      aktA = 90; aktB = 150;
      Serial.println("Kalibrierstellung: Servos 90°. Oberarm senkrecht, Unterarm 30° nach unten montieren.");
      break;
    case 'l':
      Serial.printf("Lichtschranke: %d  -> %s\n", analogRead(PIN_LSK_LDR), blockiert() ? "blockiert" : "frei");
      break;
    case 'h': referenziert = referenzfahrt(); break;
    case 'p': pausiert = !pausiert; Serial.println(pausiert ? "Pause" : "Weiter"); break;
    case 's':
      if (!referenziert) referenziert = referenzfahrt();
      laeuft = referenziert; schritt = 0; pausiert = false;
      break;
    case 't':
      basis.enableOutputs();
      Serial.println("Basis +30° (Arm sollte sich von oben gesehen GEGEN den Uhrzeigersinn drehen)");
      dreheUmGrad(30); delay(500); dreheUmGrad(-30); basis.disableOutputs();
      break;
  }
}

void setup() {
  Serial.begin(115200);
  delay(500);
  pinMode(PIN_TASTER, INPUT_PULLUP);
  pinMode(PIN_LSK_LED, OUTPUT);
  digitalWrite(PIN_LSK_LED, HIGH);
  analogReadResolution(12);

  ESP32PWM::allocateTimer(0);
  ESP32PWM::allocateTimer(1);
  sSchulter.setPeriodHertz(50); sEllbogen.setPeriodHertz(50); sGreifer.setPeriodHertz(50);
  sSchulter.attach(PIN_SCHULTER, 500, 2500);
  sEllbogen.attach(PIN_ELLBOGEN, 500, 2500);
  sGreifer.attach(PIN_GREIFER, 500, 2400);

  basis.setMaxSpeed(BASIS_MAX_SPEED);
  basis.setAcceleration(BASIS_BESCHL);

  // sichere Startstellung: Arm hoch, Greifer offen
  float a, t;
  if (rechne(ablauf[0].reichweite, ablauf[0].hoehe, a, t)) { aktA = a; aktB = t; }
  aktG = AUF;
  schreibeServos(aktA, aktB, aktG);
  delay(1000);

  Serial.println("\nRoboterarm AM-R1 bereit. Befehle: k l h p s t");
  referenziert = referenzfahrt();
  laeuft = referenziert;          // startet automatisch
}

void loop() {
  pruefeEingaben();
  if (!laeuft || pausiert) { delay(10); return; }
  Serial.printf("Schritt %d/%d\n", schritt + 1, ANZAHL);
  fahre(ablauf[schritt]);
  unsigned long t0 = millis();
  while (millis() - t0 < (unsigned long)ablauf[schritt].pauseMs) { pruefeEingaben(); delay(5); }
  schritt = (schritt + 1) % ANZAHL;
}
