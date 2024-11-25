const http2 = require('http2');
const fs = require('fs');
const path = require('path');

const options = {
  key: fs.readFileSync('../secrets/server.key'),
  cert: fs.readFileSync('../secrets/server.crt'),
};

const delay = (ms) => new Promise((resolve) => setTimeout(resolve, ms));

const server = http2.createSecureServer(options);

server.on('stream', async (stream, headers) => {
  const filePath = headers[':path'];

  await delay(30);

  if (filePath === '/with-preload') {
    stream.respondWithFile(path.join(__dirname, 'with-preload.html'), {
      'content-type': 'text/html',
    });
  } else if (filePath === '/without-preload') {
    stream.respondWithFile(path.join(__dirname, 'without-preload.html'), {
      'content-type': 'text/html',
    });
  } else {
    const assetPath = path.join(__dirname, 'assets', filePath.replace('/assets/', ''));
    if (fs.existsSync(assetPath)) {
      const contentType = filePath.endsWith('.css')
        ? 'text/css'
        : filePath.endsWith('.js')
        ? 'application/javascript'
        : filePath.endsWith('.jpg')
        ? 'image/jpeg'
        : filePath.endsWith('.ttf')
        ? 'font/ttf'
        : 'application/octet-stream';

      stream.respondWithFile(assetPath, { 'content-type': contentType });
    } else {
      stream.respond({ ':status': 404 });
    }
  }
});

server.listen(8443, () => {
  console.log('https://localhost:8443');
});