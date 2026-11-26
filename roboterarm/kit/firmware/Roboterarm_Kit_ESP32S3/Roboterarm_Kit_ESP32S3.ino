// =====================================================================
//  Roboterarm AM-R1 Kit-Version – Freenove ESP32-S3 (FNK0082)
//
//  Basis     28BYJ-48 + ULN2003, Zahnkranz 1:5, Referenz über Lichtschranke
//  Schulter  Gleichstrommotor über L293D, Getriebe 1:400 (selbsthemmend),
//            Winkel kommt vom Potentiometer auf der Schulterachse
//  Greifer   SG90 mit Fin-Ray-Fingern
//
//  Bibliotheken (Arduino-Bibliotheksverwalter):  ESP32Servo, AccelStepper
//  Board: "ESP32S3 Dev Module", Arduino-ESP32 Core 3.x
//
//  Serieller Monitor 115200 Baud, Befehle mit "h" anzeigen.
// =====================================================================
#include <ESP32Servo.h>
#include <AccelStepper.h>
#include <Preferences.h>

// ---------- Pins ----------
// Freenove ESP32-S3 WROOM: Kamera (GPIO 4-13, 15-18), SD-Karte (38-40) und PSRAM (35-37)
// bleiben frei. Benutzt werden nur Pins ohne Doppelbelegung.
const int PIN_IN1  = 47;   // L293D Pin 2  (PWM)   – L293D Pin 1 (Enable) fest an 5V
const int PIN_IN2  = 42;   // L293D Pin 7  (PWM)
const int PIN_POTI = 1;    // Schleifer Potentiometer (ADC1_CH0)
const int PIN_SG90 = 41;
const int PIN_ST1 = 2, PIN_ST2 = 3, PIN_ST3 = 45, PIN_ST4 = 46;   // ULN2003 IN1..IN4 (alle beim Start LOW = Motor aus)
const int PIN_LDR  = 14;   // Fotowiderstand (Spannungsteiler mit 10 kOhm), LED der Lichtschranke leuchtet dauernd
const int PIN_TASTER = 21; // Taster gegen GND

// ---------- Geometrie (wie kit.scad) ----------
const float L_LENKER   = 120.0;  // Lenkerlänge
const float H_SCHULTER = 160.0;  // Schulterachse über Tisch
const float GREIF_TIEF = 80.0;   // Greifmitte unter dem Handgelenk
const float GREIF_VOR  = 100.0;  // Greifmitte vor dem Handgelenk
const float PHI_MIN = -33.0, PHI_MAX = 45.0;

// ---------- Basis ----------
const float SCHRITTE_PRO_GRAD = 4096.0 * 75.0 / 15.0 / 360.0;   // Halbschritt, Zahnkranz 1:5
const float BASIS_MIN = -200.0, BASIS_MAX = 200.0;
int LDR_SCHWELLE = 1500;           // MESSEN: Befehl "l" zeigt die Werte. Fotowiderstand an 3,3 V, 10 kOhm nach GND

// ---------- Schulter (Werte mit Befehl "k" kalibrieren) ----------
int   POTI_NULL = 2048;            // ADC-Wert bei phi = 0 (Arm waagerecht)
float POTI_PRO_GRAD = 13.65;       // ADC pro Grad (300°-Poti: 4095/300). Vorzeichen = Drehsinn
int   MOTOR_RICHTUNG = 1;          // -1 wenn der Arm in die falsche Richtung fährt
const int   PWM_MIN = 120, PWM_MAX = 255;
const float TOTZONE = 0.6;         // Grad
const float KP = 25.0;             // PWM pro Grad Abweichung

// ---------- Greifer ----------
int GREIFER_AUF = 60, GREIFER_ZU = 105;   // Grad, mit Befehl "s" testen

Servo greifer;
AccelStepper basis(AccelStepper::HALF4WIRE, PIN_ST1, PIN_ST3, PIN_ST2, PIN_ST4);
Preferences speicher;

bool laeuft = false;
float phiZiel = 0;

// =====================================================================
//  Schulter
// =====================================================================
int potiRoh() {
  long s = 0;
  for (int i = 0; i < 16; i++) s += analogRead(PIN_POTI);
  return s / 16;
}
float phiIst() { return (potiRoh() - POTI_NULL) / POTI_PRO_GRAD; }

