FeatureScript 1576;
import(path : "onshape/std/geometry.fs", version : "1576.0");

// =====================================================================================
//  Roboterarm AM-R1  Version 2  –  vollständiges, detailliertes Modell als Onshape-Feature
//
//  • alle Druckteile 1:1 wie cad/roboterarm.scad (Druckdateien in stl/)
//  • exakte Evolventen-Zahnräder (Splines), alle Zahnpaare in Eingriff
//  • Kaufteile detailliert: DS3218 / MG996R mit Kabel und Stecker, Alu-Servoscheiben,
//    Lager 6801-2RS (Ringe, Laufbahnen, Kugeln, Käfig, Dichtungen), 28BYJ-48 mit Kabel
//  • alle Normteile: ISO 4762, ISO 10642, ISO 7380, ISO 4032, ISO 10511, ISO 7089, Gummifüße
//  • Elektronik: ESP32-S3-Board, Steckbrett 830, ULN2003, Netzteil, Elko, Widerstände,
//    Taster, LED und Fotowiderstand der Lichtschranke, Sandfüllung der Basis
//  • Werkstoffe mit Dichte (Masse / Schwerpunkt in Onshape), Kaufteile mit echter Masse
//  • Mate Connectors an allen Gelenken auf beiden Teilen
//
//  Einbau: neues Feature Studio anlegen, die ersten zwei Zeilen dort stehen lassen und
//  alles ab Zeile 3 dieser Datei darunter einfügen. Im Part Studio: "Roboterarm AM-R1".
//  Nicht modelliert: Gewindegänge (Schrauben als Nenndurchmesser) und die Steckkabel
//  zwischen den Bauteilen (Verlauf hängt vom Aufbau ab).
// =====================================================================================

// ---------- Kugellager 6801-2RS ----------
const LAGER_D = 21;   const LAGER_DI = 12;   const LAGER_B = 5;
const SITZ_D = 20.95; const LIPPE_D = 16;

// ---------- Basis ----------
const BASIS_R = 110;  const BODEN_T = 3;  const BASIS_H = 55;
const INNEN_R1 = 66;  const INNEN_R2 = 70;  const AUSSEN_T = 2.4;
const DECKEL_T = 4;   const ROLLE_R = 88;  const ACHSE_H = 12.5;
const DECKEL_OBEN = 59;  const TT_UNTEN = 82;  const TT_T = 8;  const TT_OBEN = 90;
const TT_R = 106;  const SCHUERZE_R = 111;  const SAEULE_R = 15;  const KABEL_R = 8;
const ZR_M = 1.25;  const ZR_Z1 = 15;  const ZR_Z2 = 75;
const KRANZ_RI = 40;  const KRANZ_UNTEN = 74;
const SM_OHR = 17.5;  const RITZEL_OBEN = 81.5;
const SM_WX = 56.4;  const SM_GX = 48.4;
const LSK_Z = 77;  const ROCK_R1 = 23;  const ROCK_R2 = 25;

// ---------- Schulter ----------
const H_S = 70;  const S_M = 1.5;  const S_Z1 = 22;  const S_Z2 = 44;  const S_ZR_H = 7;
const YI = 19;  const YW = 24.5;  const YO = 33;  const YC = 41;
const HX1 = -96;  const HX2 = 40;  const HV1 = -70;  const HV2 = 44;
const PIN_A = [-49.65, 0];
const PIN_B = [-24.825, -42.998];
const ALPHA_MIN = -5;  const ALPHA_MAX = 100;
const GH_FLANSCH_V = 6;  const DECKEL_UNTEN = -62;
const GH_LOECHER = [[-60, -47], [-60, 47], [0, -47], [0, 47]];
const DECKEL_SCHRAUBEN = [[-88, 34], [-80, -56], [16, -56], [34, -20], [-40, 36], [5, 40]];
const VERBINDER = [[-88, 25], [-88, -55], [30, -55], [25, -62]];

// ---------- Arm ----------
const L1 = 120;  const L2 = 80;  const KURBEL = 15;
const K_H = 28;  const K_Y = 18;  const K_WAND = 6;  const K_RAND = 5;  const STANGE_T = 6;
const UA_H = 22;  const UA_I = 19;  const UA_A = 25;
const SW = 10.2;  const SW_LOCH = 10.45;
const KASTEN_SCHRAUBEN = [[20, 25.5], [60, 25.5], [100, 25.5], [20, -25.5], [60, -25.5], [100, -25.5], [-25.5, 0], [145.5, 0]];
const UA_SCHRAUBEN = [[40, 19.5], [70, 19.5], [68, -19.5], [77, 8], [77, -8]];
const UA_FLANSCH = [[12, -14], [-12, -14], [12, -2], [-12, -2]];      // [y, v]
const ACHSE_SCHULTER = [[6, "s"], [6.5, "r"], [8.5, "s"], [5, "r"]];
const ACHSE_KURBEL   = [[13.9, "s"], [11.6, "r"], [8.5, "s"], [5, "r"]];
const ACHSE_ELLBOGEN = [[13.9, "s"], [6.1, "r"], [6, "s"]];
const ACHSE_STUMMEL  = [[6, "s"], [6, "r"]];

// ---------- Greifer ----------
const G_M = 1.5;  const G_Z = 24;  const G_ZR_H = 8;  const G_AB = 18.15;
const G_RUECK = -24;  const G_UNTEN = -36;  const G_GA = 43.5;
const FINGER_L = 60;  const FINGER_H = 30;  const FINGER_B = 22;  const FINGER_X = 22;
const WAND = 1.2;  const RIPPE = 1;  const RIPPEN_ABST = 7;  const FINGER_DY = 7.15;
const G_BODEN_SCHR = [[-18, -41], [-18, 41], [8, -41], [8, 41]];
const G_HAUBE_SCHR = [[-16, -41], [-16, 41], [2, -41], [2, 41]];

// ---------- Farben ----------
const F_BASIS = color(0.80, 0.80, 0.80);   const F_GRAU = color(0.35, 0.40, 0.45);
const F_DUNKEL = color(0.22, 0.27, 0.30);  const F_ARM = color(0.86, 0.32, 0.25);
const F_ARM2 = color(0.74, 0.26, 0.22);    const F_GOLD = color(0.92, 0.74, 0.20);
const F_STANGE = color(0.85, 0.80, 0.50);  const F_GREIFER = color(0.25, 0.48, 0.70);
const F_HEBEL = color(0.95, 0.60, 0.10);   const F_TPU = color(0.15, 0.15, 0.15);
const F_WEISS = color(0.92, 0.92, 0.92);   const F_METALL = color(0.72, 0.73, 0.76);
const F_STAHL = color(0.55, 0.57, 0.60);   const F_SCHWARZ = color(0.07, 0.07, 0.07);
const F_MOTOR = color(0.20, 0.35, 0.85);   const F_GUMMI = color(0.12, 0.12, 0.12);
const F_ALU = color(0.80, 0.82, 0.86);     const F_BRAUN = color(0.45, 0.25, 0.10);
const F_ROT = color(0.85, 0.10, 0.10);     const F_ORANGE = color(0.98, 0.55, 0.05);
const F_GELB = color(0.95, 0.85, 0.10);    const F_ROSA = color(0.95, 0.55, 0.70);
const F_BLAU = color(0.10, 0.35, 0.90);    const F_GRUEN_PCB = color(0.05, 0.40, 0.15);
const F_BLAU_PCB = color(0.05, 0.20, 0.55);const F_SAND = color(0.85, 0.75, 0.55);
const F_MESSING = color(0.80, 0.65, 0.25); const F_NYLON = color(0.95, 0.95, 0.90);

// ---------- Werkstoffe [Name, Dichte g/cm³] ----------
const W_PLA = ["PLA (Druck)", 1.24];        const W_PETG = ["PETG (Druck)", 1.27];
const W_TPU = ["TPU 95A (Druck)", 1.21];    const W_STAHL = ["Stahl 8.8 verzinkt", 7.85];
const W_LSTAHL = ["Wälzlagerstahl 100Cr6", 7.81];  const W_ALU = ["Aluminium EN AW-6061", 2.70];
const W_NBR = ["NBR-Gummi", 1.20];          const W_PA = ["PA6", 1.14];
const W_FR4 = ["FR4 Leiterplatte", 1.85];   const W_ABS = ["ABS", 1.04];
const W_CU = ["Kupfer", 8.96];              const W_PVC = ["PVC", 1.40];
const W_SAND = ["Sand trocken", 1.60];      const W_MESSING = ["Messing", 8.50];
const W_PBT = ["PBT", 1.31];

const ALPHA_GRENZEN = { (degree) : [-5, 50, 100] } as AngleBoundSpec;
const BETA_GRENZEN = { (degree) : [-130, -60, 10] } as AngleBoundSpec;
const GREIF_GRENZEN = { (degree) : [0, 15, 35] } as AngleBoundSpec;
const BASIS_GRENZEN = { (degree) : [-200, 0, 200] } as AngleBoundSpec;

