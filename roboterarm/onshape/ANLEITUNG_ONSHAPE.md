# Roboterarm AM-R1 in Onshape

`Roboterarm_AM-R1.fs` ist ein **Feature Studio** (FeatureScript). Es baut das komplette Modell direkt in Onshape auf:
- alle 30 Druckteile mit denselben Maßen wie die STL-Dateien
- Evolventen-Zahnräder, Sechskante, Senkungen, Muttertaschen und Fin-Ray-Rippen
- dazu die Kaufteile: 2× DS3218, MG996R, 28BYJ-48, 11 Lager 6801 und die Stangenstifte
- an allen Gelenken fertige **Mate Connectors**

## 1. Einfügen

1. In Onshape ein neues Dokument öffnen.
2. Unten auf **+** klicken, dann **Create Feature Studio**.
3. In der neuen Feature Studio stehen zwei Zeilen (`FeatureScript …;` und `import(…)`). **Diese zwei Zeilen stehen lassen.**
4. Aus `Roboterarm_AM-R1.fs` alles **ab Zeile 3** kopieren und darunter einfügen.
5. Oben rechts auf **Commit** klicken. Unten darf keine rote Fehlermeldung stehen.

> Falls Onshape meldet, dass eine Funktion nicht gefunden wird: Prüfe, ob du wirklich die Kopfzeilen deiner eigenen Feature Studio behalten hast. Ansonsten schick mir die Fehlermeldung mit Zeilennummer.

## 2. Modell erzeugen

1. Zum Tab **Part Studio** wechseln.
2. Rechts in der Werkzeugleiste auf **Custom features** (Symbol mit Puzzleteil) klicken. Dort dieses Dokument und dann **Roboterarm AM-R1** wählen.
3. Das Feature einfügen. Der Aufbau dauert beim ersten Mal etwas, es entstehen über 50 Teile.

| Parameter | Bedeutung |
|---|---|
| Oberarm (alpha) | Winkel des Oberarms über der Waagerechten, −5° … 100° |
| Unterarm absolut (beta) | Winkel des Unterarms zur Waagerechten, −130° … 10° |
| Greifer offen | Öffnungswinkel je Hebel, 0° … 35° |
| Basis drehen | −200° … 200° |
| Häkchen | Baugruppen und Kaufteile ein- oder ausblenden. Ausblenden macht das Feature schneller. |

Die Grundstellung (alpha 50°, beta −60°) ist die **Kalibrierstellung** aus der Bauanleitung.

## 3. Assembly zum Animieren

1. Unten auf **+**, dann **Create Assembly**.
2. **Insert**: das Part Studio wählen, „**Part Studio**“ (alle Teile), dann Einfügen am Ursprung bestätigen. Alle Teile stehen sofort richtig.
3. Rechtsklick auf **Basis**, dann **Fix**.

### Gruppen bilden (Teile, die sich zusammen bewegen)
Jeder Teilname beginnt mit seiner Gruppe. Pro Gruppe alle Teile mit diesem Anfang markieren und **Group** wählen (Mate-Menü, Symbol mit zwei Würfeln):

| Gruppe | Teile mit Namensanfang |
|---|---|
| Basis | `Basis - …` und `Basis` (dann fixieren) |
| Schulter | `Schulter - …` (Drehteller, Gehäuse, Deckel, Lager, beide DS3218) |
| Oberarm | `Oberarm - …` |
| Kurbel | `Kurbel` und `Kurbel - …` |
| Unterarm | `Unterarm - …` (auch Greifer-Gehäuse und MG996R) |
| Greiferhebel A | `Greiferhebel A …` |
| Greiferhebel B | `Greiferhebel B …` |
| einzeln | `Koppelstange`, `Ritzel Basis`, `Ritzel Schulter` + `Ritzel Schulter - Servoscheibe`, `Ritzel Ellbogen` + Servoscheibe (die zwei Ritzel jeweils mit ihrer Scheibe gruppieren) |

