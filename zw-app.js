/* Adam Mechatronik – eigenständige App
   Stellt der Firmenseite dieselbe Schnittstelle bereit wie auf claude.ai (window.claude.use),
   nur mit eigenem Login und eigener Datenbank (Firebase Auth + Firestore).
   APP_VERSION wird bei jedem Update erhöht; sw.js muss dieselbe Nummer tragen. */
(function () {
  "use strict";
  var APP_VERSION = "2.9.0";
  window.ZW_APP_VERSION = APP_VERSION;

  // ---------- Warten auf Login ----------
  var authReady, resolveAuth;
  authReady = new Promise(function (r) { resolveAuth = r; });
  var fb = null, auth = null, fs = null, member = null, profileCache = {};

  window.claude = {
    use: function (name) {
      if (name === "db") return authReady.then(function (ok) { return ok ? fs : null; });
      if (name === "user") return authReady.then(function (ok) { return ok ? userNs : null; });
      return Promise.resolve(null); // sample, comments usw. gibt es nur auf claude.ai
    }
  };

  function initials(s) {
    var p = String(s || "?").replace(/@.*/, "").split(/[\s._-]+/).filter(Boolean);
    return ((p[0] || "?")[0] + (p[1] ? p[1][0] : "")).toUpperCase();
  }
  function colorFor(id) {
    var c = ["#E8763A", "#2F8F5B", "#2F4BA8", "#8A5A00", "#7B2CBF", "#C2412D", "#0F9FA8"], h = 0;
    for (var i = 0; i < String(id).length; i++) h = (h * 31 + String(id).charCodeAt(i)) >>> 0;
    return c[h % c.length];
  }
  function avatar(name, id, photo) {
    if (photo) return photo;
    var svg = '<svg xmlns="http://www.w3.org/2000/svg" width="64" height="64"><circle cx="32" cy="32" r="32" fill="' + colorFor(id) + '"/>' +
      '<text x="32" y="41" font-size="26" font-family="Inter,Arial,sans-serif" font-weight="700" text-anchor="middle" fill="#fff">' +
      initials(name).replace(/[<&>]/g, "") + "</text></svg>";
    return "data:image/svg+xml," + encodeURIComponent(svg);
  }
  function rolle() { return member && String(member.rolle || "").trim().toLowerCase(); }
  function canWrite() { return rolle() === "admin" || rolle() === "bearbeiten"; }

  var userNs = {
    isOwner: function () { return Promise.resolve(rolle() === "admin"); },
    canEdit: function () { return Promise.resolve(rolle() === "admin"); },
    can: function (n) { return Promise.resolve(n === "data.write" ? canWrite() : false); },
    id: function () { return Promise.resolve(auth.currentUser ? auth.currentUser.uid : null); },
    name: function () { return userNs.me().then(function (m) { return m.name; }); },
    me: function () {
      var u = auth.currentUser;
      var name = u ? (u.displayName || (member && member.name) || u.email || "") : "";
      return Promise.resolve({ id: u ? u.uid : null, name: name, email: u ? u.email : null,
        avatarUrl: avatar(name, u ? u.uid : "x", u && u.photoURL), color: colorFor(u ? u.uid : "x"),
        isOwner: rolle() === "admin", canEdit: rolle() === "admin" });
    },
    profiles: function (ids) {
      ids = [].concat(ids).filter(function (x, i, a) { return x && a.indexOf(x) === i; });
      var me = auth.currentUser && auth.currentUser.uid;
      return Promise.all(ids.map(function (id) {
        if (profileCache[id]) return profileCache[id];
        return (profileCache[id] = fs.doc("nutzer/" + id).get().then(function (d) { return d.exists ? d.data() : {}; }, function () { return {}; }));
      })).then(function (list) {
        var o = {};
        ids.forEach(function (id, i) {
          var v = list[i] || {};
          o[id] = { id: id, name: v.name || "", avatarUrl: avatar(v.name || "?", id, v.photo), color: colorFor(id), email: null, isMe: id === me, guest: false };
        });
        return o;
      });
    },
    search: function () { return Promise.resolve([]); }
  };

  // ---------- Login-Oberfläche ----------
  var css = '' +
    '#zw-gate{position:fixed;inset:0;z-index:9999;display:grid;place-items:center;background:radial-gradient(40% 45% at 20% 25%,#FDE6D6,transparent 70%),radial-gradient(40% 45% at 85% 80%,#E6EBFA,transparent 70%),#F5F5F8;font-family:Inter,system-ui,-apple-system,"Segoe UI",sans-serif;color:#141726;padding:16px}' +
    '#zw-gate .box{width:min(420px,100%);background:#fff;border:1px solid #E4E5EB;border-radius:20px;padding:28px 26px;box-shadow:0 24px 60px rgba(20,23,38,.10);display:flex;flex-direction:column;gap:14px}' +
    '#zw-gate h1{margin:0;font-size:28px;letter-spacing:-.02em}#zw-gate p{margin:0;color:#5F6475;font-size:15px;line-height:1.45}' +
    '#zw-gate .logo{width:52px;height:52px;border-radius:14px;background:#E8763A;color:#fff;display:grid;place-items:center;font-weight:800;font-size:20px}' +
    '#zw-gate input{font:inherit;font-size:16px;padding:11px 12px;border:1px solid #D7D9E0;border-radius:10px;width:100%;box-sizing:border-box}' +
    '#zw-gate input:focus{outline:2px solid #E8763A;outline-offset:1px;border-color:#E8763A}' +
    '#zw-gate button{font:inherit;font-size:16px;font-weight:700;padding:12px;border-radius:10px;border:1px solid #D7D9E0;background:#fff;color:#141726;cursor:pointer}' +
    '#zw-gate button.pri{background:#E8763A;border-color:#E8763A;color:#fff}#zw-gate .row{display:flex;gap:10px}#zw-gate .row>*{flex:1}' +
    '#zw-gate .lnk{background:none;border:0;padding:0;color:#B9531F;font-weight:600;font-size:14px;text-decoration:underline;text-underline-offset:3px;align-self:flex-start}' +
    '#zw-gate .err{color:#C2412D;font-size:14px;min-height:1em}#zw-gate .sep{display:flex;align-items:center;gap:10px;color:#9AA0B2;font-size:13px}#zw-gate .sep:before,#zw-gate .sep:after{content:"";flex:1;height:1px;background:#E4E5EB}' +
    '#zw-upd{position:fixed;left:50%;bottom:calc(16px + env(safe-area-inset-bottom,0px));transform:translateX(-50%);z-index:10001;background:#141726;color:#fff;border-radius:14px;padding:10px 12px 10px 16px;display:flex;gap:12px;align-items:center;font:600 14px Inter,system-ui,sans-serif;box-shadow:0 14px 40px rgba(0,0,0,.25)}' +
    '#zw-upd button{font:inherit;background:#E8763A;color:#fff;border:0;border-radius:9px;padding:8px 12px;cursor:pointer}' +
    '#zw-menu{position:fixed;z-index:9997;background:#fff;border:1px solid #E4E5EB;border-radius:14px;box-shadow:0 18px 50px rgba(20,23,38,.15);padding:8px;display:flex;flex-direction:column;min-width:220px;font:500 15px Inter,system-ui,sans-serif}' +
    '#zw-menu button{font:inherit;text-align:left;background:none;border:0;padding:10px 12px;border-radius:9px;cursor:pointer;color:#141726}#zw-menu button:hover{background:#FDEEE5}#zw-menu small{color:#5F6475;padding:4px 12px 8px;font-size:12.5px}' +
    '#zw-team{position:fixed;inset:0;z-index:9999;background:rgba(20,23,38,.35);display:grid;place-items:center;padding:16px;font-family:Inter,system-ui,sans-serif}' +
    '#zw-team .box{width:min(560px,100%);max-height:90vh;overflow:auto;background:#fff;border-radius:18px;padding:22px;display:flex;flex-direction:column;gap:12px}' +
    '#zw-team h2{margin:0}#zw-team .m{display:grid;grid-template-columns:minmax(0,1fr) 130px 36px;gap:8px;align-items:center;font-size:14.5px}' +
    '#zw-team .m span{overflow:hidden;text-overflow:ellipsis}#zw-team select,#zw-team input{font:inherit;font-size:15px;padding:8px;border:1px solid #D7D9E0;border-radius:8px;min-width:0}' +
    '#zw-team button{font:inherit;font-weight:600;padding:9px 12px;border-radius:9px;border:1px solid #D7D9E0;background:#fff;cursor:pointer}#zw-team button.pri{background:#E8763A;color:#fff;border-color:#E8763A}';
  var st = document.createElement("style"); st.textContent = css; document.head.appendChild(st);

  function gate(html) {
    var g = document.getElementById("zw-gate");
    if (!g) { g = document.createElement("div"); g.id = "zw-gate"; document.body.appendChild(g); }
    g.innerHTML = '<div class="box" role="dialog" aria-modal="true" aria-labelledby="zw-gt"><div class="logo">ZW</div>' + html + "</div>";
    return g;
  }
  function closeGate() { var g = document.getElementById("zw-gate"); if (g) g.remove(); }
  function esc(s) { return String(s == null ? "" : s).replace(/[&<>"]/g, function (c) { return { "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;" }[c]; }); }
  function deErr(e) {
    var c = (e && e.code) || "";
    if (/invalid-credential|wrong-password|user-not-found|invalid-login/.test(c)) return "E-Mail oder Passwort stimmt nicht.";
    if (/email-already-in-use/.test(c)) return "Für diese E-Mail gibt es schon ein Konto. Bitte anmelden.";
    if (/weak-password/.test(c)) return "Das Passwort braucht mindestens 6 Zeichen.";
    if (/invalid-email/.test(c)) return "Bitte eine gültige E-Mail eingeben.";
    if (/popup-closed|cancelled-popup/.test(c)) return "";
    if (/unauthorized-domain/.test(c)) return "Diese Adresse ist in Firebase noch nicht freigegeben (Authentication > Einstellungen > Autorisierte Domains).";
    if (/network/.test(c)) return "Keine Verbindung. Bitte Internet prüfen.";
    return "Das hat nicht geklappt (" + (c || "unbekannt") + ").";
  }

  function showLogin(mode) {
    var reg = mode === "register";
    var g = gate('<h1 id="zw-gt">' + (reg ? "Konto anlegen" : "Anmelden") + '</h1><p>Adam Mechatronik · Firmen-App</p>' +
      '' +
      '<form id="zw-form" style="display:flex;flex-direction:column;gap:10px" novalidate>' +
      (reg ? '<input id="zw-name" autocomplete="name" placeholder="Dein Name" aria-label="Name">' : "") +
      '<input id="zw-mail" type="email" autocomplete="email" placeholder="E-Mail" aria-label="E-Mail" required>' +
      '<input id="zw-pass" type="password" autocomplete="' + (reg ? "new-password" : "current-password") + '" placeholder="Passwort" aria-label="Passwort" required>' +
      '<button class="pri" type="submit">' + (reg ? "Konto anlegen" : "Anmelden") + "</button></form>" +
      '<div class="err" id="zw-err" role="alert"></div>' +
      '<div class="row" style="align-items:center"><button type="button" class="lnk" id="zw-switch">' + (reg ? "Ich habe schon ein Konto" : "Neues Konto anlegen") + "</button>" +
      (reg ? "" : '<button type="button" class="lnk" id="zw-reset" style="text-align:right;align-self:center">Passwort vergessen?</button>') + "</div>");
    var err = g.querySelector("#zw-err");
    if (g.querySelector("#zw-google")) g.querySelector("#zw-google").onclick = function () {
      var p = new fb.auth.GoogleAuthProvider();
      auth.signInWithPopup(p).catch(function (e) {
        if (/popup-blocked|operation-not-supported/.test(e.code)) return auth.signInWithRedirect(p);
        err.textContent = deErr(e);
      });
    };
    g.querySelector("#zw-switch").onclick = function () { showLogin(reg ? "login" : "register"); };
    var rs = g.querySelector("#zw-reset");
    if (rs) rs.onclick = function () {
      var m = g.querySelector("#zw-mail").value.trim();
      if (!m) { err.textContent = "Bitte zuerst die E-Mail eintragen."; return; }
      auth.sendPasswordResetEmail(m).then(function () { err.style.color = "#2F8F5B"; err.textContent = "E-Mail zum Zurücksetzen ist unterwegs."; }, function (e) { err.textContent = deErr(e); });
    };
    g.querySelector("#zw-form").onsubmit = function (ev) {
      ev.preventDefault(); err.style.color = ""; err.textContent = "";
      var m = g.querySelector("#zw-mail").value.trim(), pw = g.querySelector("#zw-pass").value;
      var pr = reg ? auth.createUserWithEmailAndPassword(m, pw).then(function (c) {
        var n = (g.querySelector("#zw-name").value || "").trim();
        return n ? c.user.updateProfile({ displayName: n }) : null;
      }) : auth.signInWithEmailAndPassword(m, pw);
      pr.catch(function (e) { err.textContent = deErr(e); });
    };
    setTimeout(function () { var f = g.querySelector(reg ? "#zw-name" : "#zw-mail"); if (f) f.focus(); }, 50);
  }

  function showNoAccess(u) {
    var g = gate('<h1 id="zw-gt">Noch kein Zugriff</h1><p>Du bist angemeldet als <b>' + esc(u.email) + '</b>, aber noch nicht freigeschaltet. ' +
      'Bitte den Admin, diese E-Mail im Team freizuschalten. Danach diese Seite neu laden.</p>' +
      '<div class="row"><button class="pri" type="button" id="zw-again">Neu prüfen</button><button type="button" id="zw-out">Abmelden</button></div>');
    g.querySelector("#zw-again").onclick = function () { location.reload(); };
    g.querySelector("#zw-out").onclick = function () { logout(); };
  }

  function showSetup() {
    gate('<h1 id="zw-gt">Einrichtung fehlt</h1><p>In <code>firebase-config.js</code> sind noch keine Firebase-Zugangsdaten eingetragen. ' +
      "Die Anleitung steht in EINRICHTUNG.md.</p>");
    resolveAuth(false);
  }

  // ---------- Menü am Namen oben rechts ----------
  function menuAt(anchor) {
    var old = document.getElementById("zw-menu"); if (old) { old.remove(); return; }
    var r = anchor.getBoundingClientRect(), m = document.createElement("div"); m.id = "zw-menu";
    m.innerHTML = "<small>" + esc(auth.currentUser.email) + " · " + esc(rolle() === "admin" ? "Admin" : rolle() === "bearbeiten" ? "Bearbeiten" : "Nur lesen") + "</small>" +
      (rolle() === "admin" ? '<button data-a="team">Team verwalten</button>' : "") +
      '<button data-a="upd">Nach Update suchen</button><button data-a="out">Abmelden</button>' +
      '<small>Version ' + esc(APP_VERSION) + "</small>";
    m.style.top = (r.bottom + 8) + "px"; m.style.right = Math.max(12, window.innerWidth - r.right) + "px";
    document.body.appendChild(m);
    m.onclick = function (e) {
      var b = e.target.closest("button"); if (!b) return; m.remove();
      if (b.dataset.a === "out") logout();
      if (b.dataset.a === "team") showTeam();
      if (b.dataset.a === "upd") checkUpdate(true);
    };
    setTimeout(function () { document.addEventListener("click", function h(ev) { if (!m.contains(ev.target) && ev.target !== anchor) { m.remove(); document.removeEventListener("click", h); } }); }, 0);
  }
  function hookWho() {
    var w = document.getElementById("who");
    if (!w || w.dataset.zw) return; w.dataset.zw = "1";
    w.style.cursor = "pointer"; w.setAttribute("role", "button"); w.tabIndex = 0; w.title = "Konto und Abmelden";
    w.addEventListener("click", function () { menuAt(w); });
    w.addEventListener("keydown", function (e) { if (e.key === "Enter" || e.key === " ") { e.preventDefault(); menuAt(w); } });
  }

  // ---------- Team verwalten (nur Admin) ----------
  function showTeam() {
    var t = document.createElement("div"); t.id = "zw-team";
    t.innerHTML = '<div class="box" role="dialog" aria-modal="true" aria-labelledby="zw-tt"><h2 id="zw-tt">Team</h2>' +
      '<p style="margin:0;color:#5F6475">Wer hier steht, kann sich anmelden. „Bearbeiten“ darf Einträge und Checklisten ändern, „Nur lesen“ nur ansehen.</p>' +
      '<div id="zw-ml">Lade …</div><form id="zw-add" class="m" style="grid-template-columns:minmax(0,1fr) 130px auto">' +
      '<input type="email" id="zw-am" placeholder="E-Mail der Person" aria-label="E-Mail" required><select id="zw-ar" aria-label="Rolle"><option value="bearbeiten">Bearbeiten</option><option value="lesen">Nur lesen</option><option value="admin">Admin</option></select>' +
      '<button class="pri">Hinzufügen</button></form><div style="text-align:right"><button type="button" id="zw-tc">Schließen</button></div></div>';
    document.body.appendChild(t);
    var list = t.querySelector("#zw-ml");
    var unsub = fs.collection("Mitglieder").onSnapshot(function (snap) {
      var me = auth.currentUser.email.toLowerCase();
      list.innerHTML = snap.docs.map(function (d) {
        var v = d.data() || {};
        return '<div class="m"><span title="' + esc(d.id) + '">' + esc(d.id) + (d.id === me ? " (du)" : "") + '</span><select data-mail="' + esc(d.id) + '"' + (d.id === me ? " disabled" : "") + ">" +
          [["admin", "Admin"], ["bearbeiten", "Bearbeiten"], ["lesen", "Nur lesen"]].map(function (o) { return '<option value="' + o[0] + '"' + (String(v.rolle || "").trim().toLowerCase() === o[0] ? " selected" : "") + ">" + o[1] + "</option>"; }).join("") +
          '</select><button type="button" data-del="' + esc(d.id) + '" aria-label="Entfernen"' + (d.id === me ? " disabled" : "") + ">×</button></div>";
      }).join("") || "<p>Noch niemand.</p>";
    });
    list.onchange = function (e) { var s = e.target; if (s.dataset.mail) fs.doc("Mitglieder/" + s.dataset.mail).set({ rolle: s.value }, { merge: true }); };
    list.onclick = function (e) { var b = e.target.closest("[data-del]"); if (b) fs.doc("Mitglieder/" + b.dataset.del).delete(); };
    t.querySelector("#zw-add").onsubmit = function (e) {
      e.preventDefault(); var m = t.querySelector("#zw-am").value.trim().toLowerCase(); if (!m) return;
      fs.doc("Mitglieder/" + m).set({ rolle: t.querySelector("#zw-ar").value, seit: Date.now() }, { merge: true }).then(function () { t.querySelector("#zw-am").value = ""; });
    };
    function close() { unsub(); t.remove(); }
    t.querySelector("#zw-tc").onclick = close;
    t.addEventListener("click", function (e) { if (e.target === t) close(); });
  }

  // ---------- Updates ----------
  var swReg = null;
  function showUpdate(worker) {
    if (document.getElementById("zw-gate") && worker) { worker.postMessage("skipWaiting"); return; }   // Anmeldefenster offen: sofort aktualisieren
    if (document.getElementById("zw-upd")) return;
    var u = document.createElement("div"); u.id = "zw-upd"; u.setAttribute("role", "status");
    u.innerHTML = '<span>Neue Version verfügbar</span><button type="button">Jetzt aktualisieren</button>';
    document.body.appendChild(u);
    u.querySelector("button").onclick = function () { if (worker) worker.postMessage("skipWaiting"); else location.reload(); };
  }
  function checkUpdate(manual) {
    if (swReg) swReg.update().then(function () { if (manual && !swReg.waiting && !swReg.installing) toast("Du hast schon die neueste Version."); });
    else if (manual) location.reload();
  }
  function toast(msg) {
    var u = document.createElement("div"); u.id = "zw-upd"; u.textContent = msg; document.body.appendChild(u);
    setTimeout(function () { u.remove(); }, 2500);
  }
  if ("serviceWorker" in navigator && location.protocol === "https:") {
    var reloading = false;
    navigator.serviceWorker.addEventListener("controllerchange", function () { if (!reloading) { reloading = true; location.reload(); } });
    navigator.serviceWorker.register("sw.js", { updateViaCache: "none" }).then(function (reg) {
      swReg = reg;
      if (reg.waiting && navigator.serviceWorker.controller) showUpdate(reg.waiting);
      reg.addEventListener("updatefound", function () {
        var w = reg.installing;
        w.addEventListener("statechange", function () { if (w.state === "installed" && navigator.serviceWorker.controller) showUpdate(w); });
      });
      setInterval(function () { reg.update(); }, 15 * 60 * 1000);
      document.addEventListener("visibilitychange", function () { if (document.visibilityState === "visible") reg.update(); });
    }).catch(function () {});
  }


  // ---------- Abmelden: auch lokale Kopien der Daten löschen ----------
  function logout() {
    auth.signOut().then(function () { return fs && fs.terminate ? fs.terminate() : null; })
      .then(function () { return fs && fs.clearPersistence ? fs.clearPersistence() : null; })
      .catch(function () {}).then(function () { location.reload(); });
  }

  // ---------- Geschützter Inhalt ----------
  // Die eigentliche Firmenseite liegt verschlüsselt in inhalt.bin. Den Schlüssel gibt es nur
  // aus der Datenbank (inhalt/schluessel) – und die liest nur, wer freigeschaltet ist.
  function b64bytes(s) {
    s = String(s).replace(/-/g, "+").replace(/_/g, "/"); while (s.length % 4) s += "=";
    var bin = atob(s), a = new Uint8Array(bin.length);
    for (var i = 0; i < bin.length; i++) a[i] = bin.charCodeAt(i);
    return a;
  }
  function getKey() {
    var ref = fs.doc("inhalt/schluessel");
    var m = /[#&]schluessel=([A-Za-z0-9_-]{40,})/.exec(location.hash);
    var put = m ? ref.set({ k: m[1], seit: Date.now() }).then(function () {
      try { history.replaceState(null, "", location.pathname + location.search); } catch (e) {}
    }, function () {}) : Promise.resolve();
    return put.then(function () {
      return ref.get({ source: "server" }).catch(function (e) { if (e && e.code === "permission-denied") throw e; return ref.get(); });
    }).then(function (d) {
      if (!d.exists || !(d.data() || {}).k) { var e = new Error("kein Schlüssel"); e.code = "no-key"; throw e; }
      return d.data().k;
    });
  }
  function loadContent() {
    if (!window.crypto || !crypto.subtle || typeof DecompressionStream === "undefined") {
      var e0 = new Error("Dieser Browser ist zu alt für die geschützte App. Bitte iPadOS/Browser aktualisieren."); e0.code = "old"; return Promise.reject(e0);
    }
    return Promise.all([
      getKey().then(function (k) { return crypto.subtle.importKey("raw", b64bytes(k), "AES-GCM", false, ["decrypt"]); }).then(function (ck) { contentKey = ck; return ck; }),
      fetch("inhalt.bin?v=" + APP_VERSION).then(function (r) { if (!r.ok) throw new Error("inhalt.bin " + r.status); return r.arrayBuffer(); })
    ]).then(function (x) {
      var buf = new Uint8Array(x[1]);
      return crypto.subtle.decrypt({ name: "AES-GCM", iv: buf.slice(0, 12) }, x[0], buf.slice(12)).catch(function () {
        var e = new Error("Schlüssel passt nicht zum Inhalt"); e.code = "bad-key"; throw e;
      });
    }).then(function (plain) {
      return new Response(new Blob([plain]).stream().pipeThrough(new DecompressionStream("gzip"))).text();
    }).then(function (t) { return JSON.parse(t); });
  }
  // Marktanalyse: eigene Datei, wird bei jedem Öffnen frisch geladen (wöchentlich aktualisiert)
  var contentKey = null, marktPriv = null;
  function loadMarkt() {
    if (!contentKey) return;
    fetch("markt.bin?t=" + Date.now(), { cache: "no-store" }).then(function (r) { if (!r.ok) throw new Error("markt " + r.status); return r.arrayBuffer(); })
      .then(function (b) {
        b = new Uint8Array(b);
        if (b[0] === 77 && b[1] === 75 && b[2] === 50) {   // "MK2": AES-Schlüssel mit RSA-OAEP verschlüsselt
          if (!marktPriv) throw new Error("kein Marktschlüssel");
          var n = (b[3] << 8) | b[4], ek = b.slice(5, 5 + n), iv = b.slice(5 + n, 17 + n), ct = b.slice(17 + n);
          return crypto.subtle.importKey("pkcs8", b64bytes(marktPriv), { name: "RSA-OAEP", hash: "SHA-256" }, false, ["decrypt"])
            .then(function (pk) { return crypto.subtle.decrypt({ name: "RSA-OAEP" }, pk, ek); })
            .then(function (raw) { return crypto.subtle.importKey("raw", raw, "AES-GCM", false, ["decrypt"]); })
            .then(function (ak) { return crypto.subtle.decrypt({ name: "AES-GCM", iv: iv }, ak, ct); });
        }
        return crypto.subtle.decrypt({ name: "AES-GCM", iv: b.slice(0, 12) }, contentKey, b.slice(12));
      })
      .then(function (p) { return new Response(new Blob([p]).stream().pipeThrough(new DecompressionStream("gzip"))).text(); })
      .then(function (t) { window.ZW_MARKT = JSON.parse(t); window.dispatchEvent(new Event("zw-markt")); })
      .catch(function () {});
  }
  document.addEventListener("visibilitychange", function () { if (document.visibilityState === "visible") loadMarkt(); });
  function injectPage(bundle) {
    var page = bundle.page;
    Object.keys(bundle.assets || {}).forEach(function (name) {
      var a = bundle.assets[name], url = URL.createObjectURL(new Blob([b64bytes(a.b64)], { type: a.type }));
      page = page.split(name).join(url);
    });
    var doc = new DOMParser().parseFromString(page, "text/html");
    var t = doc.querySelector("title"); if (t) document.title = t.textContent;
    var scripts = [];
    [].slice.call(doc.head.children).concat([].slice.call(doc.body.childNodes)).forEach(function (n) {
      if (n.tagName === "TITLE" || n.tagName === "META") return;
      if (n.tagName === "SCRIPT") { scripts.push(n); return; }
      if (n.tagName === "LINK" || n.tagName === "STYLE") { document.head.appendChild(document.importNode(n, true)); return; }
      document.body.appendChild(document.importNode(n, true));
    });
    return scripts.reduce(function (p, old) {
      return p.then(function () {
        return new Promise(function (res, rej) {
          var s = document.createElement("script");
          if (old.src) { s.src = old.getAttribute("src"); s.onload = res; s.onerror = function () { rej(new Error("Laden fehlgeschlagen: " + old.getAttribute("src"))); }; document.body.appendChild(s); }
          else { s.textContent = old.textContent; document.body.appendChild(s); res(); }
        });
      });
    }, Promise.resolve());
  }
  function contentError(e) {
    var msg;
    if (e && e.code === "no-key") msg = rolle() === "admin"
      ? "Der Schlüssel für die geschützten Inhalte ist noch nicht hinterlegt. Öffne einmal den Freischalt-Link, den du von Claude bekommen hast."
      : "Die Inhalte sind noch nicht freigegeben. Bitte den Admin fragen.";
    else if (e && e.code === "permission-denied") msg = "Kein Zugriff auf die Inhalte (Regeln). Bitte den Admin fragen.";
    else msg = "Die Inhalte konnten nicht geladen werden" + (e && e.message ? " (" + e.message + ")" : "") + ".";
    var g = gate('<h1 id="zw-gt">Fast geschafft</h1><p>' + esc(msg) + '</p><div class="row"><button class="pri" type="button" id="zw-again">Neu laden</button><button type="button" id="zw-out">Abmelden</button></div>');
    g.querySelector("#zw-again").onclick = function () { location.reload(); };
    g.querySelector("#zw-out").onclick = function () { logout(); };
  }

  // ---------- Start ----------
  function start() {
    var cfg = window.FIREBASE_CONFIG;
    if (!window.firebase || !cfg || !cfg.apiKey || /DEIN|YOUR/.test(cfg.apiKey)) { showSetup(); return; }
    fb = window.firebase; fb.initializeApp(cfg); auth = fb.auth(); fs = fb.firestore();
    try { fs.enablePersistence({ synchronizeTabs: true }).catch(function () {}); } catch (e) {}
    gate('<h1 id="zw-gt">Einen Moment …</h1><p>Anmeldung wird geprüft.</p>');
    var first = true;
    auth.onAuthStateChanged(function (u) {
      if (!u) { if (!first) location.reload(); showLogin("login"); first = false; return; }
      first = false;
      var mail = (u.email || "").toLowerCase();
      var ref = fs.doc("Mitglieder/" + mail);
      ref.get({ source: "server" }).catch(function (e) { if (e && e.code === "permission-denied") throw e; return ref.get(); }).then(function (d) {
        if (!d.exists && !sessionStorage.getItem("zw-boot")) {   // Inhaber legt seinen Admin-Eintrag selbst an (nur per Regel erlaubt)
          try { sessionStorage.setItem("zw-boot", "1"); } catch (e) {}
          ref.set({ rolle: "admin", seit: Date.now() }, { merge: true }).then(function () { location.reload(); }, function () { diag(u, mail); showNoAccess(u); });
          return;
        }
        if (!d.exists) { diag(u, mail); showNoAccess(u); var g0 = document.getElementById("zw-gt"); if (g0 && g0.parentNode) { var h0 = document.createElement("p"); h0.style.cssText = "font-size:12px;opacity:.6"; h0.textContent = "Hinweis: Kein Eintrag „Mitglieder/" + mail + "“ gefunden."; g0.parentNode.appendChild(h0); } return; }
        member = d.data() || {};
        fs.doc("nutzer/" + u.uid).set({ name: u.displayName || member.name || u.email, photo: u.photoURL || "", zuletzt: Date.now() }, { merge: true }).catch(function () {});
        gate('<h1 id="zw-gt">Einen Moment …</h1><p>Inhalte werden geladen.</p>');
        loadContent().then(function (bundle) {
          window.ADAM_START = bundle.start; marktPriv = bundle.mk || null;
          var seed = (rolle() === "admin" && window.ADAM_START) ? fs.collection("bereiche").limit(1).get().then(function (q) {
            if (!q.empty) return;
            var b = fs.batch();
            Object.keys(window.ADAM_START).forEach(function (k) { b.set(fs.doc("bereiche/" + k), window.ADAM_START[k]); });
            return b.commit();
          }).catch(function () {}) : Promise.resolve();
          return seed.then(function () { return injectPage(bundle); });
        }).then(function () {
          closeGate(); resolveAuth(true); loadMarkt();
          var tries = 0, iv = setInterval(function () { hookWho(); if (document.getElementById("who") && document.getElementById("who").dataset.zw || ++tries > 40) clearInterval(iv); }, 250);
        }, contentError);
      }, function (e) {
        if (e && e.code === "permission-denied") { showNoAccess(u); var g = document.getElementById("zw-gt"); if (g && g.parentNode) { var h = document.createElement("p"); h.style.cssText = "font-size:12px;opacity:.6"; h.textContent = "Hinweis: Regeln verweigern den Zugriff (permission-denied)."; g.parentNode.appendChild(h); } }
        else { gate('<h1 id="zw-gt">Keine Verbindung</h1><p>Die Datenbank ist gerade nicht erreichbar. Bitte später neu laden.</p>'); }
      });
    });
  }

  function diag(u, mail) {
    try {
      var pid = (window.FIREBASE_CONFIG || {}).projectId;
      u.getIdToken().then(function (tok) {
        return fetch("https://firestore.googleapis.com/v1/projects/" + pid + "/databases/(default)/documents/Mitglieder/" + encodeURIComponent(mail), { headers: { Authorization: "Bearer " + tok } });
      }).then(function (r) { return r.text().then(function (t) { return r.status + " " + t.slice(0, 160); }); })
        .catch(function (e) { return "Fehler " + (e && e.message); })
        .then(function (txt) {
          var g = document.getElementById("zw-gt"); if (!g || !g.parentNode) return;
          var h = document.createElement("p"); h.style.cssText = "font-size:11px;opacity:.55;word-break:break-all"; h.textContent = "Server: " + txt; g.parentNode.appendChild(h);
        });
    } catch (e) {}
  }
  window.addEventListener("hashchange", function () { if (/schluessel=/.test(location.hash)) location.reload(); });
  if (document.readyState === "loading") document.addEventListener("DOMContentLoaded", start); else start();
})();