annotation { "Feature Type Name" : "Roboterarm AM-R1" }
export const roboterarmAMR1 = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Oberarm (alpha)" }
        isAngle(definition.alpha, ALPHA_GRENZEN);
        annotation { "Name" : "Unterarm absolut (beta)" }
        isAngle(definition.beta, BETA_GRENZEN);
        annotation { "Name" : "Greifer offen" }
        isAngle(definition.greif, GREIF_GRENZEN);
        annotation { "Name" : "Basis drehen" }
        isAngle(definition.basisw, BASIS_GRENZEN);

        annotation { "Name" : "Servo: Ohr-Oberseite bis Fläche Servoscheibe" }
        isLength(definition.hornAbst, { (millimeter) : [10, 18, 30] } as LengthBoundSpec);
        annotation { "Name" : "Servoscheibe Durchmesser" }
        isLength(definition.hornD, { (millimeter) : [15, 25, 30] } as LengthBoundSpec);
        annotation { "Name" : "Servoscheibe Lochkreis" }
        isLength(definition.hornLK, { (millimeter) : [8, 16, 22] } as LengthBoundSpec);
        annotation { "Name" : "Servo Lochabstand lang" }
        isLength(definition.svLochA, { (millimeter) : [40, 49.5, 60] } as LengthBoundSpec);
        annotation { "Name" : "Servo Lochabstand quer" }
        isLength(definition.svLochB, { (millimeter) : [5, 10, 16] } as LengthBoundSpec);
        annotation { "Name" : "Servo Länge" }
        isLength(definition.svL, { (millimeter) : [35, 40.7, 45] } as LengthBoundSpec);
        annotation { "Name" : "Servo Breite" }
        isLength(definition.svB, { (millimeter) : [15, 20, 25] } as LengthBoundSpec);
        annotation { "Name" : "Servo Welle ab Vorderkante" }
        isLength(definition.svW, { (millimeter) : [5, 10.2, 15] } as LengthBoundSpec);
        annotation { "Name" : "Servo Höhe unter den Ohren" }
        isLength(definition.svUnter, { (millimeter) : [20, 27.6, 35] } as LengthBoundSpec);
        annotation { "Name" : "Greifer: Zahnrad über Platte" }
        isLength(definition.gZ0, { (millimeter) : [5, 11, 20] } as LengthBoundSpec);
        annotation { "Name" : "28BYJ-48: Ohr-Unterseite bis Wellenende" }
        isLength(definition.smWelle, { (millimeter) : [6, 10, 14] } as LengthBoundSpec);

        annotation { "Name" : "Basis + Drehteller", "Default" : true }
        definition.zeigeBasis is boolean;
        annotation { "Name" : "Schultergehäuse", "Default" : true }
        definition.zeigeSchulter is boolean;
        annotation { "Name" : "Arm", "Default" : true }
        definition.zeigeArm is boolean;
        annotation { "Name" : "Greifer", "Default" : true }
        definition.zeigeGreifer is boolean;
        annotation { "Name" : "Kaufteile (Servos, Lager, Motor)", "Default" : true }
        definition.zeigeKaufteile is boolean;
        annotation { "Name" : "Normteile (Schrauben, Muttern, Scheiben)", "Default" : true }
        definition.zeigeNormteile is boolean;
        annotation { "Name" : "Elektronik", "Default" : true }
        definition.zeigeElektronik is boolean;
        annotation { "Name" : "Zahnform exakt (Splines)", "Default" : true }
        definition.zahnExakt is boolean;
        annotation { "Name" : "Werkstoffe zuweisen (Masse)", "Default" : true }
        definition.werkstoffe is boolean;
        annotation { "Name" : "Mate Connectors an den Gelenken", "Default" : true }
        definition.zeigeConnectors is boolean;
    }
    {
        var w = {
            "ctx" : context, "id" : id, "z" : new box(0), "nt" : new box({}), "g" : new box("Basis"),
            "hA" : definition.hornAbst / millimeter, "hD" : definition.hornD / millimeter, "hLK" : definition.hornLK / millimeter,
            "lA" : definition.svLochA / millimeter, "lB" : definition.svLochB / millimeter,
            "sL" : definition.svL / millimeter, "sB" : definition.svB / millimeter, "sW" : definition.svW / millimeter,
            "sU" : definition.svUnter / millimeter, "gZ0" : definition.gZ0 / millimeter,
            "smOZ" : RITZEL_OBEN - definition.smWelle / millimeter, "smWL" : definition.smWelle / millimeter,
            "exakt" : definition.zahnExakt, "werkstoff" : definition.werkstoffe
        };
        const al = definition.alpha / degree;
        const bt = definition.beta / degree;
        const th = bt + 180;
        const gr = definition.greif / degree;
        const bw = definition.basisw / degree;
        const S = TT_OBEN + H_S;
        const E = [L1 * cos(al * degree), L1 * sin(al * degree)];
        const K = [KURBEL * cos(th * degree), KURBEL * sin(th * degree)];
        const pA = 180 / S_Z1;                       // halbe Teilung Ritzel
        // Zahnstellungen, damit alle Paare in Eingriff stehen
        const phRitzelBasis = 180 - 180 / ZR_Z1 - 5 * bw;
        const phRitzelA = pA - 2 * (al - 50);
        const phRitzelB = -60 - pA + 2 * (th - 120);

        // ---------- Lagen ----------
        const T_dreh = Rz(bw);
        const T_sch = T_dreh * Tt(0, 0, S);
        const T_ober = T_sch * Ry(-al);
        const T_kurb = T_sch * Ry(-th);
        const T_ell = T_sch * Tt(E[0], 0, E[1]);
        const T_unter = T_ell * Ry(-bt);
        const T_greif = T_unter * Tt(L2 - G_RUECK, 0, 8);
        const T_hebA = T_greif * Tt(0, -G_AB, w.gZ0) * Rz(-gr);
        const T_hebB = T_greif * Tt(0, G_AB, w.gZ0) * Rz(gr);
        const T_pinA = T_sch * Tt(PIN_A[0], -25, PIN_A[1]) * Rx(90);
        const T_pinB = T_sch * Tt(PIN_B[0], 25, PIN_B[1]) * Rx(-90);
        const oyA = -(YW + 0.5 - w.hA);              // Ohr-Oberseite Servo A (y)
        const T_svA = T_sch * Tt(PIN_A[0], oyA, PIN_A[1]) * Rx(90);
        const T_svB = T_sch * Tt(PIN_B[0], -oyA, PIN_B[1]) * Rx(-90);
        const T_svG = T_greif * Tt(0, -G_AB, -5) * Rz(-90);
        const T_achseS = T_ober * Tt(0, -12, 0) * Rx(90);
        const T_achseSt = T_ell * Ry(-al) * Tt(0, -12, 0) * Rx(90);
        const T_achseK = T_kurb * Tt(0, -1, 0) * Rx(-90);
        const T_achseE = T_ell * Ry(-th) * Tt(0, -1, 0) * Rx(-90);
        const T_kurbel = T_kurb * Tt(0, -1, 0) * Rx(-90);
        const T_hebel = T_ell * Ry(-th) * Tt(0, -1, 0) * Rx(-90);

        var teile = {};

        // ================= BASIS =================
        if (definition.zeigeBasis)
        {
            teile.basis = fertig(w, baueBasis(w), "Basis", F_BASIS, identityTransform(), W_PLA);
            teile.deckelring = fertig(w, baueDeckelring(w), "Basis - Deckelring", F_DUNKEL, Tt(0, 0, BASIS_H), W_PETG);
            for (var k = 0; k < 4; k += 1)
            {
                fertig(w, baueRollenachse(w), "Basis - Rollenachse " ~ (k + 1), F_GOLD, Rz(45 + 90 * k) * Tt(ROLLE_R, 0, DECKEL_OBEN + ACHSE_H), W_PETG);
            }
            teile.ritzelBasis = fertig(w, baueRitzelBasis(w, phRitzelBasis), "Ritzel Basis", F_METALL, Tt(SM_WX, 0, KRANZ_UNTEN + 0.5), W_PETG);
            teile.kappe = fertig(w, baueKappe(w), "Basis - Kappe", F_DUNKEL, Tt(0, 0, TT_OBEN + 0.2), W_PLA);
            fertig(w, baueSand(w), "Basis - Sandfüllung", F_SAND, identityTransform(), W_SAND);
            teile.drehteller = fertig(w, baueDrehteller(w), "Schulter - Drehteller", F_WEISS, T_dreh * Tt(0, 0, TT_UNTEN), W_PETG);
            w.g[] = "Basis";
            for (var k = 0; k < 6; k += 1)
            {
                const p = polar((INNEN_R1 + INNEN_R2) / 2, (30 + 60 * k) * PI / 180);
                nt(w, "4762", 10, Tt(p[0], p[1], BASIS_H + DECKEL_T - 2.8));
            }
            for (var k = 0; k < 3; k += 1)
            {
                const p = polar(11.5, (60 + 120 * k) * PI / 180);
                nt(w, "10642", 10, Tt(p[0], p[1], TT_OBEN + 0.2 + 4));
            }
            for (var s in [-1, 1])
            {
                nt(w, "4762", 8, Tt(SM_GX, s * SM_OHR, w.smOZ + 0.8));
            }
            for (var k = 0; k < 4; k += 1)
            {
                const p = polar(BASIS_R - 15, (45 + 90 * k) * PI / 180);
                nt(w, "fuss", 0, Tt(p[0], p[1], 1) * Rx(180));
            }
        }
        // ================= SCHULTER =================
        if (definition.zeigeSchulter)
        {
            teile.gehL = fertig(w, baueGehaeuse(w, -1), "Schulter - Gehäuse links", F_GRAU, T_sch, W_PETG);
            teile.gehR = fertig(w, baueGehaeuse(w, 1), "Schulter - Gehäuse rechts", F_GRAU, T_sch, W_PETG);
            fertig(w, baueGehaeuseDeckel(w, -1), "Schulter - Deckel links", F_DUNKEL, T_sch, W_PETG);
            fertig(w, baueGehaeuseDeckel(w, 1), "Schulter - Deckel rechts", F_DUNKEL, T_sch, W_PETG);
            teile.ritzelA = fertig(w, baueSchulterRitzel(w, phRitzelA), "Ritzel Schulter", F_METALL, T_pinA, W_PETG);
            teile.ritzelB = fertig(w, baueSchulterRitzel(w, phRitzelB), "Ritzel Ellbogen", F_METALL, T_pinB, W_PETG);
            w.g[] = "Schulter";
            for (var p in GH_LOECHER)
            {
                nt(w, "4762", 16, T_sch * Tt(p[0], p[1], HV1 + GH_FLANSCH_V));
            }
            for (var p in VERBINDER)
            {
                nt(w, "4762", 45, T_sch * Tt(p[0], YO - 12, p[1]) * richtung("+y"));
                nt(w, "4032_30", 0, T_sch * Tt(p[0], -YO + 12, p[1]) * richtung("-y"));
            }
            for (var p in DECKEL_SCHRAUBEN)
            {
                nt(w, "4762", 12, T_sch * Tt(p[0], YC - 3, p[1]) * richtung("+y"));
                nt(w, "4762", 12, T_sch * Tt(p[0], -YC + 3, p[1]) * richtung("-y"));
            }
            for (var P in [[PIN_A, -1], [PIN_B, 1]])
            {
                const m = P[0][0] + w.sW - w.sL / 2;
                const oy = P[1] * (YW + 0.5 - w.hA);
                for (var dx in [-1, 1])
                {
                    for (var dv in [-1, 1])
                    {
                        nt(w, "4762", 10, T_sch * Tt(m + dx * w.lA / 2, oy - P[1] * 2.5, P[0][1] + dv * w.lB / 2) * richtung(P[1] < 0 ? "+y" : "-y"));
                    }
                }
            }
            for (var Tp in [[T_pinA, T_svA, "Ritzel Schulter"], [T_pinB, T_svB, "Ritzel Ellbogen"]])
            {
                w.g[] = Tp[2];
                for (var k = 0; k < 4; k += 1)
                {
                    const p = polar(w.hLK / 2, (45 + 90 * k) * PI / 180);
                    nt(w, "10642", 10, Tp[0] * Tt(p[0], p[1], S_ZR_H));
                }
                nt(w, "4762", 6, Tp[1] * Tt(0, 0, w.hA));
            }
        }
        // ================= ARM =================
        if (definition.zeigeArm)
        {
            teile.kasten = fertig(w, baueKasten(w), "Oberarm - Kasten", F_ARM, T_ober, W_PETG);
            fertig(w, baueKastenDeckel(w), "Oberarm - Deckel", F_ARM2, T_ober, W_PETG);
            fertig(w, baueAchse(w, ACHSE_SCHULTER), "Oberarm - Achse Schulter", F_GOLD, T_achseS, W_PETG);
            fertig(w, baueAchse(w, ACHSE_STUMMEL), "Oberarm - Achse Stummel", F_GOLD, T_achseSt, W_PETG);
            teile.zahnradA = fertig(w, baueSchulterZahnrad(w, 180 - 50), "Oberarm - Zahnrad Schulter", F_METALL, T_ober * Tt(0, -24.5, 0) * Rx(90), W_PETG);
            fertig(w, baueRing(w, 1.5), "Oberarm - Distanzring Schulter", F_GOLD, T_sch * Tt(0, -18, 0) * Rx(90), W_PETG);
            fertig(w, baueRing(w, 1), "Oberarm - Distanzring Ellbogen", F_GOLD, T_ell * Tt(0, -18, 0) * Rx(90), W_PETG);

            teile.kurbel = fertig(w, baueKurbel(w), "Kurbel", F_GOLD, T_kurbel, W_PETG);
            fertig(w, baueAchse(w, ACHSE_KURBEL), "Kurbel - Achse", F_GOLD, T_achseK, W_PETG);
            teile.zahnradB = fertig(w, baueSchulterZahnrad(w, 240), "Kurbel - Zahnrad Ellbogen", F_METALL, T_kurb * Tt(0, 24.5, 0) * Rx(-90), W_PETG);
            fertig(w, baueRing(w, 1.5), "Kurbel - Distanzring", F_GOLD, T_sch * Tt(0, 18, 0) * Rx(-90), W_PETG);

            teile.stange = fertig(w, baueKoppelstange(w), "Koppelstange", F_STANGE, T_sch * Tt(K[0], 0, K[1]) * Ry(-al) * Tt(0, -8, 0) * Rx(-90), W_PETG);

            teile.unterarm = fertig(w, baueUnterarmKanal(w), "Unterarm - Kanal", F_ARM, T_unter, W_PETG);
            fertig(w, baueUnterarmDeckel(w), "Unterarm - Deckel", F_ARM2, T_unter, W_PETG);
            teile.hebel = fertig(w, baueKurbel(w), "Unterarm - Hebel", F_GOLD, T_hebel, W_PETG);
            fertig(w, baueAchse(w, ACHSE_ELLBOGEN), "Unterarm - Achse Ellbogen", F_GOLD, T_achseE, W_PETG);
            fertig(w, baueRing(w, 1), "Unterarm - Distanzring", F_GOLD, T_ell * Tt(0, 18, 0) * Rx(-90), W_PETG);

            // Achsschrauben (Mutter steckt verdrehsicher im Achsende) + Scheiben
            w.g[] = "Oberarm";
            nt(w, "7089", 0, T_sch * Tt(0, -YO - LAGER_B, 0) * richtung("-y"));
            nt(w, "4762", 25, T_sch * Tt(0, -YO - LAGER_B - 0.5, 0) * richtung("-y"));
            nt(w, "4032_30", 0, T_achseS * Tt(0, 0, 1.6));
            w.g[] = "Kurbel";
            nt(w, "7089", 0, T_sch * Tt(0, YO + LAGER_B, 0) * richtung("+y"));
            nt(w, "4762", 40, T_sch * Tt(0, YO + LAGER_B + 0.5, 0) * richtung("+y"));
            nt(w, "4032_30", 0, T_achseK * Tt(0, 0, 1.6));
            w.g[] = "Unterarm";
            nt(w, "7089", 0, T_ell * Tt(0, UA_A, 0) * richtung("+y"));
            nt(w, "4762", 25, T_ell * Tt(0, UA_A + 0.5, 0) * richtung("+y"));
            nt(w, "4032_30", 0, T_achseE * Tt(0, 0, 1.6));
            w.g[] = "Oberarm";
            nt(w, "7089", 0, T_ell * Tt(0, -UA_I - LAGER_B, 0) * richtung("-y"));
            nt(w, "4762", 12, T_ell * Tt(0, -UA_I - LAGER_B - 0.5, 0) * richtung("-y"));
            nt(w, "4032_30", 0, T_achseSt * Tt(0, 0, 1.6));
            // Koppelstange: Flachkopf + Stoppmutter in der Kurbeltasche
            w.g[] = "Kurbel";
            nt(w, "7380", 16, T_sch * Tt(K[0], -8, K[1]) * richtung("-y"));
            nt(w, "10511", 0, T_kurbel * Tt(KURBEL, 0, 8 - 4.2));
            w.g[] = "Unterarm";
            nt(w, "7380", 16, T_ell * Tt(K[0], -8, K[1]) * richtung("-y"));
            nt(w, "10511", 0, T_hebel * Tt(KURBEL, 0, 8 - 4.2));
            w.g[] = "Oberarm";
            for (var p in KASTEN_SCHRAUBEN)
            {
                nt(w, "10642", 12, T_ober * Tt(p[0], K_Y, p[1]) * richtung("+y"));
            }
            w.g[] = "Unterarm";
            for (var p in UA_SCHRAUBEN)
            {
                nt(w, "10642", 12, T_unter * Tt(p[0], -UA_A, p[1]) * richtung("-y"));
            }
        }
        // ================= GREIFER =================
        if (definition.zeigeGreifer)
        {
            teile.greifer = fertig(w, baueGreiferGehaeuse(w), "Unterarm - Greifer-Gehäuse", F_GREIFER, T_greif, W_PETG);
            fertig(w, baueGreiferHaube(w), "Unterarm - Greifer-Haube", F_GREIFER, T_greif * Tt(0, 0, 24), W_PETG);
            fertig(w, baueGreiferBoden(w), "Unterarm - Greifer-Boden", F_GREIFER, T_greif * Tt(0, 0, G_UNTEN - 4), W_PETG);
            teile.hebelA = fertig(w, baueGreiferhebel(w, true), "Greiferhebel A (Antrieb)", F_HEBEL, T_hebA, W_PETG);
            fertig(w, baueFinger(w), "Greiferhebel A - Finger (TPU)", F_TPU, T_hebA * Tt(FINGER_X, FINGER_DY, 3), W_TPU);
            fertig(w, baueKlemmplatte(w, 1), "Greiferhebel A - Klemmplatte", F_HEBEL, T_hebA * Tt(FINGER_X, FINGER_DY, 3 + FINGER_H), W_PETG);
            teile.hebelB = fertig(w, baueGreiferhebel(w, false), "Greiferhebel B (Mitläufer)", F_HEBEL, T_hebB, W_PETG);
            fertig(w, baueFinger(w), "Greiferhebel B - Finger (TPU)", F_TPU, T_hebB * Tt(FINGER_X, -FINGER_DY, 3 + FINGER_H) * Rx(180), W_TPU);
            fertig(w, baueKlemmplatte(w, -1), "Greiferhebel B - Klemmplatte", F_HEBEL, T_hebB * Tt(FINGER_X, -FINGER_DY, 3 + FINGER_H), W_PETG);

            w.g[] = "Unterarm";
            for (var f in UA_FLANSCH)
            {
                nt(w, "4762", 12, T_greif * Tt(G_RUECK + 4, f[0], f[1] - 8) * richtung("+x"));
                nt(w, "4032_30", 0, T_unter * Tt(L2 - 3.4, f[0], f[1]) * richtung("-x"));
            }
            for (var dy in [-1, 1])
            {
                for (var dx in [-1, 1])
                {
                    nt(w, "4762", 10, T_greif * Tt(dx * w.lB / 2, -G_AB - w.sW + w.sL / 2 + dy * w.lA / 2, -9.3) * richtung("-z"));
                }
            }
            nt(w, "4762", 25, T_greif * Tt(0, G_AB, 27));
            for (var p in G_HAUBE_SCHR)
            {
                nt(w, "10642", 10, T_greif * Tt(p[0], p[1], 27));
            }
            for (var p in G_BODEN_SCHR)
            {
                nt(w, "4762", 10, T_greif * Tt(p[0], p[1], G_UNTEN - 4 + 3) * richtung("-z"));
            }
            for (var Th in [[T_hebA, 1, "Greiferhebel A"], [T_hebB, -1, "Greiferhebel B"]])
            {
                w.g[] = Th[2];
                for (var y in [-5, 5])
                {
                    nt(w, "4762", 40, Th[0] * Tt(FINGER_X + 4, Th[1] * (y + FINGER_DY), 3 + FINGER_H + 3));
                    nt(w, "4032_0", 0, Th[0] * Tt(FINGER_X + 4, Th[1] * (y + FINGER_DY), 2.4) * richtung("-z"));
                }
            }
            w.g[] = "Greiferhebel A";
            for (var k = 0; k < 4; k += 1)
            {
                const p = polar(w.hLK / 2, (45 + 90 * k) * PI / 180);
                nt(w, "10642", 10, T_hebA * Tt(p[0], p[1], G_ZR_H));
            }
            nt(w, "4762", 6, T_hebA * Tt(0, 0, 2));
        }
        // ================= KAUFTEILE =================
        if (definition.zeigeKaufteile)
        {
            if (definition.zeigeBasis)
            {
                baueSchrittmotor(w, Tt(SM_WX, 0, w.smOZ));
                var rollen = [];
                for (var k = 0; k < 4; k += 1)
                {
                    rollen = append(rollen, Rz(45 + 90 * k) * Tt(ROLLE_R - LAGER_B / 2, 0, DECKEL_OBEN + ACHSE_H) * Ry(90));
                }
                baueLager(w, rollen, "Basis");
                baueLichtschranke(w);
            }
            if (definition.zeigeSchulter)
            {
                baueLager(w, [T_sch * Tt(0, -YW, 0) * Rx(-90), T_sch * Tt(0, YW - LAGER_B, 0) * Rx(-90),
                              T_sch * Tt(0, -YO - LAGER_B, 0) * Rx(-90), T_sch * Tt(0, YO, 0) * Rx(-90)], "Schulter");
                baueServo(w, [T_svA, T_svB], false, "Schulter - Servo DS3218");
                baueServoscheibe(w, T_svA, "Ritzel Schulter - Servoscheibe");
                baueServoscheibe(w, T_svB, "Ritzel Ellbogen - Servoscheibe");
            }
            if (definition.zeigeArm)
            {
                baueLager(w, [T_ober * Tt(0, K_Y - LAGER_B, 0) * Rx(-90), T_ober * Tt(L1, K_Y - LAGER_B, 0) * Rx(-90)], "Oberarm");
                baueLager(w, [T_ell * Tt(0, -UA_I - LAGER_B, 0) * Rx(-90)], "Unterarm");
            }
            if (definition.zeigeGreifer)
            {
                baueServo(w, [T_svG], true, "Unterarm - Servo MG996R (Greifer)");
                baueServoscheibe(w, T_hebA * Tt(0, 0, 2 - w.hA) , "Greiferhebel A - Servoscheibe");
            }
        }
        if (definition.zeigeNormteile)
        {
            baueNormteile(w);
        }
        if (definition.zeigeElektronik)
        {
            baueElektronik(w);
        }
        // ================= MATE CONNECTORS =================
        if (definition.zeigeConnectors)
        {
            const Y = vector(0, 1, 0);  const Z = vector(0, 0, 1);  const X = vector(1, 0, 0);
            gelenk(w, teile, ["basis", "drehteller"], T_dreh * Tt(0, 0, TT_OBEN), Z, X);                       // J1 Basis
            gelenk(w, teile, ["gehL", "kasten"], T_sch, Y, X);                                                // J2 Schulter
            gelenk(w, teile, ["gehR", "kurbel"], T_sch, Y, X);                                                // J3 Kurbel
            gelenk(w, teile, ["kasten", "unterarm"], T_ell, Y, X);                                            // J4 Ellbogen
            gelenk(w, teile, ["kurbel", "stange"], T_sch * Tt(K[0], 0, K[1]), Y, X);                          // J5 Stange oben
            gelenk(w, teile, ["hebel", "stange"], T_ell * Tt(K[0], 0, K[1]), Y, X);                           // J6 Stange unten
            gelenk(w, teile, ["greifer", "hebelA"], T_greif * Tt(0, -G_AB, w.gZ0), Z, X);                     // J7 Greifer A
            gelenk(w, teile, ["greifer", "hebelB"], T_greif * Tt(0, G_AB, w.gZ0), Z, X);                      // J8 Greifer B
            gelenk(w, teile, ["gehL", "ritzelA"], T_sch * Tt(PIN_A[0], -25, PIN_A[1]), Y, X);                 // J9 Ritzel Schulter
            gelenk(w, teile, ["gehR", "ritzelB"], T_sch * Tt(PIN_B[0], 25, PIN_B[1]), Y, X);                  // J10 Ritzel Ellbogen
            gelenk(w, teile, ["basis", "ritzelBasis"], Tt(SM_WX, 0, KRANZ_UNTEN + 0.5), Z, X);               // J11 Ritzel Basis
            gelenk(w, teile, ["kasten", "zahnradA"], T_sch * Tt(0, -24.5, 0), Y, X);
            gelenk(w, teile, ["kurbel", "zahnradB"], T_sch * Tt(0, 24.5, 0), Y, X);
        }
    });

