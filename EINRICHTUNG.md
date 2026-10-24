# Einrichtung (Kurzfassung)
1. Firebase-Projekt mit Authentication (E-Mail/Passwort) und Firestore.
2. `firestore.rules` in Firestore → Regeln einfügen; `INHABER_EMAIL` durch die Inhaber-Adresse ersetzen.
3. Zugangsdaten der Web-App in `firebase-config.js`.
4. GitHub Pages (Branch `main`, Ordner `/`) einschalten und die github.io-Domain in Firebase → Authentication → Autorisierte Domains eintragen.
5. Inhaber meldet sich an (Eintrag in `Mitglieder` entsteht automatisch) und öffnet einmal den Freischalt-Link mit dem Schlüssel.
