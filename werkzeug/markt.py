"""Marktanalyse ver- und entschlüsseln (Datei ../markt.bin, unabhängig von der App-Version).
  python3 werkzeug/markt.py entpacken [SCHLUESSEL]   -> werkzeug/markt/markt.json
  python3 werkzeug/markt.py packen    [SCHLUESSEL]   -> ../markt.bin
Ohne SCHLUESSEL wird werkzeug/.schluessel benutzt."""
import sys, os, json, gzip, base64, secrets
from cryptography.hazmat.primitives.ciphers.aead import AESGCM
H = os.path.dirname(os.path.abspath(__file__)); R = os.path.dirname(H)
J = os.path.join(H, "markt", "markt.json"); BIN = os.path.join(R, "markt.bin")
k = sys.argv[2].strip() if len(sys.argv) > 2 else open(os.path.join(H, ".schluessel")).read().strip()
key = base64.urlsafe_b64decode(k + "=" * (-len(k) % 4))
if sys.argv[1] == "entpacken":
    b = open(BIN, "rb").read(); os.makedirs(os.path.dirname(J), exist_ok=True)
    json.dump(json.loads(gzip.decompress(AESGCM(key).decrypt(b[:12], b[12:], None))), open(J, "w", encoding="utf-8"), ensure_ascii=False, indent=1)
    print("entpackt ->", J)
elif sys.argv[1] == "packen":
    d = json.load(open(J, encoding="utf-8"))
    for f in ("stand", "fazit", "kennzahlen", "markt", "wettbewerb", "swot", "zielgruppen", "preis", "naechste", "aenderungen", "quellen"):
        assert f in d, "Feld fehlt: " + f
    iv = secrets.token_bytes(12)
    open(BIN, "wb").write(iv + AESGCM(key).encrypt(iv, gzip.compress(json.dumps(d, ensure_ascii=False).encode(), 9), None))
    print("gepackt ->", BIN, "Stand", d["stand"])