// =====================================================================================
//  Grundwerkzeuge
// =====================================================================================
function nid(w is map) returns Id
{
    w.z[] = w.z[] + 1;
    return w.id + ("n" ~ w.z[]);
}

function V(p is array) returns Vector { return vector(p[0], p[1], p[2]) * millimeter; }
function V2(p is array) returns Vector { return vector(p[0], p[1]) * millimeter; }
function Tt(x is number, y is number, z is number) returns Transform { return transform(vector(x, y, z) * millimeter); }
function Rx(a is number) returns Transform { return rotationAround(line(vector(0, 0, 0) * millimeter, vector(1, 0, 0)), a * degree); }
function Ry(a is number) returns Transform { return rotationAround(line(vector(0, 0, 0) * millimeter, vector(0, 1, 0)), a * degree); }
function Rz(a is number) returns Transform { return rotationAround(line(vector(0, 0, 0) * millimeter, vector(0, 0, 1)), a * degree); }

// lokale +z-Achse eines Normteils (Kopfseite) in die gewünschte Richtung drehen
function richtung(r is string) returns Transform
{
    if (r == "+y") { return Rx(-90); }
    if (r == "-y") { return Rx(90); }
    if (r == "+x") { return Ry(90); }
    if (r == "-x") { return Ry(-90); }
    if (r == "-z") { return Rx(180); }
    return identityTransform();
}

function dreh2(p is array, a is number) returns array
{
    return [p[0] * cos(a * degree) - p[1] * sin(a * degree), p[0] * sin(a * degree) + p[1] * cos(a * degree)];
}

function polar(r is number, a is number) returns array { return [r * cos(a * radian), r * sin(a * radian)]; }

function zyl(w is map, p1 is array, p2 is array, d is number) returns Query
{
    const i = nid(w);
    fCylinder(w.ctx, i, { "bottomCenter" : V(p1), "topCenter" : V(p2), "radius" : d / 2 * millimeter });
    return qCreatedBy(i, EntityType.BODY);
}

function kegel(w is map, p1 is array, p2 is array, d1 is number, d2 is number) returns Query
{
    const i = nid(w);
    fCone(w.ctx, i, { "bottomCenter" : V(p1), "topCenter" : V(p2), "bottomRadius" : d1 / 2 * millimeter, "topRadius" : d2 / 2 * millimeter });
    return qCreatedBy(i, EntityType.BODY);
}

function kugel(w is map, p is array, d is number) returns Query
{
    const i = nid(w);
    fSphere(w.ctx, i, { "center" : V(p), "radius" : d / 2 * millimeter });
    return qCreatedBy(i, EntityType.BODY);
}

function quader(w is map, c is array, g is array) returns Query
{
    const i = nid(w);
    fCuboid(w.ctx, i, { "corner1" : V(c), "corner2" : V([c[0] + g[0], c[1] + g[1], c[2] + g[2]]) });
    return qCreatedBy(i, EntityType.BODY);
}

function ZY(w is map, d is number, y1 is number, y2 is number, x is number, v is number) returns Query { return zyl(w, [x, y1, v], [x, y2, v], d); }
function ZZ(w is map, d is number, x is number, y is number, z1 is number, z2 is number) returns Query { return zyl(w, [x, y, z1], [x, y, z2], d); }
function ZX(w is map, d is number, x1 is number, x2 is number, y is number, z is number) returns Query { return zyl(w, [x1, y, z], [x2, y, z], d); }

function rohr(w is map, ri is number, ra is number, z1 is number, z2 is number) returns Query
{
    return abzug(w, ZZ(w, 2 * ra, 0, 0, z1, z2), [ZZ(w, 2 * ri, 0, 0, z1 - 1, z2 + 1)]);
}

function bewege(w is map, q is Query, T is Transform) returns Query
{
    opTransform(w.ctx, nid(w), { "bodies" : q, "transform" : T });
    return q;
}

function vereinige(w is map, qs is array) returns Query
{
    const q = qUnion(qs);
    if (size(evaluateQuery(w.ctx, q)) > 1)
    {
        opBoolean(w.ctx, nid(w), { "tools" : q, "operationType" : BooleanOperationType.UNION });
    }
    return q;
}

function abzug(w is map, ziel is Query, werkzeuge is array) returns Query
{
    const t = qUnion(werkzeuge);
    if (size(evaluateQuery(w.ctx, t)) > 0)
    {
        opBoolean(w.ctx, nid(w), { "targets" : ziel, "tools" : t, "operationType" : BooleanOperationType.SUBTRACTION });
    }
    return ziel;
}

function schnitt(w is map, a is Query, b is Query) returns Query
{
    const q = qUnion([a, b]);
    opBoolean(w.ctx, nid(w), { "tools" : q, "operationType" : BooleanOperationType.INTERSECTION });
    return q;
}

// Skizze aus Kreisen [x, y, r] und Polygonen [[x, y], ...] auf einer Ebene, um h extrudiert
function profil(w is map, ursprung is array, normale is array, xr is array, h is number, kreise is array, polys is array) returns Query
{
    const sid = nid(w);
    const eid = nid(w);
    const n = vector(normale[0], normale[1], normale[2]);
    const sk = newSketchOnPlane(w.ctx, sid, { "sketchPlane" : plane(V(ursprung), n, vector(xr[0], xr[1], xr[2])) });
    var k = 0;
    for (var c in kreise)
    {
        k += 1;
        skCircle(sk, "k" ~ k, { "center" : V2(c), "radius" : c[2] * millimeter });
    }
    for (var p in polys)
    {
        k += 1;
        var pts = [];
        for (var q in p)
        {
            pts = append(pts, V2(q));
        }
        pts = append(pts, V2(p[0]));
        skPolyline(sk, "p" ~ k, { "points" : pts });
    }
    skSolve(sk);
    opExtrude(w.ctx, eid, { "entities" : qSketchRegion(sid), "direction" : n, "endBound" : BoundingType.BLIND, "endDepth" : h * millimeter });
    opDeleteBodies(w.ctx, nid(w), { "entities" : qCreatedBy(sid) });
    return vereinige(w, [qCreatedBy(eid, EntityType.BODY)]);
}

function PZ(w is map, z1 is number, z2 is number, kreise is array, polys is array) returns Query
{
    return profil(w, [0, 0, z1], [0, 0, 1], [1, 0, 0], z2 - z1, kreise, polys);
}

function PXV(w is map, y1 is number, y2 is number, kreise is array, polys is array) returns Query
{
    return profil(w, [0, y2, 0], [0, -1, 0], [1, 0, 0], y2 - y1, kreise, polys);
}

// Kreis (Radius r, Mittelpunkt im Abstand R, Höhe zc) um die z-Achse drehen -> Torus
function torus(w is map, R is number, r is number, zc is number) returns Query
{
    const sid = nid(w);
    const rid = nid(w);
    const sk = newSketchOnPlane(w.ctx, sid, { "sketchPlane" : plane(V([0, 0, 0]), vector(0, -1, 0), vector(1, 0, 0)) });
    skCircle(sk, "k", { "center" : V2([R, zc]), "radius" : r * millimeter });
    skSolve(sk);
    opRevolve(w.ctx, rid, { "entities" : qSketchRegion(sid), "axis" : line(V([0, 0, 0]), vector(0, 0, 1)), "angleForward" : 360 * degree });
    opDeleteBodies(w.ctx, nid(w), { "entities" : qCreatedBy(sid) });
    return qCreatedBy(rid, EntityType.BODY);
}

// Kugelkappe: Grundfläche Ø d bei z = 0, Höhe h nach +z
function kugelkappe(w is map, d is number, h is number) returns Query
{
    const R = (d * d / 4 + h * h) / (2 * h);
    return schnitt(w, kugel(w, [0, 0, h - R], 2 * R), ZZ(w, d + 0.01, 0, 0, 0, h));
}

function tangenten(a is array, r1 is number, b is array, r2 is number) returns array
{
    const dx = b[0] - a[0];
    const dy = b[1] - a[1];
    const L = sqrt(dx * dx + dy * dy);
    const phi = atan2(dy, dx);
    const be = acos((r1 - r2) / L);
    const p1 = phi + be;
    const p2 = phi - be;
    return [[a[0] + r1 * cos(p1), a[1] + r1 * sin(p1)], [b[0] + r2 * cos(p1), b[1] + r2 * sin(p1)],
            [b[0] + r2 * cos(p2), b[1] + r2 * sin(p2)], [a[0] + r1 * cos(p2), a[1] + r1 * sin(p2)]];
}

function tangentenpunkt(c is array, r is number, p is array, seite is number) returns array
{
    const dx = p[0] - c[0];
    const dy = p[1] - c[1];
    const d = sqrt(dx * dx + dy * dy);
    const a = atan2(dy, dx) + seite * acos(r / d);
    return [c[0] + r * cos(a), c[1] + r * sin(a)];
}

function kreuz(o is array, a is array, b is array) returns number
{
    return (a[0] - o[0]) * (b[1] - o[1]) - (a[1] - o[1]) * (b[0] - o[0]);
}

function huelle(punkte is array) returns array
{
    const p = sort(punkte, function(a, b) { return (a[0] == b[0]) ? (a[1] - b[1]) : (a[0] - b[0]); });
    var unten = [];
    for (var q in p)
    {
        while (size(unten) >= 2 && kreuz(unten[size(unten) - 2], unten[size(unten) - 1], q) <= 0)
        {
            unten = subArray(unten, 0, size(unten) - 1);
        }
        unten = append(unten, q);
    }
    var oben = [];
    for (var i = size(p) - 1; i >= 0; i -= 1)
    {
        const q = p[i];
        while (size(oben) >= 2 && kreuz(oben[size(oben) - 2], oben[size(oben) - 1], q) <= 0)
        {
            oben = subArray(oben, 0, size(oben) - 1);
        }
        oben = append(oben, q);
    }
    return concatenateArrays([subArray(unten, 0, size(unten) - 1), subArray(oben, 0, size(oben) - 1)]);
}

