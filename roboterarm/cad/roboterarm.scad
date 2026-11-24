// =====================================================================
//  Roboterarm "AM-R1" Version 2  –  geschlossen, 500 g Nutzlast
//
//  Basis      28BYJ-48 + Zahnräder 1:5, Drehteller auf 4 Lagern 6801, Ø 220
//  Schulter   DS3218 (270°) + Zahnräder 1:2, Welle beidseitig gelagert
//  Ellbogen   DS3218 (270°) + Zahnräder 1:2 -> Kurbel -> Koppelstange
//             (Kurbel, Stange und Hebel liegen IM Oberarm-Kasten)
//  Greifer    MG996R, Fin-Ray-Finger aus TPU, eigenes Gehäuse
//  Lager      11 x 6801-2RS (12 x 21 x 5)
//
//  Export:  openscad -D 'teil="basis"' -o basis.stl roboterarm.scad
//  Werte mit "MESSEN" an deinen Teilen prüfen.
// =====================================================================

teil = "zusammenbau";

$fn = 64;

// ---------- Kugellager 6801-2RS ----------
lager_D = 21.0;  lager_d = 12.0;  lager_b = 5.0;
sitz_D  = lager_D - 0.05;      // Presssitz
lippe_D = 16;                  // Loch hinter dem Lager (Innenring bleibt frei)

// ---------- Basis ----------
basis_r = 110;  boden_t = 3;  basis_h = 55;
innen_r1 = 66;  innen_r2 = 70;  aussen_t = 2.4;
deckel_t = 4;   rolle_r = 88;   rolle_anz = 4;
achse_h  = lager_D/2 + 2;
deckel_oben = basis_h + deckel_t;
tt_unten = deckel_oben + achse_h + lager_D/2;   // 82
tt_t = 8;  tt_oben = tt_unten + tt_t;          // 90
tt_r = 106;
schuerze_r = basis_r + 1;                      // Schürze über den Spalt
saeule_r = 15;  kabel_r = 8;

zr_m = 1.25;  zr_z1 = 15;  zr_z2 = 75;          // Basis 1:5
zr_a = zr_m*(zr_z1+zr_z2)/2 + 0.15;
kranz_ri = 40;  kranz_unten = tt_unten - 8;

sm_welle_l = 10;   // 28BYJ-48: Ohr-Unterseite bis Wellenende   MESSEN
sm_versatz = 8;  sm_ohr = 17.5;
ritzel_oben = tt_unten - 0.5;
sm_ohr_z = ritzel_oben - sm_welle_l;
lsk_z = tt_unten - 5;  rock_r1 = 23;  rock_r2 = 25;

// ---------- Servos (DS3218 und MG996R haben fast gleiche Maße) ----------
sv_l = 40.7;  sv_b = 20.0;  sv_w = 10.2;       // Länge, Breite, Welle ab Vorderkante
sv_unter = 27.6;   // Gehäuse unterhalb der Ohren
sv_ohr_l = 54.5;
sv_loch_a = 49.5;  sv_loch_b = 10;              // MESSEN
horn_abst = 18;    // Ohr-Oberseite bis Fläche der Servoscheibe   MESSEN
horn_d = 25;       // Servoscheibe (Alu-Horn) Durchmesser          MESSEN

// ---------- Schulter ----------
H_s  = 70;               // Schulterachse über Drehteller
s_m  = 1.5;  s_z1 = 22;  s_z2 = 44;            // 1:2
s_a  = s_m*(s_z1+s_z2)/2 + 0.15;
s_zr_h = 7;               // Zahnbreite (Nabe 8.5)
yi = 19;                 // Innenfläche Innenwand (Kasten +-18)
yw = 24.5;               // Außenfläche Innenwand = Beginn Zahnradraum
yo = 33;                 // Gehäuse außen (danach Deckel)
yc = 41;                 // Deckel außen
hx1 = -96;  hx2 = 40;  hv1 = -H_s;  hv2 = 44;
pinA = [-s_a, 0];
pinB = [s_a*cos(-120), s_a*sin(-120)];
alpha_max = 100;  alpha_min = -5;

// ---------- Arm ----------
L1 = 120;                // Schulter -> Ellbogen
L2 = 80;                 // Ellbogen -> Greiferflansch
kurbel = 15;             // Kurbel / Hebel im Kasten
k_h = 28;                // halbe Kastenhöhe
k_y = 18;                // halbe Kastenbreite
k_wand = 6;  k_rand = 5;
stange_t = 6;
ua_h = 22;               // halbe Höhe Unterarm
ua_i = 19;  ua_a = 25;   // Unterarm Wände innen/außen

