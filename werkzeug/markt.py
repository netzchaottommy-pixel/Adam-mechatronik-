"""Marktanalyse ver- und entschlüsseln (Datei ../markt.bin).
Verschlüsseln braucht KEINEN geheimen Schlüssel – nur den öffentlichen Schlüssel werkzeug/markt_public.pem.
Lesen kann nur die App nach dem Login (privater Schlüssel liegt verschlüsselt in inhalt.bin).
  python3 werkzeug/markt.py packen      -> werkzeug/markt/markt.json  =>  ../markt.bin
  python3 werkzeug/markt.py entpacken   (nur mit werkzeug/.markt_privat, für Claude im Haupt-Chat)"""
import sys, os, json, gzip, base64, secrets, struct
from cryptography.hazmat.primitives.ciphers.aead import AESGCM
from cryptography.hazmat.primitives import serialization, hashes
from cryptography.hazmat.primitives.asymmetric import padding
H = os.path.dirname(os.path.abspath(__file__)); R = os.path.dirname(H)
J = os.path.join(H, "markt", "markt.json"); BIN = os.path.join(R, "markt.bin")
OAEP = padding.OAEP(mgf=padding.MGF1(hashes.SHA256()), algorithm=hashes.SHA256(), label=None)
FELDER = ("stand", "fazit", "kennzahlen", "markt", "wettbewerb", "swot", "zielgruppen", "preis", "naechste", "aenderungen", "quellen")
if sys.argv[1] == "packen":
    d = json.load(open(J, encoding="utf-8"))
    for f in FELDER: assert f in d, "Feld fehlt: " + f
    pub = serialization.load_pem_public_key(open(os.path.join(H, "markt_public.pem"), "rb").read())
    ak = secrets.token_bytes(32); iv = secrets.token_bytes(12)
    ek = pub.encrypt(ak, OAEP)
    ct = AESGCM(ak).encrypt(iv, gzip.compress(json.dumps(d, ensure_ascii=False).encode(), 9), None)
    open(BIN, "wb").write(b"MK2" + struct.pack(">H", len(ek)) + ek + iv + ct)
    print("gepackt ->", BIN, "Stand", d["stand"])
elif sys.argv[1] == "entpacken":
    der = base64.b64decode(open(os.path.join(H, ".markt_privat")).read().strip())
    priv = serialization.load_der_private_key(der, None)
    b = open(BIN, "rb").read(); assert b[:3] == b"MK2"
    n = struct.unpack(">H", b[3:5])[0]; ek = b[5:5+n]; iv = b[5+n:17+n]; ct = b[17+n:]
    d = json.loads(gzip.decompress(AESGCM(priv.decrypt(ek, OAEP)).decrypt(iv, ct, None)))
    os.makedirs(os.path.dirname(J), exist_ok=True); json.dump(d, open(J, "w", encoding="utf-8"), ensure_ascii=False, indent=1)
    print("entpackt ->", J)