// Polygon (gegen den Uhrzeigersinn) um d nach innen versetzen
function versetzt(pts is array, d is number) returns array
{
    const n = size(pts);
    var erg = [];
    for (var i = 0; i < n; i += 1)
    {
        const a = pts[(i + n - 1) % n];
        const b = pts[i];
        const c = pts[(i + 1) % n];
        const l1 = sqrt((b[0] - a[0]) ^ 2 + (b[1] - a[1]) ^ 2);
        const l2 = sqrt((c[0] - b[0]) ^ 2 + (c[1] - b[1]) ^ 2);
        const n1 = [-(b[1] - a[1]) / l1, (b[0] - a[0]) / l1];
        const n2 = [-(c[1] - b[1]) / l2, (c[0] - b[0]) / l2];
        const r1 = n1[0] * a[0] + n1[1] * a[1] + d;
        const r2 = n2[0] * b[0] + n2[1] * b[1] + d;
        const det = n1[0] * n2[1] - n1[1] * n2[0];
        erg = append(erg, [(r1 * n2[1] - r2 * n1[1]) / det, (n1[0] * r2 - n2[0] * r1) / det]);
    }
    return erg;
}

function sechseck(s is number, drehung is number, mx is number, my is number) returns array   // s = Schlüsselweite
{
    var pts = [];
    const r = s / (2 * cos(30 * degree));
    for (var k = 0; k < 6; k += 1)
    {
        pts = append(pts, [mx + r * cos((drehung + 60 * k) * degree), my + r * sin((drehung + 60 * k) * degree)]);
    }
    return pts;
}

function sechseckRadius(d is number, drehung is number, mx is number, my is number) returns array  // d = Eckmaß
{
    return sechseck(d * cos(30 * degree), drehung, mx, my);
}

// ---------- Evolventen-Zahnrad ----------
function flankenpunkte(m is number, z is number, N is number) returns map
{
    const rp = m * z / 2;
    const ra = rp + m;
    const rf = rp - 1.25 * m;
    const rb = rp * cos(20 * degree);
    const tmax = sqrt((ra / rb) ^ 2 - 1);
    const t0 = (rf > rb) ? sqrt((rf / rb) ^ 2 - 1) : 0;
    const inv20 = tan(20 * degree) - 20 * PI / 180;
    const halb = PI / (2 * z) + inv20 - 0.08 / rp / 2;
    var fl = [];
    for (var i = 0; i <= N; i += 1)
    {
        const t = t0 + (tmax - t0) * i / N;
        const x = rb * (cos(t * radian) + t * sin(t * radian));
        const y = rb * (sin(t * radian) - t * cos(t * radian));
        fl = append(fl, [sqrt(x * x + y * y), atan2(y, x) / radian - halb]);
    }
    return { "fl" : fl, "rf" : rf, "rb" : rb, "ra" : ra };
}

function zahnradPunkte(m is number, z is number, phase is number) returns array
{
    const g = flankenpunkte(m, z, 6);
    const fl = g.fl;
    const N = 6;
    var pts = [];
    for (var k = 0; k < z; k += 1)
    {
        const c = 2 * PI * k / z + phase * PI / 180;
        if (g.rf < g.rb) { pts = append(pts, polar(g.rf, c + fl[0][1])); }
        for (var i = 0; i <= N; i += 1) { pts = append(pts, polar(fl[i][0], c + fl[i][1])); }
        for (var i = N; i >= 0; i -= 1) { pts = append(pts, polar(fl[i][0], c - fl[i][1])); }
        if (g.rf < g.rb) { pts = append(pts, polar(g.rf, c - fl[0][1])); }
        const a1 = c - fl[0][1];
        const a2 = c + 2 * PI / z + fl[0][1];
        for (var j = 1; j <= 2; j += 1) { pts = append(pts, polar(g.rf, a1 + (a2 - a1) * j / 3)); }
    }
    return pts;
}

// Zahnrad z1..z2; exakt: Flanken als Splines, Kopf- und Fußkreis als Bögen
function zahnrad(w is map, m is number, z is number, z1 is number, z2 is number, phase is number) returns Query
{
    if (!w.exakt)
    {
        return PZ(w, z1, z2, [], [zahnradPunkte(m, z, phase)]);
    }
    const N = 8;
    const g = flankenpunkte(m, z, N);
    const fl = g.fl;
    const sid = nid(w);
    const eid = nid(w);
    const sk = newSketchOnPlane(w.ctx, sid, { "sketchPlane" : plane(V([0, 0, z1]), vector(0, 0, 1), vector(1, 0, 0)) });
    for (var k = 0; k < z; k += 1)
    {
        const c = 2 * PI * k / z + phase * PI / 180;
        var unten = [];
        var oben = [];
        for (var i = 0; i <= N; i += 1)
        {
            unten = append(unten, V2(polar(fl[i][0], c + fl[i][1])));
            oben = append(oben, V2(polar(fl[N - i][0], c - fl[N - i][1])));
        }
        if (g.rf < g.rb)
        {
            skLineSegment(sk, "a" ~ k, { "start" : V2(polar(g.rf, c + fl[0][1])), "end" : unten[0] });
            skLineSegment(sk, "b" ~ k, { "start" : oben[N], "end" : V2(polar(g.rf, c - fl[0][1])) });
        }
        skFitSpline(sk, "u" ~ k, { "points" : unten });
        skArc(sk, "s" ~ k, { "start" : unten[N], "mid" : V2(polar(g.ra, c)), "end" : oben[0] });
        skFitSpline(sk, "o" ~ k, { "points" : oben });
        const a1 = c - fl[0][1];
        const a2 = c + 2 * PI / z + fl[0][1];
        skArc(sk, "f" ~ k, { "start" : V2(polar(g.rf, a1)), "mid" : V2(polar(g.rf, (a1 + a2) / 2)), "end" : V2(polar(g.rf, a2)) });
    }
    skSolve(sk);
    opExtrude(w.ctx, eid, { "entities" : qSketchRegion(sid), "direction" : vector(0, 0, 1), "endBound" : BoundingType.BLIND, "endDepth" : (z2 - z1) * millimeter });
    opDeleteBodies(w.ctx, nid(w), { "entities" : qCreatedBy(sid) });
    return vereinige(w, [qCreatedBy(eid, EntityType.BODY)]);
}

function setzeWerkstoff(w is map, q is Query, wst is array)
{
    if (w.werkstoff && wst[1] > 0)
    {
        setProperty(w.ctx, { "entities" : q, "propertyType" : PropertyType.MATERIAL, "value" : material(wst[0], wst[1] * gram / centimeter ^ 3) });
    }
}

// Werkstoff so wählen, dass das Teil die angegebene Masse in Gramm hat
function setzeMasse(w is map, q is Query, name is string, masse is number)
{
    if (w.werkstoff)
    {
        const v = evVolume(w.ctx, { "entities" : q });
        setProperty(w.ctx, { "entities" : q, "propertyType" : PropertyType.MATERIAL, "value" : material(name, masse * gram / v) });
    }
}

function fertig(w is map, q is Query, name is string, farbe is Color, T is Transform, wst is array) returns Query
{
    opTransform(w.ctx, nid(w), { "bodies" : q, "transform" : T });
    setProperty(w.ctx, { "entities" : q, "propertyType" : PropertyType.NAME, "value" : name });
    setProperty(w.ctx, { "entities" : q, "propertyType" : PropertyType.APPEARANCE, "value" : farbe });
    setzeWerkstoff(w, q, wst);
    return q;
}

// Ein Teil einmal bauen und an mehrere Stellen kopieren (Muster)
function verteile(w is map, q is Query, Ts is array, name is string, farbe is Color, wst is array) returns Query
{
    var namen = [];
    for (var i = 0; i < size(Ts); i += 1)
    {
        namen = append(namen, "i" ~ i);
    }
    const pid = nid(w);
    opPattern(w.ctx, pid, { "entities" : q, "transforms" : Ts, "instanceNames" : namen });
    opDeleteBodies(w.ctx, nid(w), { "entities" : q });
    const kopien = qCreatedBy(pid, EntityType.BODY);
    setProperty(w.ctx, { "entities" : kopien, "propertyType" : PropertyType.NAME, "value" : name });
    setProperty(w.ctx, { "entities" : kopien, "propertyType" : PropertyType.APPEARANCE, "value" : farbe });
    setzeWerkstoff(w, kopien, wst);
    return kopien;
}

function gelenk(w is map, teile is map, namen is array, T is Transform, achse is Vector, xr is Vector)
{
    for (var n in namen)
    {
        if (teile[n] == undefined)
        {
            return;
        }
    }
    const cs = coordSystem(T * (vector(0, 0, 0) * millimeter), T.linear * xr, T.linear * achse);
    for (var n in namen)
    {
        opMateConnector(w.ctx, nid(w), { "coordSystem" : cs, "owner" : teile[n] });
    }
}

// =====================================================================================
//  BASIS
// =====================================================================================
function rippenUndKanal(w is map) returns array
{
    var r = [quader(w, [-9, -(BASIS_R - 0.5), 0], [18, BASIS_R - 0.5 - INNEN_R1, 17])];
    for (var k = 0; k < 8; k += 1)
    {
        r = append(r, bewege(w, quader(w, [INNEN_R2 - 1, -1.2, 0], [BASIS_R - INNEN_R2, 2.4, 30]), Rz(22.5 + 45 * k)));
    }
    return r;
}

function baueBasis(w is map) returns Query
{
    var teile = concatenateArrays([[
        ZZ(w, 2 * BASIS_R, 0, 0, 0, BODEN_T),
        rohr(w, BASIS_R - AUSSEN_T, BASIS_R, 0, BASIS_H),
        rohr(w, INNEN_R1, INNEN_R2, 0, BASIS_H),
        ZZ(w, 2 * SAEULE_R, 0, 0, 0, TT_OBEN + 0.2),
        quader(w, [SM_GX - 4, -SM_OHR, 0], [8, 2 * SM_OHR, 8]),
        quader(w, [-(ROCK_R1 - 2), -5, 0], [ROCK_R1 - SAEULE_R, 10, LSK_Z + 5]),
        quader(w, [-(ROCK_R2 + 10), -5, 0], [8, 10, LSK_Z + 5]),
        quader(w, [-INNEN_R1 - 1, -1.5, 0], [INNEN_R1 + 1 - ROCK_R2 - 9, 3, BASIS_H])
    ], rippenUndKanal(w)]);
    for (var s in [-1, 1])
    {
        teile = append(teile, quader(w, [SM_GX - 4, s * SM_OHR - 3, 0], [8, 6, w.smOZ]));
        teile = append(teile, quader(w, [SM_GX, s * SM_OHR - 1.5, 0], [INNEN_R1 - SM_GX + 1, 3, BASIS_H]));
    }
    const b = vereinige(w, teile);
    var wz = [
        ZZ(w, 2 * KABEL_R, 0, 0, -1, 199),
        quader(w, [-6, -(SAEULE_R + 2), BODEN_T], [12, SAEULE_R + 2, 18]),
        quader(w, [-7, -(INNEN_R1 + 54), BODEN_T], [14, 60, 12]),
        zyl(w, [-(ROCK_R2 + 12), 0, LSK_Z], [-(ROCK_R2), 0, LSK_Z], 5.2),
        zyl(w, [-ROCK_R1, 0, LSK_Z], [-KABEL_R + 1, 0, LSK_Z], 5.1)
    ];
    for (var k = 0; k < 6; k += 1)
    {
        const p = polar((INNEN_R1 + INNEN_R2) / 2, (30 + 60 * k) * PI / 180);
        wz = append(wz, ZZ(w, 2.6, p[0], p[1], BASIS_H - 12, BASIS_H + 1));
    }
    for (var k = 0; k < 3; k += 1)
    {
        const p = polar(11.5, (60 + 120 * k) * PI / 180);
        wz = append(wz, ZZ(w, 2.6, p[0], p[1], TT_OBEN - 10, TT_OBEN + 1));
    }
    for (var s in [-1, 1])
    {
        wz = append(wz, ZZ(w, 2.6, SM_GX, s * SM_OHR, w.smOZ - 9, w.smOZ + 1));
    }
    for (var k = 0; k < 4; k += 1)
    {
        const p = polar(BASIS_R - 15, (45 + 90 * k) * PI / 180);
        wz = append(wz, ZZ(w, 12, p[0], p[1], -0.01, 1));
    }
    return abzug(w, b, wz);
}

function baueSand(w is map) returns Query
{
    return abzug(w, rohr(w, INNEN_R2, BASIS_R - AUSSEN_T, BODEN_T, BASIS_H - 1), rippenUndKanal(w));
}

function tropfen(w is map, x1 is number, x2 is number, zc is number, d is number) returns Query
{
    const r = d / 2;
    return profil(w, [x1, 0, 0], [1, 0, 0], [0, 1, 0], x2 - x1, [[0, zc, r]],
        [[[-r / sqrt(2), zc + r / sqrt(2)], [0, zc + r * sqrt(2)], [r / sqrt(2), zc + r / sqrt(2)]]]);
}

function baueDeckelring(w is map) returns Query
{
    var teile = [rohr(w, INNEN_R1, BASIS_R, 0, DECKEL_T)];
    for (var k = 0; k < 4; k += 1)
    {
        for (var s in [-1, 1])
        {
            teile = append(teile, bewege(w, quader(w, [ROLLE_R + s * 4.8 - 2, -8, 0], [4, 16, DECKEL_T + ACHSE_H + LAGER_DI / 2 + 3]), Rz(45 + 90 * k)));
        }
    }
    const d = vereinige(w, teile);
    var wz = [];
    for (var k = 0; k < 6; k += 1)
    {
        const p = polar((INNEN_R1 + INNEN_R2) / 2, (30 + 60 * k) * PI / 180);
        wz = append(wz, ZZ(w, 3.3, p[0], p[1], -1, 9));
        wz = append(wz, ZZ(w, 6.2, p[0], p[1], DECKEL_T - 2.8, DECKEL_T + 0.2));
    }
    for (var k = 0; k < 4; k += 1)
    {
        wz = append(wz, bewege(w, tropfen(w, ROLLE_R - 15, ROLLE_R + 15, DECKEL_T + ACHSE_H, LAGER_DI + 0.15), Rz(45 + 90 * k)));
    }
    return abzug(w, d, wz);
}

