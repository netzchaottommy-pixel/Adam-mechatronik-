// =====================================================================
//  Roboterarm AM-R1 "Kit-Version" – nur mit dem Freenove ESP32-S3 Kit
//
//  Basis      28BYJ-48 + Zahnkranz 1:5, Lichtschranke (LED + Fotowiderstand)
//  Schulter   Gleichstrommotor aus dem Kit -> Stirnrad 1:5 -> Schnecke 1:80
//             (1:400, selbsthemmend), Winkel misst das Potentiometer aus dem Kit
//  Arm        Parallelogramm: Greifer bleibt immer waagerecht
//  Greifer    SG90 + Fin-Ray-Finger aus TPU (wird umgedreht montiert)
//  Ohne Schrauben: Steckachsen mit Clips, Presspassungen, Bajonett-Kappe.
//  Nur die kleinen Schrauben aus dem SG90-Beutel werden benutzt.
//  Lager 6801 (12x21x5) optional: für jedes gibt es ein gedrucktes Ersatzteil.
//
//  Export:  openscad -D 'teil="basis"' -o basis.stl kit.scad
//  Werte mit "MESSEN" an deinen Kit-Teilen prüfen.
// =====================================================================

teil = "zusammenbau";
$fn = 64;

// ---------- Lager 6801 / Ersatz ----------
lager_D = 21; lager_d = 12; lager_b = 5;
sitz_D = lager_D - 0.05;  lippe_D = 16;

// ---------- Basis (wie Version 1) ----------
basis_r = 90;  boden_t = 3;  basis_h = 55;
innen_r1 = 66; innen_r2 = 70; aussen_t = 2.4;
deckel_t = 4;  rolle_r = 78;
achse_h = lager_D/2 + 2;
deckel_oben = basis_h + deckel_t;               // 59
tt_unten = deckel_oben + achse_h + lager_D/2;   // 82
tt_t = 8;  tt_oben = tt_unten + tt_t;           // 90
tt_r = 86;  saeule_r = 15;  kabel_r = 8;
zr_m = 1.25; zr_z1 = 15; zr_z2 = 75;
zr_a = zr_m*(zr_z1+zr_z2)/2 + 0.15;
kranz_ri = 40;  kranz_unten = tt_unten - 8;
sm_welle_l = 10;   // 28BYJ-48: Ohr-Unterseite bis Wellenende       MESSEN
sm_versatz = 8;  sm_ohr = 17.5;
ritzel_oben = tt_unten - 0.5;
sm_ohr_z = ritzel_oben - sm_welle_l;
lsk_z = tt_unten - 5;  rock_r1 = 23;  rock_r2 = 25;
kappe_h = 7;  saeule_oben = tt_oben + 0.2 + kappe_h + 0.2;
bock_stifte = [[-55,-30],[-55,30],[20,-30],[20,30]];   // Schulterbock -> Drehteller

// ---------- Schulter ----------
H_s = 70;                // Schulterachse über Drehteller
S_z = tt_oben + H_s;     // 160 über Tisch
P_dz = 30;               // oberer Drehpunkt des Parallelogramms
yi = 16;  yp = 22;       // Seitenwände y +-16 .. +-22
w_m = 0.8;  w_z = 80;    // Schneckenrad 80 Zähne, Modul 0.8
s_d1 = 10;               // Schnecke Teilkreis
s_lead = PI*w_m;         // Steigung (eingängig)
s_a = (w_m*w_z + s_d1)/2 + 0.15;   // Achsabstand 37.15
s_y = 28;                // Ebene Schneckenrad / Schnecke
w_h = 8;                 // Breite Schneckenrad
st_m = 0.8; st_z1 = 10; st_z2 = 50;   // Stirnrad Motor -> Schnecke 1:5
st_a = st_m*(st_z1+st_z2)/2 + 0.15;   // 24.15
st_x = 31;               // Stirnrad-Ebene x 31..35
mot_d = 20.4; mot_fl = 15.1; mot_l = 25;  mot_welle = 2.0;   // Gleichstrommotor (130er)  MESSEN
mot_z = -s_a + st_a;     // Motorachse (Schulter-Koordinaten)
mot_x1 = 43;             // Motor-Körper x 43..68
pot_buchse = 7.2;  pot_welle = 6.0;  pot_tief = 11;          // Potentiometer           MESSEN
phi_min = -33;  phi_max = 45;

// ---------- Arm ----------
L = 120;                 // Lenkerlänge
hl_y1 = -10;  hl_y2 = -2;     // Hauptlenker-Ebene
pl_y1 = 2;    pl_y2 = 8;      // Parallellenker-Ebene
hg_y = [[-15,-11],[9,13]];    // Handgelenk-Platten
muenz_r = 62;  muenz_d = 23.6;     // Münzmagazin (1-Euro-Münzen)
stift_d = 5;

