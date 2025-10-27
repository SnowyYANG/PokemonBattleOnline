const WebSocket = require('ws');

const wss = new WebSocket.Server({ port: 8080 }, () => {
  console.log('WebSocket server listening on ws://localhost:8080');
});

let clients = []
let preparedTeams = []
let games = []

const send = (room, message) => {
    clients.filter(c => c.room === room).forEach(c => {
        c.ws.send(JSON.stringify(message));
    });
}

const log = (msg) => {
  console.log(new Date().toISOString() + '<Server> ' + msg);
}

wss.on('connection', (ws, req) => {
  const ip = req.socket.remoteAddress;
  console.log('Client connected:', ip);
  const id = Date.now() + Math.random().toString(36).substring(2, 9)
  let clientInfo = null;

  const clientLog = (msg) => {
    console.log(new Date().toISOString() + `<${ip}> ${msg}`);
  }

  // 发送欢迎消息
  ws.send(JSON.stringify({ secret: 'PBOtest', message: 'Hello from PBOtest server', serverVersion: '0.10' }));

  // 接收消息并回显
  ws.on('message', (data) => {
    data = String(data)
    if ((data[0] == '{') || (data[0] == '[')) {
      data = JSON.parse(data);
    } else {
      clientLog(data)
      clientLog('Received non-JSON message, closing connection.');
      ws.close();
      return;
    }
    if (clientInfo !== null) {
      switch (data.head) {
        case 'quit':
          send(clientInfo.room, { head: 'userQuit', id: clientInfo.id, nickname: clientInfo.nickname });
          clients = clients.filter(c => c.id !== id);
          ws.close();
          console.log(`Client quit: ${ip}`);
          break;
        case 'chat':
          send(clientInfo.room, { head: 'chat', id: clientInfo.id, nickname: clientInfo.nickname, message: data.message });
          break;
        case 'prepare':
          if (clientInfo.seat !== 'spectator') {
            const preparedTeam = {
              clientId: clientInfo.id,
              nickname: clientInfo.nickname,
              room: clientInfo.room,
              seat: clientInfo.seat,
              team: data.team,
              mode: data.mode
            }
            if (data.mode == 'single') {
              let others = preparedTeams.filter(t => t.room == clientInfo.room && t.clientId !== clientInfo.id)
              if (others[0]) {
                if (others[0].mode == preparedTeam.mode) {
                  preparedTeams = preparedTeams.filter(t => t.room != clientInfo.room);
                  //game start
                  send(clientInfo.room, { head: 'gameStart', teams: [preparedTeam, others[0]] });
                } else {
                  preparedTeams.push(preparedTeam);
                }
              }
            }
          }  else {
            clientLog('Spectator attempted to send prepare message, ignored.');
          }
          break;
        case 'game':
          if (clientInfo.seat !== 'spectator') {

          } else {
            clientLog('Spectator attempted to send game message, ignored.');
          }
          break;
        default:
          clientLog('Unknown message head:', data.head);
      }
    } else if (data.secret == 'PBOtest' && data.room && data.nickname) {
      clientInfo = {
        id: id,
        ws: ws,
        ip: ip,
        version: data.version || 'unknown',
        room: data.room,
        seat: data.seat || 'spectator',
        nickname: data.nickname,
        lastPing: Date.now()
      }
      clients.push(clientInfo)
      clientLog(`Client logged in: ${data.nickname} to room ${data.room}`);
    } else {
      client.close()
    }
  });

  ws.on('ping', () => {
    if (clientInfo !== null) {
      clientInfo.lastPing = Date.now()
      ws.pong()
    }
  });

  ws.on('close', () => {
    clientLog('Client disconnected.')
    if (clientInfo !== null) {
      clients = clients.filter(c => c.id !== clientInfo.id);
      preparedTeams = preparedTeams.filter(t => t.clientId !== clientInfo.id);
      send(clientInfo.room, { head: 'userClosed', id: clientInfo.id, nickname: clientInfo.nickname });
    }
  })
  ws.on('error', (err) => console.error('Client error:', err))
});
