FeatureScript 1576;
import(path : "onshape/std/geometry.fs", version : "1576.0");

// ▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼▼
//  AB HIER KOPIEREN  (diese Zeile mit) – bis ganz zum Ende der Datei
// ▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲▲

// =====================================================================================
//  Roboterarm AM-R1  Kit-Version  –  vollständiges Modell als Onshape-Feature
//
//  Nur Freenove ESP32-S3 Kit, ohne Schrauben (Steckachsen, Clips, Presspassungen,
//  Bajonett). Maße 1:1 wie kit/cad/kit.scad (Druckdateien in kit/stl/).
//
//  • alle Druckteile, exakte Evolventen-Zahnräder (Splines), Schnecke als Schraubenlinie
//  • Kit-Teile: 28BYJ-48, Gleichstrommotor 130, Potentiometer, SG90 mit Horn und Kabel,
//    LED und Fotowiderstand der Lichtschranke, 15 Ein-Euro-Münzen als Gegengewicht
//  • Lager 6801 ODER die gedruckten Ersatzteile (Laufrollen / Lagerbuchsen), umschaltbar
//  • Elektronik: ESP32-S3-Board, Steckbrett, L293D, ULN2003, Elko, Widerstände, Taster,
//    USB-Netzteil
//  • Werkstoffe mit Dichte (Masse / Schwerpunkt), Kit-Teile mit echter Masse
//  • Mate Connectors an allen Gelenken auf beiden Teilen
// =====================================================================================

// ---------- Lager 6801 / Ersatz ----------
const LAGER_D = 21;  const LAGER_DI = 12;  const LAGER_B = 5;  const SITZ_D = 20.95;  const LIPPE_D = 16;

// ---------- Basis ----------
const BASIS_R = 90;  const BODEN_T = 3;  const BASIS_H = 55;
const INNEN_R1 = 66;  const INNEN_R2 = 70;  const AUSSEN_T = 2.4;
const DECKEL_T = 4;  const ROLLE_R = 78;  const ACHSE_H = 12.5;
const DECKEL_OBEN = 59;  const TT_UNTEN = 82;  const TT_T = 8;  const TT_OBEN = 90;
const TT_R = 86;  const SAEULE_R = 15;  const KABEL_R = 8;
const ZR_M = 1.25;  const ZR_Z1 = 15;  const ZR_Z2 = 75;
const KRANZ_RI = 40;  const KRANZ_UNTEN = 74;  const RITZEL_OBEN = 81.5;
const SM_OHR = 17.5;  const SM_WX = 56.4;  const SM_GX = 48.4;
const LSK_Z = 77;  const ROCK_R1 = 23;  const ROCK_R2 = 25;
const KAPPE_H = 7;  const SAEULE_OBEN = 97.4;
const BOCK_STIFTE = [[-55, -30], [-55, 30], [20, -30], [20, 30]];

// ---------- Schulter (Koordinaten: Schulterachse im Ursprung) ----------
const H_S = 70;  const P_DZ = 30;  const YI = 16;  const YP = 22;
const W_M = 0.8;  const W_Z = 80;  const S_D1 = 10;  const S_A = 37.15;  const S_Y = 28;  const W_H = 8;
const ST_M = 0.8;  const ST_Z1 = 10;  const ST_Z2 = 50;  const ST_A = 24.15;
const MOT_Z = -13;  const MOT_X1 = 43;  const MOT_L = 25;
const DECKEL_STIFTE = [[-28, 46], [-28, 27], [68, 46]];
const DECKEL_ANKER = [[-28, 50], [-32, 27], [68, 50]];

// ---------- Arm ----------
const L = 120;  const HL_Y1 = -10;  const HL_Y2 = -2;  const PL_Y1 = 2;  const PL_Y2 = 8;
const HG_Y = [[-15, -11], [9, 13]];
const MUENZ_R = 62;  const MUENZ_D = 23.6;  const STIFT_D = 5;

// ---------- Greifer (SG90) ----------
const G_AB = 10.1;  const G_M = 1;  const G_Z = 20;  const G_ZR_H = 5;  const G_Z0 = 10;  const G_PLATTE = 4;
const FINGER_L = 62;  const FINGER_H = 20;  const FINGER_B = 20;  const FINGER_X = 16;
const WAND = 1.2;  const RIPPE = 1;  const RIPPEN_ABST = 7;
const SG_X1 = -16.9;  const SG_X2 = 6.2;  const SG_B = 12.4;
const HG_FLANSCH_X = 22;  const HG_Z0 = -57;

// ---------- Farben ----------
const F_BASIS = color(0.80, 0.80, 0.80);   const F_GRAU = color(0.35, 0.40, 0.45);
const F_DUNKEL = color(0.22, 0.27, 0.30);  const F_ARM = color(0.86, 0.32, 0.25);
const F_GOLD = color(0.92, 0.74, 0.20);    const F_STANGE = color(0.85, 0.80, 0.50);
const F_LILA = color(0.42, 0.35, 0.80);    const F_GREIFER = color(0.25, 0.48, 0.70);
const F_HEBEL = color(0.95, 0.60, 0.10);   const F_TPU = color(0.15, 0.15, 0.15);
const F_WEISS = color(0.92, 0.92, 0.92);   const F_METALL = color(0.72, 0.73, 0.76);
const F_STAHL = color(0.55, 0.57, 0.60);   const F_SCHWARZ = color(0.07, 0.07, 0.07);
const F_GUMMI = color(0.12, 0.12, 0.12);   const F_BRAUN = color(0.45, 0.25, 0.10);
const F_ROT = color(0.85, 0.10, 0.10);     const F_ORANGE = color(0.98, 0.55, 0.05);
const F_GELB = color(0.95, 0.85, 0.10);    const F_ROSA = color(0.95, 0.55, 0.70);
const F_BLAU = color(0.10, 0.35, 0.90);    const F_SG90 = color(0.15, 0.35, 0.85);
const F_GRUEN_PCB = color(0.05, 0.40, 0.15);  const F_BLAU_PCB = color(0.05, 0.20, 0.55);
const F_MESSING = color(0.80, 0.65, 0.25); const F_MUENZE = color(0.85, 0.72, 0.35);

// ---------- Werkstoffe [Name, Dichte g/cm³] ----------
const W_PLA = ["PLA (Druck)", 1.24];        const W_PETG = ["PETG (Druck)", 1.27];
const W_TPU = ["TPU 95A (Druck)", 1.21];    const W_STAHL = ["Stahl", 7.85];
const W_LSTAHL = ["Wälzlagerstahl 100Cr6", 7.81];  const W_NBR = ["NBR-Gummi", 1.20];
const W_PA = ["PA6", 1.14];  const W_FR4 = ["FR4 Leiterplatte", 1.85];  const W_ABS = ["ABS", 1.04];
const W_CU = ["Kupfer", 8.96];  const W_PVC = ["PVC", 1.40];  const W_MESSING = ["Messing", 8.50];
const W_PBT = ["PBT", 1.31];  const W_EPOXID = ["Epoxid", 1.9];

const PHI_GRENZEN = { (degree) : [-33, 0, 45] } as AngleBoundSpec;
const GREIF_GRENZEN = { (degree) : [0, 15, 30] } as AngleBoundSpec;
const BASIS_GRENZEN = { (degree) : [-200, 0, 200] } as AngleBoundSpec;

