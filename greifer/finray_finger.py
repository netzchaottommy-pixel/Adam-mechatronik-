#!/usr/bin/env python3
"""Parametrischer Fin-Ray-Greiferfinger (adaptiver Finger, umschließt Teile).

Prinzip: zwei biegsame Flanken laufen zu einer Spitze zusammen und sind über
dünne Querstege verbunden. Drückt ein Teil gegen die Kontaktflanke, biegt sich
der Finger zum Teil hin und legt sich darum.

Erzeugt eine STL-Datei (flach liegend drucken, Profil auf dem Druckbett).
Benötigt: pip install trimesh shapely mapbox_earcut
Vorschau (optional): pip install matplotlib

Beispiel:
  python3 finray_finger.py                      # Standardmaße
  python3 finray_finger.py --laenge 110 --tiefe 25 --stege 9
"""
import argparse
import math

from shapely.geometry import Point, Polygon, box
from shapely.ops import unary_union
import trimesh


def profil(a):
    L, W, Hb = a.laenge, a.breite, a.sockel
    xt = W * a.spitze_pos  # x-Position der Spitze (0 = Rückseite, 1 = Kontaktseite)

    # Außenkontur: Sockel + Dreieck bis zur Spitze
    aussen = Polygon([(0, 0), (W, 0), (W, Hb), (xt, L), (0, Hb)])
    r = a.spitzen_radius
    aussen = aussen.buffer(-r, join_style=1).buffer(r, join_style=1)

    # Innenraum = Außenkontur um Flankenstärke verkleinert, nur oberhalb des Sockels
    innen = aussen.buffer(-a.flanke, join_style=2).difference(box(-1, -1, W + 1, Hb))

    finger = aussen.difference(innen)

    # Querstege, gleichmäßig verteilt, mit einstellbarer Neigung
    y0, y1 = Hb, L - a.flanke * 4
    steg_flaechen = []
    winkel = math.radians(a.steg_winkel)
    for i in range(1, a.stege + 1):
        y = y0 + (y1 - y0) * i / (a.stege + 1)
        lang = W * 3
        dx, dy = math.cos(winkel) * lang, math.sin(winkel) * lang
        mitte = (W * 0.5, y)
        p1 = (mitte[0] - dx, mitte[1] - dy)
        p2 = (mitte[0] + dx, mitte[1] + dy)
        from shapely.geometry import LineString
        steg = LineString([p1, p2]).buffer(a.steg / 2, cap_style=2)
        steg_flaechen.append(steg.intersection(innen.buffer(0.01)))
    finger = unary_union([finger] + steg_flaechen)

    # Befestigungslöcher im Sockel (quer durch die Fingertiefe)
    for fx in (0.28, 0.72):
        finger = finger.difference(Point(W * fx, Hb / 2).buffer(a.loch / 2, resolution=32))
    return finger


def main():
    p = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    p.add_argument("--laenge", type=float, default=90, help="Fingerlänge in mm (inkl. Sockel)")
    p.add_argument("--breite", type=float, default=30, help="Breite am Sockel in mm")
    p.add_argument("--tiefe", type=float, default=20, help="Fingertiefe = Extrusionshöhe beim Druck, mm")
    p.add_argument("--sockel", type=float, default=10, help="Höhe des massiven Sockels, mm")
    p.add_argument("--flanke", type=float, default=1.6, help="Flankenstärke, mm (4 Linien bei 0,4er Düse)")
    p.add_argument("--steg", type=float, default=1.2, help="Stegstärke, mm (3 Linien bei 0,4er Düse)")
    p.add_argument("--stege", type=int, default=7, help="Anzahl Querstege")
    p.add_argument("--steg_winkel", type=float, default=0, help="Stegneigung in Grad (0 = waagerecht)")
    p.add_argument("--spitze_pos", type=float, default=0.5, help="Lage der Spitze 0..1 (1 = Kontaktseite gerade)")
    p.add_argument("--spitzen_radius", type=float, default=1.5, help="Rundung der Spitze, mm")
    p.add_argument("--loch", type=float, default=3.4, help="Lochdurchmesser für M3, mm")
    p.add_argument("--out", default="finray_finger.stl")
    p.add_argument("--vorschau", default=None, help="PNG-Vorschau des Profils speichern")
    a = p.parse_args()

    prof = profil(a)
    if prof.geom_type != "Polygon":
        raise SystemExit("Profil zerfällt in mehrere Teile, bitte Maße prüfen")
    mesh = trimesh.creation.extrude_polygon(prof, a.tiefe)
    assert mesh.is_watertight, "Netz nicht geschlossen"
    mesh.export(a.out)
    print(f"{a.out}: {len(mesh.faces)} Dreiecke, Volumen {mesh.volume / 1000:.1f} cm³")

    if a.vorschau:
        import matplotlib
        matplotlib.use("Agg")
        import matplotlib.pyplot as plt
        fig, ax = plt.subplots(figsize=(4, 9))
        ax.fill(*prof.exterior.xy, color="#1f6feb")
        for ring in prof.interiors:
            ax.fill(*ring.xy, color="white")
        ax.set_aspect("equal")
        ax.set_title("Fin-Ray-Finger (Profil)")
        ax.text(a.breite + 1, a.laenge / 2, "Kontaktseite", rotation=-90, va="center")
        fig.savefig(a.vorschau, dpi=120, bbox_inches="tight")


if __name__ == "__main__":
    main()
