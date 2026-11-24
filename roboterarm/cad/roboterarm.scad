// =====================================================================
//  Roboterarm "AM-R1" – komplett 3D-gedruckt
//  Fin-Ray-Greifer (TPU), Basis 1:5 mit 28BYJ-48, Schulter/Ellbogen MG996R,
//  Greifer SG90, Drehteller auf 4 Kugellagern 6801-2RS (12x21x5)
//
//  Bedienung:  in OpenSCAD die Variable  teil  setzen und mit F6 rendern,
//              danach Datei > Export > STL.  Oder per Kommandozeile:
//              openscad -D 'teil="basis"' -o basis.stl roboterarm.scad
//  Alle Maße in mm.  Werte mit "MESSEN" bitte an deinen Teilen nachmessen.
// =====================================================================

teil = "zusammenbau";   // siehe Liste ganz unten

$fn = 64;
sp  = 0.2;              // Spiel für Passungen (Bambu P2S: 0.2 passt meist)

// ---------- Kugellager 6801-2RS ----------
lager_D = 21.0;         // außen   (gemessen 21.05)
lager_d = 12.0;         // innen   (gemessen 12.04)
lager_b = 5.0;          // breit   (gemessen 4.98)

// ---------- Basis ----------
basis_r   = 90;         // Ø 180
boden_t   = 3;
basis_h   = 55;         // Wandhöhe
innen_r1  = 66;         // Innenwand (Sandkammer innen)
innen_r2  = 70;
aussen_t  = 2.4;
deckel_t  = 4;          // Deckelring auf der Sandkammer
rolle_r   = 78;         // Radius, auf dem die Lager laufen
rolle_anz = 4;
achse_h   = lager_D/2 + 2;                     // Achsmitte über Deckelring
deckel_oben = basis_h + deckel_t;              // 59
tt_unten  = deckel_oben + achse_h + lager_D/2; // Unterseite Drehteller (82)
tt_t      = 8;
tt_oben   = tt_unten + tt_t;                   // 90
tt_r      = 86;
saeule_r  = 15;         // Mittelsäule (Kabel innen)
kabel_r   = 8;

// ---------- Zahnräder Basis 1:5 ----------
zr_m   = 1.25;
zr_z1  = 15;            // Ritzel am Schrittmotor
zr_z2  = 75;            // Zahnkranz am Drehteller
zr_a   = zr_m*(zr_z1+zr_z2)/2 + 0.15;          // Achsabstand + Spiel
kranz_ri = 40;          // Innenradius Zahnkranz-Ring
kranz_unten = tt_unten - 8;                    // Zahnkranz 8 mm hoch

// ---------- Schrittmotor 28BYJ-48 ----------
sm_welle_l = 10;        // Ohr-Unterseite bis Wellenende   MESSEN
sm_versatz = 8;         // Welle zu Gehäusemitte
sm_ohr     = 17.5;      // Ohrlöcher +-17.5 (35 mm)
ritzel_oben = tt_unten - 0.5;
sm_ohr_z   = ritzel_oben - sm_welle_l;         // Oberkante Säulen

// ---------- Lichtschranke (Referenzpunkt) ----------
lsk_z   = tt_unten - 5; // Höhe des Lichtstrahls
rock_r1 = 23; rock_r2 = 25;                    // Halbkreis-Blende am Drehteller

// ---------- Türme / Schulter ----------
schulter_h = 65;        // Schulterachse über Drehteller
turm_t     = 6;
horn_abst  = 18;        // Ohr-Auflage bis Horn-Fläche MG996R   MESSEN
ebene      = 16.5;      // Horn-Flächen bei +-16.5
turm_i     = ebene + horn_abst - turm_t;       // Innenfläche Turm (28.5)
turm_a     = turm_i + turm_t;                  // Außenfläche (34.5)
turm_x1 = -42; turm_x2 = 22;
mg_l = 40.7; mg_b = 19.7; mg_w = 10.2;         // MG996R: Länge, Breite, Welle ab Vorderkante
mg_loch_a = 49.5; mg_loch_b = 10;              // Lochbild Ohren   MESSEN
horn_d = 25;            // Vertiefung für runde Servoscheibe

