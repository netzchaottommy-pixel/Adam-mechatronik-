/*
  Roboterarm AM-R1  Version 2 (geschlossen, 500 g) – Automatischer Ablauf
  Board:   Freenove ESP32-S3 WROOM (Kit FNK0082)
           Arduino IDE: Board "ESP32S3 Dev Module", USB CDC On Boot: Enabled
  Bibliotheken (Bibliotheksverwalter):  ESP32Servo  (Kevin Harrington)
                                         AccelStepper (Mike McCauley)

  Achsen:
    Basis     28BYJ-48 + ULN2003, Zahnräder 15:75 (1:5), Bereich +-200°
    Schulter  DS3218 270°, Zahnräder 22:44 (1:2)   -> Oberarm
    Ellbogen  DS3218 270°, Zahnräder 22:44 (1:2)   -> Kurbel -> Koppelstange -> Unterarm
    Greifer   MG996R 180°  (Fin-Ray-Finger aus TPU)

  Serieller Monitor (115200 Baud), Befehle:
    k = Kalibrierstellung (Schulter/Ellbogen-Servo Mitte 135°, Greifer zu) -> zum Montieren
    l = Lichtschranke anzeigen
    h = Referenzfahrt Basis
    p = Pause / weiter        (auch über den Taster)
    s = Ablauf starten
    t = Testfahrt Basis +30° / -30°
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
const float GRIFF_V   = 37.0;    // Greifpunkt senkrecht zum Unterarm (Finger sitzen oben)
const float SCHULTER_HOEHE = 160.0;  // Schulterachse über dem Tisch
const float MAX_REICHWEITE_500G = 220;  // bis hier volle 500 g

// Grenzen (Gehäuse / Parallelogramm)
const float ALPHA_MIN = -5, ALPHA_MAX = 100;   // Oberarm
const float KNICK_MIN = 30, KNICK_MAX = 120;   // Winkel zwischen Ober- und Unterarm

// ---------------- Kalibrierung ----------------
// Montage: Befehl "k" -> beide DS3218 stehen in der Mitte (135°).
//   Oberarm auf 50° über der Waagerechten stellen, dann Zahnrad einrasten.
//   Unterarm 60° schräg nach unten (absolut, beta = -60°), dann Zahnrad einrasten.
const float A_MITTE = 50;       // Oberarmwinkel bei Servo-Mitte
const float THETA_MITTE = 120;  // Kurbelwinkel (= beta + 180) bei Servo-Mitte
const float UEBERSETZUNG_GELENK = 2.0;
int   SCHULTER_RICHTUNG = 1;    // Zahnräder drehen die Richtung um -> ggf. -1
int   ELLBOGEN_RICHTUNG = 1;
float SCHULTER_TRIMM = 0;       // Feinkorrektur in Servo-Grad
float ELLBOGEN_TRIMM = 0;
const int SERVO270_MIN_US = 500, SERVO270_MAX_US = 2500;

int   GREIFER_BERUEHRT = 90;    // Servowinkel: Finger berühren sich
int   GREIFER_OFFEN    = 125;
int   GREIFER_DRUCK    = 10;    // weiter zu = fester Griff (500 g brauchen Druck)

// Basis
const float SCHRITTE_PRO_UMDREHUNG = 4096.0;
const float SCHRITTE_PRO_GRAD = SCHRITTE_PRO_UMDREHUNG * (75.0 / 15.0) / 360.0;
int   BASIS_RICHTUNG = 1;
const float BASIS_MIN = -200, BASIS_MAX = 200;
int   LSK_SCHWELLE = 1500;

// Geschwindigkeiten (mit Last lieber ruhig)
const float GELENK_GRAD_PRO_S = 35;
const float BASIS_MAX_SPEED  = 600;
const float BASIS_BESCHL     = 300;

// ---------------- Ablauf ----------------
const int AUF = 100, ZU = 0;
struct Punkt { float basis, reichweite, hoehe; int greifer; int pauseMs; };
Punkt ablauf[] = {
  {   0, 180, 140, AUF, 500 },   // Startstellung
  {  60, 170, 110, AUF,   0 },   // über Platz A
  {  60, 170,  45, AUF, 300 },   // runter
  {  60, 170,  45, ZU,  800 },   // greifen (Zeit zum Zudrücken)
  {  60, 170, 130, ZU,    0 },   // anheben
  {-120, 170, 130, ZU,    0 },   // halbe Drehung zu Platz B
  {-120, 170,  50, ZU,  300 },   // absetzen
  {-120, 170,  50, AUF, 600 },
  {-120, 170, 130, AUF,   0 },
  { 190, 190, 150, AUF, 800 },   // fast ganz herum
  {-120, 170, 110, AUF,   0 },
  {-120, 170,  45, AUF, 300 },
  {-120, 170,  45, ZU,  800 },
  {-120, 170, 130, ZU,    0 },
  {  60, 170, 130, ZU,    0 },
  {  60, 170,  50, ZU,  300 },
  {  60, 170,  50, AUF, 600 },
  {  60, 170, 130, AUF,   0 },
  {   0, 180, 140, AUF, 1500 },  // Parken bei 0° (wichtig für die Referenz)
};
const int ANZAHL = sizeof(ablauf) / sizeof(ablauf[0]);

// ---------------- Zustand ----------------
Servo sSchulter, sEllbogen, sGreifer;
AccelStepper basis(AccelStepper::HALF4WIRE, PIN_IN1, PIN_IN3, PIN_IN2, PIN_IN4);

float aktA = A_MITTE, aktT = THETA_MITTE, aktG = 100;
bool  pausiert = false, laeuft = false, referenziert = false;
int   schritt = 0;

float rad(float g) { return g * PI / 180.0; }
float grad(float r) { return r * 180.0 / PI; }

void schreibe270(Servo &s, float servoGrad) {
  servoGrad = constrain(servoGrad, 0, 270);
  s.writeMicroseconds(SERVO270_MIN_US + (int)(servoGrad / 270.0 * (SERVO270_MAX_US - SERVO270_MIN_US)));
}
float servoA(float alpha) { return 135 + SCHULTER_RICHTUNG * UEBERSETZUNG_GELENK * (alpha - A_MITTE) + SCHULTER_TRIMM; }
float servoB(float theta) { return 135 + ELLBOGEN_RICHTUNG * UEBERSETZUNG_GELENK * (theta - THETA_MITTE) + ELLBOGEN_TRIMM; }

void schreibeServos(float alpha, float theta, float greifer) {
  schreibe270(sSchulter, servoA(alpha));
  schreibe270(sEllbogen, servoB(theta));
  float zu = GREIFER_BERUEHRT - GREIFER_DRUCK;
  sGreifer.write(constrain((int)lround(zu + (GREIFER_OFFEN - zu) * greifer / 100.0), 0, 180));
}

// Rückwärtsrechnung: Reichweite/Höhe -> alpha (Oberarm), theta (Kurbel)
bool rechne(float r, float h, float &alpha, float &theta) {
  float dx = r, dz = h - SCHULTER_HOEHE;
  float L2 = sqrtf(L2_GRIFF * L2_GRIFF + GRIFF_V * GRIFF_V);
  float delta = grad(atan2f(GRIFF_V, L2_GRIFF));
  float d = sqrtf(dx * dx + dz * dz);
  if (d > L1 + L2 - 1 || d < fabsf(L1 - L2) + 1) return false;
  float a = grad(atan2f(dz, dx)) + grad(acosf((L1 * L1 + d * d - L2 * L2) / (2 * L1 * d)));
  float ex = L1 * cosf(rad(a)), ez = L1 * sinf(rad(a));
  float b = grad(atan2f(dz - ez, dx - ex)) - delta;
  float knick = a - b;
  if (a < ALPHA_MIN || a > ALPHA_MAX) return false;
  if (knick < KNICK_MIN || knick > KNICK_MAX) return false;
  float t = b + 180;
  if (servoA(a) < 0 || servoA(a) > 270 || servoB(t) < 0 || servoB(t) > 270) return false;
  alpha = a; theta = t;
  return true;
}

bool blockiert() {
  long s = 0;
  for (int i = 0; i < 4; i++) s += analogRead(PIN_LSK_LDR);
  return (s / 4) < LSK_SCHWELLE;
}

bool dreheBis(bool zielBlockiert, int richtung, float maxGrad, float speed) {
  long start = basis.currentPosition();
  long maxSchritte = maxGrad * SCHRITTE_PRO_GRAD;
  basis.setSpeed(richtung * BASIS_RICHTUNG * speed);
  while (labs(basis.currentPosition() - start) < maxSchritte) {
    if (basis.runSpeed() && blockiert() == zielBlockiert) return true;
  }
  return false;
}
void dreheUmGrad(float g) { basis.move(g * SCHRITTE_PRO_GRAD * BASIS_RICHTUNG); basis.runToPosition(); }

bool referenzfahrt() {
  Serial.println("Referenzfahrt Basis ...");
  basis.enableOutputs();
  if (!blockiert()) {
    if (!dreheBis(true, -1, 200, 400)) { Serial.println("FEHLER: Blende nicht gefunden"); return false; }
    dreheUmGrad(-5);
  }
  if (!dreheBis(false, +1, 200, 150)) { Serial.println("FEHLER: Kante nicht gefunden"); return false; }
  basis.setCurrentPosition(0);
  basis.disableOutputs();
  Serial.println("Referenz OK, Basis = 0°");
  return true;
}

bool fahre(const Punkt &p) {
  float zA, zT;
  if (!rechne(p.reichweite, p.hoehe, zA, zT)) {
    Serial.printf("Punkt nicht erreichbar: r=%.0f h=%.0f\n", p.reichweite, p.hoehe);
    return false;
  }
  if (p.reichweite > MAX_REICHWEITE_500G && p.greifer < 50)
    Serial.println("Hinweis: mit Last weiter als 22 cm -> weniger als 500 g");
  float b = constrain(p.basis, BASIS_MIN, BASIS_MAX);
  long ziel = lround(b * SCHRITTE_PRO_GRAD) * BASIS_RICHTUNG;

  float sA = aktA, sT = aktT, sG = aktG;
  float maxD = fmaxf(fmaxf(fabsf(zA - sA), fabsf(zT - sT)), fabsf(p.greifer - sG) * 0.5f);
  unsigned long dauer = (unsigned long)(maxD / GELENK_GRAD_PRO_S * 1000);
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
      k = k * k * (3 - 2 * k);
      aktA = sA + (zA - sA) * k;
      aktT = sT + (zT - sT) * k;
      aktG = sG + (p.greifer - sG) * k;
      schreibeServos(aktA, aktT, aktG);
    }
    if (t >= dauer && basis.distanceToGo() == 0) break;
  }
  basis.disableOutputs();
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
      schreibe270(sSchulter, 135); schreibe270(sEllbogen, 135); sGreifer.write(GREIFER_BERUEHRT);
      aktA = A_MITTE; aktT = THETA_MITTE; aktG = 0;
      Serial.println("Kalibrierstellung: Servos Mitte. Oberarm 50° hoch, Unterarm 60° nach unten, Finger berühren sich.");
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
      Serial.println("Basis +30° (von oben gesehen GEGEN den Uhrzeigersinn)");
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
  sSchulter.attach(PIN_SCHULTER, SERVO270_MIN_US, SERVO270_MAX_US);
  sEllbogen.attach(PIN_ELLBOGEN, SERVO270_MIN_US, SERVO270_MAX_US);
  sGreifer.attach(PIN_GREIFER, 500, 2500);

  basis.setMaxSpeed(BASIS_MAX_SPEED);
  basis.setAcceleration(BASIS_BESCHL);

  float a, t;
  if (rechne(ablauf[0].reichweite, ablauf[0].hoehe, a, t)) { aktA = a; aktT = t; }
  aktG = AUF;
  schreibeServos(aktA, aktT, aktG);
  delay(1500);

  Serial.println("\nRoboterarm AM-R1 v2 bereit. Befehle: k l h p s t");
  referenziert = referenzfahrt();
  laeuft = referenziert;
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