void motor(int pwm) {             // pwm -255..255, positiv = Arm hebt
  pwm *= MOTOR_RICHTUNG;
  // Vorwärts: IN1 = PWM, IN2 = LOW.  Rückwärts: IN1 = LOW, IN2 = PWM.  0 = beide LOW (Motor frei)
  ledcWrite(PIN_IN1, pwm > 0 ? pwm : 0);
  ledcWrite(PIN_IN2, pwm < 0 ? -pwm : 0);
}

// Regelt die Schulter einen Schritt; true = Ziel erreicht
bool schulterRegeln() {
  float e = phiZiel - phiIst();
  if (fabsf(e) < TOTZONE) { motor(0); return true; }
  int pwm = (int)(KP * fabsf(e));
  pwm = constrain(pwm, PWM_MIN, PWM_MAX);
  motor(e > 0 ? pwm : -pwm);
  return false;
}

// =====================================================================
//  Basis + Lichtschranke
// =====================================================================
bool dunkel() {                   // Rock unterbricht die Lichtschranke
  return analogRead(PIN_LDR) < LDR_SCHWELLE;   // wenig Licht am Fotowiderstand = Rock dazwischen
}
float basisWinkel() { return basis.currentPosition() / SCHRITTE_PRO_GRAD; }

void basisSpeichern() { speicher.putFloat("basis", basisWinkel()); }

// Referenzfahrt: Rock deckt -180°..0° ab, die Kante bei 0° ist der Nullpunkt
bool basisReferenz() {
  float gemerkt = speicher.getFloat("basis", 0);
  bool d = dunkel();
  // Vorsicht bei aufgewickeltem Kabel: gemerkter Winkel jenseits von ±180° -> erst zurückdrehen
  if (gemerkt > 180 || gemerkt < -180) {
    Serial.printf("Basis stand bei %.0f°, drehe erst zurück\n", gemerkt);
    basis.setCurrentPosition(gemerkt * SCHRITTE_PRO_GRAD);
    basis.runToNewPosition((gemerkt > 0 ? 150 : -150) * SCHRITTE_PRO_GRAD);
    d = dunkel();
  }
  int richtung = d ? 1 : -1;
  basis.setMaxSpeed(500);
  long start = basis.currentPosition();
  long grenze = 200 * SCHRITTE_PRO_GRAD;
  basis.setSpeed(richtung * 400);
  while (dunkel() == d) {
    basis.runSpeed();
    if (labs(basis.currentPosition() - start) > grenze) { Serial.println("FEHLER: Lichtschranke nicht gefunden"); return false; }
  }
  // Kante langsam von der hellen Seite anfahren
  basis.setSpeed(-richtung * 400);  long t = basis.currentPosition();
  while (labs(basis.currentPosition() - t) < 3 * SCHRITTE_PRO_GRAD) basis.runSpeed();
  basis.setSpeed(richtung * 80);
  bool hell0 = dunkel();
  while (dunkel() == hell0) basis.runSpeed();
  basis.setCurrentPosition(0);
  basis.setMaxSpeed(900); basis.setAcceleration(600);
  basisSpeichern();
  Serial.println("Basis referenziert");
  return true;
}

// =====================================================================
//  Bewegung: Greifpunkt (Radius r, Höhe h über Tisch, Basiswinkel w)
// =====================================================================
bool punktErreichbar(float r, float h, float &phi) {
  float s = (h + GREIF_TIEF - H_SCHULTER) / L_LENKER;
  if (s < -1 || s > 1) return false;
  phi = asinf(s) * 180.0 / PI;
  if (phi < PHI_MIN || phi > PHI_MAX) return false;
  float rErr = L_LENKER * cosf(phi * PI / 180.0) + GREIF_VOR;
  if (fabsf(rErr - r) > 3) Serial.printf("Hinweis: Radius bei h=%.0f ist %.0f mm (nicht %.0f)\n", h, rErr, r);
  return true;
}

void warten(unsigned long ms) {   // wartet, regelt die Schulter weiter
  unsigned long t = millis();
  while (millis() - t < ms) { schulterRegeln(); delay(5); }
}