// ---------- Achsen ----------
sw = 10.2;               // Sechskant Schlüsselweite (passt durch 12er Lager)
sw_loch = sw + 0.25;

// ---------- Greifer ----------
g_m = 1.5;  g_z = 24;  g_zr_h = 8;
g_ab = (g_m*g_z + 0.3)/2;   // 18.15
g_z0 = 11;               // Zahnrad-Unterseite über Platte   MESSEN
g_rueck = -24;           // Rückseite (Flansch)
g_unten = -36;
finger_l = 60;  finger_h = 30;  finger_b = 22;
finger_x = 22;  wand = 1.2;  rippe = 1.0;  rippen_abst = 7;
finger_dy = g_ab - finger_b/2;   // Finger seitlich versetzt

// =====================================================================
//  Hilfen
// =====================================================================
function rot2(p, a) = [p[0]*cos(a)-p[1]*sin(a), p[0]*sin(a)+p[1]*cos(a)];
function evolv(rb, t) = let(d = t*180/PI) [rb*(cos(d)+t*sin(d)), rb*(sin(d)-t*cos(d))];
function summe(v, i) = i <= 0 ? 0 : v[i-1][0] + summe(v, i-1);
module zahnrad2d(m, z, spiel=0.08) {
    rp = m*z/2; ra = rp + m; rf = rp - 1.25*m; rb = rp*cos(20);
    tmax = sqrt(pow(ra/rb,2) - 1);
    inv20 = (tan(20) - 20*PI/180)*180/PI;
    halb = 90/z + inv20 - (spiel/rp)*180/PI/2;
    fl = [for (i=[0:12]) rot2(evolv(rb, tmax*i/12), -halb)];
    zahn = concat([[0,0]], fl, [for (i=[12:-1:0]) [fl[i][0], -fl[i][1]]]);
    union() { circle(r=rf, $fn=z*4); for (k=[0:z-1]) rotate(k*360/z) polygon(zahn); }
}
module zahnrad(m, z, h) linear_extrude(h) zahnrad2d(m, z);
module rohr(ri, ra, h) difference() { cylinder(r=ra, h=h, $fn=180); translate([0,0,-1]) cylinder(r=ri, h=h+2, $fn=180); }
module tropfen(d, l) rotate([0,90,0]) linear_extrude(l, center=true) hull() { circle(d=d); translate([-d/2*1.4142,0]) circle(d=0.01); }
module mutter(h=2.6) cylinder(d=6.4, h=h, $fn=6);
module sechskant2d(s) circle(d=s/cos(30), $fn=6);
module stab2d(a, b, r1, r2) hull() { translate(a) circle(r=r1); translate(b) circle(r=r2); }
// 2D-Profil in der Armebene (x, v) zwischen y1 und y2 extrudieren
module XV(y1, y2) translate([0,y2,0]) rotate([90,0,0]) linear_extrude(y2-y1) children();
// Zylinder entlang y von y1 bis y2
module ZY(d, y1, y2, x=0, v=0) translate([x,y2,v]) rotate([90,0,0]) cylinder(d=d, h=y2-y1);
// Teil, das in XY gezeichnet ist (Dicke in z), in die Armebene legen: z -> +y
module P(y) translate([0,y,0]) multmatrix([[1,0,0,0],[0,0,1,0],[0,1,0,0],[0,0,0,1]]) children();
// ... und z -> -y
module Q(y) translate([0,y,0]) multmatrix([[1,0,0,0],[0,0,-1,0],[0,1,0,0],[0,0,0,1]]) children();

// =====================================================================
//  BASIS  (Ø 220, Sandkammer, Schrittmotor, Lichtschranke)
// =====================================================================
sm_wx = zr_a;  sm_gx = zr_a - sm_versatz;

