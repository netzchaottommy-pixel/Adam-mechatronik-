# Marktanalyse ZW-1 – wöchentliche Aktualisierung

Die Seite „Marktanalyse“ in der App liest `markt.bin` (verschlüsselt, gleicher Schlüssel wie `inhalt.bin`).
Sie wird bei jedem Öffnen frisch geladen – kein neues App-Update nötig.

Ablauf:
1. `python3 werkzeug/markt.py entpacken <SCHLUESSEL>` → `werkzeug/markt/markt.json`
2. Im Web recherchieren: Wettbewerber (Preise, neue Produkte, Übernahmen), Marktzahlen, Förderprogramme,
   Konjunktur im Verarbeitenden Gewerbe, neue Anbieter günstiger OEE-/Stückzahl-Boxen.
3. `markt.json` aktualisieren – Struktur beibehalten:
   - `stand`: heutiges Datum (JJJJ-MM-TT)
   - `fazit.ampel`: `gruen` | `gelbgruen` | `gelb` | `rot`; `fazit.titel`, `fazit.text`
   - `kennzahlen[]`: {wert, label, quelle = Index in `quellen`}
   - `wettbewerb[]`: {name, art, preis, plus, minus, quelle}
   - `swot`: {staerken[], schwaechen[], chancen[], risiken[]}
   - `markt[]`, `zielgruppen[]`, `naechste[]`: kurze deutsche Sätze
   - `preis`: {vorschlag, begruendung[], szenario}
   - `aenderungen[]`: neuen Eintrag VORNE einfügen {datum, text} – was sich seit letzter Woche geändert hat
     (oder „Keine wesentlichen Änderungen.“); höchstens 20 Einträge behalten
   - `quellen[]`: {titel, url} – nur echte, geprüfte Links
   Sprache: einfaches Deutsch, keine erfundenen Zahlen; unsichere Angaben als „ca.“ oder „laut Anbieter“.
4. `python3 werkzeug/markt.py packen <SCHLUESSEL>`
5. Nur `markt.bin` committen und pushen. Niemals `werkzeug/markt/`, `werkzeug/quelle/` oder den Schlüssel committen.
