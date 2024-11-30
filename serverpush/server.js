const http2 = require('http2');
const fs = require('fs');

const options = {
  key: fs.readFileSync('../secrets/server.key'),
  cert: fs.readFileSync('../secrets/server.crt')
};

const server = http2.createSecureServer(options);

server.on('stream', (stream, headers) => {
  const path = headers[':path'];

  if (path === '/') {
    stream.pushStream({ ':path': '/style.css' }, (err, pushStream) => {
      if (err) {
        console.error('Erreur Server Push:', err);
        return;
      }
      pushStream.respondWithFile('./style.css', {
        'content-type': 'text/css'
      });
    });

    stream.pushStream({ ':path': '/script.js' }, (err, pushStream) => {
      if (err) {
        console.error('Erreur Server Push:', err);
        return;
      }
      pushStream.respondWithFile('./script.js', {
        'content-type': 'application/javascript'
      });
    });

    stream.respondWithFile('./index.html', {
      'content-type': 'text/html'
    });
  } else if (path === '/style.css') {
    stream.respondWithFile('./style.css', {
      'content-type': 'text/css'
    });
  } else if (path === '/script.js') {
    stream.respondWithFile('./script.js', {
      'content-type': 'application/javascript'
    });
  } else {
    stream.respond({ ':status': 404 });
  }
});

server.listen(8443, () => {
  console.log('Serveur HTTP/2 en écoute sur https://localhost:8443');
});
