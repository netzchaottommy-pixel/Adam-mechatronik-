# Marktanalyse – wöchentliche Aktualisierung

Die Seite „Marktanalyse“ in der App liest `markt.bin`. Die Datei wird mit dem **öffentlichen** Schlüssel
`werkzeug/markt_public.pem` verschlüsselt – zum Aktualisieren braucht man also kein Geheimnis.
Lesen kann sie nur die App nach dem Login. Die App lädt sie bei jedem Öffnen frisch und merkt sich den
Änderungsverlauf selbst.

Ablauf für die wöchentliche Aktualisierung:
1. Im Web recherchieren (Wettbewerber, Preise, Marktzahlen, Konjunktur, Förderprogramme).
2. `werkzeug/markt/markt.json` komplett neu schreiben (Ordner anlegen), Felder:
   - `stand`: heutiges Datum (JJJJ-MM-TT), `rhythmus`: "jeden Montag automatisch"
   - `fazit`: {ampel: gruen | gelbgruen | gelb | rot, titel, text}
   - `kennzahlen[]`: {wert, label, quelle = Index in `quellen`} – 4 Stück
   - `markt[]`, `zielgruppen[]`, `naechste[]`: kurze deutsche Sätze
   - `wettbewerb[]`: {name, art, preis, plus, minus, quelle}
   - `swot`: {staerken[], schwaechen[], chancen[], risiken[]}
   - `preis`: {vorschlag, begruendung[], szenario}
   - `aenderungen[]`: Neuigkeiten der letzten 7 Tage als {datum, text}; gibt es keine: [{datum, "Keine wesentlichen Neuigkeiten."}]
   - `quellen[]`: {titel, url} – nur echte, geprüfte Links
   Einfaches Deutsch, keine erfundenen Zahlen; Unsicheres als „ca.“ oder „laut Anbieter“ kennzeichnen.
3. `python3 -I werkzeug/markt.py packen`
4. Nur `markt.bin` committen und auf `main` pushen. `werkzeug/markt/` nie committen.
