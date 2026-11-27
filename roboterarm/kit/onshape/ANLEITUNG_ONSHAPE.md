# Roboterarm Kit-Version in Onshape

`Roboterarm_Kit.fs` ist ein **Feature Studio** (FeatureScript). Es baut die komplette Kit-Version in Onshape auf, mit denselben Maßen wie `cad/kit.scad` und den STL-Dateien.

**Druckteile**
- alle Druckteile der Kit-Version
- exakte Evolventen-Zahnräder: Zahnkranz, Ritzel, Schneckenrad, Stirnräder, Greiferhebel
- Schnecke mit echter Schraubenlinie. Falls Onshape die Schraubenlinie nicht erzeugen kann, baut das Feature die Zähne stattdessen als Ringe. Das gibt keinen Fehler.
- Fin-Ray-Finger mit Rippen und Griffnoppen
- Steckachsen, Clips, Hülsen und Distanzrohre an ihrer Einbaustelle

**Kit-Teile**
- 28BYJ-48 mit Kabel und Stecker
- Gleichstrommotor mit Lötfahnen und Kabel
- Potentiometer mit Mutter
- SG90 mit Horn, Kabel und seinen zwei Schrauben
- LED und Fotowiderstand der Lichtschranke
- 15 Euro-Münzen im Magazin

**Lager:** Mit dem Häkchen „Kugellager 6801“ siehst du die Lager (Ringe, Kugeln, Käfig, Dichtungen). Ohne Häkchen zeigt das Modell die gedruckten Laufrollen und Lagerbuchsen.

**Elektronik:** ESP32-S3-Board, Steckbrett, L293D, ULN2003-Platine, Elko, Widerstände, Taster und USB-Netzteil.

Die Kit-Version hat **keine Schrauben**. Deshalb gibt es auch keinen Normteile-Teil, der bei Version 2 Probleme gemacht hat.

## 1. Einfügen

1. In Onshape ein neues Dokument öffnen.
2. Unten auf **+** klicken, dann **Create Feature Studio**.
3. In der neuen Feature Studio stehen zwei Zeilen. **Diese zwei Zeilen stehen lassen.**
4. In `Roboterarm_Kit.fs` die Zeile mit **AB HIER KOPIEREN** suchen. Ab dort alles bis zum Ende kopieren und unter die zwei Zeilen einfügen.
5. Oben rechts auf **Commit** klicken. Unten darf keine rote Fehlermeldung stehen.

> Bei einer roten Meldung schick mir bitte einen Screenshot mit Zeilennummer.

## 2. Modell erzeugen

1. Zum Tab **Part Studio** wechseln.
2. In der Werkzeugleiste auf **Custom features** (Puzzleteil) klicken, dieses Dokument wählen und dann **Roboterarm Kit**.
3. Bestätigen. Der erste Aufbau dauert etwas.

| Parameter | Bedeutung |
|---|---|
| Schulter (phi) | −33° (ganz unten) … +45° (ganz oben), 0° = Arm waagerecht |
| Greifer offen | 0° … 30° je Hebel |
| Basis drehen | −200° … +200° |
| Motor Durchmesser / Abflachungen / Welle | deine gemessenen Werte des Gleichstrommotors |
| Poti Gewindebuchse / Welle | deine gemessenen Werte des Potentiometers |
| 28BYJ-48 Wellenlänge | Ohr-Unterseite bis Wellenende |
| Häkchen | Baugruppen ein- und ausblenden, Lager oder Ersatzteile, Zahnform exakt, Werkstoffe, Mate Connectors |

> Zum schnellen Arbeiten „Elektronik“ und „Zahnform exakt“ ausschalten.

> Wenn du Maße änderst, ändere sie auch in `cad/kit.scad` und exportiere die STL-Dateien neu.

## 3. Assembly zum Animieren

1. Unten auf **+**, dann **Create Assembly**.
2. **Insert**: das Part Studio wählen, „Part Studio“ (alle Teile), am Ursprung einfügen.
3. Rechtsklick auf **Basis**, dann **Fix**.

