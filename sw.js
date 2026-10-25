/* Service Worker: macht die App offline-fähig und erkennt neue Versionen.
   VERSION wird beim Build gesetzt. Neue Version = neuer Cache = Update-Hinweis in der App. */
var VERSION = "2.6.0";
var CACHE = "zw-" + VERSION;
var CORE = ["./", "index.html", "zw-app.js", "firebase-config.js", "inhalt.bin", "manifest.webmanifest",
  "lib/three.min.js", "lib/OrbitControls.js", "lib/RoundedBoxGeometry.js", "lib/RoomEnvironment.js", "lib/GLTFLoader.js",
  "icons/icon-192.png", "icons/icon-512.png"];
self.addEventListener("install", function (e) {
  e.waitUntil(caches.open(CACHE).then(function (c) { return c.addAll(CORE.map(function (u) { return new Request(u, { cache: "reload" }); })); }));
});
self.addEventListener("activate", function (e) {
  e.waitUntil(caches.keys().then(function (ks) { return Promise.all(ks.filter(function (k) { return k.indexOf("zw-") === 0 && k !== CACHE; }).map(function (k) { return caches.delete(k); })); }).then(function () { return self.clients.claim(); }));
});
self.addEventListener("message", function (e) { if (e.data === "skipWaiting") self.skipWaiting(); });
self.addEventListener("fetch", function (e) {
  var req = e.request, url = new URL(req.url);
  if (req.method !== "GET" || url.origin !== location.origin) return;   // Firebase & Google laufen direkt
  e.respondWith(caches.open(CACHE).then(function (c) {
    return c.match(req, { ignoreSearch: true }).then(function (hit) {
      var net = fetch(req).then(function (res) { if (res && res.ok) c.put(req, res.clone()); return res; });
      return hit || net;          // aus dem Cache dieser Version, sonst Netz
    });
  }));
});