annotation { "Feature Type Name" : "Roboterarm Kit" }
export const roboterarmKit = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {
        annotation { "Name" : "Schulter (phi)" }
        isAngle(definition.phi, PHI_GRENZEN);
        annotation { "Name" : "Greifer offen" }
        isAngle(definition.greif, GREIF_GRENZEN);
        annotation { "Name" : "Basis drehen" }
        isAngle(definition.basisw, BASIS_GRENZEN);

        annotation { "Name" : "Motor Durchmesser" }
        isLength(definition.motD, { (millimeter) : [15, 20.4, 25] } as LengthBoundSpec);
        annotation { "Name" : "Motor Mass ueber Abflachungen" }
        isLength(definition.motFl, { (millimeter) : [10, 15.1, 25] } as LengthBoundSpec);
        annotation { "Name" : "Motor Wellendurchmesser" }
        isLength(definition.motW, { (millimeter) : [1.5, 2, 3.2] } as LengthBoundSpec);
        annotation { "Name" : "Poti Gewindebuchse" }
        isLength(definition.potB, { (millimeter) : [5, 7.2, 10] } as LengthBoundSpec);
        annotation { "Name" : "Poti Welle" }
        isLength(definition.potW, { (millimeter) : [3, 6, 7] } as LengthBoundSpec);
        annotation { "Name" : "28BYJ-48: Ohr-Unterseite bis Wellenende" }
        isLength(definition.smWelle, { (millimeter) : [6, 10, 14] } as LengthBoundSpec);

        annotation { "Name" : "Basis + Drehteller", "Default" : true }
        definition.zeigeBasis is boolean;
        annotation { "Name" : "Schulter", "Default" : true }
        definition.zeigeSchulter is boolean;
        annotation { "Name" : "Arm", "Default" : true }
        definition.zeigeArm is boolean;
        annotation { "Name" : "Greifer", "Default" : true }
        definition.zeigeGreifer is boolean;
        annotation { "Name" : "Kit-Teile (Motoren, Poti, Servo, Muenzen)", "Default" : true }
        definition.zeigeKit is boolean;
        annotation { "Name" : "Kugellager 6801 (aus: gedruckter Ersatz)", "Default" : true }
        definition.mitLagern is boolean;
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
            "ctx" : context, "id" : id, "z" : new box(0),
            "motD" : definition.motD / millimeter, "motFl" : definition.motFl / millimeter, "motW" : definition.motW / millimeter,
            "potB" : definition.potB / millimeter, "potW" : definition.potW / millimeter,
            "smWL" : definition.smWelle / millimeter, "smOZ" : RITZEL_OBEN - definition.smWelle / millimeter,
            "exakt" : definition.zahnExakt, "werkstoff" : definition.werkstoffe
        };
        const phi = definition.phi / degree;
        const gr = definition.greif / degree;
        const bw = definition.basisw / degree;
        const S = TT_OBEN + H_S;
        const A = [L * cos(phi * degree), L * sin(phi * degree)];

        // ---------- Lagen ----------
        const T_dreh = Rz(bw);
        const T_sch = T_dreh * Tt(0, 0, S);
        const T_arm = T_sch * Ry(-phi);                          // Achse, Schneckenrad, Hauptlenker
        const T_para = T_sch * Tt(0, 0, P_DZ) * Ry(-phi);
        const T_hg = T_sch * Tt(A[0], 0, A[1]);
        const T_greif = T_hg * Tt(HG_FLANSCH_X + 28, 0, HG_Z0) * Rx(180);
        const T_hebA = T_greif * Tt(0, -G_AB, G_Z0) * Rz(-gr);
        const T_hebB = T_greif * Tt(0, G_AB, G_Z0) * Rz(gr);
        const T_schnecke = T_sch * Tt(-15, S_Y, -S_A) * Ry(90);
        const T_ritzelM = T_sch * Tt(30, S_Y, MOT_Z) * Ry(90);
        const wSchnecke = 80 * phi;                              // Schnecke dreht 80 x so weit wie das Rad
        const phRitzelBasis = 180 - 180 / ZR_Z1 - 5 * bw;
        const phRitzelM = 180 / ST_Z1 - 5 * wSchnecke;

        var teile = {};

        // ================= BASIS =================
        if (definition.zeigeBasis)
        {
            teile.basis = fertig(w, baueBasis(w), "Basis", F_BASIS, identityTransform(), W_PLA);
            fertig(w, baueDeckelring(w), "Basis - Deckelring", F_DUNKEL, Tt(0, 0, BASIS_H), W_PETG);
            for (var k = 0; k < 4; k += 1)
            {
                fertig(w, baueRollenachse(w), "Basis - Rollenachse " ~ (k + 1), F_GOLD, Rz(45 + 90 * k) * Tt(ROLLE_R, 0, DECKEL_OBEN + ACHSE_H), W_PETG);
            }
            teile.ritzelBasis = fertig(w, baueRitzelBasis(w, phRitzelBasis), "Ritzel Basis", F_METALL, Tt(SM_WX, 0, KRANZ_UNTEN + 0.5), W_PETG);
            fertig(w, baueKappe(w), "Basis - Kappe (Bajonett)", F_DUNKEL, Tt(0, 0, TT_OBEN + 0.2), W_PLA);
            teile.drehteller = fertig(w, baueDrehteller(w), "Schulter - Drehteller", F_WEISS, T_dreh * Tt(0, 0, TT_UNTEN), W_PETG);
            var rollen = [];
            for (var k = 0; k < 4; k += 1)
            {
                rollen = append(rollen, Rz(45 + 90 * k) * Tt(ROLLE_R - LAGER_B / 2, 0, DECKEL_OBEN + ACHSE_H) * Ry(90));
            }
            if (definition.mitLagern)
            {
                baueLager(w, rollen, "Basis");
            }
            else
            {
                verteile(w, rohr(w, 6.2, LAGER_D / 2, 0, LAGER_B), rollen, "Basis - Laufrolle (Druck)", F_GOLD, W_PETG);
            }
        }
        // ================= SCHULTER =================
        if (definition.zeigeSchulter)
        {
            teile.bock = fertig(w, baueSchulterbock(w), "Schulter - Schulterbock", F_GRAU, T_sch, W_PETG);
            fertig(w, baueGetriebedeckel(w), "Schulter - Getriebedeckel", F_DUNKEL, T_sch, W_PLA);
            for (var p in DECKEL_STIFTE)
            {
                fertig(w, ZZ(w, 3, p[0], p[1], -H_S + 3, -H_S + 13), "Schulter - Deckelstift", F_GOLD, T_sch, W_PLA);
            }
            for (var p in BOCK_STIFTE)
            {
                fertig(w, ZZ(w, 5, p[0], p[1], -H_S - 6, -H_S + 5.5), "Schulter - Bockstift", F_GOLD, T_sch, W_PETG);
            }
            teile.schnecke = fertig(w, baueSchnecke(w, wSchnecke), "Schnecke", F_METALL, T_schnecke, W_PETG);
            teile.ritzelM = fertig(w, baueMotorritzel(w, phRitzelM), "Motorritzel", F_METALL, T_ritzelM, W_PETG);
            // Steckachse oben (Parallellenker) mit Hülsen und Clip
            fertig(w, baueSteckachse(w, 2 * YP + 3.2), "Schulter - Steckachse lang", F_GOLD, T_sch * Tt(0, -YP, P_DZ) * Rx(-90), W_PETG);
            fertig(w, rohr(w, 2.675, 4.5, 0, 18), "Schulter - Hülse 18", F_GOLD, T_sch * Tt(0, -YI, P_DZ) * Rx(-90), W_PLA);
            fertig(w, rohr(w, 2.675, 4.5, 0, 8), "Schulter - Hülse 8", F_GOLD, T_sch * Tt(0, PL_Y2, P_DZ) * Rx(-90), W_PLA);
            fertig(w, baueClip(w), "Schulter - Clip", F_GOLD, T_sch * Tt(0, YP + 2.0, P_DZ) * Rx(-90), W_PETG);
            const sitze = [T_sch * Tt(0, -YP, 0) * Rx(-90), T_sch * Tt(0, YP - LAGER_B, 0) * Rx(-90)];
            if (definition.mitLagern)
            {
                baueLager(w, sitze, "Schulter");
            }
            else
            {
                verteile(w, rohr(w, 6.15, SITZ_D / 2 + 0.02, 0, LAGER_B), sitze, "Schulter - Lagerbuchse (Druck)", F_GOLD, W_PETG);
            }
        }
        // ================= ARM =================
        if (definition.zeigeArm)
        {
            teile.haupt = fertig(w, baueHauptlenker(w), "Hauptlenker", F_ARM, T_arm, W_PETG);
            fertig(w, baueAchse(w), "Hauptlenker - Schulterachse", F_GOLD, T_arm, W_PETG);
            teile.rad = fertig(w, baueSchneckenrad(w), "Hauptlenker - Schneckenrad", F_METALL, T_arm * Tt(0, S_Y - W_H / 2, 0) * Rx(-90), W_PETG);
            fertig(w, rohr(w, 6.15, 7, 0, 7), "Hauptlenker - Distanzrohr 7", F_GOLD, T_arm * Tt(0, -17, 0) * Rx(-90), W_PLA);
            fertig(w, rohr(w, 6.15, 7, 0, 19), "Hauptlenker - Distanzrohr 19", F_GOLD, T_arm * Tt(0, -2, 0) * Rx(-90), W_PLA);
            fertig(w, rohr(w, 6.15, 7, 0, 2), "Hauptlenker - Distanzrohr 2", F_GOLD, T_arm * Tt(0, 22, 0) * Rx(-90), W_PLA);
            fertig(w, baueClipGross(w), "Hauptlenker - Clip groß", F_GOLD, T_arm * Tt(0, 32.2, 0) * Rx(-90), W_PETG);
            fertig(w, baueMuenzdeckel(w), "Hauptlenker - Münzdeckel", F_ARM, T_arm * Tt(-MUENZ_R, -43.4, 0) * Rx(-90), W_PLA);

            teile.para = fertig(w, baueParallellenker(w), "Parallellenker", F_STANGE, T_para, W_PETG);

            teile.hg = fertig(w, baueHandgelenk(w), "Handgelenk", F_LILA, T_hg, W_PETG);
            const la = HG_Y[1][1] - HG_Y[0][0] + 3.2;
            fertig(w, baueSteckachse(w, la), "Handgelenk - Steckachse unten", F_GOLD, T_hg * Tt(0, HG_Y[0][0], 0) * Rx(-90), W_PETG);
            fertig(w, baueSteckachse(w, la), "Handgelenk - Steckachse oben", F_GOLD, T_hg * Tt(0, HG_Y[0][0], P_DZ) * Rx(-90), W_PETG);
            fertig(w, baueClip(w), "Handgelenk - Clip unten", F_GOLD, T_hg * Tt(0, HG_Y[1][1] + 2.0, 0) * Rx(-90), W_PETG);
            fertig(w, baueClip(w), "Handgelenk - Clip oben", F_GOLD, T_hg * Tt(0, HG_Y[1][1] + 2.0, P_DZ) * Rx(-90), W_PETG);
            fertig(w, rohr(w, 2.675, 4.5, 0, 1), "Handgelenk - Hülse 1 unten", F_GOLD, T_hg * Tt(0, -11, 0) * Rx(-90), W_PLA);
            fertig(w, rohr(w, 2.675, 4.5, 0, 11), "Handgelenk - Hülse 11", F_GOLD, T_hg * Tt(0, -2, 0) * Rx(-90), W_PLA);
            fertig(w, rohr(w, 2.675, 4.5, 0, 13), "Handgelenk - Hülse 13", F_GOLD, T_hg * Tt(0, -11, P_DZ) * Rx(-90), W_PLA);
            fertig(w, rohr(w, 2.675, 4.5, 0, 1), "Handgelenk - Hülse 1 oben", F_GOLD, T_hg * Tt(0, 8, P_DZ) * Rx(-90), W_PLA);
        }
        // ================= GREIFER =================
        if (definition.zeigeGreifer)
        {
            teile.greifer = fertig(w, baueGreiferGehaeuse(w), "Handgelenk - Greifergehäuse", F_GREIFER, T_greif, W_PETG);
            fertig(w, vereinige(w, [ZZ(w, 3, 0, 0, 0, 52), ZZ(w, 5, 0, 0, 0, 1.2)]), "Handgelenk - Sperrstift", F_GOLD,
                T_hg * Tt(HG_FLANSCH_X + 2, -27.2, -83.6) * Rx(-90), W_PETG);
            teile.hebelA = fertig(w, baueGreiferhebel(w, 1), "Greiferhebel A (Antrieb)", F_HEBEL, T_hebA, W_PETG);
            fertig(w, baueFinger(w), "Greiferhebel A - Finger (TPU)", F_TPU, T_hebA * Tt(FINGER_X, 0, 3), W_TPU);
            fertig(w, baueKlemmplatte(w, 1), "Greiferhebel A - Klemmplatte", F_HEBEL, T_hebA * Tt(FINGER_X, 0, 3 + FINGER_H), W_PLA);
            teile.hebelB = fertig(w, baueGreiferhebel(w, -1), "Greiferhebel B (Mitläufer)", F_HEBEL, T_hebB, W_PETG);
            fertig(w, baueFinger(w), "Greiferhebel B - Finger (TPU)", F_TPU, T_hebB * Tt(FINGER_X, 0, 3 + FINGER_H) * Rx(180), W_TPU);
            fertig(w, baueKlemmplatte(w, -1), "Greiferhebel B - Klemmplatte", F_HEBEL, T_hebB * Tt(FINGER_X, 0, 3 + FINGER_H), W_PLA);
            fertig(w, baueClip(w), "Greiferhebel B - Clip", F_GOLD, T_greif * Tt(0, G_AB, G_Z0 + G_ZR_H + 1.0), W_PETG);
        }
        // ================= KIT-TEILE =================
        if (definition.zeigeKit)
        {
            if (definition.zeigeBasis)
            {
                baueSchrittmotor(w, Tt(SM_WX, 0, w.smOZ));
                baueLichtschranke(w);
            }
            if (definition.zeigeSchulter)
            {
                baueGleichstrommotor(w, T_sch * Tt(MOT_X1, S_Y, MOT_Z));
                bauePoti(w, T_sch);
            }
            if (definition.zeigeArm)
            {
                baueMuenzen(w, T_arm);
            }
            if (definition.zeigeGreifer)
            {
                baueSG90(w, T_greif);
            }
        }
        if (definition.zeigeElektronik)
        {
            baueElektronik(w);
        }
        // ================= MATE CONNECTORS =================
        if (definition.zeigeConnectors)
        {
            const X = vector(1, 0, 0);  const Y = vector(0, 1, 0);  const Z = vector(0, 0, 1);
            gelenk(w, teile, ["basis", "drehteller"], T_dreh * Tt(0, 0, TT_OBEN), Z, X);                    // J1 Basis
            gelenk(w, teile, ["bock", "haupt"], T_sch, Y, X);                                              // J2 Schulter
            gelenk(w, teile, ["bock", "para"], T_sch * Tt(0, 0, P_DZ), Y, X);                              // J3 Parallellenker oben
            gelenk(w, teile, ["haupt", "hg"], T_hg, Y, X);                                                 // J4 Handgelenk unten
            gelenk(w, teile, ["para", "hg"], T_hg * Tt(0, 0, P_DZ), Y, X);                                 // J5 Handgelenk oben
            gelenk(w, teile, ["greifer", "hebelA"], T_greif * Tt(0, -G_AB, G_Z0), Z, X);                   // J6 Greiferhebel A
            gelenk(w, teile, ["greifer", "hebelB"], T_greif * Tt(0, G_AB, G_Z0), Z, X);                    // J7 Greiferhebel B
            gelenk(w, teile, ["basis", "ritzelBasis"], Tt(SM_WX, 0, KRANZ_UNTEN + 0.5), Z, X);             // J8 Ritzel Basis
            gelenk(w, teile, ["bock", "schnecke"], T_sch * Tt(0, S_Y, -S_A), X, Y);                        // J9 Schnecke
            gelenk(w, teile, ["bock", "ritzelM"], T_sch * Tt(0, S_Y, MOT_Z), X, Y);                        // J10 Motorritzel
            gelenk(w, teile, ["haupt", "rad"], T_sch, Y, X);                                               // Schneckenrad fest am Hauptlenker
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

function polar(r is number, a is number) returns array { return [r * cos(a * radian), r * sin(a * radian)]; }

// Punkte auf einem Kreis (für konvexe Hüllen)
function kreisPunkte(c is array, r is number, n is number) returns array
{
    var p = [];
    for (var i = 0; i < n; i += 1)
    {
        p = append(p, [c[0] + r * cos((360 * i / n) * degree), c[1] + r * sin((360 * i / n) * degree)]);
    }
    return p;
}

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
    opSphere(w.ctx, i, { "center" : V(p), "radius" : d / 2 * millimeter });
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

// Ring um einen beliebigen Mittelpunkt (für Nuten)
function ring(w is map, x is number, y is number, ri is number, ra is number, z1 is number, z2 is number) returns Query
{
    return abzug(w, ZZ(w, 2 * ra, x, y, z1, z2), [ZZ(w, 2 * ri, x, y, z1 - 1, z2 + 1)]);
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

// Profil in der xy-Ebene von z1 bis z2
function PZ(w is map, z1 is number, z2 is number, kreise is array, polys is array) returns Query
{
    return profil(w, [0, 0, z1], [0, 0, 1], [1, 0, 0], z2 - z1, kreise, polys);
}

// Profil in der xz-Ebene (Skizze: x, z) von y1 bis y2
function PXV(w is map, y1 is number, y2 is number, kreise is array, polys is array) returns Query
{
    return profil(w, [0, y2, 0], [0, -1, 0], [1, 0, 0], y2 - y1, kreise, polys);
}

// Profil in der yz-Ebene (Skizze: y, z) von x1 bis x2
function PYZ(w is map, x1 is number, x2 is number, kreise is array, polys is array) returns Query
{
    return profil(w, [x1, 0, 0], [1, 0, 0], [0, 1, 0], x2 - x1, kreise, polys);
}

// Stab zwischen zwei Kreisen (2D-Hülle): Kreise + Tangentenviereck
function stab(a is array, r1 is number, b is array, r2 is number) returns map
{
    const dx = b[0] - a[0];
    const dy = b[1] - a[1];
    const l = sqrt(dx * dx + dy * dy);
    const ph = atan2(dy, dx);
    const be = acos((r1 - r2) / l);
    const p1 = ph + be;
    const p2 = ph - be;
    return { "kreise" : [[a[0], a[1], r1], [b[0], b[1], r2]],
             "poly" : [[a[0] + r1 * cos(p1), a[1] + r1 * sin(p1)], [b[0] + r2 * cos(p1), b[1] + r2 * sin(p1)],
                       [b[0] + r2 * cos(p2), b[1] + r2 * sin(p2)], [a[0] + r1 * cos(p2), a[1] + r1 * sin(p2)]] };
}

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

function kreuz(o is array, a is array, b is array) returns number
{
    return (a[0] - o[0]) * (b[1] - o[1]) - (a[1] - o[1]) * (b[0] - o[0]);
}

// konvexe Hülle (gegen den Uhrzeigersinn)
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

// ---------- Evolventen-Zahnrad (Flankenspiel einstellbar) ----------
function flankenpunkte(m is number, z is number, N is number, spiel is number) returns map
{
    const rp = m * z / 2;
    const ra = rp + m;
    const rf = rp - 1.25 * m;
    const rb = rp * cos(20 * degree);
    const tmax = sqrt((ra / rb) ^ 2 - 1);
    const t0 = (rf > rb) ? sqrt((rf / rb) ^ 2 - 1) : 0;
    const inv20 = tan(20 * degree) - 20 * PI / 180;
    const halb = PI / (2 * z) + inv20 - spiel / rp / 2;
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

function zahnradPunkte(m is number, z is number, phase is number, spiel is number) returns array
{
    const N = 6;
    const g = flankenpunkte(m, z, N, spiel);
    const fl = g.fl;
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

// Zahnrad z1..z2 (Achse z); exakt: Flanken als Splines, Kopf- und Fußkreis als Bögen
function zahnradS(w is map, m is number, z is number, z1 is number, z2 is number, phase is number, spiel is number) returns Query
{
    if (!w.exakt)
    {
        return PZ(w, z1, z2, [], [zahnradPunkte(m, z, phase, spiel)]);
    }
    const N = 8;
    const g = flankenpunkte(m, z, N, spiel);
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

function zahnrad(w is map, m is number, z is number, z1 is number, z2 is number, phase is number) returns Query
{
    return zahnradS(w, m, z, z1, z2, phase, 0.08);
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
// Schnappstift nach oben (Spreizstift durch die Ohren des Schrittmotors)
function schnappstift(w is map, x is number, y is number, z is number, d is number, h is number) returns Query
{
    const s = vereinige(w, [ZZ(w, d, x, y, z, z + h - 0.8), kegel(w, [x, y, z + h - 0.8], [x, y, z + h], d + 0.6, d - 0.4)]);
    return abzug(w, s, [quader(w, [x - 0.5, y - d, z + h - 2.6], [1, 2 * d, 3])]);
}

function baueBasis(w is map) returns Query
{
    var teile = [
        ZZ(w, 2 * BASIS_R, 0, 0, 0, BODEN_T),
        rohr(w, BASIS_R - AUSSEN_T, BASIS_R, 0, BASIS_H),
        rohr(w, INNEN_R1, INNEN_R2, 0, BASIS_H),
        ZZ(w, 2 * SAEULE_R, 0, 0, 0, SAEULE_OBEN),
        quader(w, [SAEULE_R - 0.5, -2, TT_OBEN + 2.6], [2.5, 4, 2]),
        quader(w, [-SAEULE_R - 2, -2, TT_OBEN + 2.6], [2.5, 4, 2]),
        bewege(w, quader(w, [INNEN_R1, -9, 0], [BASIS_R - INNEN_R1 - 0.5, 18, 17]), Rz(270)),
        quader(w, [SM_GX - 4, -SM_OHR, 0], [8, 2 * SM_OHR, 8]),
        quader(w, [-(ROCK_R1 - 2), -5, 0], [ROCK_R1 - 2 - SAEULE_R + 2, 10, LSK_Z + 5]),
        quader(w, [-(ROCK_R2 + 10), -5, 0], [8, 10, LSK_Z + 5]),
        quader(w, [-INNEN_R1 - 1, -1.5, 0], [INNEN_R1 + 1 - ROCK_R2 - 9, 3, BASIS_H])
    ];
    for (var s in [-1, 1])
    {
        teile = append(teile, quader(w, [SM_GX - 4, s * SM_OHR - 3, 0], [8, 6, w.smOZ]));
        teile = append(teile, schnappstift(w, SM_GX, s * SM_OHR, w.smOZ, 3.9, 2.2));
        teile = append(teile, quader(w, [SM_GX, s * SM_OHR - 1.5, 0], [INNEN_R1 - SM_GX + 1, 3, BASIS_H]));
    }
    for (var k = 0; k < 6; k += 1)
    {
        const p = polar((INNEN_R1 + INNEN_R2) / 2, (30 + 60 * k) * PI / 180);
        teile = append(teile, ZZ(w, 4, p[0], p[1], BASIS_H - 0.01, BASIS_H + DECKEL_T));
    }
    const b = vereinige(w, teile);
    return abzug(w, b, [
        ZZ(w, 2 * KABEL_R, 0, 0, -1, 300),
        bewege(w, quader(w, [0, -6, BODEN_T], [SAEULE_R + 2, 12, 18]), Rz(270)),
        bewege(w, quader(w, [INNEN_R1 - 6, -7, BODEN_T], [40, 14, 12]), Rz(270)),
        ZX(w, 5.2, -(ROCK_R2 + 12), -ROCK_R2, 0, LSK_Z),
        ZX(w, 5.1, -ROCK_R1, -ROCK_R1 + (ROCK_R1 - KABEL_R + 1), 0, LSK_Z)
    ]);
}

function baueDeckelring(w is map) returns Query
{
    var teile = [rohr(w, INNEN_R1, BASIS_R, 0, DECKEL_T)];
    var loecher = [];
    for (var k = 0; k < 4; k += 1)
    {
        for (var s in [-1, 1])
        {
            teile = append(teile, bewege(w, quader(w, [ROLLE_R + s * (LAGER_B / 2 + 0.3 + 2) - 2, -8, 0], [4, 16, DECKEL_T + ACHSE_H + LAGER_DI / 2 + 3]), Rz(45 + 90 * k)));
        }
        // Tropfenförmige Aufnahme für die Rollenachse (druckbar ohne Stützen)
        const r = (LAGER_DI + 0.15) / 2;
        loecher = append(loecher, bewege(w, PYZ(w, ROLLE_R - 15, ROLLE_R + 15, [[0, DECKEL_T + ACHSE_H, r]],
            [[[-r * 0.7071, DECKEL_T + ACHSE_H + r * 0.7071], [r * 0.7071, DECKEL_T + ACHSE_H + r * 0.7071], [0, DECKEL_T + ACHSE_H + r * 1.4142]]]), Rz(45 + 90 * k)));
    }
    for (var k = 0; k < 6; k += 1)
    {
        const p = polar((INNEN_R1 + INNEN_R2) / 2, (30 + 60 * k) * PI / 180);
        loecher = append(loecher, ZZ(w, 4.15, p[0], p[1], -1, 9));
    }
    return abzug(w, vereinige(w, teile), loecher);
}

function baueRollenachse(w is map) returns Query
{
    const l = LAGER_B + 0.6 + 8;
    return abzug(w, ZX(w, LAGER_DI - 0.05, -l / 2, l / 2, 0, 0), [quader(w, [-l, -LAGER_DI, -LAGER_DI / 2 - 1], [2 * l, 2 * LAGER_DI, 1.5])]);
}

function baueDrehteller(w is map) returns Query
{
    const kranzH = TT_UNTEN - KRANZ_UNTEN;
    const kranz = abzug(w, zahnrad(w, ZR_M, ZR_Z2, -kranzH, 0.01, 0), [ZZ(w, 2 * KRANZ_RI, 0, 0, -kranzH - 1, 1)]);
    const rockH = TT_UNTEN - (LSK_Z - 5);
    const rock = schnitt(w, rohr(w, ROCK_R1, ROCK_R2, -rockH, 0.01), quader(w, [-50, -50, -rockH - 1], [100, 50, 40]));
    const t = vereinige(w, [ZZ(w, 2 * TT_R, 0, 0, 0, TT_T), kranz, rock]);
    var wz = [ZZ(w, 2 * SAEULE_R + 0.8, 0, 0, -20, 20), PZ(w, TT_T - 0.6, TT_T + 0.4, [], [[[TT_R - 14, -5], [TT_R - 4, 0], [TT_R - 14, 5]]])];
    for (var p in BOCK_STIFTE)
    {
        wz = append(wz, ZZ(w, 5.15, p[0], p[1], 2, 22));
    }
    return abzug(w, t, wz);
}

function baueRitzelBasis(w is map, phase is number) returns Query
{
    const h = RITZEL_OBEN - (KRANZ_UNTEN + 0.5);
    return abzug(w, zahnrad(w, ZR_M, ZR_Z1, 0, h, phase), [schnitt(w, ZZ(w, 5.25, 0, 0, -1, 19), quader(w, [-1.575, -5, -1], [3.15, 10, 20]))]);
}

// Bajonett-Kappe: aufsetzen, über die Nocken schieben und 40° drehen
function baueKappe(w is map) returns Query
{
    var wz = [ZZ(w, 2 * SAEULE_R + 0.4, 0, 0, -1, KAPPE_H - 1.2), ZZ(w, 2 * KABEL_R, 0, 0, -1, KAPPE_H + 1)];
    for (var a in [0, 180])
    {
        wz = append(wz, bewege(w, quader(w, [0, -2.3, -1], [SAEULE_R + 2.6, 4.6, 5.8]), Rz(a)));
        const keil = PZ(w, 2.2, 4.8, [], [[[0, 0], [40, -2.3], [40 * cos(45 * degree) + 2.3 * sin(45 * degree), 40 * sin(45 * degree)]]]);
        wz = append(wz, bewege(w, schnitt(w, rohr(w, SAEULE_R + 0.2, SAEULE_R + 2.6, 2.2, 4.8), keil), Rz(a)));
    }
    return abzug(w, ZZ(w, 42, 0, 0, 0, KAPPE_H), wz);
}

// =====================================================================================
//  SCHULTER
// =====================================================================================
function seitenwand(w is map, y1 is number, y2 is number) returns Query
{
    const pts = concatenateArrays([kreisPunkte([0, 0], 16, 64), kreisPunkte([0, P_DZ], 10, 48), [[-30, -H_S], [30, -H_S], [30, -H_S + 1], [-30, -H_S + 1]]]);
    return PXV(w, y1, y2, [], [huelle(pts)]);
}

function baueSchulterbock(w is map) returns Query
{
    const md = w.motD;
    // Motor-Klemmring mit Abflachungen (oben und unten) und Klemmschlitz oben
    const klemm = abzug(w, ZX(w, md + 5, MOT_X1, MOT_X1 + 22, S_Y, MOT_Z), [
        schnitt(w, ZX(w, md + 0.3, MOT_X1 - 1, MOT_X1 + 23, S_Y, MOT_Z), quader(w, [MOT_X1 - 2, S_Y - 15, MOT_Z - (w.motFl + 0.3) / 2], [26, 30, w.motFl + 0.3])),
        quader(w, [MOT_X1 - 1, S_Y - 1, MOT_Z + md / 2], [24, 2, md / 2 + 3])
    ]);
    const teile = [
        schnitt(w, quader(w, [-72, -40, -H_S], [142, 92, 6]), ZZ(w, 170, 0, 0, -H_S - 1, -H_S + 9)),
        seitenwand(w, -YP, -YI),
        seitenwand(w, YI, YP),
        quader(w, [-14, -36, -10], [4, 14, 20]),
        quader(w, [10, -36, -10], [4, 14, 20]),
        quader(w, [-14, -36, -10], [28, 3, 20]),
        quader(w, [-23, S_Y - 6, -H_S], [5, 12, H_S - S_A + 6]),
        quader(w, [18, S_Y - 6, -H_S], [5, 12, H_S - S_A + 6]),
        quader(w, [MOT_X1, S_Y - 12, -H_S], [22, 24, H_S + MOT_Z - md / 2 + 2]),
        klemm
    ];
    var wz = [
        ZY(w, SITZ_D, -YP - 0.01, -YP + LAGER_B, 0, 0),
        ZY(w, SITZ_D, YP - LAGER_B, YP + 0.01, 0, 0),
        ZY(w, LIPPE_D, -YP - 1, YP + 1, 0, 0),
        ZY(w, STIFT_D + 0.1, -YP - 1, YP + 1, 0, P_DZ),
        ZY(w, w.potB, -40, -30, 0, 0),
        ZX(w, 5.3, -30, 30, S_Y, -S_A),
        ZZ(w, 44, 0, 0, -H_S - 1, -H_S + 7)
    ];
    for (var p in DECKEL_STIFTE)
    {
        wz = append(wz, ZZ(w, 3.1, p[0], p[1], -H_S + 3, -H_S + 13));
    }
    for (var p in BOCK_STIFTE)
    {
        wz = append(wz, ZZ(w, 5.1, p[0], p[1], -H_S - 1, -H_S + 7));
    }
    return abzug(w, vereinige(w, teile), wz);
}

// Getriebedeckel (rechts außen) mit drei Steckdomen
function baueGetriebedeckel(w is map) returns Query
{
    const aussenPts = concatenateArrays([kreisPunkte([0, 0], 37, 96), kreisPunkte([60, -14], 16, 48), [[-34, -H_S + 6], [72, -H_S + 6], [72, -H_S + 7], [-34, -H_S + 7]]]);
    const innenPts = concatenateArrays([kreisPunkte([0, 0], 35, 96), kreisPunkte([60, -14], 14, 48), [[-32, -H_S + 5], [70, -H_S + 5], [70, -H_S + 6], [-32, -H_S + 6]]]);
    const schale = abzug(w, PXV(w, YP, YP + 30, [], [huelle(aussenPts)]), [PXV(w, YP - 1, YP + 28, [], [huelle(innenPts)])]);
    var teile = [schale];
    for (var i = 0; i < 3; i += 1)
    {
        const s = DECKEL_STIFTE[i];
        const a = DECKEL_ANKER[i];
        const st = stab(s, 3.5, a, 2);
        teile = append(teile, PZ(w, -H_S + 6, -H_S + 16, st.kreise, [st.poly]));
    }
    var wz = [ZY(w, 16, YP - 2, YP + 40, 0, 0), ZX(w, 8, 66, 76, S_Y, MOT_Z)];
    for (var p in DECKEL_STIFTE)
    {
        wz = append(wz, ZZ(w, 3.4, p[0], p[1], -H_S, -H_S + 15));
    }
    return abzug(w, vereinige(w, teile), wz);
}

// Schulterachse: Ø12 rund in den Lagern, Sechskant 10.2 für Hauptlenker und Schneckenrad
function prismaY(w is map, y1 is number, y2 is number, pts is array) returns Query
{
    return profil(w, [0, y1, 0], [0, 1, 0], [1, 0, 0], y2 - y1, [], [pts]);
}

function baueAchse(w is map) returns Query
{
    const a = vereinige(w, [
        ZY(w, 16, -23, -22, 0, 0),
        ZY(w, LAGER_DI - 0.05, -22.01, -9.99, 0, 0),
        prismaY(w, -10, -2, sechseck(10.2, 0, 0, 0)),
        ZY(w, LAGER_DI - 0.05, -2.01, 24.01, 0, 0),
        prismaY(w, 24, 32, sechseck(10.2, 0, 0, 0)),
        ZY(w, LAGER_DI - 0.05, 31.99, 34, 0, 0)
    ]);
    const nut = abzug(w, ZY(w, 14, 32.2, 33.2, 0, 0), [ZY(w, 9, 32, 33.4, 0, 0)]);
    return abzug(w, a, [ZY(w, w.potW, -24, -23 + 11, 0, 0), nut]);
}

// Schneckenrad 80 Zähne (Achse z, z 0..8)
function baueSchneckenrad(w is map) returns Query
{
    const r = vereinige(w, [zahnradS(w, W_M, W_Z, 0, W_H, 0, 0.3), ZZ(w, 24, 0, 0, 0, W_H)]);
    var wz = [PZ(w, -1, W_H + 1, [], [sechseck(10.45, 0, 0, 0)])];
    for (var k = 0; k < 6; k += 1)
    {
        const p = polar(20, (30 + 60 * k) * PI / 180);
        wz = append(wz, ZZ(w, 12, p[0], p[1], -1, W_H + 1));
    }
    return abzug(w, r, wz);
}

// Zahnprofil der Schnecke im Achsschnitt (Skizze x = Radius, y = Achse)
function schneckenZahn(z0 is number) returns array
{
    const p = PI * W_M;
    const t = tan(20 * degree);
    const r1 = S_D1 / 2;
    const ra = r1 + W_M;
    const rf = r1 - 1.25 * W_M - 0.1;
    const hp = p / 4;
    return [[rf, z0 - hp - (r1 - rf) * t], [ra, z0 - hp + W_M * t], [ra, z0 + hp - W_M * t], [rf, z0 + hp + (r1 - rf) * t]];
}

// Gewinde der Schnecke als Schraubenlinie (Sweep); falls das nicht klappt: Ringzähne
function schneckenGewinde(w is map, lw is number) returns Query
{
    const p = PI * W_M;
    var gewinde = undefined;
    const hid = nid(w);
    const sid0 = nid(w);
    try silent
    {
        opHelix(w.ctx, hid, { "direction" : vector(0, 0, 1), "axisStart" : V([0, 0, 0]), "startPoint" : V([S_D1 / 2, 0, 0]),
                              "interval" : [0, lw / p], "clockwise" : false, "helicalPitch" : p * millimeter, "spiralPitch" : 0 * millimeter });
        const sk = newSketchOnPlane(w.ctx, sid0, { "sketchPlane" : plane(V([0, 0, 0]), vector(0, -1, 0), vector(1, 0, 0)) });
        var pts = [];
        for (var q in schneckenZahn(0))
        {
            pts = append(pts, V2(q));
        }
        pts = append(pts, pts[0]);
        skPolyline(sk, "z", { "points" : pts });
        skSolve(sk);
        const swid = nid(w);
        opSweep(w.ctx, swid, { "profiles" : qSketchRegion(sid0), "path" : qCreatedBy(hid, EntityType.EDGE) });
        if (size(evaluateQuery(w.ctx, qCreatedBy(swid, EntityType.BODY))) > 0)
        {
            gewinde = qCreatedBy(swid, EntityType.BODY);
        }
    }
    // Hilfslinie und Skizze in jedem Fall entfernen
    const reste = qUnion([qCreatedBy(sid0), qCreatedBy(hid)]);
    if (size(evaluateQuery(w.ctx, reste)) > 0)
    {
        opDeleteBodies(w.ctx, nid(w), { "entities" : reste });
    }
    if (gewinde == undefined)
    {
        // Ersatz: Zähne als Ringe (gleiche Zahnform, ohne Steigung)
        const sid = nid(w);
        const rid = nid(w);
        const sk = newSketchOnPlane(w.ctx, sid, { "sketchPlane" : plane(V([0, 0, 0]), vector(0, -1, 0), vector(1, 0, 0)) });
        var k = 0;
        for (var z = p / 2; z < lw; z += p)
        {
            var pts = [];
            for (var q in schneckenZahn(z))
            {
                pts = append(pts, V2(q));
            }
            pts = append(pts, pts[0]);
            skPolyline(sk, "z" ~ k, { "points" : pts });
            k += 1;
        }
        skSolve(sk);
        opRevolve(w.ctx, rid, { "entities" : qSketchRegion(sid), "axis" : line(V([0, 0, 0]), vector(0, 0, 1)), "angleForward" : 360 * degree });
        opDeleteBodies(w.ctx, nid(w), { "entities" : qCreatedBy(sid) });
        gewinde = qCreatedBy(rid, EntityType.BODY);
    }
    return gewinde;
}

// Schnecke mit Bund, Welle und Stirnrad 50 Z (Achse z, Gewinde z 0..30)
function baueSchnecke(w is map, drehung is number) returns Query
{
    const lw = 30;
    const s = vereinige(w, [
        ZZ(w, 8, 0, 0, -2.7, lw + 2.7),
        ZZ(w, 5, 0, 0, -11, lw + 20),
        zahnrad(w, ST_M, ST_Z2, lw + 16, lw + 20, 0),
        schneckenGewinde(w, lw)
    ]);
    return bewege(w, s, Rz(drehung));
}

function baueMotorritzel(w is map, phase is number) returns Query
{
    return abzug(w, vereinige(w, [zahnrad(w, ST_M, ST_Z1, 0, 5, phase), ZZ(w, 5, 0, 0, 0, 7)]), [ZZ(w, w.motW - 0.05, 0, 0, -1, 9)]);
}

// Steckachse Ø5 mit Kopf (z -1.6..0) und Clipnut, Achse z
function baueSteckachse(w is map, l is number) returns Query
{
    const a = vereinige(w, [ZZ(w, STIFT_D - 0.05, 0, 0, 0, l), ZZ(w, 9, 0, 0, -1.6, 0)]);
    return abzug(w, a, [rohr(w, 2.0, 3, l - 2.0, l - 1.2)]);
}

function baueClip(w is map) returns Query
{
    return abzug(w, ZZ(w, 8.5, 0, 0, -0.8, 0), [ZZ(w, 4.0, 0, 0, -2, 1), quader(w, [0, -1.6, -2], [10, 3.2, 3])]);
}

function baueClipGross(w is map) returns Query
{
    return abzug(w, ZZ(w, 17, 0, 0, 0, 0.9), [ZZ(w, 8.8, 0, 0, -1, 2), quader(w, [0, -3.4, -1], [10, 6.8, 3])]);
}

// =====================================================================================
//  ARM
// =====================================================================================
function baueHauptlenker(w is map) returns Query
{
    const vorne = stab([0, 0], 13, [L, 0], 8);
    const hinten = stab([0, 0], 13, [-MUENZ_R, 0], 7);
    const lenker = PXV(w, HL_Y1, HL_Y2, [[0, 0, 13], [L, 0, 8], [-MUENZ_R, 0, 7]], [vorne.poly, hinten.poly]);
    const h = vereinige(w, [lenker, ZY(w, MUENZ_D + 3.2, -42.2, HL_Y2, -MUENZ_R, 0)]);     // Magazin seitlich, bündig mit der Lenkerseite
    return abzug(w, h, [
        prismaY(w, HL_Y1 - 1, HL_Y2 + 1, sechseck(10.45, 0, 0, 0)),
        ZY(w, STIFT_D + 0.3, HL_Y1 - 1, HL_Y2 + 1, L, 0),
        ZY(w, MUENZ_D, -44, HL_Y2 - 1.2, -MUENZ_R, 0)
    ]);
}

function baueMuenzdeckel(w is map) returns Query
{
    return abzug(w, vereinige(w, [ZZ(w, MUENZ_D + 3.2, 0, 0, 0, 1.2), ZZ(w, MUENZ_D - 0.2, 0, 0, 0, 3)]), [ZZ(w, MUENZ_D - 2.6, 0, 0, 1.2, 4)]);
}

function baueParallellenker(w is map) returns Query
{
    const s = stab([0, 0], 6, [L, 0], 6);
    return abzug(w, PXV(w, PL_Y1, PL_Y2, s.kreise, [s.poly]), [ZY(w, STIFT_D + 0.3, PL_Y1 - 1, PL_Y2 + 1, 0, 0), ZY(w, STIFT_D + 0.3, PL_Y1 - 1, PL_Y2 + 1, L, 0)]);
}

function baueHandgelenk(w is map) returns Query
{
    const pts = concatenateArrays([kreisPunkte([0, 0], 8, 48), kreisPunkte([0, P_DZ], 8, 48), [[-8, -85], [22, -85], [22, -84], [-8, -84]]]);
    const f = HG_FLANSCH_X;
    const teile = [
        PXV(w, HG_Y[0][0], HG_Y[0][1], [], [huelle(pts)]),
        PXV(w, HG_Y[1][0], HG_Y[1][1], [], [huelle(pts)]),
        quader(w, [f - 4, HG_Y[0][0], -85], [4, HG_Y[1][1] - HG_Y[0][0], P_DZ + 8 + 85]),
        quader(w, [f - 4, -26, -85], [4, 52, 36]),
        quader(w, [f, 22.2, -85], [8, 3.8, 36]),
        quader(w, [f, -26, -85], [8, 3.8, 36]),
        quader(w, [f + 4.4, 20, -85], [3.6, 6, 36]),
        quader(w, [f + 4.4, -26, -85], [3.6, 6, 36]),
        quader(w, [f, -26, -51.8], [5.5, 52, 2.8])
    ];
    return abzug(w, vereinige(w, teile), [ZY(w, STIFT_D + 0.1, -30, 30, 0, 0), ZY(w, STIFT_D + 0.1, -30, 30, 0, P_DZ), ZY(w, 3.1, -30, 30, f + 2, -83.6)]);
}

// =====================================================================================
//  GREIFER (SG90)
// =====================================================================================
function baueGreiferGehaeuse(w is map) returns Query
{
    const dreieck = [[-24, 0], [-14, 0], [-24, 20]];
    const g = vereinige(w, [
        quader(w, [-28, -20, -G_PLATTE], [42, 40, G_PLATTE]),
        quader(w, [-28, -22, -G_PLATTE], [4, 44, 28]),
        PXV(w, -19, -15, [], [dreieck]),
        PXV(w, 15, 19, [], [dreieck]),
        ZZ(w, 9, 0, G_AB, 0, G_Z0 - 0.2),
        ZZ(w, STIFT_D - 0.1, 0, G_AB, 0, G_Z0 + G_ZR_H + 1.6)
    ]);
    const mx = (SG_X1 + SG_X2) / 2;
    return abzug(w, g, [
        quader(w, [SG_X1, -G_AB - SG_B / 2, -10], [SG_X2 - SG_X1, SG_B, 20]),
        ZZ(w, 1.8, mx - 13.8, -G_AB, -10, 10),
        ZZ(w, 1.8, mx + 13.8, -G_AB, -10, 10),
        ring(w, 0, G_AB, 2.0, 3, G_Z0 + G_ZR_H + 0.2, G_Z0 + G_ZR_H + 1.0)
    ]);
}

// sy = 1: Antriebshebel (auf dem SG90-Horn), sy = -1: Mitläufer (gespiegelt)
function baueGreiferhebel(w is map, sy is number) returns Query
{
    const ya = (sy > 0) ? -10 : -9;
    const arm = concatenateArrays([kreisPunkte([0, 0], 5.5, 48), [[FINGER_X - 3, ya], [FINGER_X, ya], [FINGER_X, ya + 19], [FINGER_X - 3, ya + 19]]]);
    var teile = [
        zahnrad(w, G_M, G_Z, 0, G_ZR_H, (sy > 0) ? 0 : -9),
        PZ(w, 0, G_ZR_H, [], [huelle(arm)]),
        quader(w, [FINGER_X - 3, ya, 0], [3, 19, FINGER_H + 6]),
        quader(w, [FINGER_X - 3, ya, 0], [11, 19, 3])
    ];
    for (var y in [-5, 4])
    {
        teile = append(teile, ZZ(w, 3.4, FINGER_X + 4, sy * y, 0, 3 + FINGER_H + 3));
    }
    const h = vereinige(w, teile);
    if (sy > 0)
    {
        const horn = huelle(concatenateArrays([kreisPunkte([0, 0], 3.8, 32), kreisPunkte([15.5, 0], 2.4, 24)]));
        return abzug(w, h, [ZZ(w, 5.5, 0, 0, -1, 19), PZ(w, -0.01, 1.8, [], [horn])]);
    }
    return abzug(w, h, [ZZ(w, STIFT_D + 0.3, 0, 0, -1, 19)]);
}

function baueKlemmplatte(w is map, sy is number) returns Query
{
    const ya = (sy > 0) ? -10 : -9;
    return abzug(w, quader(w, [0, ya, 0], [8, 19, 3]), [ZZ(w, 3.35, 4, sy * -5, -1, 4), ZZ(w, 3.35, 4, sy * 4, -1, 4)]);
}

// Fin-Ray-Finger (TPU): Dreieck mit gerundeten Ecken (r 0.6), Wänden, Rippen und Griffnoppen
function baueFinger(w is map) returns Query
{
    const b = 8;
    const tri = [[b, -FINGER_B / 2], [FINGER_L, FINGER_B / 2 - 0.01], [b, FINGER_B / 2]];
    const r = 0.6;
    const ein = versetzt(tri, r);
    var kanten = [];
    for (var i = 0; i < 3; i += 1)
    {
        const a = ein[i];
        const c = ein[(i + 1) % 3];
        const l = sqrt((c[0] - a[0]) ^ 2 + (c[1] - a[1]) ^ 2);
        const no = [(c[1] - a[1]) / l * r, -(c[0] - a[0]) / l * r];
        kanten = append(kanten, [a[0] + no[0], a[1] + no[1]]);
        kanten = append(kanten, [c[0] + no[0], c[1] + no[1]]);
    }
    const aussen = PZ(w, 0, FINGER_H, [[ein[0][0], ein[0][1], r], [ein[1][0], ein[1][1], r], [ein[2][0], ein[2][1], r]], [kanten, ein]);
    const schale = abzug(w, aussen, [PZ(w, -1, FINGER_H + 1, [], [versetzt(tri, WAND)])]);
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
    return abzug(w, vereinige(w, teile), [ZZ(w, 3.8, 4, -5, -1, FINGER_H + 1), ZZ(w, 3.8, 4, 4, -1, FINGER_H + 1)]);
}

// =====================================================================================
//  KIT-TEILE
// =====================================================================================
// Kugellager 6801-2RS: Außenring, Innenring, 10 Kugeln mit Käfig, 2 Dichtscheiben (Achse z, z 0..5)
function baueLager(w is map, Ts is array, gruppe is string)
{
    const dm = 16.5;
    const dk = 2.381;
    const aussen = abzug(w, rohr(w, 9.1, LAGER_D / 2, 0, LAGER_B),
        [torus(w, dm / 2, dk / 2 + 0.03, LAGER_B / 2), ZZ(w, 19.4, 0, 0, -1, 0.8), ZZ(w, 19.4, 0, 0, LAGER_B - 0.8, LAGER_B + 1)]);
    verteile(w, aussen, Ts, gruppe ~ " - Lager 6801 Außenring", F_METALL, W_LSTAHL);
    const innen = abzug(w, rohr(w, LAGER_DI / 2, 7.4, 0, LAGER_B), [torus(w, dm / 2, dk / 2 + 0.03, LAGER_B / 2)]);
    verteile(w, innen, Ts, gruppe ~ " - Lager 6801 Innenring", F_METALL, W_LSTAHL);
    var kugeln = [rohr(w, dm / 2 - 0.35, dm / 2 + 0.35, LAGER_B / 2 - 0.9, LAGER_B / 2 + 0.9)];
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

// 28BYJ-48 mit ULN2003-Kabel: Welle = z, Ohr-Oberseite z = 0, Körper nach -x versetzt
function baueSchrittmotor(w is map, T is Transform)
{
    const gx = -8;
    const koerper = abzug(w, vereinige(w, [
        ZZ(w, 28, gx, 0, -19, -0.8),
        ZZ(w, 28.4, gx, 0, -10.2, -9.6),
        ZZ(w, 27, gx, 0, -0.8, 0)
    ]), [ZZ(w, 1.2, gx + 9, 9, -1, 1), ZZ(w, 1.2, gx + 9, -9, -1, 1)]);
    fertig(w, koerper, "Basis - Schrittmotor 28BYJ-48 Gehäuse", F_METALL, T, W_STAHL);
    setzeMasse(w, koerper, "28BYJ-48 (Gehäuse, Spulen, Getriebe)", 26);
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
    const stecker = abzug(w, quader(w, [gx - 82, -6.2, -20.4], [12, 12.4, 5.8]), [quader(w, [gx - 83, -5.5, -19.6], [3, 11, 4.2])]);
    fertig(w, stecker, "Basis - Schrittmotor JST-XH-Stecker 5-polig", F_WEISS, T, W_PA);
}

// Lichtschranke: LED innen (strahlt nach außen), Fotowiderstand außen, Rock läuft dazwischen
function baueLichtschranke(w is map)
{
    const led = vereinige(w, [
        ZX(w, 5, -ROCK_R1 + 2.5, -15.4, 0, LSK_Z),
        schnitt(w, kugel(w, [-ROCK_R1 + 2.5, 0, LSK_Z], 5), quader(w, [-ROCK_R1 - 1, -3, LSK_Z - 3], [3.5, 6, 6])),
        ZX(w, 5.8, -15.4, -14.4, 0, LSK_Z)
    ]);
    fertig(w, led, "Basis - LED 5 mm rot", color(0.9, 0.15, 0.1), identityTransform(), ["Epoxid", 1.2]);
    for (var y in [-1.27, 1.27])
    {
        fertig(w, quader(w, [-14.4, y - 0.25, LSK_Z - 0.25], [7, 0.5, 0.5]), "Basis - LED Anschlussdraht", F_METALL, identityTransform(), W_STAHL);
    }
    const ldr = ZX(w, 5.1, -ROCK_R2 - 2.5, -ROCK_R2 - 0.5, 0, LSK_Z);
    fertig(w, ldr, "Basis - Fotowiderstand GL5528", color(0.85, 0.55, 0.25), identityTransform(), ["Keramik", 2.5]);
    for (var y in [-1.7, 1.7])
    {
        fertig(w, ZX(w, 0.5, -ROCK_R2 - 30, -ROCK_R2 - 2.5, y, LSK_Z), "Basis - Fotowiderstand Anschlussdraht", F_METALL, identityTransform(), W_STAHL);
    }
}

// Gleichstrommotor Baugröße 130: Achse +x, Motorkörper x 0..25, Welle nach -x
function baueGleichstrommotor(w is map, T is Transform)
{
    const md = w.motD;
    const fl = w.motFl;
    const dose = schnitt(w, ZX(w, md, 0, 19.5, 0, 0), quader(w, [-1, -md, -fl / 2], [21, 2 * md, fl]));
    const gehaeuse = abzug(w, vereinige(w, [dose, ZX(w, 6.2, -1.6, 0, 0, 0)]), [ZX(w, 2.2, 15, 20, 0, fl / 2 - 0.6), ZX(w, 2.2, 15, 20, 0, -fl / 2 + 0.6)]);
    fertig(w, gehaeuse, "Schulter - Gleichstrommotor Gehäuse", F_METALL, T, W_STAHL);
    setzeMasse(w, gehaeuse, "Gleichstrommotor 130 (Gehäuse, Anker, Magnete)", 14);
    const kappe = schnitt(w, vereinige(w, [ZX(w, md - 0.4, 19.5, MOT_L, 0, 0), ZX(w, 5, MOT_L, MOT_L + 1.2, 0, 0)]), quader(w, [19, -md, -fl / 2], [8, 2 * md, fl]));
    fertig(w, kappe, "Schulter - Gleichstrommotor Endkappe", F_WEISS, T, W_PBT);
    setzeMasse(w, qUnion([kappe]), "Gleichstrommotor 130 (Anteil Kappe)", 3);
    fertig(w, ZX(w, w.motW, -12, MOT_L + 1, 0, 0), "Schulter - Gleichstrommotor Welle", F_STAHL, T, W_STAHL);
    for (var s in [-1, 1])
    {
        const fahne = abzug(w, quader(w, [MOT_L - 0.5, s * 5.5 - 1.5, -0.2], [4.5, 3, 0.4]), [ZX(w, 1.2, MOT_L + 1.8, MOT_L + 4, s * 5.5, 0)]);
        fertig(w, bewege(w, fahne, Rx(90)), "Schulter - Gleichstrommotor Lötfahne", F_MESSING, T, W_MESSING);
    }
    fertig(w, ZX(w, 1.4, MOT_L + 3, MOT_L + 60, 0, 5.5), "Schulter - Gleichstrommotor Kabel rot", F_ROT, T, W_PVC);
    fertig(w, ZX(w, 1.4, MOT_L + 3, MOT_L + 60, 0, -5.5), "Schulter - Gleichstrommotor Kabel schwarz", F_SCHWARZ, T, W_PVC);
}

// Potentiometer 10 kΩ links am Schulterbock (Schulter-Koordinaten)
function bauePoti(w is map, T is Transform)
{
    const gehaeuse = vereinige(w, [ZY(w, 16.5, -48, -42, 0, 0), ZY(w, 17, -42, -41, 0, 0)]);
    fertig(w, gehaeuse, "Schulter - Potentiometer Gehäuse", F_METALL, T, W_STAHL);
    setzeMasse(w, gehaeuse, "Potentiometer 10 kOhm", 7);
    fertig(w, ZY(w, 7, -41, -30, 0, 0), "Schulter - Potentiometer Gewindebuchse M7", F_MESSING, T, W_MESSING);
    fertig(w, abzug(w, prismaY(w, -33, -31.2, sechseck(10, 30, 0, 0)), [ZY(w, 7, -34, -30, 0, 0)]), "Schulter - Potentiometer Mutter M7", F_METALL, T, W_MESSING);
    fertig(w, ZY(w, w.potW, -30, -12, 0, 0), "Schulter - Potentiometer Welle", F_METALL, T, W_STAHL);
    for (var x in [-5, 0, 5])
    {
        fertig(w, abzug(w, quader(w, [x - 1, -46.5, -8.25 - 6], [2, 0.4, 6.2]), [ZY(w, 1, -47, -46, x, -8.25 - 4)]), "Schulter - Potentiometer Anschluss", F_MESSING, T, W_MESSING);
    }
}

// 15 Ein-Euro-Münzen im Magazin (Arm-Koordinaten)
function baueMuenzen(w is map, T is Transform)
{
    for (var i = 0; i < 15; i += 1)
    {
        const y = -3.2 - (i + 1) * 2.33;
        const m = vereinige(w, [ZY(w, 23.25, y, y + 2.33, -MUENZ_R, 0)]);
        fertig(w, m, "Hauptlenker - Münze 1 Euro", F_MUENZE, T, W_MESSING);
        setzeMasse(w, m, "1-Euro-Münze", 7.5);
    }
}

// SG90 im Greifergehäuse (Greifer-Koordinaten wie kit.scad)
function baueSG90(w is map, T is Transform)
{
    const g = -G_AB;
    const koerper = vereinige(w, [quader(w, [-16.6, g - 6, -19.9], [22.5, 12, 22.7]), ZZ(w, 11.8, 0, g, 2.8, 6.8), ZZ(w, 5.5, -5.5, g, 2.8, 5.8)]);
    fertig(w, koerper, "Handgelenk - Servo SG90 Gehäuse", F_SG90, T, W_ABS);
    setzeMasse(w, koerper, "SG90 (Gehäuse, Motor, Getriebe)", 8);
    const mx = (SG_X1 + SG_X2) / 2;
    const ohren = abzug(w, quader(w, [-21.4, g - 6, -6.5], [32.2, 12, 2.5]), [ZZ(w, 2, mx - 13.8, g, -7, -3), ZZ(w, 2, mx + 13.8, g, -7, -3)]);
    fertig(w, ohren, "Handgelenk - Servo SG90 Befestigungslaschen", F_SG90, T, W_ABS);
    fertig(w, ZZ(w, 4.8, 0, g, 6.8, 10), "Handgelenk - Servo SG90 Abtriebswelle", F_WEISS, T, W_PA);
    const horn = vereinige(w, [PZ(w, 10, 11.8, [], [huelle(concatenateArrays([kreisPunkte([0, g], 3.6, 32), kreisPunkte([15.5, g], 2.2, 24)]))]), ZZ(w, 6.5, 0, g, 7.5, 10)]);
    fertig(w, abzug(w, horn, [ZZ(w, 2, 0, g, 9, 13)]), "Handgelenk - Servo SG90 Horn (einarmig)", F_WEISS, T, W_PA);
    for (var x in [mx - 13.8, mx + 13.8])
    {
        fertig(w, vereinige(w, [ZZ(w, 1.8, x, g, -6.5, 1), ZZ(w, 3.6, x, g, -7.7, -6.5)]), "Handgelenk - SG90-Schraube (aus dem Servo-Beutel)", F_STAHL, T, W_STAHL);
    }
    const farben = [F_BRAUN, F_ROT, F_ORANGE];
    const namen = ["braun", "rot", "orange"];
    for (var i = 0; i < 3; i += 1)
    {
        fertig(w, ZX(w, 1.1, -60, -16.6, g + (i - 1) * 1.15, -17), "Handgelenk - Servo SG90 Kabel " ~ namen[i], farben[i], T, W_PVC);
    }
}

// =====================================================================================
//  ELEKTRONIK (neben der Basis auf dem Tisch)
// =====================================================================================
function dip(w is map, x is number, y is number, z is number, n is number, name is string)
{
    const l = n / 2 * 2.54;
    fertig(w, abzug(w, quader(w, [x, y - 3.2, z + 0.5], [l, 6.4, 3.3]), [ZZ(w, 1.6, x, y, z + 3.4, z + 4)]), "Elektronik - " ~ name, F_SCHWARZ, identityTransform(), W_EPOXID);
    for (var i = 0; i < n / 2; i += 1)
    {
        for (var s in [-1, 1])
        {
            fertig(w, quader(w, [x + 1.27 + 2.54 * i - 0.25, y + s * 3.81 - 0.25, z - 3], [0.5, 0.5, 5]), "Elektronik - IC-Beinchen", F_METALL, identityTransform(), W_CU);
        }
    }
}

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
        fertig(w, quader(w, [bx + 5, by + st[0] - 0.3, z0 + 8.5], [155, 0.6, 0.05]), "Elektronik - Steckbrett Markierung", st[1], identityTransform(), ["Farbe", 0]);
    }
    const zb = z0 + 8.5;
    // ---------- ESP32-S3-Board (Freenove WROOM), Stiftreihen 22.86 mm ----------
    const ex = bx + 3.81 + 2.54 * 2;  const ey = by + yc;
    const pcb = quader(w, [ex - 2, ey - 13, zb + 2.5], [63.5, 26, 1.6]);
    fertig(w, abzug(w, pcb, [ZZ(w, 3, ex + 0.5, ey - 10.5, zb, zb + 5), ZZ(w, 3, ex + 0.5, ey + 10.5, zb, zb + 5), ZZ(w, 3, ex + 59, ey - 10.5, zb, zb + 5), ZZ(w, 3, ex + 59, ey + 10.5, zb, zb + 5)]),
        "Elektronik - ESP32-S3 Leiterplatte", F_SCHWARZ, identityTransform(), W_FR4);
    const zp = zb + 4.1;
    fertig(w, quader(w, [ex + 39, ey - 9, zp], [18, 25.5, 0.8]), "Elektronik - ESP32-S3-WROOM-1 Modul-Platine", F_SCHWARZ, identityTransform(), W_FR4);
    fertig(w, quader(w, [ex + 39.4, ey - 8.3, zp + 0.8], [15.8, 17.6, 2.3]), "Elektronik - ESP32-S3-WROOM-1 Abschirmung", F_METALL, identityTransform(), ["Weißblech", 7.3]);
    fertig(w, quader(w, [ex + 26, ey - 8, zp], [5, 16, 1.2]), "Elektronik - Kamera-Steckverbinder", F_WEISS, identityTransform(), W_PA);
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
    fertig(w, quader(w, [ex + 14, ey - 9, zp], [6.5, 3.5, 1.6]), "Elektronik - Spannungsregler", F_SCHWARZ, identityTransform(), W_EPOXID);
    for (var s in [-1, 1])
    {
        fertig(w, quader(w, [ex - 1.27, ey + s * 11.43 - 1.27, zb], [50.8, 2.54, 2.5]), "Elektronik - Stiftleiste 20-polig", F_SCHWARZ, identityTransform(), W_PBT);
        for (var i = 0; i < 20; i += 1)
        {
            fertig(w, quader(w, [ex + 2.54 * i - 0.32, ey + s * 11.43 - 0.32, zb - 6], [0.64, 0.64, 10.6]), "Elektronik - Stift", F_MESSING, identityTransform(), W_MESSING);
        }
    }
    // ---------- L293D über der Mittelrinne ----------
    dip(w, bx + 3.81 + 2.54 * 30 - 1.27, by + yc, zb, 16, "L293D Motortreiber");
    // ---------- ULN2003-Treiberplatine ----------
    const ux = 95;  const uy = -205;
    fertig(w, quader(w, [ux, uy, z0 + 2], [35, 32, 1.6]), "Elektronik - ULN2003 Leiterplatte", F_GRUEN_PCB, identityTransform(), W_FR4);
    const zu = z0 + 3.6;
    dip(w, ux + 8, uy + 16, zu, 16, "ULN2003A");
    for (var i = 0; i < 4; i += 1)
    {
        fertig(w, vereinige(w, [ZZ(w, 3, ux + 6 + 6 * i, uy + 4, zu, zu + 3.5), kugel(w, [ux + 6 + 6 * i, uy + 4, zu + 3.5], 3)]), "Elektronik - LED 3 mm rot", color(0.9, 0.15, 0.1), identityTransform(), ["Epoxid", 1.2]);
    }
    fertig(w, abzug(w, quader(w, [ux + 2, uy + 23, zu], [12.4, 5.75, 7]), [quader(w, [ux + 2.6, uy + 23.5, zu + 1], [11.2, 4.4, 7])]), "Elektronik - JST-XH-Buchse 5-polig", F_WEISS, identityTransform(), W_PA);
    fertig(w, quader(w, [ux + 16.7, uy + 23.7, zu], [15.24, 2.54, 2.5]), "Elektronik - Stiftleiste 6-polig", F_SCHWARZ, identityTransform(), W_PBT);
    // ---------- Elko 100 µF an L293D ----------
    const kx = bx + 3.81 + 2.54 * 38;  const ky = by + 5.08;
    fertig(w, ZZ(w, 6.3, kx, ky, zb + 2, zb + 13), "Elektronik - Elko 100 µF", color(0.10, 0.20, 0.45), identityTransform(), ["Elko", 1.3]);
    fertig(w, ZZ(w, 5.5, kx, ky, zb + 13, zb + 13.3), "Elektronik - Elko Deckel", F_METALL, identityTransform(), ["Aluminium", 2.7]);
    // ---------- Widerstände 220 Ω und 10 kΩ ----------
    const ringe = [[[F_ROT, F_ROT, F_BRAUN, F_GELB], "220 Ohm"], [[F_BRAUN, F_SCHWARZ, F_ORANGE, F_GELB], "10 kOhm"]];
    for (var i = 0; i < 2; i += 1)
    {
        const rx = bx + 3.81 + 2.54 * (44 + 6 * i);  const ry = by + yc + 8.89;
        fertig(w, ZX(w, 2.5, rx - 3.15, rx + 3.15, ry, zb + 3), "Elektronik - Widerstand " ~ ringe[i][1], color(0.35, 0.55, 0.85), identityTransform(), ["Keramik", 2.5]);
        for (var r = 0; r < 4; r += 1)
        {
            const xr = rx - 2.3 + r * 1.2 + (r == 3 ? 0.9 : 0);
            fertig(w, ZX(w, 2.6, xr, xr + 0.6, ry, zb + 3), "Elektronik - Farbring", ringe[i][0][r], identityTransform(), ["Farbe", 0]);
        }
        for (var s in [-1, 1])
        {
            fertig(w, ZX(w, 0.6, rx + s * 3.15, rx + s * 5.08, ry, zb + 3), "Elektronik - Anschlussdraht", F_METALL, identityTransform(), W_CU);
            fertig(w, ZZ(w, 0.6, rx + s * 5.08, ry, zb - 6, zb + 3), "Elektronik - Anschlussdraht", F_METALL, identityTransform(), W_CU);
        }
    }
    // ---------- Taster 6x6 (Start / Stopp) ----------
    const tx = bx + 3.81 + 2.54 * 56;  const ty = by + yc;
    fertig(w, quader(w, [tx - 3, ty - 3, zb], [6, 6, 3.5]), "Elektronik - Taster 6x6 Gehäuse", F_SCHWARZ, identityTransform(), W_PA);
    fertig(w, ZZ(w, 3.5, tx, ty, zb + 3.5, zb + 5), "Elektronik - Taster 6x6 Knopf", F_SCHWARZ, identityTransform(), W_PA);
    // ---------- USB-Netzteil 5 V / 2 A mit USB-C-Kabel ----------
    const nx = -150;  const ny = -230;
    const netz = vereinige(w, [quader(w, [nx, ny, z0], [40, 40, 26])]);
    fertig(w, netz, "Elektronik - USB-Netzteil 5 V 2 A", F_WEISS, identityTransform(), W_ABS);
    setzeMasse(w, netz, "USB-Netzteil", 60);
    for (var dx in [-9.5, 9.5])
    {
        fertig(w, ZX(w, 4, nx - 19, nx, ny + 20 + dx, z0 + 13), "Elektronik - Netzstecker Stift", F_METALL, identityTransform(), W_MESSING);
    }
    fertig(w, zyl(w, [nx + 40, ny + 20, z0 + 13], [ex - 20, ey - 1, z0 + 13], 3.5), "Elektronik - USB-C-Kabel", F_SCHWARZ, identityTransform(), W_PVC);
}