// ---------- Greifer (SG90, wie Version 1) ----------
g_ab = 10.1;  g_m = 1;  g_z = 20;  g_zr_h = 5;  g_z0 = 10;  g_platte = 4;
finger_l = 62; finger_h = 20; finger_b = 20; finger_x = 16;
wand = 1.2; rippe = 1.0; rippen_abst = 7;
sg_x1 = -16.6-0.3; sg_x2 = 5.9+0.3; sg_b = 12.4;
hg_flansch_x = 22;      // Vorderseite Handgelenk -> Greiferflansch
hg_z0 = -57;            // Greifer: z = -z_g + hg_z0 (umgedreht)

// =====================================================================
//  Hilfen
// =====================================================================
function rot2(p, a) = [p[0]*cos(a)-p[1]*sin(a), p[0]*sin(a)+p[1]*cos(a)];
function evolv(rb, t) = let(d = t*180/PI) [rb*(cos(d)+t*sin(d)), rb*(sin(d)-t*cos(d))];
module zahnrad2d(m, z, spiel=0.08) {
    rp = m*z/2; ra = rp + m; rf = rp - 1.25*m; rb = rp*cos(20);
    tmax = sqrt(pow(ra/rb,2) - 1);
    inv20 = (tan(20) - 20*PI/180)*180/PI;
    halb = 90/z + inv20 - (spiel/rp)*180/PI/2;
    fl = [for (i=[0:12]) rot2(evolv(rb, tmax*i/12), -halb)];
    zahn = concat([[0,0]], fl, [for (i=[12:-1:0]) [fl[i][0], -fl[i][1]]]);
    union() { circle(r=rf, $fn=z*4); for (k=[0:z-1]) rotate(k*360/z) polygon(zahn); }
}
module zahnrad(m, z, h, twist=0, spiel=0.08) linear_extrude(h, twist=twist, slices=max(1, abs(twist)>0 ? 4 : 1)) zahnrad2d(m, z, spiel);
module rohr(ri, ra, h) difference() { cylinder(r=ra, h=h, $fn=180); translate([0,0,-1]) cylinder(r=ri, h=h+2, $fn=180); }
module tropfen(d, l) rotate([0,90,0]) linear_extrude(l, center=true) hull() { circle(d=d); translate([-d/2*1.4142,0]) circle(d=0.01); }
module sechskant2d(s) circle(d=s/cos(30), $fn=6);
module stab2d(a, b, r1, r2) hull() { translate(a) circle(r=r1); translate(b) circle(r=r2); }
module XV(y1, y2) translate([0,y2,0]) rotate([90,0,0]) linear_extrude(y2-y1) children();
module ZY(d, y1, y2, x=0, v=0) translate([x,y2,v]) rotate([90,0,0]) cylinder(d=d, h=y2-y1);
module ZX(d, x1, x2, y=0, z=0) translate([x1,y,z]) rotate([0,90,0]) cylinder(d=d, h=x2-x1);
// Schnapp-Stift (Spreizstift) nach oben: Ø d, Höhe h, Haken oben
module schnappstift(d, h) difference() {
    union() { cylinder(d=d, h=h); translate([0,0,h-0.8]) cylinder(d1=d+0.6, d2=d-0.4, h=0.8); }
    translate([-0.5,-d,h-2.6]) cube([1, 2*d, 3]);
}

// =====================================================================
//  BASIS (Version 1, schraubenlos)
// =====================================================================
sm_wx = zr_a;  sm_gx = zr_a - sm_versatz;