function baueRollenachse(w is map) returns Query
{
    const l = LAGER_B + 0.6 + 8;
    return abzug(w, zyl(w, [-l / 2, 0, 0], [l / 2, 0, 0], LAGER_DI - 0.05), [quader(w, [-l, -LAGER_DI, -LAGER_DI / 2 - 1], [2 * l, 2 * LAGER_DI, 1.5])]);
}

function baueDrehteller(w is map) returns Query
{
    const kranz = abzug(w, zahnrad(w, ZR_M, ZR_Z2, -8, 0.01, 0), [ZZ(w, 2 * KRANZ_RI, 0, 0, -9, 2)]);
    const blende = schnitt(w, rohr(w, ROCK_R1, ROCK_R2, -10, 0.01), quader(w, [-50, -50, -11], [100, 50, 40]));
    const t = vereinige(w, [
        ZZ(w, 2 * TT_R, 0, 0, 0, TT_T),
        rohr(w, TT_R - 1, SCHUERZE_R + 2.4, TT_T - 2, TT_T),
        rohr(w, SCHUERZE_R, SCHUERZE_R + 2.4, -(TT_UNTEN - (DECKEL_OBEN - 2)), TT_T),
        kranz, blende
    ]);
    var wz = [ZZ(w, 2 * (SAEULE_R + 0.4), 0, 0, -20, 20)];
    for (var p in GH_LOECHER)
    {
        wz = append(wz, ZZ(w, 2.6, p[0], p[1], -12, 18));
    }
    return abzug(w, t, wz);
}

function baueRitzelBasis(w is map, phase is number) returns Query
{
    const h = RITZEL_OBEN - (KRANZ_UNTEN + 0.5);
    const loch = schnitt(w, ZZ(w, 5.25, 0, 0, -1, 19), quader(w, [-1.575, -5, -20], [3.15, 10, 40]));
    return abzug(w, zahnrad(w, ZR_M, ZR_Z1, 0, h, phase), [loch]);
}

function baueKappe(w is map) returns Query
{
    var wz = [ZZ(w, 2 * KABEL_R, 0, 0, -1, 5)];
    for (var k = 0; k < 3; k += 1)
    {
        const p = polar(11.5, (60 + 120 * k) * PI / 180);
        wz = append(wz, ZZ(w, 3.3, p[0], p[1], -1, 5));
        wz = append(wz, kegel(w, [p[0], p[1], 3], [p[0], p[1], 5.01], 3.3, 6.5));
    }
    return abzug(w, ZZ(w, 40, 0, 0, 0, 4), wz);
}

// =====================================================================================
//  SCHULTERGEHÄUSE  (x vorne, y seitlich, z = v; Schulterachse im Ursprung)
// =====================================================================================
function rundRechteck(w is map, y1 is number, y2 is number) returns Query
{
    const r = 10;
    return PXV(w, y1, y2,
        [[HX1 + r, HV1 + r, r], [HX2 - r, HV1 + r, r], [HX2 - r, HV2 - r, r], [HX1 + r, HV2 - r, r]],
        [[[HX1, HV1 + r], [HX2, HV1 + r], [HX2, HV2 - r], [HX1, HV2 - r]],
         [[HX1 + r, HV1], [HX2 - r, HV1], [HX2 - r, HV2], [HX1 + r, HV2]]]);
}

function servotasche(w is map, P is array, s is number) returns array
{
    const oy = s * (YW + 0.5 - w.hA);
    const m = P[0] + w.sW - w.sL / 2;
    const ohrL = w.sL + 13.8;
    var wz = [
        quader(w, [P[0] + w.sW - w.sL - 0.4, min(oy, s * (YW + 0.1)), P[1] - w.sB / 2 - 0.4], [w.sL + 0.8, abs(s * (YW + 0.1) - oy), w.sB + 0.8]),
        quader(w, [m - ohrL / 2 - 0.5, min(oy, oy - s * (w.sU + 0.8)), P[1] - w.sB / 2 - 0.4], [ohrL + 1, w.sU + 0.8, w.sB + 0.8])
    ];
    for (var dx in [-1, 1])
    {
        for (var dv in [-1, 1])
        {
            wz = append(wz, zyl(w, [m + dx * w.lA / 2, oy, P[1] + dv * w.lB / 2], [m + dx * w.lA / 2, oy + s * 12, P[1] + dv * w.lB / 2], 2.6));
        }
    }
    if (s < 0)
    {
        wz = append(wz, ZY(w, w.hD + 5, -YW - 1, -YW + 6, P[0], P[1]));
    }
    else
    {
        wz = append(wz, ZY(w, w.hD + 5, YW - 6, YW + 1, P[0], P[1]));
    }
    return wz;
}

function baueGehaeuse(w is map, haelfte is number) returns Query
{
    const g = vereinige(w, [rundRechteck(w, -YO, YO), quader(w, [-80, -YC - 12, HV1], [100, 2 * (YC + 12), GH_FLANSCH_V])]);
    var polys = [];
    for (var a = ALPHA_MIN; a < ALPHA_MAX; a += 5)
    {
        var pts = [];
        for (var aa in [a, a + 5])
        {
            for (var e in [[0, -K_H - 2], [300, -K_H - 2], [300, K_H + 2], [0, K_H + 2]])
            {
                pts = append(pts, dreh2(e, aa));
            }
        }
        polys = append(polys, huelle(pts));
    }
    var wz = [
        PXV(w, -YI, YI, [[0, 0, 31]], polys),
        PXV(w, -YO - 1, -YW, [[0, 0, 36], [PIN_A[0], PIN_A[1], 19.5]], [tangenten([0, 0], 36, PIN_A, 19.5)]),
        PXV(w, YW, YO + 1, [[0, 0, 36], [PIN_B[0], PIN_B[1], 19.5]], [tangenten([0, 0], 36, PIN_B, 19.5)]),
        ZY(w, SITZ_D, -YW - 0.01, -YW + LAGER_B, 0, 0),
        ZY(w, SITZ_D, YW - LAGER_B, YW + 0.01, 0, 0),
        ZY(w, LIPPE_D, -YW, YW, 0, 0),
        ZZ(w, 44, 0, 0, HV1 - 1, HV1 + 6),
        ZZ(w, 18, 0, 0, HV1 - 1, HV1 + 11),
        ZZ(w, 10, -45, 12, -64, -12),
        ZZ(w, 10, -10, 0, -64, -24),
        PZ(w, HV1 + 4, HV1 + 10, [[-45, 12, 5], [0, 0, 5]], [tangenten([-45, 12], 5, [0, 0], 5)])
    ];
    wz = concatenateArrays([wz, servotasche(w, PIN_A, -1), servotasche(w, PIN_B, 1)]);
    for (var p in DECKEL_SCHRAUBEN)
    {
        wz = append(wz, ZY(w, 2.6, YO - 12, YO + 1, p[0], p[1]));
        wz = append(wz, ZY(w, 2.6, -YO - 1, -YO + 12, p[0], p[1]));
    }
    for (var p in VERBINDER)
    {
        wz = append(wz, ZY(w, 3.3, -YO - 1, YO + 1, p[0], p[1]));
        wz = append(wz, ZY(w, 6.5, YO - 12, YO + 1, p[0], p[1]));
        wz = append(wz, profil(w, [0, -YO + 12, 0], [0, -1, 0], [1, 0, 0], 13, [], [sechseckRadius(6.6, 30, p[0], p[1])]));
    }
    for (var p in GH_LOECHER)
    {
        wz = append(wz, ZZ(w, 3.3, p[0], p[1], HV1 - 1, HV1 + 19));
        wz = append(wz, ZZ(w, 6.5, p[0], p[1], HV1 + GH_FLANSCH_V - 0.01, HV1 + GH_FLANSCH_V + 19));
    }
    if (haelfte < 0)
    {
        wz = append(wz, quader(w, [-200, 0, -200], [400, 100, 400]));
    }
    else
    {
        wz = append(wz, quader(w, [-200, -100, -200], [400, 100, 400]));
    }
    return abzug(w, g, wz);
}

function ys(s is number, y1 is number, y2 is number) returns array
{
    return (s > 0) ? [y1, y2] : [-y2, -y1];
}

function baueGehaeuseDeckel(w is map, s is number) returns Query
{
    const yy = ys(s, YO, YC);
    const d = rundRechteck(w, yy[0], yy[1]);
    var wz = [quader(w, [-200, -100, -200], [400, 200, 200 + DECKEL_UNTEN])];
    var a = ys(s, YO - 0.01, YO + LAGER_B);
    wz = append(wz, ZY(w, SITZ_D, a[0], a[1], 0, 0));
    a = ys(s, YO, YC + 1);
    wz = append(wz, ZY(w, LIPPE_D, a[0], a[1], 0, 0));
    for (var p in DECKEL_SCHRAUBEN)
    {
        a = ys(s, YO - 1, YC + 1);
        wz = append(wz, ZY(w, 3.3, a[0], a[1], p[0], p[1]));
        a = ys(s, YC - 3, YC + 1);
        wz = append(wz, ZY(w, 6.2, a[0], a[1], p[0], p[1]));
    }
    return abzug(w, d, wz);
}

// Ritzel: Zähne z 0..7, Nabe z -3..0 nimmt die Servoscheibe auf, 4x Senkung für M3
function baueSchulterRitzel(w is map, phase is number) returns Query
{
    const r = vereinige(w, [zahnrad(w, S_M, S_Z1, 0, S_ZR_H, phase), ZZ(w, w.hD + 3, 0, 0, -3, 0.01)]);
    var wz = [ZZ(w, 9, 0, 0, -4, 19), ZZ(w, w.hD + 0.4, 0, 0, -3.01, 0)];
    for (var k = 0; k < 4; k += 1)
    {
        const p = polar(w.hLK / 2, (45 + 90 * k) * PI / 180);
        wz = append(wz, ZZ(w, 3.3, p[0], p[1], -4, 19));
        wz = append(wz, kegel(w, [p[0], p[1], S_ZR_H - 1.7], [p[0], p[1], S_ZR_H + 0.01], 3.3, 6.6));
    }
    return abzug(w, r, wz);
}

function baueSchulterZahnrad(w is map, phase is number) returns Query
{
    const r = vereinige(w, [zahnrad(w, S_M, S_Z2, 0.5, 0.5 + S_ZR_H, phase), ZZ(w, 14, 0, 0, 0, 8.5)]);
    var wz = [PZ(w, -2, 18, [], [sechseck(SW_LOCH, 0, 0, 0)])];
    for (var k = 0; k < 6; k += 1)
    {
        const p = polar(21, (60 * k + 30) * PI / 180);
        wz = append(wz, ZZ(w, 11, p[0], p[1], -2, 18));
    }
    return abzug(w, r, wz);
}

// =====================================================================================
//  ACHSEN, RINGE
// =====================================================================================
function baueAchse(w is map, a is array) returns Query
{
    var teile = [];
    var z = 0;
    for (var seg in a)
    {
        if (seg[1] == "s")
        {
            teile = append(teile, PZ(w, z, z + seg[0], [], [sechseck(SW, 0, 0, 0)]));
        }
        else
        {
            teile = append(teile, schnitt(w, ZZ(w, LAGER_DI - 0.05, 0, 0, z, z + seg[0]), quader(w, [-10, -SW / 2, z], [20, 20, seg[0]])));
        }
        z += seg[0];
    }
    const k = vereinige(w, teile);
    return abzug(w, k, [ZZ(w, 3.4, 0, 0, -1, z + 1), PZ(w, -0.01, 3.99, [], [sechseckRadius(6.5, 30, 0, 0)])]);
}

function baueRing(w is map, t is number) returns Query
{
    return abzug(w, ZZ(w, 14, 0, 0, 0, t), [ZZ(w, 12.3, 0, 0, -1, t + 1)]);
}

// =====================================================================================
//  OBERARM
// =====================================================================================
function kastenProfil(w is map, y1 is number, y2 is number, r is number) returns Query
{
    return PXV(w, y1, y2, [[0, 0, r], [L1, 0, r]], [[[0, -r], [L1, -r], [L1, r], [0, r]]]);
}

function baueKasten(w is map) returns Query
{
    const rand = abzug(w, kastenProfil(w, -K_Y + K_WAND, K_Y - K_WAND, K_H), [kastenProfil(w, -K_Y + K_WAND - 1, K_Y - K_WAND + 1, K_H - K_RAND)]);
    const k = vereinige(w, [kastenProfil(w, -K_Y, -K_Y + K_WAND, K_H), rand]);
    var wz = [];
    for (var x in [0, L1])
    {
        wz = append(wz, PXV(w, -K_Y - 1, -K_Y + K_WAND + 1, [], [sechseck(SW_LOCH, 0, x, 0)]));
    }
    for (var p in KASTEN_SCHRAUBEN)
    {
        wz = append(wz, ZY(w, 2.6, K_Y - K_WAND - 12, K_Y - K_WAND + 1, p[0], p[1]));
    }
    return abzug(w, k, wz);
}

function baueKastenDeckel(w is map) returns Query
{
    const d = kastenProfil(w, K_Y - K_WAND, K_Y, K_H);
    var wz = [];
    for (var x in [0, L1])
    {
        wz = append(wz, ZY(w, SITZ_D, K_Y - LAGER_B, K_Y + 0.01, x, 0));
        wz = append(wz, ZY(w, LIPPE_D, K_Y - K_WAND - 1, K_Y, x, 0));
    }
    for (var p in KASTEN_SCHRAUBEN)
    {
        wz = append(wz, ZY(w, 3.3, K_Y - K_WAND - 1, K_Y + 1, p[0], p[1]));
        wz = append(wz, kegel(w, [p[0], K_Y + 0.01, p[1]], [p[0], K_Y - 1.79, p[1]], 6.5, 3.3));
    }
    return abzug(w, d, wz);
}

function baueKurbel(w is map) returns Query
{
    const k = vereinige(w, [PZ(w, 0, 8, [[0, 0, 10], [KURBEL, 0, 6.5]], [tangenten([0, 0], 10, [KURBEL, 0], 6.5)]), ZZ(w, 14, 0, 0, 0, 13.9)]);
    return abzug(w, k, [
        PZ(w, -1, 29, [], [sechseck(SW_LOCH, 0, 0, 0)]),
        ZZ(w, 3.3, KURBEL, 0, -1, 19),
        PZ(w, 8 - 4.2, 8.8, [], [sechseckRadius(6.5, 30, KURBEL, 0)])
    ]);
}

function baueKoppelstange(w is map) returns Query
{
    const k = PZ(w, 0, STANGE_T, [[0, 0, 6.5], [L1, 0, 6.5]], [[[0, -6.5], [L1, -6.5], [L1, 6.5], [0, 6.5]]]);
    return abzug(w, k, [ZZ(w, 3.4, 0, 0, -1, 19), ZZ(w, 3.4, L1, 0, -1, 19)]);
}

