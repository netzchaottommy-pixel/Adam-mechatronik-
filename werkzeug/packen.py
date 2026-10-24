"""Verschlüsselt quelle/ wieder zu ../inhalt.bin und setzt eine neue Versionsnummer.
Aufruf: python3 werkzeug/packen.py <neue Version, z. B. 2.0.1>"""
import sys, os, json, gzip, base64, secrets, re
from cryptography.hazmat.primitives.ciphers.aead import AESGCM
H = os.path.dirname(os.path.abspath(__file__)); R = os.path.dirname(H); Q = os.path.join(H, "quelle")
new = sys.argv[1]; old = open(os.path.join(R, "VERSION")).read().strip()
k = open(os.path.join(H, ".schluessel")).read().strip(); key = base64.urlsafe_b64decode(k + "=" * (-len(k) % 4))
types = json.load(open(os.path.join(Q, "dateien.json")))
bundle = {"page": open(os.path.join(Q, "seite.html"), encoding="utf-8").read(),
          "start": json.load(open(os.path.join(Q, "startdaten.json"), encoding="utf-8")),
          "assets": {n: {"type": t, "b64": base64.b64encode(open(os.path.join(Q, "dateien", n), "rb").read()).decode()} for n, t in types.items()}}
iv = secrets.token_bytes(12)
open(os.path.join(R, "inhalt.bin"), "wb").write(iv + AESGCM(key).encrypt(iv, gzip.compress(json.dumps(bundle, ensure_ascii=False).encode(), 9), None))
for f, pat, rep in (("sw.js", r'var VERSION = "[^"]*"', 'var VERSION = "%s"' % new),
                    ("zw-app.js", r'var APP_VERSION = "[^"]*"', 'var APP_VERSION = "%s"' % new),
                    ("index.html", r'zw-app\.js\?v=[^"]*"', 'zw-app.js?v=%s"' % new)):
    p = os.path.join(R, f); s = open(p, encoding="utf-8").read(); s2 = re.sub(pat, rep, s); assert s2 != s or old == new, f; open(p, "w", encoding="utf-8").write(s2)
open(os.path.join(R, "VERSION"), "w").write(new + "\n")
print("gepackt", old, "->", new)