// ---------- Arm ----------
L1 = 120;               // Schulter -> Ellbogen
L2 = 80;                // Ellbogen -> Greiferflansch
kurbel = 35;            // Kurbel / Hebel des Parallelogramms
arm_t = 8; stange_t = 6;

// ---------- Greifer ----------
g_ab   = 10.1;          // Zahnradachsen bei y = +-g_ab
g_m    = 1; g_z = 20;
g_zr_h = 5;
g_z0   = 10;            // Zahnrad-Unterseite über Greiferplatte   MESSEN
g_platte = 4;
finger_l = 62; finger_h = 20; finger_b = 20;
finger_x = 16;          // Fingerfuß ab Zahnradachse
wand = 1.2; rippe = 1.0; rippen_abst = 7;

// =====================================================================
//  Hilfsfunktionen
// =====================================================================
function rot2(p, a) = [p[0]*cos(a)-p[1]*sin(a), p[0]*sin(a)+p[1]*cos(a)];
function evolv(rb, t) = let(d = t*180/PI)
    [rb*(cos(d)+t*sin(d)), rb*(sin(d)-t*cos(d))];

module zahnrad2d(m, z, spiel=0.08) {
    rp = m*z/2; ra = rp + m; rf = rp - 1.25*m; rb = rp*cos(20);
    tmax = sqrt(pow(ra/rb,2) - 1);
    inv20 = (tan(20) - 20*PI/180)*180/PI;
    halb = 90/z + inv20 - (spiel/rp)*180/PI/2;
    fl = [for (i=[0:12]) rot2(evolv(rb, tmax*i/12), -halb)];
    zahn = concat([[0,0]], fl, [for (i=[12:-1:0]) [fl[i][0], -fl[i][1]]]);
    union() {
        circle(r=rf, $fn=z*4);
        for (k=[0:z-1]) rotate(k*360/z) polygon(zahn);
    }
}
module zahnrad(m, z, h) linear_extrude(h) zahnrad2d(m, z);

module rohr(ri, ra, h) difference() { cylinder(r=ra, h=h, $fn=180); translate([0,0,-1]) cylinder(r=ri, h=h+2, $fn=180); }
module tropfen(d, l) {          // liegende Bohrung entlang x, Spitze nach oben (druckbar ohne Stütze)
    rotate([0,90,0]) linear_extrude(l, center=true)
        hull() { circle(d=d); translate([-d/2*1.4142, 0]) circle(d=0.01); }
}
module mutter(h=2.6) cylinder(d=6.4, h=h, $fn=6);
module stab2d(a, b, r1, r2) hull() { translate(a) circle(r=r1); translate(b) circle(r=r2); }

// =====================================================================
//  BASIS
// =====================================================================
sm_wx = zr_a;                     // Welle Schrittmotor auf +x
sm_gx = zr_a - sm_versatz;        // Gehäusemitte (nach innen)

module basis() {
    difference() {
        union() {
            cylinder(r=basis_r, h=boden_t, $fn=180);
            rohr(basis_r-aussen_t, basis_r, basis_h);
            rohr(innen_r1, innen_r2, basis_h);
            cylinder(r=saeule_r, h=tt_oben+0.2);
            // Kabelkanal durch die Sandkammer (hinten links, 270°)
            rotate(270) translate([innen_r1, -9, 0]) cube([basis_r-innen_r1-0.5, 18, 17]);
            // Säulen für den Schrittmotor + Stege zur Innenwand
            for (s=[-1,1]) {
                translate([sm_gx-4, s*sm_ohr-3, 0]) cube([8, 6, sm_ohr_z]);
                translate([sm_gx, s*sm_ohr-1.5, 0]) cube([innen_r1-sm_gx+1, 3, basis_h]);
            }
            translate([sm_gx-4, -sm_ohr, 0]) cube([8, 2*sm_ohr, 8]);
            // Lichtschranke: LED-Block an der Säule, Fotowiderstand-Säule (bei 180°)
            translate([-(rock_r1-2), -5, 0]) cube([rock_r1-2-saeule_r+2, 10, lsk_z+5]);
            translate([-(rock_r2+10), -5, 0]) cube([8, 10, lsk_z+5]);
            translate([-innen_r1-1, -1.5, 0]) cube([innen_r1+1-rock_r2-9, 3, basis_h]);
        }
        translate([0,0,-1]) cylinder(r=kabel_r, h=200);
        rotate(270) translate([0, -6, boden_t]) cube([saeule_r+2, 12, 18]);          // Kabelausgang Säule
        rotate(270) translate([innen_r1-6, -7, boden_t]) cube([40, 14, 12]);           // Kanal innen
        for (k=[0:5]) rotate(30+k*60) translate([(innen_r1+innen_r2)/2, 0, basis_h-12]) cylinder(d=2.6, h=13); // Deckelring
        for (k=[0:2]) rotate(k*120+60) translate([11.5, 0, tt_oben-10]) cylinder(d=2.6, h=11);               // Kappe
        for (s=[-1,1]) translate([sm_gx, s*sm_ohr, sm_ohr_z-9]) cylinder(d=2.6, h=10);                      // Motor
        translate([-(rock_r2+12), 0, lsk_z]) rotate([0,90,0]) cylinder(d=5.2, h=12);                          // Fotowiderstand
        translate([-(rock_r1), 0, lsk_z]) rotate([0,90,0]) cylinder(d=5.1, h=rock_r1-kabel_r+1);              // LED (von innen)
    }
}