// =====================================================================================
//  UNTERARM
// =====================================================================================
function unterarmProfil(w is map, y1 is number, y2 is number) returns Query
{
    const tOben = tangentenpunkt([0, 0], 26, [L2, UA_H], 1);
    const tUnten = tangentenpunkt([0, 0], 26, [L2, -UA_H], -1);
    return PXV(w, y1, y2, [[0, 0, 26]], [[[L2, UA_H], tOben, tUnten, [L2, -UA_H]], [[0, -UA_H], [L2, -UA_H], [L2, UA_H], [0, UA_H]]]);
}

function baueUnterarmKanal(w is map) returns Query
{
    const k = vereinige(w, [
        unterarmProfil(w, UA_I, UA_A),
        quader(w, [32, -UA_I, UA_H - 5], [L2 - 32, 2 * UA_I, 5]),
        quader(w, [60, -UA_I, -UA_H], [L2 - 60, 2 * UA_I, 5]),
        quader(w, [L2 - 6, -UA_I, -UA_H], [6, 2 * UA_I, 2 * UA_H])
    ]);
    var wz = [PXV(w, UA_I - 1, UA_A + 1, [], [sechseck(SW_LOCH, 0, 0, 0)]), zyl(w, [L2 - 10, 0, -8], [L2 + 10, 0, -8], 10)];
    for (var p in UA_SCHRAUBEN)
    {
        wz = append(wz, ZY(w, 2.6, -UA_I - 1, -UA_I + 12, p[0], p[1]));
    }
    for (var f in UA_FLANSCH)
    {
        wz = append(wz, zyl(w, [L2 - 10, f[0], f[1]], [L2 + 10, f[0], f[1]], 3.3));
        wz = append(wz, profil(w, [L2 - 10, 0, 0], [1, 0, 0], [0, 1, 0], 6.6, [], [sechseckRadius(6.6, 0, f[0], f[1])]));
    }
    return abzug(w, k, wz);
}

function baueUnterarmDeckel(w is map) returns Query
{
    const d = unterarmProfil(w, -UA_A, -UA_I);
    var wz = [ZY(w, SITZ_D, -UA_I - LAGER_B - 0.01, -UA_I + 0.01, 0, 0), ZY(w, LIPPE_D, -UA_A - 1, -UA_I, 0, 0)];
    for (var p in UA_SCHRAUBEN)
    {
        wz = append(wz, ZY(w, 3.3, -UA_A - 1, -UA_I + 1, p[0], p[1]));
        wz = append(wz, kegel(w, [p[0], -UA_A - 0.01, p[1]], [p[0], -UA_A + 1.79, p[1]], 6.5, 3.3));
    }
    return abzug(w, d, wz);
}

// =====================================================================================
//  GREIFER  (x vorne, y seitlich, z hoch; Platte oben bei z = 0)
// =====================================================================================
function baueGreiferGehaeuse(w is map) returns Query
{
    var teile = [
        quader(w, [G_RUECK, -G_GA, -5], [30 - G_RUECK, 2 * G_GA, 5]),
        quader(w, [G_RUECK, -G_GA, G_UNTEN], [4, 2 * G_GA, 24 - G_UNTEN]),
        quader(w, [10.6, -G_GA, G_UNTEN], [4, 2 * G_GA, -5 - G_UNTEN]),
        ZZ(w, 10, 0, G_AB, -1, w.gZ0 - 0.2)
    ];
    for (var s in [-1, 1])
    {
        const y0 = (s > 0) ? 38.5 : -G_GA;
        teile = append(teile, quader(w, [G_RUECK, y0, G_UNTEN], [8 - G_RUECK, 5, 24 - G_UNTEN]));
        teile = append(teile, quader(w, [G_RUECK, y0, G_UNTEN], [14.6 - G_RUECK, 5, -G_UNTEN]));
    }
    const g = vereinige(w, teile);
    var wz = [
        quader(w, [-w.sB / 2 - 0.4, -G_AB - w.sW - 0.4, -10], [w.sB + 0.8, w.sL + 0.8, 20]),
        ZZ(w, 2.6, 0, G_AB, -2, 38),
        zyl(w, [G_RUECK - 1, 0, -16], [G_RUECK + 9, 0, -16], 10)
    ];
    for (var dy in [-1, 1])
    {
        for (var dx in [-1, 1])
        {
            wz = append(wz, ZZ(w, 2.6, dx * w.lB / 2, -G_AB - w.sW + w.sL / 2 + dy * w.lA / 2, -10, 0.5));
        }
    }
    for (var p in G_HAUBE_SCHR)
    {
        wz = append(wz, ZZ(w, 2.6, p[0], p[1], 10, 30));
    }
    for (var f in UA_FLANSCH)
    {
        wz = append(wz, zyl(w, [G_RUECK - 1, f[0], f[1] - 8], [G_RUECK + 9, f[0], f[1] - 8], 3.3));
    }
    for (var p in G_BODEN_SCHR)
    {
        wz = append(wz, ZZ(w, 2.6, p[0], p[1], G_UNTEN - 1, G_UNTEN + 10));
    }
    return abzug(w, g, wz);
}

function baueGreiferHaube(w is map) returns Query
{
    var wz = [ZZ(w, 10, 0, -G_AB, -1, 4), ZZ(w, 3.3, 0, G_AB, -1, 4)];
    for (var p in G_HAUBE_SCHR)
    {
        wz = append(wz, ZZ(w, 3.3, p[0], p[1], -1, 4));
        wz = append(wz, kegel(w, [p[0], p[1], 1.2], [p[0], p[1], 3.01], 3.3, 6.5));
    }
    return abzug(w, quader(w, [G_RUECK, -G_GA, 0], [8 - G_RUECK, 2 * G_GA, 3]), wz);
}

function baueGreiferBoden(w is map) returns Query
{
    var wz = [];
    for (var p in G_BODEN_SCHR)
    {
        wz = append(wz, ZZ(w, 3.3, p[0], p[1], -1, 9));
        wz = append(wz, ZZ(w, 6.2, p[0], p[1], -0.01, 3));        // Kopf von unten
    }
    return abzug(w, quader(w, [G_RUECK, -G_GA, 0], [14.6 - G_RUECK, 2 * G_GA, 4]), wz);
}

function baueGreiferhebel(w is map, antrieb is boolean) returns Query
{
    const sy = antrieb ? 1 : -1;
    const y1 = -FINGER_B / 2 + FINGER_DY;
    const y2 = FINGER_B / 2 + FINGER_DY - 1;
    const ya = min(sy * y1, sy * y2);
    const yb = max(sy * y1, sy * y2);
    var huellPoly = [];
    if (antrieb)
    {
        huellPoly = [tangentenpunkt([0, 0], 8, [FINGER_X, ya], -1), [FINGER_X, ya], [FINGER_X, yb], [FINGER_X - 3, yb], tangentenpunkt([0, 0], 8, [FINGER_X - 3, yb], 1)];
    }
    else
    {
        huellPoly = [tangentenpunkt([0, 0], 8, [FINGER_X - 3, ya], -1), [FINGER_X - 3, ya], [FINGER_X, ya], [FINGER_X, yb], tangentenpunkt([0, 0], 8, [FINGER_X, yb], 1)];
    }
    const arm = PZ(w, 0, G_ZR_H, [[0, 0, 8]], [huellPoly]);
    const h = vereinige(w, [
        zahnrad(w, G_M, G_Z, 0, G_ZR_H, antrieb ? 0 : -180 / G_Z),
        arm,
        quader(w, [FINGER_X - 3, ya, 0], [3, yb - ya, FINGER_H + 6]),
        quader(w, [FINGER_X - 3, ya, 0], [11, yb - ya, 3])
    ]);
    var wz = [];
    if (antrieb)
    {
        wz = append(wz, ZZ(w, 9, 0, 0, -1, 19));
        wz = append(wz, ZZ(w, w.hD + 0.4, 0, 0, -0.01, 2));
        for (var k = 0; k < 4; k += 1)
        {
            const p = polar(w.hLK / 2, (45 + 90 * k) * PI / 180);
            wz = append(wz, ZZ(w, 3.3, p[0], p[1], -1, 19));
            wz = append(wz, kegel(w, [p[0], p[1], G_ZR_H - 1.7], [p[0], p[1], G_ZR_H + 0.01], 3.3, 6.6));
        }
    }
    else
    {
        wz = append(wz, ZZ(w, 3.4, 0, 0, -1, 19));
    }
    for (var y in [-5, 5])
    {
        wz = append(wz, ZZ(w, 3.3, FINGER_X + 4, sy * (y + FINGER_DY), -1, 59));
        wz = append(wz, PZ(w, -0.01, 2.39, [], [sechseckRadius(6.4, 0, FINGER_X + 4, sy * (y + FINGER_DY))]));
    }
    return abzug(w, h, wz);
}

function baueKlemmplatte(w is map, sy is number) returns Query
{
    const y0 = (sy > 0) ? -FINGER_B / 2 : -FINGER_B / 2 + 1;
    return abzug(w, quader(w, [0, y0, 0], [8, FINGER_B - 1, 3]), [ZZ(w, 3.3, 4, -5, -1, 4), ZZ(w, 3.3, 4, 5, -1, 4)]);
}

// Fin-Ray-Finger (TPU): Dreieck mit gerundeten Ecken (r 0.6), Wänden, Rippen und Griffnoppen
function baueFinger(w is map) returns Query
{
    const b = 8;
    const tri = [[b, -FINGER_B / 2], [FINGER_L, FINGER_B / 2 - 0.01], [b, FINGER_B / 2]];
    // gerundetes Außendreieck = um 0.6 eingerücktes Dreieck + Kreise r 0.6 + Kantenstreifen
    const r = 0.6;
    const ein = versetzt(tri, r);
    var kanten = [];
    for (var i = 0; i < 3; i += 1)
    {
        const a = ein[i];
        const c = ein[(i + 1) % 3];
        const l = sqrt((c[0] - a[0]) ^ 2 + (c[1] - a[1]) ^ 2);
        const no = [(c[1] - a[1]) / l * r, -(c[0] - a[0]) / l * r];       // Normale nach außen
        kanten = append(kanten, [a[0] + no[0], a[1] + no[1]]);
        kanten = append(kanten, [c[0] + no[0], c[1] + no[1]]);
    }
    const aussen = PZ(w, 0, FINGER_H, [[ein[0][0], ein[0][1], r], [ein[1][0], ein[1][1], r], [ein[2][0], ein[2][1], r]], [kanten, ein]);
    const innen = versetzt(tri, WAND);
    const schale = abzug(w, aussen, [PZ(w, -1, FINGER_H + 1, [], [innen])]);
    var rippen = [];
    for (var x = b + RIPPEN_ABST; x <= FINGER_L - 6; x += RIPPEN_ABST)
    {
        rippen = append(rippen, quader(w, [x - RIPPE / 2, -FINGER_B, 0], [RIPPE, 2 * FINGER_B, FINGER_H]));
    }
    const rp = schnitt(w, vereinige(w, rippen), PZ(w, 0, FINGER_H, [], [tri]));
    var teile = [schale, rp, quader(w, [0, -FINGER_B / 2, 0], [b, FINGER_B, FINGER_H])];
    for (var x = b + 4; x <= FINGER_L - 6; x += 5)
    {
        teile = append(teile, ZZ(w, 1.6, x, FINGER_B / 2, 0, FINGER_H));
    }
    const f = vereinige(w, teile);
    return abzug(w, f, [ZZ(w, 3.4, 4, -5, -1, FINGER_H + 1), ZZ(w, 3.4, 4, 5, -1, FINGER_H + 1)]);
}

// =====================================================================================
//  KAUFTEILE
// =====================================================================================
// Kugellager 6801-2RS: Außenring, Innenring, 10 Kugeln mit Käfig, 2 Dichtscheiben (Achse z, z 0..5)
function baueLager(w is map, Ts is array, gruppe is string)
{
    const dm = 16.5;                     // Teilkreis der Kugeln
    const dk = 2.381;                    // Kugeldurchmesser (3/32")
    const aussen = abzug(w, rohr(w, 9.1, LAGER_D / 2, 0, LAGER_B),
        [torus(w, dm / 2, dk / 2 + 0.03, LAGER_B / 2), ZZ(w, 19.4, 0, 0, -1, 0.8), ZZ(w, 19.4, 0, 0, LAGER_B - 0.8, LAGER_B + 1)]);
    verteile(w, aussen, Ts, gruppe ~ " - Lager 6801 Außenring", F_METALL, W_LSTAHL);
    const innen = abzug(w, rohr(w, LAGER_DI / 2, 7.4, 0, LAGER_B), [torus(w, dm / 2, dk / 2 + 0.03, LAGER_B / 2)]);
    verteile(w, innen, Ts, gruppe ~ " - Lager 6801 Innenring", F_METALL, W_LSTAHL);
    var kugeln = [abzug(w, rohr(w, dm / 2 - 0.35, dm / 2 + 0.35, LAGER_B / 2 - 0.9, LAGER_B / 2 + 0.9), [])];
    for (var k = 0; k < 10; k += 1)
    {
        const p = polar(dm / 2, k * 2 * PI / 10);
        kugeln = append(kugeln, kugel(w, [p[0], p[1], LAGER_B / 2], dk));
    }
    verteile(w, vereinige(w, kugeln), Ts, gruppe ~ " - Lager 6801 Kugeln mit Käfig", F_STAHL, W_LSTAHL);
    for (var z in [0.2, LAGER_B - 0.8])
    {
        verteile(w, rohr(w, 7.45, 9.7, z, z + 0.6), Ts, gruppe ~ " - Lager 6801 Dichtscheibe", F_GUMMI, W_NBR);
    }
}

