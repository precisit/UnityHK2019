const WebSocket = require('ws');
const uuidv4 = require('uuid/v4');

const wss = new WebSocket.Server({
    port: 1337
}, () => {
    console.log('TopDown super server listening on port', wss.options.port);
});

var clientMap = {}; // Map of clients, for cross client communication

wss.on('connection', ws => {
    ws.clientId = uuidv4();
    clientMap[ws.clientId] = ws; 

    console.log(`Client connected: ${ws.clientId}`);
    
    ws.send(ws.clientId); //Inform client of clientId

    ws.on('message', message => {
        console.log('From Unity: %s', message);

/*        wss.clients.forEach(function each(ws) { // Primite broadcast / echo 
            ws.send(message);
        });*/
    });
});