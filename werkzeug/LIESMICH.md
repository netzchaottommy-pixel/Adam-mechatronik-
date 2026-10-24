# Werkzeug für Updates
1. `python3 werkzeug/entpacken.py <SCHLUESSEL>`: legt `werkzeug/quelle/` an (Seite, Startdaten, Dateien). Den Schlüssel gibt es in Firestore unter `inhalt/schluessel`.
2. Dateien in `werkzeug/quelle/` ändern.
3. `python3 werkzeug/packen.py <neue Version>`: verschlüsselt neu und erhöht die Version, damit die App „Neue Version verfügbar“ zeigt.
4. Committen und pushen. `quelle/` und `.schluessel` werden nie hochgeladen.