// Servo DS3218 (oder MG996R mit Gummitüllen): Welle = z, Ohr-Oberseite z = 0, Körper nach -x
function baueServo(w is map, Ts is array, mg is boolean, name is string)
{
    const x1 = w.sW - w.sL;
    const m = w.sW - w.sL / 2;
    const ohrL = w.sL + 13.8;
    const zb = -w.sU;                     // Unterseite
    // Gehäuse: Unterteil, Mittelteil mit Ohren, Oberteil mit Lagerdom
    var g = vereinige(w, [
        quader(w, [x1, -w.sB / 2, zb], [w.sL, w.sB, 3.5]),
        quader(w, [x1, -w.sB / 2, zb + 3.5], [w.sL, w.sB, -zb - 3.5]),
        quader(w, [m - ohrL / 2, -w.sB / 2 + 1, -2.5], [ohrL, w.sB - 2, 2.5]),
        quader(w, [x1, -w.sB / 2, 0], [w.sL, w.sB, 10.4]),
        quader(w, [x1 + 0.8, -w.sB / 2 + 0.8, 10.4], [w.sL - 1.6, w.sB - 1.6, 2.5]),
        ZZ(w, 13, 0, 0, 12.8, 14.3),
        ZZ(w, 9, -10.5, 0, 12.8, 13.6)
    ]);
    var wz = [];
    for (var dx in [-1, 1])
    {
        for (var dy in [-1, 1])
        {
            const hx = m + dx * w.lA / 2;
            wz = append(wz, ZZ(w, 4.5, hx, dy * w.lB / 2, -3.5, 1));
            wz = append(wz, quader(w, [dx > 0 ? hx : m - ohrL / 2 - 1, dy * w.lB / 2 - 0.6, -3.5], [dx > 0 ? ohrL / 2 - w.lA / 2 + 1 : ohrL / 2 - w.lA / 2 + 1, 1.2, 4.5]));
            wz = append(wz, ZZ(w, 2.2, m + dx * (w.sL / 2 - 3), dy * (w.sB / 2 - 3), zb - 1, zb + 1.2));       // Schrauben unten
        }
    }
    wz = append(wz, quader(w, [x1 + 6, w.sB / 2 - 0.3, zb + 5], [w.sL - 12, 1, 14]));       // Etikettfläche
    wz = append(wz, quader(w, [x1 + 6, -w.sB / 2 - 0.7, zb + 5], [w.sL - 12, 1, 14]));
    g = abzug(w, g, wz);
    const gk = verteile(w, g, Ts, name ~ " Gehäuse mit Motor und Getriebe", F_SCHWARZ, ["", 0]);
    setzeMasse(w, gk, mg ? "MG996R (Gehäuse, Motor, Getriebe)" : "DS3218 (Gehäuse, Motor, Getriebe)", (mg ? 48 : 52) * size(Ts));   // Gesamt 55 g / 60 g
    // Abtriebswelle mit 25 Zähnen (Messing) und Gewinde M3
    var zaehne = [ZZ(w, 5.3, 0, 0, 14.2, 18)];
    for (var k = 0; k < 25; k += 1)
    {
        const a = k * 360 / 25;
        zaehne = append(zaehne, bewege(w, PZ(w, 14.2, 18, [], [[[2.6, -0.3], [2.95, -0.12], [2.95, 0.12], [2.6, 0.3]]]), Rz(a)));
    }
    const welle = abzug(w, vereinige(w, zaehne), [ZZ(w, 2.5, 0, 0, 14, 19)]);
    verteile(w, welle, Ts, name ~ " Abtriebswelle 25T", F_MESSING, W_MESSING);
    // Kabel: Zugentlastung, drei Litzen, Dupont-Stecker
    const zug = quader(w, [x1 - 4, -3.5, zb + 1.5], [4.5, 7, 4]);
    verteile(w, zug, Ts, name ~ " Zugentlastung", F_SCHWARZ, W_PVC);
    const farben = [F_BRAUN, F_ROT, F_ORANGE];
    const litzen = ["braun (GND)", "rot (+)", "orange (Signal)"];
    for (var i = 0; i < 3; i += 1)
    {
        const litze = ZX(w, 1.1, x1 - 60, x1 - 3.5, (i - 1) * 1.2, zb + 3.5);
        verteile(w, litze, Ts, name ~ " Litze " ~ litzen[i], farben[i], W_PVC);
    }
    const stecker = abzug(w, quader(w, [x1 - 74, -3.75, zb + 2.25], [14, 7.5, 2.5]),
        [quader(w, [x1 - 74.5, -3.3, zb + 2.6], [2, 1.8, 1.8]), quader(w, [x1 - 74.5, -0.9, zb + 2.6], [2, 1.8, 1.8]), quader(w, [x1 - 74.5, 1.5, zb + 2.6], [2, 1.8, 1.8])]);
    verteile(w, stecker, Ts, name ~ " Dupont-Stecker 3-polig", F_SCHWARZ, W_PBT);
    if (mg)
    {
        // Gummitüllen mit Messing-Hülsen in den Ohren
        for (var dx in [-1, 1])
        {
            for (var dy in [-1, 1])
            {
                const hx = m + dx * w.lA / 2;
                const tuelle = abzug(w, vereinige(w, [ZZ(w, 4.4, hx, dy * w.lB / 2, -3.3, 0.8), ZZ(w, 6.4, hx, dy * w.lB / 2, -4.3, -3.3), ZZ(w, 6.4, hx, dy * w.lB / 2, 0.8, 1.8)]),
                    [ZZ(w, 3.2, hx, dy * w.lB / 2, -5, 3)]);
                verteile(w, tuelle, Ts, name ~ " Gummitülle", F_GUMMI, W_NBR);
                const huelse = abzug(w, vereinige(w, [ZZ(w, 3.2, hx, dy * w.lB / 2, -4.3, 1.8), ZZ(w, 4.2, hx, dy * w.lB / 2, 1.8, 2.2)]), [ZZ(w, 2.5, hx, dy * w.lB / 2, -5, 3)]);
                verteile(w, huelse, Ts, name ~ " Messinghülse", F_MESSING, W_MESSING);
            }
        }
    }
}

// Alu-Servoscheibe 25T: Fläche bei z = Hornabstand, Nabe zur Servoseite, 8 Gewindelöcher M3
function baueServoscheibe(w is map, T is Transform, name is string)
{
    const zF = w.hA;
    const s = vereinige(w, [ZZ(w, w.hD, 0, 0, zF - 2.5, zF), ZZ(w, 10, 0, 0, zF - 3.4, zF - 2.4)]);
    var wz = [ZZ(w, 6.0, 0, 0, zF - 4, zF - 0.8), ZZ(w, 3.2, 0, 0, zF - 1, zF + 1)];
    for (var k = 0; k < 8; k += 1)
    {
        const rr = (k % 2 == 0) ? w.hLK / 2 : min(w.hLK / 2 + 3, w.hD / 2 - 2.2);
        const p = polar(rr, (45 * k + 45) * PI / 180);
        wz = append(wz, ZZ(w, 2.5, p[0], p[1], zF - 3, zF + 1));
    }
    fertig(w, abzug(w, s, wz), name, F_ALU, T, W_ALU);
}

// Schrittmotor 28BYJ-48 mit ULN-Kabel und JST-XH-Stecker: Welle = z, Ohr-Unterseite z = 0
function baueSchrittmotor(w is map, T is Transform)
{
    const gx = -8;
    const koerper = abzug(w, vereinige(w, [
        ZZ(w, 28, gx, 0, -19, -0.8),
        ZZ(w, 28.4, gx, 0, -10.2, -9.6),                     // Falz
        ZZ(w, 27, gx, 0, -0.8, 0)
    ]), [ZZ(w, 1.2, gx + 9, 9, -1, 1), ZZ(w, 1.2, gx + 9, -9, -1, 1)]);
    fertig(w, koerper, "Basis - Schrittmotor 28BYJ-48 Gehäuse", F_METALL, T, W_STAHL);
    setzeMasse(w, koerper, "28BYJ-48 (Gehäuse, Spulen, Getriebe)", 26);        // gesamt ca. 30 g
    const deckel = abzug(w, vereinige(w, [PZ(w, 0, 0.8, [[gx, -SM_OHR, 3.5], [gx, SM_OHR, 3.5], [gx, 0, 14]], [[[gx - 3.5, -SM_OHR], [gx + 3.5, -SM_OHR], [gx + 3.5, SM_OHR], [gx - 3.5, SM_OHR]]]),
                                          ZZ(w, 9, 0, 0, 0.8, 2.3)]),
        [ZZ(w, 4.2, gx, -SM_OHR, -1, 2), ZZ(w, 4.2, gx, SM_OHR, -1, 2)]);
    fertig(w, deckel, "Basis - Schrittmotor 28BYJ-48 Flansch", F_METALL, T, W_STAHL);
    const zf = max(w.smWL - 6, 3.3);
    const welle = vereinige(w, [ZZ(w, 5, 0, 0, 2.3, zf), schnitt(w, ZZ(w, 5, 0, 0, zf, w.smWL), quader(w, [-1.5, -3, zf], [3, 6, w.smWL - zf]))]);
    fertig(w, welle, "Basis - Schrittmotor 28BYJ-48 Welle", F_STAHL, T, W_STAHL);
    const kappe = vereinige(w, [quader(w, [gx - 17, -8.5, -19], [4, 17, 16]), quader(w, [gx - 17, -4, -19], [2, 8, 3])]);
    fertig(w, kappe, "Basis - Schrittmotor 28BYJ-48 Kabelkappe", F_BLAU, T, W_ABS);
    const farben = [F_BLAU, F_ROSA, F_GELB, F_ORANGE, F_ROT];
    const fn = ["blau", "rosa", "gelb", "orange", "rot"];
    for (var i = 0; i < 5; i += 1)
    {
        fertig(w, ZX(w, 1.0, gx - 70, gx - 16.5, (i - 2) * 1.05, -17.5), "Basis - Schrittmotor Litze " ~ fn[i], farben[i], T, W_PVC);
    }
    const stecker = abzug(w, quader(w, [gx - 82, -6.2, -20.4], [12.4 - 0.4, 12.4, 5.8]), [quader(w, [gx - 83, -5.5, -19.6], [3, 11, 4.2])]);
    fertig(w, stecker, "Basis - Schrittmotor JST-XH-Stecker 5-polig", F_WEISS, T, W_PA);
}

// LED 5 mm (Säule) und Fotowiderstand (Gegenstück) der Lichtschranke
function baueLichtschranke(w is map)
{
    const led = vereinige(w, [
        ZX(w, 5, -21, -13.4, 0, LSK_Z), schnitt(w, kugel(w, [-21, 0, LSK_Z], 5), quader(w, [-24, -3, LSK_Z - 3], [3, 6, 6])),
        ZX(w, 5.8, -13.4, -12.4, 0, LSK_Z)
    ]);
    fertig(w, led, "Basis - LED 5 mm rot", color(0.9, 0.15, 0.1), identityTransform(), ["Epoxid", 1.2]);
    for (var y in [-1.27, 1.27])
    {
        fertig(w, quader(w, [-12.4, y - 0.25, LSK_Z - 0.25], [18, 0.5, 0.5]), "Basis - LED Anschlussdraht", F_METALL, identityTransform(), W_STAHL);
    }
    const ldr = ZX(w, 5.1, -29, -27, 0, LSK_Z);
    fertig(w, ldr, "Basis - Fotowiderstand GL5528", color(0.85, 0.55, 0.25), identityTransform(), ["Keramik", 2.5]);
    for (var y in [-1.7, 1.7])
    {
        fertig(w, ZX(w, 0.5, -45, -29, y, LSK_Z), "Basis - Fotowiderstand Anschlussdraht", F_METALL, identityTransform(), W_STAHL);
    }
}

// =====================================================================================
//  NORMTEILE
// =====================================================================================
function nt(w is map, typ is string, L is number, T is Transform)
{
    var m = w.nt[];
    const key = w.g[] ~ "|" ~ typ ~ "_" ~ L;
    if (m[key] == undefined)
    {
        m[key] = { "typ" : typ, "L" : L, "T" : [], "gruppe" : w.g[] };
    }
    var e = m[key];
    e.T = append(e.T, T);
    m[key] = e;
    w.nt[] = m;
}

function sechskantMutterKoerper(w is map, h is number, eck is number) returns Query
{
    const hx = PZ(w, 0, h, [], [sechseck(5.5, eck, 0, 0)]);
    const fase = vereinige(w, [kegel(w, [0, 0, 0], [0, 0, 0.45], 5.2, 6.4), ZZ(w, 6.4, 0, 0, 0.45, h - 0.45), kegel(w, [0, 0, h - 0.45], [0, 0, h], 6.4, 5.2)]);
    return schnitt(w, hx, fase);
}

function baueNormteile(w is map)
{
    const liste = w.nt[];
    for (var key in keys(liste))
    {
        const e = liste[key];
        const L = e.L;
        if (e.typ == "4762")                // Zylinderschraube mit Innensechskant
        {
            const s = abzug(w, vereinige(w, [ZZ(w, 5.5, 0, 0, 0, 2.7), kegel(w, [0, 0, 2.7], [0, 0, 3], 5.5, 4.9),
                                              ZZ(w, 3, 0, 0, -L + 0.3, 0.01), kegel(w, [0, 0, -L], [0, 0, -L + 0.3], 2.4, 3)]),
                            [PZ(w, 1.6, 3.1, [], [sechseck(2.5, 0, 0, 0)])]);
            verteile(w, s, e.T, e.gruppe ~ " - ISO 4762 M3x" ~ L, F_STAHL, W_STAHL);
        }
        else if (e.typ == "10642")          // Senkschraube mit Innensechskant
        {
            const s = abzug(w, vereinige(w, [kegel(w, [0, 0, -1.6], [0, 0, 0], 3, 6), ZZ(w, 3, 0, 0, -L + 0.3, -1.59), kegel(w, [0, 0, -L], [0, 0, -L + 0.3], 2.4, 3)]),
                            [PZ(w, -1.1, 0.1, [], [sechseck(2, 0, 0, 0)])]);
            verteile(w, s, e.T, e.gruppe ~ " - ISO 10642 M3x" ~ L, F_STAHL, W_STAHL);
        }
        else if (e.typ == "7380")           // Flachkopfschraube
        {
            const s = abzug(w, vereinige(w, [kugelkappe(w, 5.7, 1.65), ZZ(w, 3, 0, 0, -L + 0.3, 0.01), kegel(w, [0, 0, -L], [0, 0, -L + 0.3], 2.4, 3)]),
                            [PZ(w, 0.6, 1.8, [], [sechseck(2, 0, 0, 0)])]);
            verteile(w, s, e.T, e.gruppe ~ " - ISO 7380 M3x" ~ L, F_STAHL, W_STAHL);
        }
        else if (e.typ == "4032_30" || e.typ == "4032_0")    // Sechskantmutter
        {
            const eck = (e.typ == "4032_30") ? 30 : 0;
            const n = abzug(w, sechskantMutterKoerper(w, 2.4, eck), [ZZ(w, 2.5, 0, 0, -1, 3.4), kegel(w, [0, 0, -0.01], [0, 0, 0.3], 3.2, 2.5), kegel(w, [0, 0, 2.1], [0, 0, 2.41], 2.5, 3.2)]);
            verteile(w, n, e.T, e.gruppe ~ " - ISO 4032 M3", F_STAHL, W_STAHL);
        }
        else if (e.typ == "10511")          // Sicherungsmutter mit Klemmteil
        {
            const n = abzug(w, vereinige(w, [sechskantMutterKoerper(w, 3, 30), ZZ(w, 5.2, 0, 0, 2.99, 4)]), [ZZ(w, 2.5, 0, 0, -1, 3.5), ZZ(w, 4.2, 0, 0, 3.0, 5)]);
            verteile(w, n, e.T, e.gruppe ~ " - ISO 10511 M3", F_STAHL, W_STAHL);
            verteile(w, rohr(w, 1.3, 2.1, 3.0, 3.95), e.T, e.gruppe ~ " - ISO 10511 Nylonring", F_NYLON, W_PA);
        }
        else if (e.typ == "7089")           // Scheibe
        {
            verteile(w, rohr(w, 1.6, 3.5, 0, 0.5), e.T, e.gruppe ~ " - ISO 7089 3", F_STAHL, W_STAHL);
        }
        else if (e.typ == "fuss")           // Gummifuß (Puffer Ø12 x 3.5, im 1 mm tiefen Sitz)
        {
            verteile(w, kugelkappe(w, 12, 3.5), e.T, e.gruppe ~ " - Gummifuß 12x3,5", F_GUMMI, W_NBR);
        }
    }
}

