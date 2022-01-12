const { createServer } = require("https");
const { parse } = require("url");
const next = require("next");
const fs = require("fs");
const path = require('path');

const dev = process.env.NODE_ENV !== "production";
const app = next({ dev });
const handle = app.getRequestHandler();
const httpsOptions = {
  key: fs.readFileSync(path.join(__dirname,"localhost.key")),
  cert: fs.readFileSync(path.join(__dirname,"localhost.crt")),
};

app.prepare().then(() => {
  createServer(httpsOptions, (req, res) => {
    const parsedUrl = parse(req.url, true);
    handle(req, res, parsedUrl);
  }).listen(3000, (err) => {
    if (err) throw err;
    console.log("Node Environment:" + process.env.NODE_ENV)
    console.log(`NODE_TLS_REJECT_UNAUTHORIZED: ${process.env.NODE_TLS_REJECT_UNAUTHORIZED}`)
    console.log("AGS Identity Url:" + process.env.ags_identity_authentication_url)
    console.log("AGS Web Url:" + process.env.ags_web_host)
    console.log("> Server started on https://localhost:3000");
  });
});