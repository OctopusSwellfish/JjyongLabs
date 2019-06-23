var express = require('express');
var router = express.Router();

var sequelize = require('../models').sequelize;
var robot = require('../models').robot;
var scene = require('../models').scene;

router.put('/', function(req, res, next) {
	var data = req.body;
		
	console.log(data);

	var Result = req.body.result;
	var GripperValue = req.body.gripperValue;
	var transform_Value_0 = req.body.transform_value[0];
	var transform_Value_1 = req.body.transform_value[1];
	var transform_Value_2 = req.body.transform_value[2];
	var transform_Value_3 = req.body.transform_value[3];
	var transform_Value_4 = req.body.transform_value[4];
	var transform_Value_5 = req.body.transform_value[5];
	
	console.log(Result);
	console.log(GripperValue);
	console.log(transform_Value_0);
	console.log(transform_Value_1);
	console.log(transform_Value_2);
	console.log(transform_Value_3);
	console.log(transform_Value_4);
	console.log(transform_Value_5);

	robot.create({
		result: Result,
		gripperValue: GripperValue,
		transform_value_0: transform_Value_0,
		transform_value_1: transform_Value_1,
		transform_value_2: transform_Value_2,
		transform_value_3: transform_Value_3,
		transform_value_4: transform_Value_4,
		transform_value_5: transform_Value_5,
	}).then(function(data){
		console.log("추가 성공!");
		res.send("success");
	}).catch(function(err){
		console.log("err=>"+err);
	});
});

router.get('/', function(req, res, next){
	res.render('store', {title:'get'});
});


router.get('/data', function(req, res, next){ 
	
	sequelize.query('select * from robots order by id desc limit 1', {type: sequelize.QueryTypes.SELECT}).then(function(resultSet){
	console.log(resultSet);
	
	res.json(resultSet);
	console.log("보냈습니다");
	}).catch(function(err){
		console.log("에러 :"+err);
	});

});

router.put('/data', function(req, res, next){
	var scene = req.body.step;
	
	scene.create({
		step: scene,
	}).then(function(data){
		console.log("시나리오 추가 성공!");
	}).catch(function(err){
		console.log("err=>"+err);
	});
	

});

router.post('/data', function(req, res, next){
	console.log("/data ajax 호출");

	var meg = req.body;
	console.log(meg);

	sequelize.query('select * from scenes order by id desc limit 1', {type:sequelize.QueryTypes.SELECT})
	.then(function(resultSet){
	var response = resultSet;
	
	console.log(response);
	res.json(response);
	console.log("step 보냈습니다");
	}).catch(function(err){
		console.log("에러 : "+err);
	});

});

router.post('/', function(req, res, next){
	console.log("/ ajax 호출");

	var meg = req.body;	
	console.log(meg);

	
	robot.findAll({
		attributes: {exclude: ['id'] }
	}).then(function(resultSet){
		var response = resultSet;
		res.send(resultSet);
	}).catch(function(err){
		console.log("err" + err);
	});	
	
	//console.log(response);
});



module.exports = router;
