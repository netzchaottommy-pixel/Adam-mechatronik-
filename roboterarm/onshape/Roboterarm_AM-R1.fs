FeatureScript 1576;
import(path : "onshape/std/geometry.fs", version : "1576.0");

// =====================================================================================
//  Roboterarm AM-R1  Version 2  –  komplettes Modell als Onshape-Feature
//
//  Gleiche Maße und Teile wie roboterarm.scad (Druckdateien). Jedes Teil ist ein eigener
//  Part mit Namen und Farbe. An allen Gelenken liegen Mate Connectors auf beiden Teilen,
//  damit im Assembly nur noch "Revolute" geklickt werden muss.
//
//  Einbau: neues Feature Studio anlegen, die ersten zwei Zeilen dort stehen lassen und
//  alles ab hier darunter einfügen. Dann im Part Studio: Werkzeugleiste -> "Roboterarm AM-R1".
// =====================================================================================

// ---------- Kugellager 6801-2RS ----------
const LAGER_D = 21;   const LAGER_DI = 12;   const LAGER_B = 5;
const SITZ_D = 20.95; const LIPPE_D = 16;

// ---------- Basis ----------
const BASIS_R = 110;  const BODEN_T = 3;  const BASIS_H = 55;
const INNEN_R1 = 66;  const INNEN_R2 = 70;  const AUSSEN_T = 2.4;
const DECKEL_T = 4;   const ROLLE_R = 88;
const ACHSE_H = 12.5;                         // Rollenachse über Deckelring
const DECKEL_OBEN = 59;  const TT_UNTEN = 82;  const TT_T = 8;  const TT_OBEN = 90;
const TT_R = 106;  const SCHUERZE_R = 111;  const SAEULE_R = 15;  const KABEL_R = 8;
const ZR_M = 1.25;  const ZR_Z1 = 15;  const ZR_Z2 = 75;  const ZR_A = 56.4;
const KRANZ_RI = 40;  const KRANZ_UNTEN = 74;
const SM_OHR = 17.5;  const RITZEL_OBEN = 81.5;  const SM_OHR_Z = 71.5;
const SM_WX = 56.4;  const SM_GX = 48.4;
const LSK_Z = 77;  const ROCK_R1 = 23;  const ROCK_R2 = 25;

// ---------- Servos DS3218 / MG996R ----------
const SV_L = 40.7;  const SV_B = 20;  const SV_W = 10.2;  const SV_UNTER = 27.6;
const SV_OHR_L = 54.5;  const SV_LOCH_A = 49.5;  const SV_LOCH_B = 10;
const HORN_ABST = 18;  const HORN_D = 25;

// ---------- Schulter ----------
const H_S = 70;  const S_M = 1.5;  const S_Z1 = 22;  const S_Z2 = 44;  const S_A = 49.65;
const S_ZR_H = 7;
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
const G_M = 1.5;  const G_Z = 24;  const G_ZR_H = 8;  const G_AB = 18.15;  const G_Z0 = 11;
const G_RUECK = -24;  const G_UNTEN = -36;  const G_GA = 43.5;
const FINGER_L = 60;  const FINGER_H = 30;  const FINGER_B = 22;  const FINGER_X = 22;
const WAND = 1.2;  const RIPPE = 1;  const RIPPEN_ABST = 7;  const FINGER_DY = 7.15;
const G_BODEN_SCHR = [[-18, -41], [-18, 41], [8, -41], [8, 41]];
const G_HAUBE_SCHR = [[-16, -41], [-16, 41], [2, -41], [2, 41]];

