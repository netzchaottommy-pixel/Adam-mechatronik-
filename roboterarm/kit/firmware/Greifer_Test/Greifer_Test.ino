// =====================================================================
//  Greifer-Test – Fin-Ray-Greifer mit SG90 allein ausprobieren
//  Freenove ESP32-S3, Bibliothek: ESP32Servo
//
//  Verdrahtung (gleiche Pins wie die Arm-Firmware):
//    SG90 orange -> GPIO 41, rot -> 5V, braun -> GND
//    Potentiometer: außen 3V3 und GND, Mitte -> GPIO 1
//    Taster: GPIO 21 und GND
//
//  Bedienung
//    Poti drehen      Greifer folgt direkt (Hand-Modus)
//    Taster drücken   schaltet zwischen Hand-Modus und Auf/Zu-Modus
//    Seriell 115200:  h = Hilfe
// =====================================================================
#include <ESP32Servo.h>

const int PIN_SG90 = 41;
const int PIN_POTI = 1;
const int PIN_TASTER = 21;

int AUF = 60;          // Grad: Greifer ganz offen       (mit "a <grad>" ändern)
int ZU = 105;          // Grad: Finger berühren sich     (mit "z <grad>" ändern)

Servo greifer;
bool handModus = true;
bool istZu = false;
int stellung = 60;

void fahre(int ziel, int msProGrad) {       // langsam fahren, damit man zusehen kann
  ziel = constrain(ziel, 0, 180);
  while (stellung != ziel) {
    stellung += (ziel > stellung) ? 1 : -1;
    greifer.write(stellung);
    delay(msProGrad);
  }
}

void hilfe() {
  Serial.println(F(
    "\nGreifer-Test\n"
    "  h          Hilfe\n"
    "  o          öffnen\n"
    "  c          schließen (schnell)\n"
    "  s          sanft schließen (langsam, gut für Eier und Obst)\n"
    "  g <grad>   auf Winkel fahren\n"
    "  a <grad>   Winkel für 'offen' festlegen\n"
    "  z <grad>   Winkel für 'zu' festlegen\n"
    "  d          Dauertest: 20 x auf und zu\n"
    "  p          Hand-Modus mit Poti an/aus\n"
    "  i          aktuelle Werte"));
}

void setup() {
  Serial.begin(115200);
  pinMode(PIN_TASTER, INPUT_PULLUP);
  analogReadResolution(12);
  greifer.setPeriodHertz(50);
  greifer.attach(PIN_SG90, 500, 2400);
  greifer.write(stellung);
  delay(500);
  hilfe();
}

void loop() {
  // Taster: Modus wechseln
  static bool alt = HIGH;
  bool t = digitalRead(PIN_TASTER);
  if (alt == HIGH && t == LOW) {
    delay(30);
    if (handModus) { handModus = false; Serial.println("Auf/Zu-Modus: Taster öffnet und schließt"); }
    else { istZu = !istZu; fahre(istZu ? ZU : AUF, 4); Serial.println(istZu ? "zu" : "auf"); }
  }
  // langer Druck (1 s) zurück in den Hand-Modus
  static unsigned long tDruck = 0;
  if (t == LOW) { if (tDruck == 0) tDruck = millis(); if (millis() - tDruck > 1000 && !handModus) { handModus = true; Serial.println("Hand-Modus: Poti steuert den Greifer"); } }
  else tDruck = 0;
  alt = t;

  // Hand-Modus: Poti -> Winkel zwischen AUF und ZU
  if (handModus) {
    long s = 0;
    for (int i = 0; i < 8; i++) s += analogRead(PIN_POTI);
    int ziel = map(s / 8, 0, 4095, AUF, ZU);
    if (abs(ziel - stellung) >= 1) { stellung = ziel; greifer.write(stellung); }
  }

  if (Serial.available()) {
    String z = Serial.readStringUntil('\n'); z.trim();
    if (z.length() == 0) return;
    int w = z.length() > 1 ? z.substring(1).toInt() : 0;
    if (strchr("ocsgd", z[0])) handModus = false;     // Fahrbefehle beenden den Hand-Modus
    switch (z[0]) {
      case 'h': hilfe(); break;
      case 'o': fahre(AUF, 2); istZu = false; break;
      case 'c': fahre(ZU, 1); istZu = true; break;
      case 's': fahre(ZU, 25); istZu = true; break;
      case 'g': fahre(w, 3); break;
      case 'a': AUF = w; Serial.printf("offen = %d°\n", AUF); break;
      case 'z': ZU = w; Serial.printf("zu = %d°\n", ZU); break;
      case 'd': for (int i = 0; i < 20; i++) { fahre(ZU, 1); delay(300); fahre(AUF, 1); delay(300); } break;
      case 'p': handModus = true; Serial.println("Hand-Modus: Poti steuert den Greifer"); break;
      case 'i': Serial.printf("Stellung %d°, offen %d°, zu %d°, Poti %d\n", stellung, AUF, ZU, analogRead(PIN_POTI)); break;
    }
  }
  delay(10);
}
