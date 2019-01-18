var http = require('http');

//HTTP Request 의 옵션 설정
var options = {
  host: 'localhost',
  port: '8081',
  path: '/index.html'
};

// 콜백 함수로 Response 를 받아온다
var callback = (res)=>{
  // Response 이벤트가 감지되면 데이터를 body 에 받아온다
  var body = '';
  res.on('data', (data) =>{
    body += data;
  });

  // end 이벤트가 감지되면 데이터 수신을 종료하고 내용을 출력한다
  res.on('end', () =>{
    console.log(body);
  });
}

var req = http.request(options, callback);
req.end();
