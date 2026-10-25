"""Entschlüsselt ../inhalt.bin in den Ordner quelle/ (wird NICHT hochgeladen).
Aufruf: python3 werkzeug/entpacken.py <SCHLUESSEL>   (Schlüssel steht in Firestore unter inhalt/schluessel)"""
import sys, os, json, gzip, base64
from cryptography.hazmat.primitives.ciphers.aead import AESGCM
H = os.path.dirname(os.path.abspath(__file__)); R = os.path.dirname(H); Q = os.path.join(H, "quelle")
k = sys.argv[1].strip(); key = base64.urlsafe_b64decode(k + "=" * (-len(k) % 4))
open(os.path.join(H, ".schluessel"), "w").write(k)
b = open(os.path.join(R, "inhalt.bin"), "rb").read()
d = json.loads(gzip.decompress(AESGCM(key).decrypt(b[:12], b[12:], None)))
os.makedirs(os.path.join(Q, "dateien"), exist_ok=True)
open(os.path.join(Q, "seite.html"), "w", encoding="utf-8").write(d["page"])
json.dump(d["start"], open(os.path.join(Q, "startdaten.json"), "w", encoding="utf-8"), ensure_ascii=False, indent=1)
for n, a in d["assets"].items():
    open(os.path.join(Q, "dateien", n), "wb").write(base64.b64decode(a["b64"]))
json.dump({n: a["type"] for n, a in d["assets"].items()}, open(os.path.join(Q, "dateien.json"), "w"), indent=1)
if d.get("mk"): open(os.path.join(H, ".markt_privat"), "w").write(d["mk"])
print("entpackt nach", Q)
