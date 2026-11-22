"""Legt eine Datei verschlüsselt in ../dateien/ ab und trägt sie in die Dateiliste der App ein.
Nur wer in der App angemeldet ist, kann sie herunterladen.

Aufruf:
  python3 werkzeug/datei_packen.py <gruppe> <titel> <beschreibung> <pfad> [<download-name>]
  python3 werkzeug/datei_packen.py --liste            (zeigt alle Dateien)
  python3 werkzeug/datei_packen.py --weg <id>          (entfernt eine Datei)
  python3 werkzeug/datei_packen.py --holen <id> <ziel> (entschlüsselt eine Datei wieder)
Gruppen: gehaeuse, halter, plaene, firmware, skripte, praesentation"""
import sys, os, json, gzip, base64, secrets, re, mimetypes, time
from cryptography.hazmat.primitives.ciphers.aead import AESGCM
H = os.path.dirname(os.path.abspath(__file__)); R = os.path.dirname(H); D = os.path.join(R, "dateien")
k = open(os.path.join(H, ".schluessel")).read().strip(); key = base64.urlsafe_b64decode(k + "=" * (-len(k) % 4))
def enc(obj, path):
    iv = secrets.token_bytes(12)
    open(path, "wb").write(iv + AESGCM(key).encrypt(iv, gzip.compress(json.dumps(obj, ensure_ascii=False).encode(), 9), None))
def dec(path):
    b = open(path, "rb").read(); return json.loads(gzip.decompress(AESGCM(key).decrypt(b[:12], b[12:], None)))
IDX = os.path.join(D, "index.bin")
os.makedirs(D, exist_ok=True)
idx = dec(IDX) if os.path.exists(IDX) else {"dateien": []}
a = sys.argv[1:]
if a and a[0] == "--liste":
    for f in idx["dateien"]: print(f["id"], "|", f["gruppe"], "|", f["titel"], "|", f["name"], f["groesse"])
    sys.exit()
if a and a[0] == "--weg":
    idx["dateien"] = [f for f in idx["dateien"] if f["id"] != a[1]]
    p = os.path.join(D, a[1] + ".bin")
    if os.path.exists(p): os.remove(p)
    enc(idx, IDX); print("entfernt", a[1]); sys.exit()
if a and a[0] == "--holen":
    f = dec(os.path.join(D, a[1] + ".bin")); open(a[2], "wb").write(base64.b64decode(f["b64"])); print("gespeichert", a[2]); sys.exit()
gruppe, titel, beschr, pfad = a[:4]
name = a[4] if len(a) > 4 else os.path.basename(pfad)
fid = re.sub(r"[^a-z0-9]+", "-", name.lower()).strip("-")
typ = mimetypes.guess_type(name)[0] or ("model/stl" if name.lower().endswith(".stl") else "application/octet-stream")
raw = open(pfad, "rb").read()
enc({"name": name, "type": typ, "b64": base64.b64encode(raw).decode()}, os.path.join(D, fid + ".bin"))
idx["dateien"] = [f for f in idx["dateien"] if f["id"] != fid] + [{"id": fid, "gruppe": gruppe, "titel": titel, "text": beschr, "name": name, "groesse": len(raw), "stand": time.strftime("%Y-%m-%d")}]
enc(idx, IDX)
print("abgelegt", fid, len(raw), "Bytes")