### Gruppen bilden
Jeder Teilname beginnt mit seiner Gruppe. Pro Gruppe alle Teile mit diesem Anfang markieren und **Group** wählen:

| Gruppe | Teile mit Namensanfang |
|---|---|
| Basis | `Basis`, `Basis - …`, `Elektronik - …` (dann fixieren) |
| Schulter | `Schulter - …` (Drehteller, Schulterbock, Deckel, Motor, Poti, Lager) |
| Hauptlenker | `Hauptlenker`, `Hauptlenker - …` (Achse, Schneckenrad, Münzen) |
| Parallellenker | `Parallellenker` |
| Handgelenk | `Handgelenk`, `Handgelenk - …` (Greifergehäuse, SG90) |
| Greiferhebel A | `Greiferhebel A …` |
| Greiferhebel B | `Greiferhebel B …` |
| einzeln | `Schnecke`, `Motorritzel`, `Ritzel Basis` |

### Gelenke (Revolute Mates)
An jedem Gelenk liegt auf **beiden Teilen** ein Mate Connector an derselben Stelle. Revolute wählen und die zwei Connectoren anklicken.

| Nr. | Gelenk | Connector auf … | … und auf |
|---|---|---|---|
| J1 | Basis dreht | Basis | Schulter - Drehteller |
| J2 | Schulter | Schulter - Schulterbock | Hauptlenker |
| J3 | Parallellenker oben | Schulter - Schulterbock | Parallellenker |
| J4 | Handgelenk unten | Hauptlenker | Handgelenk |
| J5 | Handgelenk oben | Parallellenker | Handgelenk |
| J6 | Greiferhebel A | Handgelenk - Greifergehäuse | Greiferhebel A |
| J7 | Greiferhebel B | Handgelenk - Greifergehäuse | Greiferhebel B |
| J8 | Ritzel Basis | Basis | Ritzel Basis |
| J9 | Schnecke | Schulter - Schulterbock | Schnecke |
| J10 | Motorritzel | Schulter - Schulterbock | Motorritzel |

J2 bis J5 bilden das **Parallelogramm**. Der Arm hat danach nur einen Freiheitsgrad (J2), und das Handgelenk bleibt immer senkrecht.

### Zahnräder koppeln (Gear Relations)

| Mates | Verhältnis | Richtung |
|---|---|---|
| J8 → J1 | 15 : 75 | umkehren |
| J9 → J2 (Schnecke) | 1 : 80 | |
| J10 → J9 | 10 : 50 | umkehren |
| J6 → J7 | 20 : 20 | umkehren |

Dreht sich der Motor bei der Animation falsch herum, bei der Gear Relation „Reverse“ umschalten.

### Grenzen (wie in der Firmware)

| Mate | min | max |
|---|---|---|
| J1 Basis | −200° | +200° |
| J2 Schulter | −33° | +45° |
| J6 Greifer | 0° | 30° |

## 4. Masse und Schwerpunkt

- Druckteile haben die Dichte von PLA, PETG oder TPU bei 100 % Füllung, also eine obere Grenze.
- Kit-Teile haben ihre echte Masse: 28BYJ-48 30 g, Gleichstrommotor 17 g, SG90 9 g, Potentiometer 7 g, jede Münze 7,5 g.

Teile markieren und unten rechts **Mass properties** öffnen. So prüfst du, ob die Münzen den Arm gut ausgleichen: Der Schwerpunkt der Gruppe „Hauptlenker + Parallellenker + Handgelenk + Greifer“ sollte nahe an der Schulterachse liegen.

## 5. Animieren

- **Ein Gelenk:** Rechtsklick auf J2, dann **Animate**, zum Beispiel von −33° bis 45°. Über die Gear Relations drehen sich Schnecke und Motorritzel mit.
- **Stellungen:** Gelenke ziehen und als **Named position** speichern, zum Beispiel „Greifen links“, „Anheben“ und „Ablegen rechts“.
- **Explosionsansicht:** Mit **Exploded view** siehst du, wie die Teile zusammengebaut werden.