// ---------- Farben ----------
const F_BASIS = color(0.80, 0.80, 0.80);
const F_GRAU = color(0.35, 0.40, 0.45);
const F_DUNKEL = color(0.22, 0.27, 0.30);
const F_ARM = color(0.86, 0.32, 0.25);
const F_ARM2 = color(0.74, 0.26, 0.22);
const F_GOLD = color(0.92, 0.74, 0.20);
const F_STANGE = color(0.85, 0.80, 0.50);
const F_GREIFER = color(0.25, 0.48, 0.70);
const F_HEBEL = color(0.95, 0.60, 0.10);
const F_TPU = color(0.15, 0.15, 0.15);
const F_METALL = color(0.70, 0.70, 0.74);
const F_SERVO = color(0.08, 0.08, 0.08);
const F_MOTOR = color(0.20, 0.35, 0.85);

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
        annotation { "Name" : "Mate Connectors an den Gelenken", "Default" : true }
        definition.zeigeConnectors is boolean;
    }
    {
        var w = { "ctx" : context, "id" : id, "z" : new box(0) };
        const al = definition.alpha / degree;
        const bt = definition.beta / degree;
        const th = bt + 180;
        const gr = definition.greif / degree;
        const bw = definition.basisw / degree;
        const S = TT_OBEN + H_S;
        const E = [L1 * cos(al * degree), L1 * sin(al * degree)];
        const K = [KURBEL * cos(th * degree), KURBEL * sin(th * degree)];

        // ---------- Lagen (Transformationen) ----------
        const T_dreh = Rz(bw);
        const T_sch = T_dreh * Tt(0, 0, S);
        const T_ober = T_sch * Ry(-al);
        const T_kurb = T_sch * Ry(-th);
        const T_ell = T_sch * Tt(E[0], 0, E[1]);
        const T_unter = T_ell * Ry(-bt);
        const T_greif = T_unter * Tt(L2 - G_RUECK, 0, 8);
        const T_hebA = T_greif * Tt(0, -G_AB, G_Z0) * Rz(-gr);
        const T_hebB = T_greif * Tt(0, G_AB, G_Z0) * Rz(gr);

        var teile = {};

        // ================= BASIS =================
        if (definition.zeigeBasis)
        {
            teile.basis = fertig(w, baueBasis(w), "Basis", F_BASIS, identityTransform());
            teile.deckelring = fertig(w, baueDeckelring(w), "Basis - Deckelring", F_DUNKEL, Tt(0, 0, BASIS_H));
            for (var k = 0; k < 4; k += 1)
            {
                fertig(w, baueRollenachse(w), "Basis - Rollenachse " ~ (k + 1), F_GOLD, Rz(45 + 90 * k) * Tt(ROLLE_R, 0, DECKEL_OBEN + ACHSE_H));
            }
            teile.ritzelBasis = fertig(w, baueRitzelBasis(w), "Ritzel Basis", F_METALL, Tt(SM_WX, 0, KRANZ_UNTEN + 0.5));
            teile.kappe = fertig(w, baueKappe(w), "Basis - Kappe", F_DUNKEL, Tt(0, 0, TT_OBEN + 0.2));
            teile.drehteller = fertig(w, baueDrehteller(w), "Schulter - Drehteller", color(0.92, 0.92, 0.92), T_dreh * Tt(0, 0, TT_UNTEN));
        }
        // ================= SCHULTER =================
        if (definition.zeigeSchulter)
        {
            teile.gehL = fertig(w, baueGehaeuse(w, -1), "Schulter - Gehäuse links", F_GRAU, T_sch);
            teile.gehR = fertig(w, baueGehaeuse(w, 1), "Schulter - Gehäuse rechts", F_GRAU, T_sch);
            fertig(w, baueGehaeuseDeckel(w, -1), "Schulter - Deckel links", F_DUNKEL, T_sch);
            fertig(w, baueGehaeuseDeckel(w, 1), "Schulter - Deckel rechts", F_DUNKEL, T_sch);
            teile.ritzelA = fertig(w, baueSchulterRitzel(w), "Ritzel Schulter", F_METALL, T_sch * Tt(PIN_A[0], -25, PIN_A[1]) * Rx(90));
            teile.ritzelB = fertig(w, baueSchulterRitzel(w), "Ritzel Ellbogen", F_METALL, T_sch * Tt(PIN_B[0], 25, PIN_B[1]) * Rx(-90));
        }
        // ================= ARM =================
        if (definition.zeigeArm)
        {
            teile.kasten = fertig(w, baueKasten(w), "Oberarm - Kasten", F_ARM, T_ober);
            fertig(w, baueKastenDeckel(w), "Oberarm - Deckel", F_ARM2, T_ober);
            fertig(w, baueAchse(w, ACHSE_SCHULTER), "Oberarm - Achse Schulter", F_GOLD, T_ober * Tt(0, -12, 0) * Rx(90));
            fertig(w, baueAchse(w, ACHSE_STUMMEL), "Oberarm - Achse Stummel", F_GOLD, T_ell * Ry(-al) * Tt(0, -12, 0) * Rx(90));
            teile.zahnradA = fertig(w, baueSchulterZahnrad(w), "Oberarm - Zahnrad Schulter", F_METALL, T_ober * Tt(0, -24.5, 0) * Rx(90));
            fertig(w, baueRing(w, 1.5), "Oberarm - Distanzring Schulter", F_GOLD, T_sch * Tt(0, -18, 0) * Rx(90));
            fertig(w, baueRing(w, 1), "Oberarm - Distanzring Ellbogen", F_GOLD, T_ell * Tt(0, -18, 0) * Rx(90));

            teile.kurbel = fertig(w, baueKurbel(w), "Kurbel", F_GOLD, T_kurb * Tt(0, -1, 0) * Rx(-90));
            fertig(w, baueAchse(w, ACHSE_KURBEL), "Kurbel - Achse", F_GOLD, T_kurb * Tt(0, -1, 0) * Rx(-90));
            teile.zahnradB = fertig(w, baueSchulterZahnrad(w), "Kurbel - Zahnrad Ellbogen", F_METALL, T_kurb * Tt(0, 24.5, 0) * Rx(-90));
            fertig(w, baueRing(w, 1.5), "Kurbel - Distanzring", F_GOLD, T_sch * Tt(0, 18, 0) * Rx(-90));

            teile.stange = fertig(w, baueKoppelstange(w), "Koppelstange", F_STANGE, T_sch * Tt(K[0], 0, K[1]) * Ry(-al) * Tt(0, -8, 0) * Rx(-90));

            teile.unterarm = fertig(w, baueUnterarmKanal(w), "Unterarm - Kanal", F_ARM, T_unter);
            fertig(w, baueUnterarmDeckel(w), "Unterarm - Deckel", F_ARM2, T_unter);
            teile.hebel = fertig(w, baueKurbel(w), "Unterarm - Hebel", F_GOLD, T_ell * Ry(-th) * Tt(0, -1, 0) * Rx(-90));
            fertig(w, baueAchse(w, ACHSE_ELLBOGEN), "Unterarm - Achse Ellbogen", F_GOLD, T_ell * Ry(-th) * Tt(0, -1, 0) * Rx(-90));
            fertig(w, baueRing(w, 1), "Unterarm - Distanzring", F_GOLD, T_ell * Tt(0, 18, 0) * Rx(-90));
        }
        // ================= GREIFER =================
        if (definition.zeigeGreifer)
        {
            teile.greifer = fertig(w, baueGreiferGehaeuse(w), "Unterarm - Greifer-Gehäuse", F_GREIFER, T_greif);
            fertig(w, baueGreiferHaube(w), "Unterarm - Greifer-Haube", F_GREIFER, T_greif * Tt(0, 0, 24));
            fertig(w, baueGreiferBoden(w), "Unterarm - Greifer-Boden", F_GREIFER, T_greif * Tt(0, 0, G_UNTEN - 4));
            teile.hebelA = fertig(w, baueGreiferhebel(w, true), "Greiferhebel A (Antrieb)", F_HEBEL, T_hebA);
            fertig(w, baueFinger(w), "Greiferhebel A - Finger (TPU)", F_TPU, T_hebA * Tt(FINGER_X, FINGER_DY, 3));
            fertig(w, baueKlemmplatte(w, 1), "Greiferhebel A - Klemmplatte", F_HEBEL, T_hebA * Tt(FINGER_X, FINGER_DY, 3 + FINGER_H));
            teile.hebelB = fertig(w, baueGreiferhebel(w, false), "Greiferhebel B (Mitläufer)", F_HEBEL, T_hebB);
            fertig(w, baueFinger(w), "Greiferhebel B - Finger (TPU)", F_TPU, T_hebB * Tt(FINGER_X, -FINGER_DY, 3 + FINGER_H) * Rx(180));
            fertig(w, baueKlemmplatte(w, -1), "Greiferhebel B - Klemmplatte", F_HEBEL, T_hebB * Tt(FINGER_X, -FINGER_DY, 3 + FINGER_H));
        }
        // ================= KAUFTEILE =================
        if (definition.zeigeKaufteile)
        {
            if (definition.zeigeBasis)
            {
                fertig(w, baueSchrittmotor(w), "Basis - Schrittmotor 28BYJ-48", F_MOTOR, Tt(SM_WX, 0, SM_OHR_Z));
                for (var k = 0; k < 4; k += 1)
                {
                    fertig(w, baueLager(w), "Basis - Lager 6801 Laufrolle " ~ (k + 1), F_METALL,
                        Rz(45 + 90 * k) * Tt(ROLLE_R - LAGER_B / 2, 0, DECKEL_OBEN + ACHSE_H) * Ry(90));
                }
            }
            if (definition.zeigeSchulter)
            {
                fertig(w, baueLager(w), "Schulter - Lager 6801 Innenwand links", F_METALL, T_sch * Tt(0, -YW, 0) * Rx(-90));
                fertig(w, baueLager(w), "Schulter - Lager 6801 Innenwand rechts", F_METALL, T_sch * Tt(0, YW - LAGER_B, 0) * Rx(-90));
                fertig(w, baueLager(w), "Schulter - Lager 6801 Deckel links", F_METALL, T_sch * Tt(0, -YO - LAGER_B, 0) * Rx(-90));
                fertig(w, baueLager(w), "Schulter - Lager 6801 Deckel rechts", F_METALL, T_sch * Tt(0, YO, 0) * Rx(-90));
                fertig(w, baueServo(w), "Schulter - Servo DS3218 (Schulter)", F_SERVO, T_sch * Tt(PIN_A[0], -7, PIN_A[1]) * Rx(90));
                fertig(w, baueServo(w), "Schulter - Servo DS3218 (Ellbogen)", F_SERVO, T_sch * Tt(PIN_B[0], 7, PIN_B[1]) * Rx(-90));
                fertig(w, baueServoscheibe(w), "Ritzel Schulter - Servoscheibe", F_METALL, T_sch * Tt(PIN_A[0], -7, PIN_A[1]) * Rx(90));
                fertig(w, baueServoscheibe(w), "Ritzel Ellbogen - Servoscheibe", F_METALL, T_sch * Tt(PIN_B[0], 7, PIN_B[1]) * Rx(-90));
            }
            if (definition.zeigeArm)
            {
                fertig(w, baueLager(w), "Oberarm - Lager 6801 Schulter", F_METALL, T_ober * Tt(0, K_Y - LAGER_B, 0) * Rx(-90));
                fertig(w, baueLager(w), "Oberarm - Lager 6801 Ellbogen", F_METALL, T_ober * Tt(L1, K_Y - LAGER_B, 0) * Rx(-90));
                fertig(w, baueLager(w), "Unterarm - Lager 6801", F_METALL, T_ell * Tt(0, -UA_I - LAGER_B, 0) * Rx(-90));
                fertig(w, baueStift(w), "Kurbel - Stift M3x16", F_METALL, T_sch * Tt(K[0], 0, K[1]));
                fertig(w, baueStift(w), "Unterarm - Stift M3x16", F_METALL, T_ell * Tt(K[0], 0, K[1]));
            }
            if (definition.zeigeGreifer)
            {
                fertig(w, baueServo(w), "Unterarm - Servo MG996R (Greifer)", F_SERVO, T_greif * Tt(0, -G_AB, -5) * Rz(-90));
                fertig(w, baueServoscheibe(w), "Greiferhebel A - Servoscheibe", F_METALL, T_greif * Tt(0, -G_AB, -5) * Rz(-90));
            }
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
            gelenk(w, teile, ["greifer", "hebelA"], T_greif * Tt(0, -G_AB, G_Z0), Z, X);                      // J7 Greifer A
            gelenk(w, teile, ["greifer", "hebelB"], T_greif * Tt(0, G_AB, G_Z0), Z, X);                       // J8 Greifer B
            gelenk(w, teile, ["gehL", "ritzelA"], T_sch * Tt(PIN_A[0], -25, PIN_A[1]), Y, X);                 // J9 Ritzel Schulter
            gelenk(w, teile, ["gehR", "ritzelB"], T_sch * Tt(PIN_B[0], 25, PIN_B[1]), Y, X);                  // J10 Ritzel Ellbogen
            gelenk(w, teile, ["basis", "ritzelBasis"], Tt(SM_WX, 0, KRANZ_UNTEN + 0.5), Z, X);               // J11 Ritzel Basis
            gelenk(w, teile, ["kasten", "zahnradA"], T_sch * Tt(0, -24.5, 0), Y, X);                          // Zahnrad fest am Oberarm
            gelenk(w, teile, ["kurbel", "zahnradB"], T_sch * Tt(0, 24.5, 0), Y, X);                           // Zahnrad fest an Kurbel
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

function V(p is array) returns Vector
{
    return vector(p[0], p[1], p[2]) * millimeter;
}

function V2(p is array) returns Vector
{
    return vector(p[0], p[1]) * millimeter;
}

function Tt(x is number, y is number, z is number) returns Transform
{
    return transform(vector(x, y, z) * millimeter);
}

function Rx(a is number) returns Transform
{
    return rotationAround(line(vector(0, 0, 0) * millimeter, vector(1, 0, 0)), a * degree);
}

function Ry(a is number) returns Transform
{
    return rotationAround(line(vector(0, 0, 0) * millimeter, vector(0, 1, 0)), a * degree);
}

function Rz(a is number) returns Transform
{
    return rotationAround(line(vector(0, 0, 0) * millimeter, vector(0, 0, 1)), a * degree);
}

function dreh2(p is array, a is number) returns array
{
    return [p[0] * cos(a * degree) - p[1] * sin(a * degree), p[0] * sin(a * degree) + p[1] * cos(a * degree)];
}

function polar(r is number, a is number) returns array      // a in rad
{
    return [r * cos(a * radian), r * sin(a * radian)];
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

function quader(w is map, c is array, g is array) returns Query
{
    const i = nid(w);
    fCuboid(w.ctx, i, { "corner1" : V(c), "corner2" : V([c[0] + g[0], c[1] + g[1], c[2] + g[2]]) });
    return qCreatedBy(i, EntityType.BODY);
}

// Zylinder entlang y (wie ZY in OpenSCAD)
function ZY(w is map, d is number, y1 is number, y2 is number, x is number, v is number) returns Query
{
    return zyl(w, [x, y1, v], [x, y2, v], d);
}

// Zylinder entlang z
function ZZ(w is map, d is number, x is number, y is number, z1 is number, z2 is number) returns Query
{
    return zyl(w, [x, y, z1], [x, y, z2], d);
}

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

// Profil in der xy-Ebene von z1 bis z2
function PZ(w is map, z1 is number, z2 is number, kreise is array, polys is array) returns Query
{
    return profil(w, [0, 0, z1], [0, 0, 1], [1, 0, 0], z2 - z1, kreise, polys);
}

// Profil in der Armebene (x, v) zwischen y1 und y2 (wie XV in OpenSCAD)
function PXV(w is map, y1 is number, y2 is number, kreise is array, polys is array) returns Query
{
    return profil(w, [0, y2, 0], [0, -1, 0], [1, 0, 0], y2 - y1, kreise, polys);
}

// Außentangenten zwischen zwei Kreisen -> Viereck (zusammen mit den Kreisen = Hülle)
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

// Tangentenpunkt von Punkt p an Kreis (c, r); seite = +1 / -1
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

// konvexe Hülle (monotone chain)
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

function sechseckRadius(d is number, drehung is number, mx is number, my is number) returns array  // d = Eckmaß (wie $fn=6)
{
    return sechseck(d * cos(30 * degree), drehung, mx, my);
}

// Evolventen-Zahnrad (Umriss als Polygon), phase in Grad
function zahnradPunkte(m is number, z is number, phase is number) returns array
{
    const rp = m * z / 2;
    const ra = rp + m;
    const rf = rp - 1.25 * m;
    const rb = rp * cos(20 * degree);
    const tmax = sqrt((ra / rb) ^ 2 - 1);
    const t0 = (rf > rb) ? sqrt((rf / rb) ^ 2 - 1) : 0;
    const inv20 = tan(20 * degree) - 20 * PI / 180;
    const halb = PI / (2 * z) + inv20 - 0.08 / rp / 2;
    const N = 6;
    var fl = [];
    for (var i = 0; i <= N; i += 1)
    {
        const t = t0 + (tmax - t0) * i / N;
        const x = rb * (cos(t * radian) + t * sin(t * radian));
        const y = rb * (sin(t * radian) - t * cos(t * radian));
        fl = append(fl, [sqrt(x * x + y * y), atan2(y, x) / radian - halb]);
    }
    var pts = [];
    for (var k = 0; k < z; k += 1)
    {
        const c = 2 * PI * k / z + phase * PI / 180;
        if (rf < rb)
        {
            pts = append(pts, polar(rf, c + fl[0][1]));
        }
        for (var i = 0; i <= N; i += 1)
        {
            pts = append(pts, polar(fl[i][0], c + fl[i][1]));
        }
        for (var i = N; i >= 0; i -= 1)
        {
            pts = append(pts, polar(fl[i][0], c - fl[i][1]));
        }
        if (rf < rb)
        {
            pts = append(pts, polar(rf, c - fl[0][1]));
        }
        const a1 = c - fl[0][1];
        const a2 = c + 2 * PI / z + fl[0][1];
        for (var j = 1; j <= 2; j += 1)
        {
            pts = append(pts, polar(rf, a1 + (a2 - a1) * j / 3));
        }
    }
    return pts;
}

function zahnrad(w is map, m is number, z is number, z1 is number, z2 is number, phase is number) returns Query
{
    return PZ(w, z1, z2, [], [zahnradPunkte(m, z, phase)]);
}

function fertig(w is map, q is Query, name is string, farbe is Color, T is Transform) returns Query
{
    opTransform(w.ctx, nid(w), { "bodies" : q, "transform" : T });
    setProperty(w.ctx, { "entities" : q, "propertyType" : PropertyType.NAME, "value" : name });
    setProperty(w.ctx, { "entities" : q, "propertyType" : PropertyType.APPEARANCE, "value" : farbe });
    return q;
}

// Mate Connector auf beiden Teilen eines Gelenks (gleiche Lage)
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
function baueBasis(w is map) returns Query
{
    var teile = [
        ZZ(w, 2 * BASIS_R, 0, 0, 0, BODEN_T),
        rohr(w, BASIS_R - AUSSEN_T, BASIS_R, 0, BASIS_H),
        rohr(w, INNEN_R1, INNEN_R2, 0, BASIS_H),
        ZZ(w, 2 * SAEULE_R, 0, 0, 0, TT_OBEN + 0.2),
        quader(w, [-9, -(BASIS_R - 0.5), 0], [18, BASIS_R - 0.5 - INNEN_R1, 17]),          // Kabelkanal
        quader(w, [SM_GX - 4, -SM_OHR, 0], [8, 2 * SM_OHR, 8]),
        quader(w, [-(ROCK_R1 - 2), -5, 0], [ROCK_R1 - SAEULE_R, 10, LSK_Z + 5]),           // LED-Block
        quader(w, [-(ROCK_R2 + 10), -5, 0], [8, 10, LSK_Z + 5]),                            // Fotowiderstand
        quader(w, [-INNEN_R1 - 1, -1.5, 0], [INNEN_R1 + 1 - ROCK_R2 - 9, 3, BASIS_H])
    ];
    for (var s in [-1, 1])
    {
        teile = append(teile, quader(w, [SM_GX - 4, s * SM_OHR - 3, 0], [8, 6, SM_OHR_Z]));
        teile = append(teile, quader(w, [SM_GX, s * SM_OHR - 1.5, 0], [INNEN_R1 - SM_GX + 1, 3, BASIS_H]));
    }
    for (var k = 0; k < 8; k += 1)
    {
        teile = append(teile, bewege(w, quader(w, [INNEN_R2 - 1, -1.2, 0], [BASIS_R - INNEN_R2, 2.4, 30]), Rz(22.5 + 45 * k)));
    }
    const b = vereinige(w, teile);
    var wz = [
        ZZ(w, 2 * KABEL_R, 0, 0, -1, 199),
        quader(w, [-6, -(SAEULE_R + 2), BODEN_T], [12, SAEULE_R + 2, 18]),                  // Kabelausgang Säule
        quader(w, [-7, -(INNEN_R1 + 54), BODEN_T], [14, 60, 12]),                            // Kanal innen
        zyl(w, [-(ROCK_R2 + 12), 0, LSK_Z], [-(ROCK_R2), 0, LSK_Z], 5.2),                    // Fotowiderstand
        zyl(w, [-ROCK_R1, 0, LSK_Z], [-KABEL_R + 1, 0, LSK_Z], 5.1)                          // LED
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
        wz = append(wz, ZZ(w, 2.6, SM_GX, s * SM_OHR, SM_OHR_Z - 9, SM_OHR_Z + 1));
    }
    for (var k = 0; k < 4; k += 1)
    {
        const p = polar(BASIS_R - 15, (45 + 90 * k) * PI / 180);
        wz = append(wz, ZZ(w, 12, p[0], p[1], -0.01, 1));                                     // Gummifüße
    }
    return abzug(w, b, wz);
}

function tropfen(w is map, x1 is number, x2 is number, zc is number, d is number) returns Query   // entlang x, Spitze oben
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

function baueRitzelBasis(w is map) returns Query
{
    const h = RITZEL_OBEN - (KRANZ_UNTEN + 0.5);
    const loch = schnitt(w, ZZ(w, 5.25, 0, 0, -1, 19), quader(w, [-1.575, -5, -20], [3.15, 10, 40]));
    return abzug(w, zahnrad(w, ZR_M, ZR_Z1, 0, h, 0), [loch]);
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
    const oy = s * (YW + 0.5 - HORN_ABST);
    const m = P[0] + SV_W - SV_L / 2;
    var wz = [
        quader(w, [P[0] + SV_W - SV_L - 0.4, min(oy, s * (YW + 0.1)), P[1] - SV_B / 2 - 0.4], [SV_L + 0.8, abs(s * (YW + 0.1) - oy), SV_B + 0.8]),
        quader(w, [m - SV_OHR_L / 2 - 0.5, min(oy, oy - s * (SV_UNTER + 0.8)), P[1] - SV_B / 2 - 0.4], [SV_OHR_L + 1, SV_UNTER + 0.8, SV_B + 0.8])
    ];
    for (var dx in [-1, 1])
    {
        for (var dv in [-1, 1])
        {
            wz = append(wz, zyl(w, [m + dx * SV_LOCH_A / 2, oy, P[1] + dv * SV_LOCH_B / 2], [m + dx * SV_LOCH_A / 2, oy + s * 12, P[1] + dv * SV_LOCH_B / 2], 2.6));
        }
    }
    if (s < 0)
    {
        wz = append(wz, ZY(w, HORN_D + 3, -YW - 1, -YW + 6, P[0], P[1]));
    }
    else
    {
        wz = append(wz, ZY(w, HORN_D + 3, YW - 6, YW + 1, P[0], P[1]));
    }
    return wz;
}

function baueGehaeuse(w is map, haelfte is number) returns Query
{
    const g = vereinige(w, [rundRechteck(w, -YO, YO), quader(w, [-80, -YC - 12, HV1], [100, 2 * (YC + 12), GH_FLANSCH_V])]);
    // Schwenkbereich des Oberarm-Kastens
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
    // nur die gewünschte Hälfte behalten
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

function baueSchulterRitzel(w is map) returns Query
{
    return abzug(w, zahnrad(w, S_M, S_Z1, 0, S_ZR_H, 0), [ZZ(w, 9, 0, 0, -1, 19), ZZ(w, HORN_D + 0.4, 0, 0, -0.01, 2)]);
}

function baueSchulterZahnrad(w is map) returns Query
{
    const r = vereinige(w, [zahnrad(w, S_M, S_Z2, 0.5, 0.5 + S_ZR_H, 0), ZZ(w, 14, 0, 0, 0, 8.5)]);
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
        PZ(w, 8 - 4.2, 8.8, [], [sechseckRadius(6.5, 30, KURBEL, 0)])          // Stoppmutter M3
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
        quader(w, [G_RUECK, -G_GA, -5], [30 - G_RUECK, 2 * G_GA, 5]),                       // Platte
        quader(w, [G_RUECK, -G_GA, G_UNTEN], [4, 2 * G_GA, 24 - G_UNTEN]),                  // Rückwand
        quader(w, [10.6, -G_GA, G_UNTEN], [4, 2 * G_GA, -5 - G_UNTEN]),                     // Front unten
        ZZ(w, 10, 0, G_AB, -1, G_Z0 - 0.2)                                                  // Zapfen
    ];
    for (var s in [-1, 1])
    {
        const y0 = (s > 0) ? 38.5 : -G_GA;
        teile = append(teile, quader(w, [G_RUECK, y0, G_UNTEN], [8 - G_RUECK, 5, 24 - G_UNTEN]));
        teile = append(teile, quader(w, [G_RUECK, y0, G_UNTEN], [14.6 - G_RUECK, 5, -G_UNTEN]));
    }
    const g = vereinige(w, teile);
    var wz = [
        quader(w, [-SV_B / 2 - 0.4, -G_AB - SV_W - 0.4, -10], [SV_B + 0.8, SV_L + 0.8, 20]),
        ZZ(w, 2.6, 0, G_AB, -2, 38),
        zyl(w, [G_RUECK - 1, 0, -16], [G_RUECK + 9, 0, -16], 10)
    ];
    for (var dy in [-1, 1])
    {
        for (var dx in [-1, 1])
        {
            wz = append(wz, ZZ(w, 2.6, dx * SV_LOCH_B / 2, -G_AB - SV_W + SV_L / 2 + dy * SV_LOCH_A / 2, -10, 0.5));
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
        wz = append(wz, ZZ(w, 6.2, p[0], p[1], 2.5, 5.5));
    }
    return abzug(w, quader(w, [G_RUECK, -G_GA, 0], [14.6 - G_RUECK, 2 * G_GA, 4]), wz);
}

// Greiferhebel mit Zahnrad; antrieb = false -> gespiegelter Mitläufer
function baueGreiferhebel(w is map, antrieb is boolean) returns Query
{
    const sy = antrieb ? 1 : -1;
    const y1 = -FINGER_B / 2 + FINGER_DY;
    const y2 = FINGER_B / 2 + FINGER_DY - 1;
    const ya = min(sy * y1, sy * y2);
    const yb = max(sy * y1, sy * y2);
    // Hülle aus Nabe (r 8) und Rückwand-Fuß
    // die Ecke nahe der Achse liegt in der Hülle -> Tangente an die äußere Ecke
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
        wz = append(wz, ZZ(w, HORN_D + 0.4, 0, 0, -0.01, 2));
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

// Fin-Ray-Finger (TPU): Dreieck mit Wänden, Querrippen und Griffnoppen
function baueFinger(w is map) returns Query
{
    const b = 8;
    const A = [b, -FINGER_B / 2];
    const B = [FINGER_L, FINGER_B / 2 - 0.01];
    const C = [b, FINGER_B / 2];
    // inneres Dreieck (Wandstärke WAND)
    const ux = B[0] - A[0];
    const uy = B[1] - A[1];
    const ul = sqrt(ux * ux + uy * uy);
    const nx = -uy / ul;
    const ny = ux / ul;                                  // Normale von AB nach innen
    const xi = b + WAND;
    const yo = FINGER_B / 2 - WAND;
    const yA = A[1] + (WAND - nx * (xi - A[0])) / ny;    // Schnitt versetzte AB-Linie mit x = xi
    const xB = A[0] + (WAND - ny * (yo - A[1])) / nx;    // ... mit y = yo
    const aussen = PZ(w, 0, FINGER_H, [], [[A, B, C]]);
    const schale = abzug(w, aussen, [PZ(w, -1, FINGER_H + 1, [], [[[xi, yA], [xB, yo], [xi, yo]]])]);
    var rippen = [];
    for (var x = b + RIPPEN_ABST; x <= FINGER_L - 6; x += RIPPEN_ABST)
    {
        rippen = append(rippen, quader(w, [x - RIPPE / 2, -FINGER_B, 0], [RIPPE, 2 * FINGER_B, FINGER_H]));
    }
    const r = schnitt(w, vereinige(w, rippen), PZ(w, 0, FINGER_H, [], [[A, B, C]]));
    var teile = [schale, r, quader(w, [0, -FINGER_B / 2, 0], [b, FINGER_B, FINGER_H])];
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
// Kugellager 6801-2RS, Achse z, von z 0 bis 5, mit Dichtscheiben
function baueLager(w is map) returns Query
{
    const l = abzug(w, ZZ(w, LAGER_D, 0, 0, 0, LAGER_B), [ZZ(w, LAGER_DI, 0, 0, -1, LAGER_B + 1)]);
    const nut1 = abzug(w, ZZ(w, 18.4, 0, 0, -1, 0.3), [ZZ(w, 14.2, 0, 0, -2, 1)]);
    const nut2 = abzug(w, ZZ(w, 18.4, 0, 0, LAGER_B - 0.3, LAGER_B + 1), [ZZ(w, 14.2, 0, 0, LAGER_B - 1, LAGER_B + 2)]);
    return abzug(w, l, [nut1, nut2]);
}

// Servo DS3218 / MG996R: Welle = z-Achse, Ohr-Oberseite bei z = 0, Körper nach -x
function baueServo(w is map) returns Query
{
    const x1 = SV_W - SV_L;
    const m = SV_W - SV_L / 2;
    const k = vereinige(w, [
        quader(w, [x1, -SV_B / 2, -SV_UNTER], [SV_L, SV_B, SV_UNTER + 12.9]),
        quader(w, [m - SV_OHR_L / 2, -SV_B / 2 + 1, -2.5], [SV_OHR_L, SV_B - 2, 2.5]),
        ZZ(w, 13, 0, 0, 12.8, 14.4),
        ZZ(w, 6, 0, 0, 14.3, 15)
    ]);
    var wz = [];
    for (var dx in [-1, 1])
    {
        for (var dy in [-1, 1])
        {
            wz = append(wz, ZZ(w, 4.5, m + dx * SV_LOCH_A / 2, dy * SV_LOCH_B / 2, -3.5, 1));
        }
    }
    return abzug(w, k, wz);
}

// runde Alu-Servoscheibe, Fläche bei z = HORN_ABST
function baueServoscheibe(w is map) returns Query
{
    var wz = [ZZ(w, 3.2, 0, 0, 14, HORN_ABST + 1)];
    for (var k = 0; k < 8; k += 1)
    {
        const p = polar(8 + 2 * (k % 2), k * PI / 4);
        wz = append(wz, ZZ(w, 2.5, p[0], p[1], 14, HORN_ABST + 1));
    }
    return abzug(w, vereinige(w, [ZZ(w, HORN_D, 0, 0, HORN_ABST - 2.5, HORN_ABST), ZZ(w, 10, 0, 0, 15, HORN_ABST)]), wz);
}

// Schrittmotor 28BYJ-48: Welle = z-Achse, Ohr-Unterseite bei z = 0
function baueSchrittmotor(w is map) returns Query
{
    const gx = -8;
    const ohren = PZ(w, 0, 1, [[gx, -SM_OHR, 3.5], [gx, SM_OHR, 3.5]], [[[gx - 3.5, -SM_OHR], [gx + 3.5, -SM_OHR], [gx + 3.5, SM_OHR], [gx - 3.5, SM_OHR]]]);
    const welle = schnitt(w, ZZ(w, 5, 0, 0, 2.5, 10), quader(w, [-1.5, -3, 2.5], [3, 6, 8]));
    const k = vereinige(w, [
        ZZ(w, 28, gx, 0, -19, 0),
        ohren,
        quader(w, [gx - 14 - 3, -8.5, -19], [6, 17, 16]),
        ZZ(w, 9, 0, 0, 0, 2.5),
        welle,
        ZZ(w, 5, 0, 0, 2.5, 4)
    ]);
    return abzug(w, k, [ZZ(w, 4.2, gx, -SM_OHR, -1, 2), ZZ(w, 4.2, gx, SM_OHR, -1, 2)]);
}

// Flachkopfschraube M3x16 (ISO 7380) für die Koppelstange, Achse y
function baueStift(w is map) returns Query
{
    return vereinige(w, [ZY(w, 5.7, -9.65, -8, 0, 0), ZY(w, 3, -8.01, 8, 0, 0)]);
}