module deckelring() {
    difference() {
        union() {
            rohr(innen_r1, basis_r, deckel_t);
            for (k=[0:rolle_anz-1]) rotate(45+k*360/rolle_anz)
                for (s=[-1,1]) translate([rolle_r + s*(lager_b/2+0.3+2), 0, 0])
                    translate([-2, -8, 0]) cube([4, 16, deckel_t+achse_h+lager_d/2+3]);
        }
        for (k=[0:5]) rotate(30+k*60) translate([(innen_r1+innen_r2)/2, 0, -1]) {
            cylinder(d=3.3, h=10); translate([0,0,deckel_t-1.8]) cylinder(d=6.2, h=3); }
        for (k=[0:rolle_anz-1]) rotate(45+k*360/rolle_anz)
            translate([rolle_r, 0, deckel_t+achse_h]) rotate([0,0,0]) tropfen(lager_d+0.15, 30);
    }
}

module rollenachse() {          // flach drucken (Abflachung unten)
    l = lager_b + 0.6 + 8;
    difference() {
        rotate([0,90,0]) cylinder(d=lager_d-0.05, h=l, center=true);
        translate([-l, -lager_d, -lager_d/2-1]) cube([2*l, 2*lager_d, 1.5]);
    }
}

module drehteller() {           // Unterseite nach OBEN drucken
    difference() {
        union() {
            cylinder(r=tt_r, h=tt_t, $fn=180);
            // Zahnkranz (Ring) und Halbkreis-Blende hängen nach unten
            translate([0,0,-(tt_unten-kranz_unten)]) difference() {
                zahnrad(zr_m, zr_z2, tt_unten-kranz_unten+0.01);
                translate([0,0,-1]) cylinder(r=kranz_ri, h=20, $fn=120);
            }
            translate([0,0,-(tt_unten-(lsk_z-5))]) intersection() {
                rohr(rock_r1, rock_r2, tt_unten-(lsk_z-5)+0.01);
                translate([-50,-50,-1]) cube([100,50,40]);         // lokale Winkel 180..360
            }
        }
        translate([0,0,-20]) cylinder(r=saeule_r+0.4, h=40);
        // Turmschrauben mit Mutterfallen unten
        for (s=[-1,1]) for (x=[-32, 14]) translate([x, s*50, -1]) {
            cylinder(d=3.3, h=20); cylinder(d=6.4, h=3.6, $fn=6); }
        // Pfeil vorne (Referenz / Ausrichtung)
        translate([tt_r-14, 0, tt_t-0.6]) linear_extrude(1) polygon([[0,-5],[10,0],[0,5]]);
    }
}

module ritzel() {
    difference() {
        zahnrad(zr_m, zr_z1, ritzel_oben-(kranz_unten+0.5));
        translate([0,0,-1]) intersection() { cylinder(d=5.25, h=20); cube([3.15, 10, 40], center=true); }
    }
}

