"""Packt die kompilierte ZW-1-Firmware verschlüsselt nach ../firmware.bin (nur nach Login in der App lesbar).
Aufruf: python3 werkzeug/firmware_packen.py <build-Ordner> <boot_app0.bin> <version>"""
import sys, os, json, gzip, base64, secrets, glob
from cryptography.hazmat.primitives.ciphers.aead import AESGCM
H = os.path.dirname(os.path.abspath(__file__)); R = os.path.dirname(H)
bd, boot_app0, ver = sys.argv[1], sys.argv[2], sys.argv[3]
k = open(os.path.join(H, ".schluessel")).read().strip(); key = base64.urlsafe_b64decode(k + "=" * (-len(k) % 4))
f = lambda pat: glob.glob(os.path.join(bd, pat))[0]
parts = [(0x0, f("*.ino.bootloader.bin")), (0x8000, f("*.ino.partitions.bin")), (0xE000, boot_app0), (0x10000, f("*.ino.bin"))]
d = {"version": ver, "chip": "ESP32-S3", "flash": "8MB", "parts": [{"offset": o, "b64": base64.b64encode(open(p, "rb").read()).decode()} for o, p in parts]}
iv = secrets.token_bytes(12)
open(os.path.join(R, "firmware.bin"), "wb").write(iv + AESGCM(key).encrypt(iv, gzip.compress(json.dumps(d).encode(), 9), None))
print("firmware.bin", ver, os.path.getsize(os.path.join(R, "firmware.bin")), "Bytes")
