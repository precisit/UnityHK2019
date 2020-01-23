var express = require('express');
var app = express();
var expressWs = require('express-ws')(app);

const port = 1337;

app.use(function (req, res, next) {
//  console.log('middleware');
//  req.testing = 'testing';
  return next();
});

app.get('/', function(req, res, next){
//  console.log('get route', req.testing);
  res.end();
});

app.ws('/', function(ws, req) {
  ws.on('message', function(msg) {
    console.log('Message from unity: ', msg);
  });
  console.log('Client connected!');
});

app.listen(port, () => console.log(`TopDownServer listening on port ${port}!`));