module basis() {
    difference() {
        union() {
            cylinder(r=basis_r, h=boden_t, $fn=200);
            rohr(basis_r-aussen_t, basis_r, basis_h);
            rohr(innen_r1, innen_r2, basis_h);
            cylinder(r=saeule_r, h=saeule_oben);
            // Bajonett-Nocken für die Kappe
            for (a=[0,180]) rotate(a) translate([saeule_r-0.5, -2, tt_oben+0.2+2.4]) cube([2.5, 4, 2]);
            rotate(270) translate([innen_r1, -9, 0]) cube([basis_r-innen_r1-0.5, 18, 17]);
            for (s=[-1,1]) {
                translate([sm_gx-4, s*sm_ohr-3, 0]) cube([8, 6, sm_ohr_z]);
                translate([sm_gx, s*sm_ohr, sm_ohr_z]) schnappstift(3.9, 2.2);      // durch die Ohren des Motors
                translate([sm_gx, s*sm_ohr-1.5, 0]) cube([innen_r1-sm_gx+1, 3, basis_h]);
            }
            translate([sm_gx-4, -sm_ohr, 0]) cube([8, 2*sm_ohr, 8]);
            translate([-(rock_r1-2), -5, 0]) cube([rock_r1-2-saeule_r+2, 10, lsk_z+5]);
            translate([-(rock_r2+10), -5, 0]) cube([8, 10, lsk_z+5]);
            translate([-innen_r1-1, -1.5, 0]) cube([innen_r1+1-rock_r2-9, 3, basis_h]);
            // Stifte für den Deckelring (Presspassung)
            for (k=[0:5]) rotate(30+k*60) translate([(innen_r1+innen_r2)/2, 0, basis_h-0.01]) cylinder(d=4, h=deckel_t);
        }
        translate([0,0,-1]) cylinder(r=kabel_r, h=300);
        rotate(270) translate([0, -6, boden_t]) cube([saeule_r+2, 12, 18]);
        rotate(270) translate([innen_r1-6, -7, boden_t]) cube([40, 14, 12]);
        translate([-(rock_r2+12), 0, lsk_z]) rotate([0,90,0]) cylinder(d=5.2, h=12);
        translate([-(rock_r1), 0, lsk_z]) rotate([0,90,0]) cylinder(d=5.1, h=rock_r1-kabel_r+1);
    }
}

module deckelring() {
    difference() {
        union() {
            rohr(innen_r1, basis_r, deckel_t);
            for (k=[0:3]) rotate(45+k*90) for (s=[-1,1]) translate([rolle_r + s*(lager_b/2+0.3+2) - 2, -8, 0])
                cube([4, 16, deckel_t+achse_h+lager_d/2+3]);
        }
        for (k=[0:5]) rotate(30+k*60) translate([(innen_r1+innen_r2)/2, 0, -1]) cylinder(d=4.15, h=10);
        for (k=[0:3]) rotate(45+k*90) translate([rolle_r, 0, deckel_t+achse_h]) tropfen(lager_d+0.15, 30);
    }
}

module rollenachse() {
    l = lager_b + 0.6 + 8;
    difference() {
        rotate([0,90,0]) cylinder(d=lager_d-0.05, h=l, center=true);
        translate([-l, -lager_d, -lager_d/2-1]) cube([2*l, 2*lager_d, 1.5]);
    }
}
// Ersatz für ein Lager 6801: Laufrolle (dreht auf der Achse) bzw. Gleitbuchse (fest im Sitz)
module laufrolle() rohr((lager_d+0.4)/2, lager_D/2, lager_b);
module lagerbuchse() rohr((lager_d+0.3)/2, sitz_D/2+0.02, lager_b);

module drehteller() {           // Unterseite nach OBEN drucken
    difference() {
        union() {
            cylinder(r=tt_r, h=tt_t, $fn=200);
            translate([0,0,-(tt_unten-kranz_unten)]) difference() {
                zahnrad(zr_m, zr_z2, tt_unten-kranz_unten+0.01);
                translate([0,0,-1]) cylinder(r=kranz_ri, h=20, $fn=120);
            }
            translate([0,0,-(tt_unten-(lsk_z-5))]) intersection() {
                rohr(rock_r1, rock_r2, tt_unten-(lsk_z-5)+0.01);
                translate([-50,-50,-1]) cube([100,50,40]);
            }
        }
        translate([0,0,-20]) cylinder(r=saeule_r+0.4, h=40);
        for (p=bock_stifte) translate([p[0], p[1], 2]) cylinder(d=5.15, h=20);
        translate([tt_r-14, 0, tt_t-0.6]) linear_extrude(1) polygon([[0,-5],[10,0],[0,5]]);
    }
}

module ritzel_basis() difference() {
    zahnrad(zr_m, zr_z1, ritzel_oben-(kranz_unten+0.5));
    translate([0,0,-1]) intersection() { cylinder(d=5.25, h=20); cube([3.15, 10, 40], center=true); }
}

// Bajonett-Kappe: hält den Drehteller unten, aufsetzen und 40° drehen
module kappe() difference() {
    cylinder(d=42, h=kappe_h);
    translate([0,0,-1]) cylinder(d=2*saeule_r+0.4, h=kappe_h-1.2);
    translate([0,0,-1]) cylinder(r=kabel_r, h=kappe_h+2);
    for (a=[0,180]) rotate(a) {
        translate([0,-2.3,-1]) cube([saeule_r+2.6, 4.6, 1+4.8]);                         // Einführschlitz
        intersection() {                                                               // Drehnut 45°
            translate([0,0,2.2]) rohr(saeule_r+0.2, saeule_r+2.6, 2.6);
            linear_extrude(10) polygon([[0,0],[40,-2.3],[40*cos(45)+2.3*sin(45), 40*sin(45)], [0,0]]);
        }
    }
}