module kappe() {
    difference() {
        cylinder(d=40, h=4);
        translate([0,0,-1]) cylinder(r=kabel_r, h=6);
        for (k=[0:2]) rotate(k*120+60) translate([11.5,0,-1]) { cylinder(d=3.3,h=6); translate([0,0,3]) cylinder(d1=3.3, d2=6.5, h=2.01); }
    }
}

// =====================================================================
//  TÜRME (Koordinaten wie im Zusammenbau, Drehteller-Oberseite = z 0)
// =====================================================================
mg_x1 = -(mg_l - mg_w) - 0.5;  mg_x2 = mg_w + 0.5;
mg_mitte = (-(mg_l - mg_w) + mg_w)/2;

module turm_rechts() {
    difference() {
        union() {
            translate([turm_x1, turm_i, 0]) cube([turm_x2-turm_x1, turm_t, schulter_h]);
            translate([turm_x1, turm_i, schulter_h]) rotate([-90,0,0])
                hull() { translate([20,0,0]) cylinder(r=20, h=turm_t); translate([turm_x2-turm_x1-20,0,0]) cylinder(r=20, h=turm_t); }
            translate([turm_x1, turm_i, 0]) cube([turm_x2-turm_x1, 56-turm_i, turm_t]);       // Fuß
            for (x=[turm_x1, turm_x2-5]) translate([x, turm_a, 0])
                hull() { cube([5, 0.1, 40]); cube([5, 56-turm_a, turm_t]); }                 // Rippen
        }
        translate([mg_x1, turm_i-1, schulter_h-mg_b/2-0.4]) cube([mg_x2-mg_x1, turm_t+2, mg_b+0.8]);
        for (dx=[-1,1]) for (dz=[-1,1])
            translate([mg_mitte+dx*mg_loch_a/2, turm_i-1, schulter_h+dz*mg_loch_b/2]) rotate([-90,0,0]) cylinder(d=2.6, h=turm_t+2);
        for (x=[-32,14]) translate([x, 50, -1]) { cylinder(d=3.3, h=20); translate([0,0,turm_t-2]) cylinder(d=6.5, h=10); }
    }
}
module turm_links() mirror([0,1,0]) turm_rechts();

// =====================================================================
//  ARM  (2D-Profile in der Armebene, x = Armrichtung)
// =====================================================================
module horn_seite() {            // Servoscheibe: Vertiefung + Mittelloch
    translate([0,0,-1]) cylinder(d=9, h=20);
    translate([0,0,-0.01]) cylinder(d=horn_d+0.4, h=2);
}

module oberarm() {               // Horn-Seite liegt unten (z=0) -> zeigt zum linken Servo
    difference() {
        linear_extrude(arm_t) union() {
            stab2d([0,0], [L1,0], 14, 10);
            stab2d([0,0], [-35,0], 14, 8);
        }
        horn_seite();
        translate([L1,0,-1]) cylinder(d=3.3, h=20);
        translate([-30,0,-1]) cylinder(d=5, h=20);     // Gegengewicht / Feder
    }
}

module kurbel_teil() {            // Horn-Seite unten -> zeigt zum rechten Servo
    difference() {
        linear_extrude(arm_t) stab2d([0,0], [kurbel,0], 14, 7);
        horn_seite();
        translate([kurbel,0,-1]) cylinder(d=3.3, h=20);
    }
}

module koppelstange() {
    difference() {
        linear_extrude(stange_t) stab2d([0,0], [L1,0], 6.5, 6.5);
        for (x=[0,L1]) translate([x,0,-1]) cylinder(d=3.3, h=20);
    }
}

module unterarm() {              // Lagerseite unten (z=0) -> zeigt zum Oberarm
    difference() {
        union() {
            linear_extrude(arm_t) union() {
                stab2d([0,0], [L2-8,0], 15, 9);
                stab2d([0,0], [-kurbel,0], 15, 7);
                translate([L2-8, -14]) square([8, 28]);      // Flansch für den Greifer
            }
        }
        translate([0,0,-0.01]) cylinder(d=lager_D-0.05, h=lager_b);  // Lagersitz (Presspassung)
        translate([0,0,-1]) cylinder(d=16, h=20);
        translate([-kurbel,0,-1]) cylinder(d=3.3, h=20);
        for (v=[-7,7]) translate([L2-10, v, arm_t/2]) {
            rotate([0,90,0]) cylinder(d=3.3, h=20);
            translate([4.5, -3.2, 0]) cube([2.6, 6.4, 10]);        // Mutterschlitz von oben
        }
    }
}