### Gelenke (Revolute Mates)
Für jedes Gelenk liegt auf **beiden Teilen** ein Mate Connector an derselben Stelle.
- **Revolute** wählen und nacheinander die zwei Connectoren anklicken. Sie werden beim Darüberfahren angezeigt.
- Weil sie schon aufeinander liegen, bewegt sich dabei nichts.

| Nr. | Gelenk | Connector auf … | … und auf |
|---|---|---|---|
| J1 | Basis dreht | Basis | Schulter - Drehteller |
| J2 | Schulter | Schulter - Gehäuse links | Oberarm - Kasten |
| J3 | Ellbogenantrieb (Kurbel) | Schulter - Gehäuse rechts | Kurbel |
| J4 | Ellbogen | Oberarm - Kasten | Unterarm - Kanal |
| J5 | Stange oben | Kurbel | Koppelstange |
| J6 | Stange unten | Unterarm - Hebel | Koppelstange |
| J7 | Greiferhebel A | Unterarm - Greifer-Gehäuse | Greiferhebel A (Antrieb) |
| J8 | Greiferhebel B | Unterarm - Greifer-Gehäuse | Greiferhebel B (Mitläufer) |
| J9 | Servo Schulter | Schulter - Gehäuse links | Ritzel Schulter |
| J10 | Servo Ellbogen | Schulter - Gehäuse rechts | Ritzel Ellbogen |
| J11 | Schrittmotor | Basis | Ritzel Basis |

J2 bis J6 bilden zusammen das **Parallelogramm**. Onshape löst diese geschlossene Schleife selbst. Der Arm hat danach genau zwei Freiheitsgrade: Oberarm (J2) und Kurbel (J3).

### Zahnräder koppeln (Gear Relations)
Im Mate-Menü **Gear relation** wählen:

| Mates | Verhältnis | Richtung |
|---|---|---|
| J11 (Ritzel Basis) → J1 (Basis) | 15 : 75 | umkehren |
| J9 (Ritzel Schulter) → J2 (Schulter) | 22 : 44 | umkehren |
| J10 (Ritzel Ellbogen) → J3 (Kurbel) | 22 : 44 | umkehren |
| J7 → J8 (Greiferhebel) | 24 : 24 | umkehren |

Danach drehen sich Motor- und Servoritzel mit, und beide Greiferfinger öffnen gleichzeitig.

### Grenzen (wie in der Firmware)
Bei den Revolute Mates **Limits** einschalten. Die Werte gelten ab der Grundstellung:

| Mate | min | max |
|---|---|---|
| J1 Basis | −200° | +200° |
| J2 Schulter | −55° | +50° (alpha −5° … 100°) |
| J4 Ellbogen | Knick zwischen Ober- und Unterarm 30° … 120°, also relativ −80° … +10° |
| J7 Greifer | 0° | 35° |

## 4. Animieren

- **Ein Gelenk:** Rechtsklick auf ein Revolute Mate (zum Beispiel J2), dann **Animate**. Start- und Endwinkel sowie Schritte eingeben, dann abspielen. Die Ritzel drehen über die Gear Relations mit.
- **Mehrere Stellungen nacheinander:** Gelenke in eine Stellung ziehen und als **Named position** speichern. Weitere Stellungen anlegen, zum Beispiel „Greifen“, „Anheben“ und „Drehen“. Die Named Positions kannst du nacheinander abrufen.
- **Explosionsansicht:** **Exploded view** zeigt, wie die Teile zusammengebaut werden.

## Hinweise

- Die Mate Connectors passen sich an, wenn du im Part Studio alpha, beta oder den Greiferwinkel änderst. Für das Assembly die Grundstellung lassen.
- Maße ändern: oben in der Feature Studio stehen alle Werte als Konstanten, gleich benannt wie in `cad/roboterarm.scad`. Zum Beispiel `HORN_ABST` und `SV_LOCH_A`, wenn deine Servos anders sind. Nach dem Ändern **Commit** klicken, dann baut sich das Modell neu.
- Druckdateien bleiben die STLs aus `stl/`. Die Teile hier entsprechen ihnen 1:1, sind aber in Montagestellung.