module basis() {
    difference() {
        union() {
            cylinder(r=basis_r, h=boden_t, $fn=200);
            rohr(basis_r-aussen_t, basis_r, basis_h);
            rohr(innen_r1, innen_r2, basis_h);
            cylinder(r=saeule_r, h=tt_oben+0.2);
            rotate(270) translate([innen_r1, -9, 0]) cube([basis_r-innen_r1-0.5, 18, 17]);
            for (s=[-1,1]) {
                translate([sm_gx-4, s*sm_ohr-3, 0]) cube([8, 6, sm_ohr_z]);
                translate([sm_gx, s*sm_ohr-1.5, 0]) cube([innen_r1-sm_gx+1, 3, basis_h]);
            }
            translate([sm_gx-4, -sm_ohr, 0]) cube([8, 2*sm_ohr, 8]);
            translate([-(rock_r1-2), -5, 0]) cube([rock_r1-2-saeule_r+2, 10, lsk_z+5]);
            translate([-(rock_r2+10), -5, 0]) cube([8, 10, lsk_z+5]);
            translate([-innen_r1-1, -1.5, 0]) cube([innen_r1+1-rock_r2-9, 3, basis_h]);
            // Rippen in der Sandkammer
            for (k=[0:7]) rotate(22.5+k*45) translate([innen_r2-1, -1.2, 0]) cube([basis_r-innen_r2, 2.4, 30]);
        }
        translate([0,0,-1]) cylinder(r=kabel_r, h=200);
        rotate(270) translate([0, -6, boden_t]) cube([saeule_r+2, 12, 18]);
        rotate(270) translate([innen_r1-6, -7, boden_t]) cube([60, 14, 12]);
        for (k=[0:5]) rotate(30+k*60) translate([(innen_r1+innen_r2)/2, 0, basis_h-12]) cylinder(d=2.6, h=13);
        for (k=[0:2]) rotate(k*120+60) translate([11.5, 0, tt_oben-10]) cylinder(d=2.6, h=11);
        for (s=[-1,1]) translate([sm_gx, s*sm_ohr, sm_ohr_z-9]) cylinder(d=2.6, h=10);
        translate([-(rock_r2+12), 0, lsk_z]) rotate([0,90,0]) cylinder(d=5.2, h=12);
        translate([-(rock_r1), 0, lsk_z]) rotate([0,90,0]) cylinder(d=5.1, h=rock_r1-kabel_r+1);
        for (k=[0:3]) rotate(45+k*90) translate([basis_r-15, 0, -0.01]) cylinder(d=12, h=1);   // Gummifüße
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
            translate([rolle_r, 0, deckel_t+achse_h]) tropfen(lager_d+0.15, 30);
    }
}

module rollenachse() {
    l = lager_b + 0.6 + 8;
    difference() {
        rotate([0,90,0]) cylinder(d=lager_d-0.05, h=l, center=true);
        translate([-l, -lager_d, -lager_d/2-1]) cube([2*l, 2*lager_d, 1.5]);
    }
}

gh_loecher = [[-60,-47],[-60,47],[0,-47],[0,47]];     // Schultergehäuse: Flansch-Schrauben von oben
gh_flansch_v = 6;  deckel_unten = -H_s + 8;