module lagerhuelse() {
    difference() {
        union() { cylinder(d=lager_d-0.05, h=lager_b+1); cylinder(d=14, h=1); }
        translate([0,0,-1]) cylinder(d=3.4, h=20);
    }
}

// =====================================================================
//  GREIFER
// =====================================================================
sg_x1 = -16.6-0.3; sg_x2 = 5.9+0.3; sg_b = 12.4;   // SG90 Ausschnitt, Welle bei x=0

module greifer_gehaeuse() {
    difference() {
        union() {
            translate([-28, -20, -g_platte]) cube([42, 40, g_platte]);
            translate([-28, -20, -g_platte]) cube([4, 40, 28]);                     // Flansch
            for (y=[-19,15]) translate([-24, y, 0]) rotate([90,0,0]) translate([0,0,-4])
                linear_extrude(4) polygon([[0,0],[10,0],[0,20]]);                  // Rippen
            translate([0, g_ab, 0]) cylinder(d=9, h=g_z0-0.2);                    // Lagerzapfen Mitläufer
        }
        translate([sg_x1, -g_ab-sg_b/2, -10]) cube([sg_x2-sg_x1, sg_b, 20]);
        for (x=[(sg_x1+sg_x2)/2-13.8, (sg_x1+sg_x2)/2+13.8]) translate([x, -g_ab, -10]) cylinder(d=1.8, h=20);
        translate([0, g_ab, -10]) cylinder(d=3.3, h=40);
        translate([0, g_ab, -g_platte-0.01]) mutter(2.6);
        for (z=[4,18]) translate([-30, 0, z]) rotate([0,90,0]) cylinder(d=3.3, h=10);
    }
}

module greiferhebel(antrieb=true) {
    difference() {
        union() {
            rotate(antrieb ? 0 : 9) zahnrad(g_m, g_z, g_zr_h);
            hull() { cylinder(r=5.5, h=g_zr_h); translate([finger_x-3, -10, 0]) cube([3, 19, g_zr_h]); }
            translate([finger_x-3, -10, 0]) cube([3, 19, finger_h+6]);             // Rückwand
            translate([finger_x-3, -10, 0]) cube([3+8, 19, 3]);                    // Bodenplatte
        }
        if (antrieb) {
            translate([0,0,-1]) cylinder(d=5.5, h=20);
            // Vertiefung für das einarmige SG90-Horn (von unten)
            translate([0,0,-0.01]) linear_extrude(1.8) hull() { circle(d=7.6); translate([15.5,0]) circle(d=4.8); }
        } else translate([0,0,-1]) cylinder(d=3.4, h=20);
        for (y=[-5,4]) translate([finger_x+4, y, -1]) cylinder(d=3.3, h=40);
        for (y=[-5,4]) translate([finger_x+4, y, -0.01]) mutter(2.4);
    }
}
module greiferhebel_mitlaeufer() mirror([0,1,0]) greiferhebel(false);

module klemmplatte() difference() {
    translate([0,-10,0]) cube([8, 19, 3]);
    for (y=[-5,4]) translate([4, y, -1]) cylinder(d=3.3, h=5);
}

// ---------- Fin-Ray-Finger (TPU) ----------
module finger2d() {
    basis = 8;
    tri = [[basis,-finger_b/2],[finger_l,finger_b/2-0.01],[basis,finger_b/2]];
    difference() {
        union() {
            translate([0,-finger_b/2]) square([basis, finger_b]);
            difference() {
                offset(r=0.6) offset(delta=-0.6) polygon(tri);
                offset(delta=-wand) polygon(tri);
            }
            intersection() {
                polygon(tri);
                for (x=[basis+rippen_abst : rippen_abst : finger_l-6]) translate([x-rippe/2, -finger_b]) square([rippe, 2*finger_b]);
            }
        }
        for (y=[-5,4]) translate([4,y]) circle(d=3.4);
    }
}
module finger() linear_extrude(finger_h) finger2d();