// =====================================================================
//  SCHULTERBOCK  (Schulter-Koordinaten: Achse im Ursprung, x vorne, y seitlich, z hoch)
// =====================================================================
module seitenwand2d() hull() { circle(r=16); translate([0,P_dz]) circle(r=10); translate([-30,-H_s]) square([60, 1]); }
schnecke_lager_x = [[-23,-18],[18,23]];

module schulterbock() {
    difference() {
        union() {
            intersection() { translate([-72, -40, -H_s]) cube([142, 92, 6]); translate([0,0,-H_s-1]) cylinder(r=85, h=10, $fn=180); }   // Grundplatte
            for (s=[-1,1]) XV(s<0 ? -yp : yi, s<0 ? -yi : yp) seitenwand2d();       // Seitenwände
            // Halter für das Potentiometer (links außen)
            for (dx=[-14, 10]) translate([dx, -36, -10]) cube([4, 14, 20]);
            translate([-14, -36, -10]) cube([28, 3, 20]);
            // Lagerböcke der Schnecke
            for (b=schnecke_lager_x) translate([b[0], s_y-6, -H_s]) cube([b[1]-b[0], 12, H_s - s_a + 6]);
            // Motorhalter (Klemmring mit Abflachungen)
            translate([mot_x1, s_y-12, -H_s]) cube([22, 24, H_s + mot_z - mot_d/2 + 2]);
            translate([mot_x1, s_y, mot_z]) rotate([0,90,0]) difference() {
                cylinder(d=mot_d+5, h=22);
                translate([0,0,-1]) intersection() { cylinder(d=mot_d+0.3, h=24); cube([mot_fl+0.3, 30, 60], center=true); }
                translate([-mot_d, -1, -1]) cube([mot_d/2, 2, 30]);                   // Schlitz zum Klemmen
            }
        }
        // Lagersitze + Lippe
        ZY(sitz_D, -yp-0.01, -yp+lager_b);  ZY(sitz_D, yp-lager_b, yp+0.01);
        ZY(lippe_D, -yp-1, yp+1);
        // Achse oberer Drehpunkt
        ZY(stift_d+0.1, -yp-1, yp+1, 0, P_dz);
        // Potentiometer-Buchse
        ZY(pot_buchse, -40, -30);
        // Schneckenwelle
        ZX(5.3, -30, 30, s_y, -s_a);
        // Platz für Kappe der Mittelsäule + Kabel
        translate([0,0,-H_s-1]) cylinder(d=44, h=8);
        // Stecklöcher Getriebedeckel
        for (p=deckel_stifte) translate([p[0], p[1], -H_s+3]) cylinder(d=3.1, h=10);
    }
    // Stifte nach unten in den Drehteller
    for (p=bock_stifte) translate([p[0], p[1], -H_s-5.5]) cylinder(d=5, h=5.6);
}
deckel_stifte = [[-28, 46],[-28, 27],[68, 46]];   // liegen in den Domen des Getriebedeckels
deckel_anker  = [[-28, 50],[-32, 27],[68, 50]];   // Dome hängen an diesen Stellen an der Deckelwand

// Getriebedeckel: deckt Schneckenrad, Schnecke, Stirnräder und Motor ab (rechts außen)
module getriebedeckel() difference() {
    union() {
        difference() {
            hull() { translate([-34, yp, -H_s+6]) cube([106, 30, 1]); translate([0, yp, 0]) rotate([-90,0,0]) cylinder(r=37, h=30); translate([60, yp, -14]) rotate([-90,0,0]) cylinder(r=16, h=30); }
            hull() { translate([-32, yp-1, -H_s+5]) cube([102, 29, 1]); translate([0, yp-1, 0]) rotate([-90,0,0]) cylinder(r=35, h=29); translate([60, yp-1, -14]) rotate([-90,0,0]) cylinder(r=14, h=29); }
        }
        // Steckdome: sitzen auf der Grundplatte, Deckelstifte halten den Deckel
        for (i=[0:2]) hull() { translate([deckel_stifte[i][0], deckel_stifte[i][1], -H_s+6]) cylinder(d=7, h=10);
                               translate([deckel_anker[i][0], deckel_anker[i][1], -H_s+6]) cylinder(d=4, h=10); }
    }
    ZY(16, yp-2, yp+40);                          // Achsende sichtbar
    for (p=deckel_stifte) translate([p[0], p[1], -H_s]) cylinder(d=3.4, h=15);
    translate([66, s_y, mot_z]) rotate([0,90,0]) cylinder(d=8, h=10);   // Luft am Motor
}
module deckelstift() cylinder(d=3, h=10);