module drehteller() {           // Unterseite nach OBEN drucken
    difference() {
        union() {
            cylinder(r=tt_r, h=tt_t, $fn=200);
            translate([0,0,tt_t-2]) rohr(tt_r-1, schuerze_r+2.4, 2);
            translate([0,0,-(tt_unten-(deckel_oben-2))]) rohr(schuerze_r, schuerze_r+2.4, tt_unten-(deckel_oben-2)+tt_t);
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
        for (p=gh_loecher) translate([p[0], p[1], -12]) cylinder(d=2.6, h=30);    // M3x16 selbstschneidend
    }
}

module ritzel() difference() {
    zahnrad(zr_m, zr_z1, ritzel_oben-(kranz_unten+0.5));
    translate([0,0,-1]) intersection() { cylinder(d=5.25, h=20); cube([3.15, 10, 40], center=true); }
}

module kappe() difference() {
    cylinder(d=40, h=4);
    translate([0,0,-1]) cylinder(r=kabel_r, h=6);
    for (k=[0:2]) rotate(k*120+60) translate([11.5,0,-1]) { cylinder(d=3.3,h=6); translate([0,0,3]) cylinder(d1=3.3, d2=6.5, h=2.01); }
}

// =====================================================================
//  SCHULTERGEHÄUSE  (x vorne, y seitlich, z = v; Schulterachse im Ursprung)
// =====================================================================
function sv_mitte(P) = P[0] + sv_w - sv_l/2;

module gehaeuse_umriss() offset(r=10) offset(delta=-10) translate([hx1, hv1]) square([hx2-hx1, hv2-hv1]);

module schwenkbereich2d() union() {
    circle(r=31);
    for (a=[alpha_min:5:alpha_max-5]) hull() {
        rotate(a)   translate([0,-k_h-2]) square([300, 2*k_h+4]);
        rotate(a+5) translate([0,-k_h-2]) square([300, 2*k_h+4]);
    }
}

// Servotasche: s = -1 (Welle zeigt nach -y, Servo A) / +1 (Servo B)
module servotasche(P, s) {
    ohr_y = s*(yw + 0.5 - horn_abst);
    m = sv_mitte(P);
    translate([P[0]+sv_w-sv_l-0.4, min(ohr_y, s*(yw+0.1)), P[1]-sv_b/2-0.4])
        cube([sv_l+0.8, abs(s*(yw+0.1)-ohr_y), sv_b+0.8]);
    translate([m-sv_ohr_l/2-0.5, min(ohr_y, ohr_y - s*(sv_unter+0.8)), P[1]-sv_b/2-0.4])
        cube([sv_ohr_l+1, sv_unter+0.8, sv_b+0.8]);
    for (dx=[-1,1]) for (dv=[-1,1])
        translate([m+dx*sv_loch_a/2, ohr_y, P[1]+dv*sv_loch_b/2]) rotate([s*90,0,0]) translate([0,0,-12]) cylinder(d=2.6, h=12);
    // Freiraum für die runde Servoscheibe
    if (s < 0) ZY(horn_d+3, -yw-1, -yw+6, P[0], P[1]); else ZY(horn_d+3, yw-6, yw+1, P[0], P[1]);
}

deckel_schrauben = [[-88,34],[-80,-56],[16,-56],[34,-20],[-40,36],[5,40]];
verbinder = [[-88,25],[-88,-55],[30,-55],[25,-62]];

module schulter_gehaeuse_voll() {
    difference() {
        union() {
            XV(-yo, yo) gehaeuse_umriss();
            translate([-80, -yc-12, hv1]) cube([100, 2*(yc+12), gh_flansch_v]);   // Flansch
        }
        XV(-yi, yi) schwenkbereich2d();
        XV(-yo-1, -yw) hull() { circle(r=36); translate(pinA) circle(r=19.5); }
        XV(yw, yo+1)  hull() { circle(r=36); translate(pinB) circle(r=19.5); }
        ZY(sitz_D, -yw-0.01, -yw+lager_b);
        ZY(sitz_D, yw-lager_b, yw+0.01);
        ZY(lippe_D, -yw, yw);
        servotasche(pinA, -1);
        servotasche(pinB, +1);
        for (p=deckel_schrauben) { ZY(2.6, yo-12, yo+1, p[0], p[1]); ZY(2.6, -yo-1, -yo+12, p[0], p[1]); }
        // Verbindungsschrauben der Hälften (M3x45)
        for (p=verbinder) {
            ZY(3.3, -yo-1, yo+1, p[0], p[1]);
            ZY(6.5, yo-12, yo+1, p[0], p[1]);
            translate([p[0], -yo+12, p[1]]) rotate([90,0,0]) rotate(30) cylinder(d=6.6, h=13, $fn=6);
        }
        for (p=gh_loecher) translate([p[0], p[1], hv1-1]) { cylinder(d=3.3, h=20); translate([0,0,gh_flansch_v+1-0.01]) cylinder(d=6.5, h=20); }
        translate([0,0,hv1-1]) cylinder(r=22, h=7);
        translate([0,0,hv1-1]) cylinder(r=9, h=12);
        translate([-45, 12, -64]) cylinder(d=10, h=52);
        translate([-10, 0, -64]) cylinder(d=10, h=40);                 // Greiferkabel aus dem Oberarm
        hull() { translate([-45,12,hv1+4]) cylinder(d=10, h=6); translate([0,0,hv1+4]) cylinder(d=10, h=6); }
    }
}
module gehaeuse_links()  intersection() { schulter_gehaeuse_voll(); translate([-200,-100,-200]) cube([400,100,400]); }
module gehaeuse_rechts() intersection() { schulter_gehaeuse_voll(); translate([-200,0,-200]) cube([400,100,400]); }

module gehaeuse_deckel() difference() {       // rechter Deckel (links: gleiches Teil umgedreht)
    XV(yo, yc) intersection() { gehaeuse_umriss(); translate([-500, deckel_unten]) square(1000); }
    ZY(sitz_D, yo-0.01, yo+lager_b);
    ZY(lippe_D, yo, yc+1);
    for (p=deckel_schrauben) { ZY(3.3, yo-1, yc+1, p[0], p[1]); ZY(6.2, yc-3, yc+1, p[0], p[1]); }
}

module schulter_ritzel() difference() {        // auf die Alu-Servoscheibe geschraubt
    zahnrad(s_m, s_z1, s_zr_h);
    translate([0,0,-1]) cylinder(d=9, h=20);
    translate([0,0,-0.01]) cylinder(d=horn_d+0.4, h=2);
}
module schulter_zahnrad() difference() {   // Nabe z 0..8.5, Zähne z 0.5..7.5
    union() { translate([0,0,0.5]) zahnrad(s_m, s_z2, s_zr_h); cylinder(d=14, h=8.5); }
    translate([0,0,-2]) linear_extrude(20) sechskant2d(sw_loch);
    for (k=[0:5]) rotate(k*60+30) translate([21,0,-2]) cylinder(r=5.5, h=20);
}

// =====================================================================
//  ACHSEN  (Sechskant 10.2 für Drehmoment, rund 12 für die Lager)
// =====================================================================
module achse(a) {
    l = summe(a, len(a));
    difference() {
        union() for (i=[0:len(a)-1]) translate([0,0,summe(a,i)])
            if (a[i][1] == "s") linear_extrude(a[i][0]) sechskant2d(sw);
            else intersection() { cylinder(d=lager_d-0.05, h=a[i][0]); translate([-10,-sw/2,0]) cube([20,20,a[i][0]]); }
        translate([0,0,-1]) cylinder(d=3.4, h=l+2);
        translate([0,0,-0.01]) rotate(30) cylinder(d=6.5, h=4, $fn=6);   // Tasche für M3-Mutter (innen)
    }
}
achse_schulter = [[6,"s"],[6.5,"r"],[8.5,"s"],[5,"r"]];         // Kasten | Ring+Lager | Zahnrad | Lager
achse_kurbel   = [[13.9,"s"],[11.6,"r"],[8.5,"s"],[5,"r"]];     // Kurbel | Lager+Ring+Lager | Zahnrad | Lager
achse_ellbogen = [[13.9,"s"],[6.1,"r"],[6,"s"]];               // Hebel | Lager+Ring | Unterarm
achse_stummel  = [[6,"s"],[6,"r"]];                            // Kasten | Ring+Lager
module achse_druck(a) translate([0,0,sw/2]) rotate([90,0,0]) achse(a);

module distanzring(t) difference() { cylinder(d=14, h=t); translate([0,0,-1]) cylinder(d=12.3, h=t+2); }

// =====================================================================
//  OBERARM-KASTEN  (x Armrichtung, y seitlich, z = v)
// =====================================================================
module kasten2d() hull() { circle(r=k_h); translate([L1,0]) circle(r=k_h); }
kasten_schrauben = [[20,k_h-2.5],[60,k_h-2.5],[100,k_h-2.5],[20,-k_h+2.5],[60,-k_h+2.5],[100,-k_h+2.5],[-k_h+2.5,0],[L1+k_h-2.5,0]];

module oberarm_kasten() difference() {      // linke Wand + Rand; rechte Wand = Deckel
    union() {
        XV(-k_y, -k_y+k_wand) kasten2d();
        XV(-k_y+k_wand, k_y-k_wand) difference() { kasten2d(); offset(delta=-k_rand) kasten2d(); }
    }
    for (x=[0, L1]) XV(-k_y-1, -k_y+k_wand+1) translate([x,0]) sechskant2d(sw_loch);
    for (p=kasten_schrauben) ZY(2.6, k_y-k_wand-12, k_y-k_wand+1, p[0], p[1]);
}
module oberarm_deckel() difference() {
    XV(k_y-k_wand, k_y) kasten2d();
    for (x=[0, L1]) { ZY(sitz_D, k_y-lager_b, k_y+0.01, x); ZY(lippe_D, k_y-k_wand-1, k_y, x); }
    for (p=kasten_schrauben) { ZY(3.3, k_y-k_wand-1, k_y+1, p[0], p[1]);
        translate([p[0], k_y+0.01, p[1]]) rotate([90,0,0]) cylinder(d1=6.5, d2=3.3, h=1.8); }
}

module kurbel_teil() difference() {          // Kurbel (Schulter) und Hebel (Ellbogen): 2x
    union() {
        linear_extrude(8) stab2d([0,0], [kurbel,0], 10, 6.5);
        cylinder(d=14, h=13.9);                  // Nabe bis zum Lager-Innenring
    }
    translate([0,0,-1]) linear_extrude(30) sechskant2d(sw_loch);
    translate([kurbel,0,-1]) cylinder(d=3.3, h=20);
    translate([kurbel,0,8-4.2]) rotate(30) cylinder(d=6.5, h=5, $fn=6);   // Stoppmutter M3
}
module koppelstange() difference() {
    linear_extrude(stange_t) stab2d([0,0], [L1,0], 6.5, 6.5);
    for (x=[0,L1]) translate([x,0,-1]) cylinder(d=3.4, h=20);
}

// =====================================================================
//  UNTERARM  (Kanal; Wände außen am Kasten; x Armrichtung, z = v)
// =====================================================================
module ua2d() hull() { circle(r=26); translate([0,-ua_h]) square([L2, 2*ua_h]); }
ua_schrauben = [[40,ua_h-2.5],[70,ua_h-2.5],[68,-ua_h+2.5],[L2-3,8],[L2-3,-8]];
ua_flansch = [[12,-14],[-12,-14],[12,-2],[-12,-2]];   // [y, v]

module unterarm_kanal() difference() {      // rechte Wand + oben + unten + Stirnwand
    union() {
        XV(ua_i, ua_a) ua2d();
        XV(-ua_i, ua_i) {
            translate([32, ua_h-5]) square([L2-32, 5]);
            translate([60, -ua_h]) square([L2-60, 5]);
            translate([L2-6, -ua_h]) square([6, 2*ua_h]);
        }
    }
    XV(ua_i-1, ua_a+1) sechskant2d(sw_loch);
    for (p=ua_schrauben) ZY(2.6, -ua_i-1, -ua_i+12, p[0], p[1]);
    for (f=ua_flansch) translate([L2-10, f[0], f[1]]) {
        rotate([0,90,0]) cylinder(d=3.3, h=20);
        rotate([0,90,0]) rotate(30) cylinder(d=6.6, h=6.6, $fn=6);
    }
    translate([L2-10, 0, -8]) rotate([0,90,0]) cylinder(d=10, h=20);
}
module unterarm_deckel() difference() {
    XV(-ua_a, -ua_i) ua2d();
    ZY(sitz_D, -ua_i-lager_b-0.01, -ua_i+0.01);
    ZY(lippe_D, -ua_a-1, -ua_i);
    for (p=ua_schrauben) { ZY(3.3, -ua_a-1, -ua_i+1, p[0], p[1]);
        translate([p[0], -ua_a-0.01, p[1]]) rotate([-90,0,0]) cylinder(d1=6.5, d2=3.3, h=1.8); }
}

// =====================================================================
//  GREIFER  (x_g vorne, y_g seitlich, z_g hoch; Platte oben bei z=0)
// =====================================================================
g_ga = 43.5;   // Greifer halbe Außenbreite
g_boden_schr = [[g_rueck+6,-41],[g_rueck+6,41],[8,-41],[8,41]];
module greifer_gehaeuse() difference() {
    union() {
        translate([g_rueck, -g_ga, -5]) cube([30-g_rueck, 2*g_ga, 5]);                   // Platte
        translate([g_rueck, -g_ga, g_unten]) cube([4, 2*g_ga, 24-g_unten]);              // Rückwand
        for (s=[-1,1]) translate([g_rueck, s>0 ? 38.5 : -g_ga, g_unten]) cube([8-g_rueck, 5, 24-g_unten]);
        for (s=[-1,1]) translate([g_rueck, s>0 ? 38.5 : -g_ga, g_unten]) cube([14.6-g_rueck, 5, -g_unten]);
        translate([10.6, -g_ga, g_unten]) cube([4, 2*g_ga, -5-g_unten]);                // Front unten
        translate([0, g_ab, 0]) cylinder(d=10, h=g_z0-0.2);                             // Zapfen
    }
    translate([-sv_b/2-0.4, -g_ab-sv_w-0.4, -10]) cube([sv_b+0.8, sv_l+0.8, 20]);
    for (dy=[-1,1]) for (dx=[-1,1]) translate([dx*sv_loch_b/2, -g_ab-sv_w+sv_l/2+dy*sv_loch_a/2, -10]) cylinder(d=2.6, h=10.5);
    translate([0, g_ab, -2]) cylinder(d=2.6, h=40);          // M3x25 selbstschneidend
    for (p=g_haube_schr) translate([p[0], p[1], 10]) cylinder(d=2.6, h=20);
    for (f=ua_flansch) translate([g_rueck-1, f[0], f[1]-8]) rotate([0,90,0]) cylinder(d=3.3, h=10);
    translate([g_rueck-1, 0, -16]) rotate([0,90,0]) cylinder(d=10, h=10);
    for (p=g_boden_schr) translate([p[0], p[1], g_unten-1]) cylinder(d=2.6, h=11);
}
g_haube_schr = [[-16,-41],[-16,41],[2,-41],[2,41]];
module greifer_haube() difference() {      // Deckel über den Zahnrädern (liegt auf den Seitenwänden)
    translate([g_rueck, -g_ga, 0]) cube([8-g_rueck, 2*g_ga, 3]);
    translate([0, -g_ab, -1]) cylinder(d=10, h=5);          // Zugang Hornschraube
    translate([0, g_ab, -1]) cylinder(d=3.3, h=5);          // Achsschraube Mitläufer
    for (p=g_haube_schr) translate([p[0], p[1], -1]) { cylinder(d=3.3, h=5); translate([0,0,1.2]) cylinder(d1=3.3, d2=6.5, h=1.81); }
}
module greifer_boden() difference() {
    translate([g_rueck, -g_ga, 0]) cube([14.6-g_rueck, 2*g_ga, 4]);
    for (p=g_boden_schr) translate([p[0], p[1], -1]) { cylinder(d=3.3, h=10); translate([0,0,2.5]) cylinder(d=6.2, h=3); }
}

module greiferhebel(antrieb=true) {
    pad_y1 = -finger_b/2 + finger_dy;  pad_y2 = finger_b/2 + finger_dy - 1;
    difference() {
        union() {
            rotate(antrieb ? 0 : 180/g_z) zahnrad(g_m, g_z, g_zr_h);
            hull() { cylinder(r=8, h=g_zr_h); translate([finger_x-3, pad_y1, 0]) cube([3, pad_y2-pad_y1, g_zr_h]); }
            translate([finger_x-3, pad_y1, 0]) cube([3, pad_y2-pad_y1, finger_h+6]);
            translate([finger_x-3, pad_y1, 0]) cube([11, pad_y2-pad_y1, 3]);
        }
        if (antrieb) { translate([0,0,-1]) cylinder(d=9, h=20); translate([0,0,-0.01]) cylinder(d=horn_d+0.4, h=2); }
        else translate([0,0,-1]) cylinder(d=3.4, h=20);
        for (y=[-5,5]) translate([finger_x+4, y+finger_dy, -1]) cylinder(d=3.3, h=60);
        for (y=[-5,5]) translate([finger_x+4, y+finger_dy, -0.01]) mutter(2.4);
    }
}
module greiferhebel_mitlaeufer() mirror([0,1,0]) greiferhebel(false);
module klemmplatte() difference() {
    translate([0, -finger_b/2, 0]) cube([8, finger_b-1, 3]);
    for (y=[-5,5]) translate([4, y, -1]) cylinder(d=3.3, h=5);
}

module finger2d() {
    basis = 8;
    tri = [[basis,-finger_b/2],[finger_l,finger_b/2-0.01],[basis,finger_b/2]];
    difference() {
        union() {
            translate([0,-finger_b/2]) square([basis, finger_b]);
            difference() { offset(r=0.6) offset(delta=-0.6) polygon(tri); offset(delta=-wand) polygon(tri); }
            intersection() {
                polygon(tri);
                for (x=[basis+rippen_abst : rippen_abst : finger_l-6]) translate([x-rippe/2, -finger_b]) square([rippe, 2*finger_b]);
            }
            for (x=[basis+4 : 5 : finger_l-6]) translate([x, finger_b/2]) circle(d=1.6, $fn=12);   // Griffnoppen
        }
        for (y=[-5,5]) translate([4,y]) circle(d=3.4);
    }
}
module finger() linear_extrude(finger_h) finger2d();

// =====================================================================
//  ZUSAMMENBAU
// =====================================================================
alpha = 55;  beta = -50;  greif = 18;  basisw = 0;

module greifer_komplett() {
    color("SteelBlue") { greifer_gehaeuse(); translate([0,0,24]) greifer_haube(); translate([0,0,g_unten-4]) greifer_boden(); }
    color("Orange") translate([0,-g_ab,g_z0]) rotate(-greif) {
        greiferhebel(true);
        color("Gold") translate([finger_x, finger_dy, 3]) finger();
        translate([finger_x, finger_dy, 3+finger_h]) klemmplatte();
    }
    color("Orange") translate([0, g_ab, g_z0]) rotate(greif) {
        greiferhebel_mitlaeufer();
        color("Gold") translate([finger_x, -finger_dy, 3+finger_h]) mirror([0,0,1]) mirror([0,1,0]) finger();
    }
}

module schulter_innen() {          // Zahnräder (Vorschau)
    color("Silver") {
        rotate([0,-alpha,0]) Q(-yw-0.5) translate([0,0,0]) zahnrad(s_m, s_z2, s_zr_h);
        rotate([0,-(beta+180),0]) P(yw+0.5) zahnrad(s_m, s_z2, s_zr_h);
        translate([pinA[0],0,pinA[1]]) Q(-yw-0.5) zahnrad(s_m, s_z1, s_zr_h);
        translate([pinB[0],0,pinB[1]]) P(yw+0.5) zahnrad(s_m, s_z1, s_zr_h);
    }
}

module zusammenbau(ohne_deckel=false) {
    E = [L1*cos(alpha), L1*sin(alpha)];
    theta = beta + 180;
    K = [kurbel*cos(theta), kurbel*sin(theta)];
    color("LightGray") basis();
    color("DimGray") translate([0,0,basis_h]) deckelring();
    color("Silver") translate([sm_wx,0,kranz_unten+0.5]) ritzel();
    rotate(basisw) {
        color("WhiteSmoke") translate([0,0,tt_unten]) drehteller();
        translate([0,0,tt_oben+H_s]) {
            color("SlateGray") { gehaeuse_links(); gehaeuse_rechts(); }
            if (!ohne_deckel) color("DarkSlateGray") { gehaeuse_deckel(); mirror([0,1,0]) gehaeuse_deckel(); }
            schulter_innen();
            rotate([0,-alpha,0]) {
                color("Tomato") oberarm_kasten();
                if (!ohne_deckel) color("IndianRed") oberarm_deckel();
            }
            color("Gold") rotate([0,-theta,0]) P(-1) kurbel_teil();
            color("Khaki") translate([K[0], 0, K[1]]) rotate([0,-alpha,0]) P(-8) koppelstange();
            translate([E[0], 0, E[1]]) {
                color("Gold") rotate([0,-theta,0]) P(-1) kurbel_teil();
                rotate([0,-beta,0]) {
                    color("Tomato") unterarm_kanal();
                    if (!ohne_deckel) color("IndianRed") unterarm_deckel();
                    translate([L2-g_rueck, 0, 8]) greifer_komplett();
                }
            }
        }
    }
}

// =====================================================================
if (teil == "zusammenbau")         zusammenbau();
if (teil == "offen")               zusammenbau(true);
if (teil == "basis")               basis();
if (teil == "deckelring")          deckelring();
if (teil == "rollenachse")         translate([0,0,lager_d/2-0.5]) rollenachse();
if (teil == "drehteller")          translate([0,0,tt_t]) rotate([180,0,0]) drehteller();
if (teil == "ritzel_basis")        ritzel();
if (teil == "kappe")               kappe();
if (teil == "gehaeuse_links")      rotate([-90,0,0]) gehaeuse_links();
if (teil == "gehaeuse_rechts")     rotate([90,0,0]) gehaeuse_rechts();
if (teil == "gehaeuse_deckel")     translate([0,0,-yo]) rotate([90,0,0]) gehaeuse_deckel();
if (teil == "schulter_ritzel")     schulter_ritzel();
if (teil == "schulter_zahnrad")    schulter_zahnrad();
if (teil == "achse_schulter")      achse_druck(achse_schulter);
if (teil == "achse_kurbel")        achse_druck(achse_kurbel);
if (teil == "achse_ellbogen")      achse_druck(achse_ellbogen);
if (teil == "achse_stummel")       achse_druck(achse_stummel);
if (teil == "distanzring_1")       distanzring(1.0);
if (teil == "distanzring_1_5")     distanzring(1.5);
if (teil == "oberarm_kasten")      translate([0,0,k_y]) rotate([90,0,0]) oberarm_kasten();
if (teil == "oberarm_deckel")      translate([0,0,-k_y+k_wand]) rotate([90,0,0]) oberarm_deckel();
if (teil == "kurbel")              kurbel_teil();
if (teil == "koppelstange")        koppelstange();
if (teil == "unterarm_kanal")      translate([0,0,ua_a]) rotate([-90,0,0]) unterarm_kanal();
if (teil == "unterarm_deckel")     translate([0,0,-ua_i]) rotate([-90,0,0]) unterarm_deckel();
if (teil == "greifer_gehaeuse")    translate([0,0,-g_unten]) greifer_gehaeuse();
if (teil == "greifer_boden")       greifer_boden();
if (teil == "greifer_haube")       greifer_haube();
if (teil == "hebel_antrieb")       greiferhebel(true);
if (teil == "hebel_mitlaeufer")    greiferhebel_mitlaeufer();
if (teil == "klemmplatte")         klemmplatte();
if (teil == "finger_tpu")          finger();
