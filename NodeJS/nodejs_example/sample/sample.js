// HTTP 모듈 불러오고 반환되는 HTTP 인슨턴스를 http변수에 저장합니다.
const http = require('http');

const hostname = '127.0.0.1';
const port = 3000;

// http 인슨턴스를 사용하여 서버 생성 메소드를 실행한다.
const server = http.createServer((req, res) => {
  res.statusCode = 200;
  res.setHeader('Content-Type', 'text/plain');
  res.end('Hello World\n');
});

// 이후 listen 메소드를 사용하여 포트를 bind 한다
server.listen(port, hostname, () => {
  console.log(`Server running at http://${hostname}:${port}/`)
});
