var express = require('express');
var app = express();
var bodyParser = require('body-parser');
var session = require('express-session');
var fs = require("fs");
var sequelize = require('./models').sequelize;

sequelize.sync();
// 서버가 읽을 수 읽도록 HTML 의 위치를 정의 해줍니다.
app.set('views', __dirname+ '/views');
// 서버가 HTML 랜더링을 할 때, EJS 엔진을 사용하도록 설정합니다.
app.set('view engine', 'ejs');
app.engine('html', require('ejs').renderFile);

var server = app.listen(3000, () =>{
  console.log("Express server has started on port 3000");
});

app.use(express.static('public'));
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
  extended: true
}));
app.use(session({
  secret: '@#@$MYSIGN#@$#$',
  resave: false,
  saveUninitialized: true
}));

var router = require('./router/main') (app, fs);