// Schulterachse: Ø12 rund in den Lagern, Sechskant 10.2 für Hauptlenker und Schneckenrad
achse_teile = [[-23, 1, "k"], [-22, 12, "r"], [-10, 8, "s"], [-2, 26, "r"], [24, 8, "s"], [32, 2, "r"]];
module achse() difference() {
    union() for (t=achse_teile) translate([0, t[0]+t[1], 0]) rotate([90,0,0])
        if (t[2]=="s") linear_extrude(t[1]) sechskant2d(10.2);
        else if (t[2]=="k") cylinder(d=16, h=t[1]);
        else cylinder(d=lager_d-0.05, h=t[1]);
    ZY(pot_welle, -24, -23+pot_tief);                          // Potentiometerwelle (Presssitz)
    translate([0, 32.2, 0]) rotate([-90,0,0]) rohr(4.5, 7, 1.0);         // Nut für den großen Clip
}
module achse_druck() translate([0,0,lager_d/2-0.5]) intersection() { rotate([90,0,0]) translate([0,0,0]) achse_liegend(); translate([-50,-50,0]) cube([100,100,100]); }
module achse_liegend() rotate([-90,0,0]) achse();
module distanzrohr(l) rohr((lager_d+0.3)/2, 7, l);

module schneckenrad() difference() {
    union() { zahnrad(w_m, w_z, w_h, twist=-w_h*tan(atan(s_lead/(PI*s_d1)))/(w_m*w_z/2)*180/PI, spiel=0.3); cylinder(d=24, h=w_h); }
    translate([0,0,-1]) linear_extrude(w_h+2) sechskant2d(10.45);
    for (k=[0:5]) rotate(30+k*60) translate([20,0,-1]) cylinder(r=6, h=w_h+2);
}

// Schnecke (eingängig, rechtssteigend) mit Welle und Stirnrad 50 Z (Achse z)
schnecke_aufl = 0.5;    // Auflösung der Schnecke (0.5 reicht für den Druck, Vorschau 0.25)
module schnecke2d() {
    p = s_lead; r1 = s_d1/2; ra = r1 + w_m; rf = r1 - 1.25*w_m;
    n = round(180*schnecke_aufl);
    polygon([for (i=[0:n-1]) let(th = i*360/n, s = p*th/360, d = abs(((s + p/2) % p) - p/2),
                                 r = max(rf, min(ra, r1 + (p/4 - d)/tan(20)))) [r*cos(th), r*sin(th)]]);
}
module schnecke() {
    lw = 30;
    union() {
        linear_extrude(lw, twist=-360*lw/s_lead, slices=round(lw/s_lead*36*schnecke_aufl)) schnecke2d();
        translate([0,0,-2.7]) cylinder(d=8, h=lw+5.4);                           // Bund: liegt an beiden Lagerböcken an
        translate([0,0,-11]) cylinder(d=5, h=lw+11+20.5);                     // Welle x -26..35.5 (durch das Stirnrad)
        translate([0,0,lw+16]) zahnrad(st_m, st_z2, 4);                       // Stirnrad x 31..35
    }
}
// Lage: Schnecke z -> x, Mitte x 0
module schnecke_lage() translate([-15, s_y, -s_a]) rotate([0,90,0]) rotate([0,0,0]) schnecke();

module motorritzel() difference() {
    union() { zahnrad(st_m, st_z1, 5); cylinder(d=5, h=7); }
    translate([0,0,-1]) cylinder(d=mot_welle-0.05, h=10);
}

// =====================================================================
//  ARM
// =====================================================================
module hauptlenker() difference() {
    union() {
        linear_extrude(hl_y2-hl_y1) union() {
            stab2d([0,0], [L,0], 13, 8);
            stab2d([0,0], [-muenz_r,0], 13, 7);
        }
        // Münzmagazin: Rohr quer zur Lenkerebene
        translate([-muenz_r, 0, (hl_y2-hl_y1)/2 - 18 - hl_y1 - 6 + 0]) cylinder(d=muenz_d+3.2, h=40);   // y -20..20: Platz für 15 Münzen + Deckel
    }
    translate([0,0,-1]) linear_extrude(20) sechskant2d(10.45);
    translate([L,0,-1]) cylinder(d=stift_d+0.3, h=20);
    translate([-muenz_r, 0, (hl_y2-hl_y1)/2 - 18 - hl_y1 - 6 + 1.2]) cylinder(d=muenz_d, h=44);
}
// Lage des Hauptlenkers: lokale z (Dicke) -> y, lokale y -> z
module muenzdeckel() difference() {
    union() { cylinder(d=muenz_d+3.2, h=1.2); cylinder(d=muenz_d-0.2, h=3); }
    translate([0,0,1.2]) cylinder(d=muenz_d-2.6, h=3);
}
module parallellenker() difference() {
    linear_extrude(pl_y2-pl_y1) stab2d([0,0], [L,0], 6, 6);
    for (x=[0,L]) translate([x,0,-1]) cylinder(d=stift_d+0.3, h=20);
}