// =====================================================================
//  ZUSAMMENBAU (Vorschau, Pose über Winkel)
// =====================================================================
alpha = 60;   // Oberarm über der Waagerechten
beta  = -35;  // Unterarm (absolut)
greif = 20;   // Öffnungswinkel je Hebel
basisw = 0;

module greifer_komplett() {
    color("SteelBlue") greifer_gehaeuse();
    color("Orange") translate([0,-g_ab,g_z0]) rotate(-greif) {
        greiferhebel(true);
        color("Gold") translate([finger_x,0,3]) finger();
        translate([finger_x,0,3+finger_h]) klemmplatte();
    }
    color("Orange") translate([0, g_ab,g_z0]) rotate(greif) {
        greiferhebel_mitlaeufer();
        color("Gold") translate([finger_x,0,3+finger_h]) mirror([0,0,1]) mirror([0,1,0]) finger();
    }
}

// Armebene: lokal x -> Welt x, lokal y -> Welt z, lokal z -> Welt +y (P) bzw. -y (Q)
module P(y) translate([0,y,0]) multmatrix([[1,0,0,0],[0,0,1,0],[0,1,0,0],[0,0,0,1]]) children();
module Q(y) translate([0,y,0]) multmatrix([[1,0,0,0],[0,0,-1,0],[0,1,0,0],[0,0,0,1]]) children();

module zusammenbau() {
    E = [L1*cos(alpha), L1*sin(alpha)];
    K = [kurbel*cos(beta+180), kurbel*sin(beta+180)];
    color("LightGray") basis();
    color("DimGray") translate([0,0,basis_h]) deckelring();
    color("Silver") translate([sm_wx,0,kranz_unten+0.5]) ritzel();
    rotate(basisw) {
        color("WhiteSmoke") translate([0,0,tt_unten]) drehteller();
        color("DimGray") translate([0,0,tt_oben+0.2]) kappe();
        translate([0,0,tt_oben]) {
            color("SlateGray") { turm_rechts(); turm_links(); }
            translate([0,0,schulter_h]) {
                color("Tomato") P(-ebene) rotate(alpha) oberarm();
                color("Tomato") Q(ebene) rotate(beta+180) kurbel_teil();
                color("Khaki")  P(-ebene+arm_t+1+arm_t+1) translate(K) rotate(alpha) koppelstange();
                P(-ebene+arm_t+1) translate(E) rotate(beta) {
                    color("Tomato") unterarm();
                    // Greifer: x_g -> u, z_g -> v, y_g -> -Seite
                    translate([L2+28, -11, arm_t/2])
                        multmatrix([[1,0,0,0],[0,0,1,0],[0,-1,0,0],[0,0,0,1]]) greifer_komplett();
                }
            }
        }
    }
}

// =====================================================================
if (teil == "zusammenbau")       zusammenbau();
if (teil == "basis")             basis();
if (teil == "deckelring")        deckelring();
if (teil == "rollenachse")       translate([0,0,lager_d/2-1.5+1]) rollenachse();
if (teil == "drehteller")        translate([0,0,tt_t]) rotate([180,0,0]) drehteller();
if (teil == "ritzel")            ritzel();
if (teil == "kappe")             kappe();
if (teil == "turm_rechts")       rotate([0,-90,0]) translate([-turm_x1,0,0]) turm_rechts();
if (teil == "turm_links")        rotate([0,-90,0]) translate([-turm_x1,0,0]) turm_links();
if (teil == "oberarm")           oberarm();
if (teil == "kurbel")            kurbel_teil();
if (teil == "koppelstange")      koppelstange();
if (teil == "unterarm")          unterarm();
if (teil == "lagerhuelse")       lagerhuelse();
if (teil == "greifer_gehaeuse")  translate([0,0,g_platte]) greifer_gehaeuse();
if (teil == "hebel_antrieb")     greiferhebel(true);
if (teil == "hebel_mitlaeufer")  greiferhebel_mitlaeufer();
if (teil == "klemmplatte")       klemmplatte();
if (teil == "finger_tpu")        finger();