// Fährt Schulter und Basis gleichzeitig
bool fahre(float phi, float w) {
  phi = constrain(phi, PHI_MIN, PHI_MAX);
  w = constrain(w, BASIS_MIN, BASIS_MAX);
  phiZiel = phi;
  basis.moveTo(lroundf(w * SCHRITTE_PRO_GRAD));
  unsigned long t0 = millis(), tBew = millis();
  float phiAlt = phiIst();
  bool schulterFertig = false;
  while (!schulterFertig || basis.distanceToGo() != 0) {
    basis.run();
    static unsigned long tr = 0;
    if (millis() - tr >= 5) {
      tr = millis();
      schulterFertig = schulterRegeln();
      float p = phiIst();
      if (fabsf(p - phiAlt) > 1.0) { phiAlt = p; tBew = millis(); }
      if (!schulterFertig && millis() - tBew > 2000) {      // Motor dreht, Winkel ändert sich nicht
        motor(0); Serial.println("FEHLER: Schulter blockiert oder Poti lose"); return false;
      }
    }
    if (laeuft && digitalRead(PIN_TASTER) == LOW) {          // Taster = Stopp
      motor(0); basis.stop(); laeuft = false; Serial.println("Stopp per Taster");
      while (digitalRead(PIN_TASTER) == LOW) delay(10);
      return false;
    }
    if (millis() - t0 > 30000) { motor(0); Serial.println("FEHLER: Zeitüberschreitung"); return false; }
  }
  basis.disableOutputs();         // Spulen stromlos, Getriebe hält
  basisSpeichern();
  return true;
}

bool fahreZu(float r, float h, float w) {
  float phi;
  if (!punktErreichbar(r, h, phi)) { Serial.printf("Punkt h=%.0f nicht erreichbar\n", h); return false; }
  return fahre(phi, w);
}

void greifen(bool zu) {
  greifer.write(zu ? GREIFER_ZU : GREIFER_AUF);
  warten(500);
}

// =====================================================================
//  Ablauf: Teil bei A aufnehmen, bei B ablegen, wieder zurück
// =====================================================================
struct Punkt { float h, w; };
// h = Greifmitte über dem Tisch (mm), w = Basiswinkel (Grad)
Punkt A_oben = {70, -90}, A_unten = {12, -90};
Punkt B_oben = {70,  90}, B_unten = {14,  90};
const float H_PARK = 95;

bool ablauf() {
  greifen(false);
  if (!fahreZu(0, A_oben.h, A_oben.w))  return false;
  if (!fahreZu(0, A_unten.h, A_unten.w)) return false;
  greifen(true);
  if (!fahreZu(0, A_oben.h, A_oben.w))  return false;
  if (!fahreZu(0, B_oben.h, B_oben.w))  return false;
  if (!fahreZu(0, B_unten.h, B_unten.w)) return false;
  greifen(false);
  if (!fahreZu(0, B_oben.h, B_oben.w))  return false;
  // und wieder zurück
  if (!fahreZu(0, B_unten.h, B_unten.w)) return false;
  greifen(true);
  if (!fahreZu(0, B_oben.h, B_oben.w))  return false;
  if (!fahreZu(0, A_oben.h, A_oben.w))  return false;
  if (!fahreZu(0, A_unten.h, A_unten.w)) return false;
  greifen(false);
  if (!fahreZu(0, A_oben.h, A_oben.w))  return false;
  return true;
}

// =====================================================================
//  Serielle Befehle
// =====================================================================
void hilfe() {
  Serial.println(F(
    "\nBefehle:\n"
    "  h            Hilfe\n"
    "  i            Ist-Werte (Poti, phi, Basis, Lichtschranke)\n"
    "  k            Schulter kalibrieren (Arm von Hand waagerecht, dann 30° hoch)\n"
    "  m <pwm>      Motor direkt (-255..255) für 0,3 s – Drehsinn prüfen\n"
    "  p <phi>      Schulter auf Winkel fahren\n"
    "  b <w>        Basis auf Winkel fahren\n"
    "  g <h> <w>    Greifmitte auf Höhe h (mm) und Basiswinkel w\n"
    "  s <grad>     Greifer-Servo stellen\n"
    "  l            Lichtschranke 5 s anzeigen\n"
    "  L <wert>     Schwelle der Lichtschranke speichern\n"
    "  r            Referenzfahrt Basis\n"
    "  a / x        Ablauf starten / stoppen (oder Taster)"));
}