// Handgelenk (Koordinaten am unteren Gelenkpunkt A, gleiche Ausrichtung wie der Schulterbock)
module hg_platte2d() hull() { circle(r=8); translate([0,P_dz]) circle(r=8); translate([-8,-85]) square([30, 1]); }
module handgelenk() difference() {
    union() {
        for (y=hg_y) XV(y[0], y[1]) hg_platte2d();
        translate([hg_flansch_x-4, hg_y[0][0], -85]) cube([4, hg_y[1][1]-hg_y[0][0], P_dz+8+85]);     // Vorderwand
        translate([hg_flansch_x-4, -26, -85]) cube([4, 52, 36]);                                    // Flanschplatte
        for (s=[-1,1]) translate([hg_flansch_x, s>0 ? 22.2 : -26, -85]) cube([8, 3.8, 36]);       // Schienen
        for (s=[-1,1]) translate([hg_flansch_x+4.4, s>0 ? 20 : -26, -85]) cube([3.6, 6, 36]);     // Lippen
        translate([hg_flansch_x, -26, -51.8]) cube([5.5, 52, 2.8]);                                // Anschlag oben (Platz für die SG90-Ohren)
    }
    for (z=[0, P_dz]) ZY(stift_d+0.1, -30, 30, 0, z);
    ZY(3.1, -30, 30, hg_flansch_x+2, -83.6);                                                        // Sperrstift
}
module sperrstift() union() { cylinder(d=3, h=52); cylinder(d=5, h=1.2); }

// Steckachse Ø5 mit Kopf und Clipnut, Clip
module steckachse(l) difference() {
    union() { cylinder(d=stift_d-0.05, h=l); translate([0,0,-1.6]) cylinder(d=9, h=1.6); }
    translate([0,0,l-2.0]) rohr(2.0, 3, 0.8);
}
module clip() difference() {
    cylinder(d=8.5, h=0.8);
    translate([0,0,-1]) cylinder(d=4.0, h=3);
    translate([0,-1.6,-1]) cube([10, 3.2, 3]);
}
// großer Clip für die Schulterachse (Nut Ø9)
module clip_gross() difference() {
    cylinder(d=17, h=0.9);
    translate([0,0,-1]) cylinder(d=8.8, h=3);
    translate([0,-3.4,-1]) cube([10, 6.8, 3]);
}
// Hülse auf den Steckachsen (hält Lenker in ihrer Ebene)
module huelse(l) rohr((stift_d+0.35)/2, 4.5, l);
achse_P_l = 2*yp + 3.2;   achse_A_l = (hg_y[1][1]-hg_y[0][0]) + 3.2;