// =====================================================================================
//  ELEKTRONIK  (liegt neben der Basis auf dem Tisch, Tischebene z = -2.5)
// =====================================================================================
function baueElektronik(w is map)
{
    const z0 = -2.5;
    // ---------- Steckbrett 830 ----------
    const bx = -82.55;  const by = -215;
    const yc = 27.3;
    var brett = quader(w, [bx, by, z0], [165.1, 54.6, 8.5]);
    var loecher = [];
    for (var i = 0; i < 63; i += 1)
    {
        const x = bx + 3.81 + 2.54 * i;
        for (var j = 0; j < 5; j += 1)
        {
            for (var s in [-1, 1])
            {
                const y = by + yc + s * (3.81 + 2.54 * j);
                loecher = append(loecher, [[x - 0.5, y - 0.5], [x + 0.5, y - 0.5], [x + 0.5, y + 0.5], [x - 0.5, y + 0.5]]);
            }
        }
    }
    for (var yy in [3.81, 6.35, 48.25, 50.79])
    {
        for (var g = 0; g < 10; g += 1)
        {
            for (var h = 0; h < 5; h += 1)
            {
                const x = bx + 8.89 + 2.54 * (g * 6 + h);
                const y = by + yy;
                loecher = append(loecher, [[x - 0.5, y - 0.5], [x + 0.5, y - 0.5], [x + 0.5, y + 0.5], [x - 0.5, y + 0.5]]);
            }
        }
    }
    brett = abzug(w, brett, [PZ(w, z0 + 2.5, z0 + 9, [], loecher), quader(w, [bx - 1, by + yc - 1.25, z0 + 7.1], [167.1, 2.5, 2])]);
    fertig(w, brett, "Elektronik - Steckbrett 830", F_WEISS, identityTransform(), W_ABS);
    for (var st in [[1.6, F_BLAU], [8.6, F_ROT], [46.0, F_BLAU], [53.0, F_ROT]])
    {
        fertig(w, quader(w, [bx + 5, by + st[0] - 0.3, z0 + 8.5], [155, 0.6, 0.05]), "Elektronik - Steckbrett Markierung", st[1], identityTransform(), [ "Farbe", 0 ]);
    }
    const zb = z0 + 8.5;                 // Oberseite Steckbrett
    // ---------- ESP32-S3-Board (Freenove), Stiftreihen 22.86 mm ----------
    const ex = bx + 3.81 + 2.54 * 2;  const ey = by + yc;
    const pcb = quader(w, [ex - 2, ey - 13, zb + 2.5], [63.5, 26, 1.6]);
    fertig(w, abzug(w, pcb, [ZZ(w, 3, ex + 0.5, ey - 10.5, zb, zb + 5), ZZ(w, 3, ex + 0.5, ey + 10.5, zb, zb + 5), ZZ(w, 3, ex + 59, ey - 10.5, zb, zb + 5), ZZ(w, 3, ex + 59, ey + 10.5, zb, zb + 5)]),
        "Elektronik - ESP32-S3 Leiterplatte", F_BLAU_PCB, identityTransform(), W_FR4);
    const zp = zb + 4.1;                  // Oberseite Leiterplatte
    fertig(w, quader(w, [ex + 39, ey - 9, zp], [18, 25.5, 0.8]), "Elektronik - ESP32-S3-WROOM-1 Modul-Platine", F_SCHWARZ, identityTransform(), W_FR4);
    fertig(w, quader(w, [ex + 39.4, ey - 8.3, zp + 0.8], [15.8, 17.6, 2.3]), "Elektronik - ESP32-S3-WROOM-1 Abschirmung", F_METALL, identityTransform(), ["Weißblech", 7.3]);
    for (var uy in [-6, 5])
    {
        fertig(w, abzug(w, quader(w, [ex - 2.6, ey + uy - 4.47 + 2, zp], [7.35, 8.94, 3.26]), [quader(w, [ex - 3, ey + uy - 3.6 + 2, zp + 0.5], [5, 7.2, 2.2])]),
            "Elektronik - USB-C Buchse", F_METALL, identityTransform(), W_STAHL);
    }
    for (var t in [[ex + 8, ey - 9], [ex + 8, ey + 9]])
    {
        fertig(w, quader(w, [t[0] - 2.1, t[1] - 1.6, zp], [4.2, 3.2, 1.5]), "Elektronik - Taster SMD (Gehäuse)", F_METALL, identityTransform(), W_STAHL);
        fertig(w, quader(w, [t[0] - 0.9, t[1] - 0.6, zp + 1.5], [1.8, 1.2, 0.6]), "Elektronik - Taster SMD (Knopf)", F_SCHWARZ, identityTransform(), W_PA);
    }
    fertig(w, quader(w, [ex + 14, ey + 4, zp], [5, 5, 1.6]), "Elektronik - RGB-LED WS2812", F_WEISS, identityTransform(), W_PA);
    fertig(w, quader(w, [ex + 14, ey - 9, zp], [6.5, 3.5, 1.6]), "Elektronik - Spannungsregler SOT-223", F_SCHWARZ, identityTransform(), ["Epoxid", 1.9]);
    fertig(w, quader(w, [ex + 22, ey - 3, zp], [4, 4, 0.8]), "Elektronik - USB-Seriell-Wandler", F_SCHWARZ, identityTransform(), ["Epoxid", 1.9]);
    for (var s in [-1, 1])
    {
        fertig(w, quader(w, [ex - 1.27, ey + s * 11.43 - 1.27, zb], [50.8, 2.54, 2.5]), "Elektronik - Stiftleiste 20-polig", F_SCHWARZ, identityTransform(), W_PBT);
        for (var i = 0; i < 20; i += 1)
        {
            fertig(w, quader(w, [ex + 2.54 * i - 0.32, ey + s * 11.43 - 0.32, zb - 6], [0.64, 0.64, 10.6]), "Elektronik - Stift", F_MESSING, identityTransform(), W_MESSING);
        }
    }
    // ---------- ULN2003-Treiberplatine ----------
    const ux = 95;  const uy = -205;
    fertig(w, quader(w, [ux, uy, z0 + 2], [35, 32, 1.6]), "Elektronik - ULN2003 Leiterplatte", F_GRUEN_PCB, identityTransform(), W_FR4);
    const zu = z0 + 3.6;
    fertig(w, quader(w, [ux + 8, uy + 12.8, zu], [19.3, 6.35, 3.3]), "Elektronik - ULN2003A (DIP16)", F_SCHWARZ, identityTransform(), ["Epoxid", 1.9]);
    for (var i = 0; i < 8; i += 1)
    {
        for (var s in [0, 1])
        {
            fertig(w, quader(w, [ux + 8.75 + 2.54 * i - 0.25, uy + 12.8 - 0.6 + s * 7.55, zu - 0.5], [0.5, 0.6, 2.5]), "Elektronik - IC-Beinchen", F_METALL, identityTransform(), W_CU);
        }
    }
    for (var i = 0; i < 4; i += 1)
    {
        fertig(w, vereinige(w, [ZZ(w, 3, ux + 6 + 6 * i, uy + 4, zu, zu + 3.5), kugel(w, [ux + 6 + 6 * i, uy + 4, zu + 3.5], 3)]), "Elektronik - LED 3 mm rot", color(0.9, 0.15, 0.1), identityTransform(), ["Epoxid", 1.2]);
    }
    fertig(w, abzug(w, quader(w, [ux + 2, uy + 23, zu], [12.4, 5.75, 7]), [quader(w, [ux + 2.6, uy + 23.5, zu + 1], [11.2, 4.4, 7])]), "Elektronik - JST-XH-Buchse 5-polig", F_WEISS, identityTransform(), W_PA);
    for (var i = 0; i < 6; i += 1)
    {
        fertig(w, quader(w, [ux + 18 + 2.54 * i - 0.32, uy + 25 - 0.32, zu - 3], [0.64, 0.64, 11.5]), "Elektronik - Stift", F_MESSING, identityTransform(), W_MESSING);
    }
    fertig(w, quader(w, [ux + 16.7, uy + 23.7, zu], [15.24, 2.54, 2.5]), "Elektronik - Stiftleiste 6-polig", F_SCHWARZ, identityTransform(), W_PBT);
    // ---------- Elko 1000 µF auf der Plus-/Minus-Schiene ----------
    const kx = bx + 8.89 + 2.54 * 50;  const ky = by + 5.08;
    fertig(w, ZZ(w, 10, kx, ky, zb + 2, zb + 18), "Elektronik - Elko 1000 µF 10 V", color(0.10, 0.20, 0.45), identityTransform(), ["Elko", 1.3]);
    fertig(w, ZZ(w, 8.8, kx, ky, zb + 18, zb + 18.3), "Elektronik - Elko Deckel", F_METALL, identityTransform(), W_ALU);
    for (var yy in [3.81, 6.35])
    {
        fertig(w, ZZ(w, 0.6, kx, by + yy, zb - 6, zb + 2.01), "Elektronik - Anschlussdraht", F_METALL, identityTransform(), W_CU);
    }
    // ---------- Widerstände 220 Ω und 10 kΩ mit Farbringen ----------
    const ringe = [[[F_ROT, F_ROT, F_BRAUN, F_GOLD], "220 Ohm"], [[F_BRAUN, F_SCHWARZ, F_ORANGE, F_GOLD], "10 kOhm"]];
    for (var i = 0; i < 2; i += 1)
    {
        const rx = bx + 3.81 + 2.54 * (36 + 6 * i);  const ry = by + yc + 8.89;
        fertig(w, ZX(w, 2.5, rx - 3.15, rx + 3.15, ry, zb + 3), "Elektronik - Widerstand " ~ ringe[i][1], color(0.85, 0.78, 0.60), identityTransform(), ["Keramik", 2.5]);
        for (var r = 0; r < 4; r += 1)
        {
            fertig(w, ZX(w, 2.6, rx - 2.3 + r * 1.2 + (r == 3 ? 0.9 : 0), rx - 1.7 + r * 1.2 + (r == 3 ? 0.9 : 0), ry, zb + 3), "Elektronik - Farbring", ringe[i][0][r], identityTransform(), ["Farbe", 0]);
        }
        for (var s in [-1, 1])
        {
            fertig(w, ZX(w, 0.6, rx + s * 3.15, rx + s * 5.08, ry, zb + 3), "Elektronik - Anschlussdraht", F_METALL, identityTransform(), W_CU);
            fertig(w, ZZ(w, 0.6, rx + s * 5.08, ry, zb - 6, zb + 3), "Elektronik - Anschlussdraht", F_METALL, identityTransform(), W_CU);
        }
    }
    // ---------- Taster 6x6 (Pause / weiter) ----------
    const tx = bx + 3.81 + 2.54 * 52;  const ty = by + yc;
    fertig(w, quader(w, [tx - 3, ty - 3, zb], [6, 6, 3.5]), "Elektronik - Taster 6x6 Gehäuse", F_SCHWARZ, identityTransform(), W_PA);
    fertig(w, ZZ(w, 3.5, tx, ty, zb + 3.5, zb + 5), "Elektronik - Taster 6x6 Knopf", F_SCHWARZ, identityTransform(), W_PA);
    for (var dx in [-2.54, 2.54])
    {
        for (var dy in [-3.81, 3.81])
        {
            fertig(w, quader(w, [tx + dx - 0.35, ty + dy - 0.2, zb - 3.5], [0.7, 0.4, 3.6]), "Elektronik - Taster Beinchen", F_METALL, identityTransform(), W_STAHL);
        }
    }
    // ---------- Netzteil 5 V / 5 A mit Hohlstecker und Schraubklemmen-Adapter ----------
    const nx = -90;  const ny = -300;
    const netz = vereinige(w, [quader(w, [nx + 4, ny, z0], [112, 50, 31]), quader(w, [nx, ny + 4, z0], [120, 42, 31]),
                               ZZ(w, 8, nx + 4, ny + 4, z0, z0 + 31), ZZ(w, 8, nx + 116, ny + 4, z0, z0 + 31), ZZ(w, 8, nx + 4, ny + 46, z0, z0 + 31), ZZ(w, 8, nx + 116, ny + 46, z0, z0 + 31)]);
    fertig(w, netz, "Elektronik - Netzteil 5 V 5 A", F_SCHWARZ, identityTransform(), W_ABS);
    setzeMasse(w, netz, "Netzteil 5 V 5 A", 250);
    fertig(w, zyl(w, [nx + 120, ny + 25, z0 + 15], [nx + 190, ny + 25, z0 + 15], 3.5), "Elektronik - Netzteil Kabel", F_SCHWARZ, identityTransform(), W_PVC);
    fertig(w, abzug(w, vereinige(w, [zyl(w, [nx + 190, ny + 25, z0 + 15], [nx + 215, ny + 25, z0 + 15], 9), zyl(w, [nx + 215, ny + 25, z0 + 15], [nx + 224, ny + 25, z0 + 15], 5.5)]),
                    [zyl(w, [nx + 218, ny + 25, z0 + 15], [nx + 225, ny + 25, z0 + 15], 2.1)]), "Elektronik - Hohlstecker 5,5/2,1", F_SCHWARZ, identityTransform(), W_PVC);
    fertig(w, quader(w, [nx + 224, ny + 18, z0 + 8], [23, 14, 10]), "Elektronik - Hohlbuchse mit Schraubklemme", F_SCHWARZ, identityTransform(), W_PA);
    fertig(w, quader(w, [nx + 241, ny + 18.5, z0 + 18], [6, 13, 4]), "Elektronik - Schraubklemme 2-polig", color(0.10, 0.55, 0.20), identityTransform(), W_PA);
}
