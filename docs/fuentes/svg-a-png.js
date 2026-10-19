const puppeteer = require(process.argv[4]);
(async () => {
  const fs = require('fs');
  const svg = fs.readFileSync(process.argv[2], 'utf8');
  const b = await puppeteer.launch({headless: 'new'});
  const pg = await b.newPage();
  const m = svg.match(/width="(\d+)" height="(\d+)"/);
  await pg.setViewport({width: +m[1], height: +m[2], deviceScaleFactor: 1.5});
  await pg.setContent(`<html><body style="margin:0">${svg}</body></html>`);
  await pg.screenshot({path: process.argv[3], clip: {x:0,y:0,width:+m[1],height:+m[2]}});
  await b.close();
})();