void kalibrieren() {
  motor(0);
  Serial.println("Arm von Hand genau waagerecht stellen (Wasserwaage), dann Enter.");
  Serial.println("(Getriebe ist selbsthemmend: Motorritzel abziehen oder mit 'm' fahren)");
  while (!Serial.available()) delay(10);
  while (Serial.available()) Serial.read();
  int p0 = potiRoh();
  Serial.println("Arm 30° anheben (Geodreieck), dann Enter.");
  while (!Serial.available()) delay(10);
  while (Serial.available()) Serial.read();
  int p30 = potiRoh();
  POTI_NULL = p0;
  POTI_PRO_GRAD = (p30 - p0) / 30.0;
  speicher.putInt("pnull", POTI_NULL);
  speicher.putFloat("pgrad", POTI_PRO_GRAD);
  Serial.printf("Gespeichert: POTI_NULL=%d  POTI_PRO_GRAD=%.2f\n", POTI_NULL, POTI_PRO_GRAD);
  if (fabsf(POTI_PRO_GRAD) < 5) Serial.println("WARNUNG: kaum Änderung – sitzt das Poti fest in der Achse?");
}

void befehl() {
  String z = Serial.readStringUntil('\n'); z.trim();
  if (z.length() == 0) return;
  char c = z[0];
  float a = 0, b = 0;
  sscanf(z.c_str() + 1, "%f %f", &a, &b);
  switch (c) {
    case 'h': hilfe(); break;
    case 'i': Serial.printf("Poti %d  phi %.1f°  Basis %.1f°  Lichtschranke %s\n", potiRoh(), phiIst(), basisWinkel(), dunkel() ? "dunkel" : "hell"); break;
    case 'k': kalibrieren(); break;
    case 'm': motor((int)a); delay(300); motor(0); Serial.printf("phi jetzt %.1f°\n", phiIst()); break;
    case 'p': fahre(a, basisWinkel()); Serial.printf("phi %.1f°\n", phiIst()); break;
    case 'b': fahre(phiIst(), a); break;
    case 'g': fahreZu(0, a, b); break;
    case 's': greifer.write((int)a); break;
    case 'l': for (int i = 0; i < 25; i++) { Serial.printf("Fotowiderstand %4d  (Schwelle %d) -> %s\n", analogRead(PIN_LDR), LDR_SCHWELLE, dunkel() ? "dunkel" : "hell"); delay(200); } break;
    case 'L': LDR_SCHWELLE = (int)a; speicher.putInt("ldr", LDR_SCHWELLE); Serial.printf("Schwelle %d gespeichert\n", LDR_SCHWELLE); break;
    case 'r': basisReferenz(); break;
    case 'a': laeuft = true; break;
    case 'x': laeuft = false; motor(0); break;
  }
}

// =====================================================================
void setup() {
  Serial.begin(115200);
  ledcAttach(PIN_IN1, 20000, 8);
  ledcAttach(PIN_IN2, 20000, 8);
  motor(0);
  pinMode(PIN_TASTER, INPUT_PULLUP);
  analogReadResolution(12);
  analogSetPinAttenuation(PIN_POTI, ADC_11db);
  analogSetPinAttenuation(PIN_LDR, ADC_11db);

  speicher.begin("roboarm", false);
  POTI_NULL = speicher.getInt("pnull", POTI_NULL);
  POTI_PRO_GRAD = speicher.getFloat("pgrad", POTI_PRO_GRAD);
  LDR_SCHWELLE = speicher.getInt("ldr", LDR_SCHWELLE);

  greifer.setPeriodHertz(50);
  greifer.attach(PIN_SG90, 500, 2400);
  greifer.write(GREIFER_AUF);

  basis.setMaxSpeed(900);
  basis.setAcceleration(600);

  delay(800);
  hilfe();
  Serial.printf("phi %.1f°  (Poti %d)\n", phiIst(), potiRoh());

  // Schulter zuerst in sichere Höhe, dann Basis referenzieren
  phiZiel = 20;
  unsigned long t = millis();
  while (!schulterRegeln() && millis() - t < 8000) delay(5);
  motor(0);
  basisReferenz();
  fahreZu(0, H_PARK, 0);
  Serial.println("Bereit. Taster oder 'a' startet den Ablauf.");
}

void loop() {
  static bool tasterAlt = HIGH;
  bool taster = digitalRead(PIN_TASTER);
  if (tasterAlt == HIGH && taster == LOW) { laeuft = !laeuft; delay(30); }
  tasterAlt = taster;

  if (Serial.available()) befehl();

  if (laeuft) {
    if (!ablauf()) { laeuft = false; motor(0); Serial.println("Ablauf angehalten."); fahreZu(0, H_PARK, basisWinkel()); }
  } else {
    schulterRegeln();             // hält die Position, falls etwas verstellt wurde
    delay(5);
  }
}