// =====================================================================
//  GREIFER (SG90, Version 1, schraubenlos)
// =====================================================================
module greifer_gehaeuse() {
    difference() {
        union() {
            translate([-28, -20, -g_platte]) cube([42, 40, g_platte]);
            translate([-28, -22, -g_platte]) cube([4, 44, 28]);                     // Flansch (gleitet in die Schienen)
            for (y=[-19,15]) translate([-24, y, 0]) rotate([90,0,0]) translate([0,0,-4]) linear_extrude(4) polygon([[0,0],[10,0],[0,20]]);
            translate([0, g_ab, 0]) cylinder(d=9, h=g_z0-0.2);                     // Zapfen
            translate([0, g_ab, 0]) cylinder(d=stift_d-0.1, h=g_z0+g_zr_h+1.6);     // Achse Mitläufer
        }
        translate([sg_x1, -g_ab-sg_b/2, -10]) cube([sg_x2-sg_x1, sg_b, 20]);
        for (x=[(sg_x1+sg_x2)/2-13.8, (sg_x1+sg_x2)/2+13.8]) translate([x, -g_ab, -10]) cylinder(d=1.8, h=20);   // SG90-Schrauben aus dem Kit
        translate([0, g_ab, g_z0+g_zr_h+0.2]) rohr(2.0, 3, 0.8);                   // Clipnut
    }
}
module greiferhebel(antrieb=true) {
    difference() {
        union() {
            rotate(antrieb ? 0 : 9) zahnrad(g_m, g_z, g_zr_h);
            hull() { cylinder(r=5.5, h=g_zr_h); translate([finger_x-3, -10, 0]) cube([3, 19, g_zr_h]); }
            translate([finger_x-3, -10, 0]) cube([3, 19, finger_h+6]);
            translate([finger_x-3, -10, 0]) cube([11, 19, 3]);
            for (y=[-5,4]) translate([finger_x+4, y, 0]) cylinder(d=3.4, h=3+finger_h+3);   // Stifte für Finger + Klemmplatte
        }
        if (antrieb) {
            translate([0,0,-1]) cylinder(d=5.5, h=20);
            translate([0,0,-0.01]) linear_extrude(1.8) hull() { circle(d=7.6); translate([15.5,0]) circle(d=4.8); }   // SG90-Horn
        } else translate([0,0,-1]) cylinder(d=stift_d+0.3, h=20);
    }
}
module greiferhebel_mitlaeufer() mirror([0,1,0]) greiferhebel(false);
module klemmplatte() difference() {
    translate([0,-10,0]) cube([8, 19, 3]);
    for (y=[-5,4]) translate([4, y, -1]) cylinder(d=3.35, h=5);                    // Presssitz auf die Stifte
}
module finger2d() {
    basis = 8;
    tri = [[basis,-finger_b/2],[finger_l,finger_b/2-0.01],[basis,finger_b/2]];
    difference() {
        union() {
            translate([0,-finger_b/2]) square([basis, finger_b]);
            difference() { offset(r=0.6) offset(delta=-0.6) polygon(tri); offset(delta=-wand) polygon(tri); }
            intersection() { polygon(tri); for (x=[basis+rippen_abst : rippen_abst : finger_l-6]) translate([x-rippe/2, -finger_b]) square([rippe, 2*finger_b]); }
            for (x=[basis+4 : 5 : finger_l-6]) translate([x, finger_b/2]) circle(d=1.6, $fn=12);
        }
        for (y=[-5,4]) translate([4,y]) circle(d=3.8);
    }
}
module finger() linear_extrude(finger_h) finger2d();

// =====================================================================
//  Kaufteile aus dem Kit (nur Vorschau)
// =====================================================================
module dummy_motor() { color("Silver") rotate([0,90,0]) intersection() { cylinder(d=mot_d, h=mot_l); cube([mot_fl, 30, 60], center=true); }
    color("Gold") translate([-12,0,0]) rotate([0,90,0]) cylinder(d=mot_welle, h=12); }
module dummy_poti() color("DodgerBlue") { ZY(16.5, -48, -41); ZY(7, -41, -30); color("Silver") ZY(6, -30, -12); }
module dummy_sg90() color("RoyalBlue") { translate([sg_x1+0.3, -g_ab-6, -g_platte-15.9]) cube([22.5, 12, 22.7]); translate([sg_x1-4.5, -g_ab-6, -g_platte-2.5]) cube([32.2, 12, 2.5]); }
module dummy_stepper() color("Silver") { translate([sm_gx,0,sm_ohr_z-19]) cylinder(d=28, h=19); color("DodgerBlue") translate([sm_gx-14-3,-8,sm_ohr_z-19]) cube([4,16,17]); }
module dummy_muenzen(n=15) color("Gold") for (i=[0:n-1]) translate([0,0,i*2.33]) cylinder(d=23.25, h=2.3);

// =====================================================================
//  ZUSAMMENBAU
// =====================================================================
phi = 0;  basisw = 0;  greif = 15;
// Teil in XY gezeichnet (Dicke z) -> Armebene: lokal z -> +y ab y0
module ebene(y0) translate([0,y0,0]) multmatrix([[1,0,0,0],[0,0,1,0],[0,1,0,0],[0,0,0,1]]) children();

module greifer_komplett() {
    color("SteelBlue") greifer_gehaeuse();
    dummy_sg90();
    color("Orange") translate([0,-g_ab,g_z0]) rotate(-greif) { greiferhebel(true); color("DimGray") translate([finger_x,0,3]) finger(); translate([finger_x,0,3+finger_h]) klemmplatte(); }
    color("Orange") translate([0, g_ab,g_z0]) rotate(greif) { greiferhebel_mitlaeufer(); color("DimGray") translate([finger_x,0,3+finger_h]) mirror([0,0,1]) mirror([0,1,0]) finger(); }
}

module arm_komplett(phi=0) {
    A = [L*cos(phi), L*sin(phi)];
    color("Tomato") ebene(hl_y1) rotate([0,0,phi]) hauptlenker();
    color("Gold") translate([-muenz_r*cos(phi), -18.8, -muenz_r*sin(phi)]) rotate([-90,0,0]) dummy_muenzen();
    color("Khaki") translate([0,0,P_dz]) ebene(pl_y1) rotate([0,0,phi]) parallellenker();
    translate([A[0], 0, A[1]]) {
        color("SlateBlue") handgelenk();
        translate([hg_flansch_x+28, 0, hg_z0]) rotate([180,0,0]) greifer_komplett();
    }
}

module zusammenbau() {
    color("LightGray") basis();
    color("DimGray") translate([0,0,basis_h]) deckelring();
    color("Silver") translate([sm_wx,0,kranz_unten+0.5]) ritzel_basis();
    dummy_stepper();
    rotate(basisw) {
        color("WhiteSmoke") translate([0,0,tt_unten]) drehteller();
        color("DimGray") translate([0,0,tt_oben+0.2]) kappe();
        translate([0,0,S_z]) {
            color("SlateGray") schulterbock();
            color("Gold") rotate([0,-phi,0]) achse();
            color("Silver") translate([0, s_y-w_h/2, 0]) rotate([-90,0,0]) rotate(-phi) schneckenrad();
            color("Silver") schnecke_lage();
            color("Silver") translate([st_x-1, s_y, mot_z]) rotate([0,90,0]) motorritzel();
            translate([mot_x1, s_y, mot_z]) dummy_motor();
            dummy_poti();
            arm_komplett(phi);
            if (teil == "zusammenbau") color("SlateGray", 0.35) getriebedeckel();
        }
    }
}

// =====================================================================
if (teil == "zusammenbau")        zusammenbau();
if (teil == "offen")              zusammenbau();
if (teil == "basis")              basis();
if (teil == "deckelring")         deckelring();
if (teil == "rollenachse")        translate([0,0,lager_d/2-0.5]) rollenachse();
if (teil == "laufrolle")          laufrolle();
if (teil == "lagerbuchse")        lagerbuchse();
if (teil == "drehteller")         translate([0,0,tt_t]) rotate([180,0,0]) drehteller();
if (teil == "ritzel_basis")       ritzel_basis();
if (teil == "kappe")              translate([0,0,kappe_h]) rotate([180,0,0]) kappe();
if (teil == "schulterbock")       translate([0,0,H_s+5.5]) schulterbock();
if (teil == "getriebedeckel")     rotate([-90,0,0]) translate([0,-yp-30,0]) getriebedeckel();   // geschlossene Seite liegt auf dem Druckbett
if (teil == "deckelstift")        deckelstift();
if (teil == "achse")              translate([0,0,lager_d/2-0.5]) intersection() { rotate([90,0,0]) achse(); translate([-60,-60,-lager_d/2+0.5]) cube([120,120,60]); }
if (teil == "distanzrohr_7")      distanzrohr(7);
if (teil == "distanzrohr_19")     distanzrohr(19);
if (teil == "distanzrohr_2")      distanzrohr(2);
if (teil == "schneckenrad")       schneckenrad();
if (teil == "schnecke")           translate([0,0,11]) schnecke();
if (teil == "motorritzel")        motorritzel();
if (teil == "hauptlenker")        hauptlenker();
if (teil == "muenzdeckel")        muenzdeckel();
if (teil == "parallellenker")     parallellenker();
if (teil == "handgelenk")         translate([0,0,15]) rotate([90,0,0]) handgelenk();
if (teil == "sperrstift")         sperrstift();
if (teil == "steckachse_lang")    translate([0,0,1.6]) steckachse(achse_P_l);
if (teil == "steckachse_kurz")    translate([0,0,1.6]) steckachse(achse_A_l);
if (teil == "clip")               clip();
if (teil == "clip_gross")         clip_gross();
if (teil == "huelse_1")           huelse(1);
if (teil == "huelse_8")           huelse(8);
if (teil == "huelse_11")          huelse(11);
if (teil == "huelse_13")          huelse(13);
if (teil == "huelse_18")          huelse(18);
if (teil == "greifer_gehaeuse")   translate([0,0,g_platte]) greifer_gehaeuse();
if (teil == "hebel_antrieb")      greiferhebel(true);
if (teil == "hebel_mitlaeufer")   greiferhebel_mitlaeufer();
if (teil == "klemmplatte")        klemmplatte();
if (teil == "finger_tpu")         finger();
if (teil == "finger_tpu_2x")      for (y=[0, 28]) translate([0, y, 0]) finger();   // beide Finger in einem Druck
