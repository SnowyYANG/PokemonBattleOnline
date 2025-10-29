const WebSocket = require('ws');
const {pokemonData2Proxy, pokemonData2Sim} = require('./pokemon.js');

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
            preparedTeams.push(preparedTeam);
            clientLog('prepared');
            if (data.mode == 'single') {
              const prepare = preparedTeams.filter(t => t.room == clientInfo.room)
              if (prepare.length == 2 && prepare[0].seat != prepare[1].seat && prepare[0].mode == prepare[1].mode) {
                  //game start
                  let team0 = prepare[0].team.map(p => pokemonData2Proxy(p, 0))
                  let team1 = prepare[1].team.map(p => pokemonData2Proxy(p, 1))
                  let game = {
                    id: Date.now() + Math.random().toString(36).substring(2, 9),
                    gaming: true,
                    turnNumber: 0,
                    settings: {
                      mode: 'single',
                      sleepRule: true,
                      terrain: 'ground',
                    },
                    teams: [[{
                      id: prepare[0].clientId,
                      nickname: prepare[0].nickname,
                      teamIndex: 0,
                      mega: false,
                      giveUp: false,
                      pokemons: team0,
                    }], [{
                      id: prepare[1].clientId,
                      nickname: prepare[1].nickname,
                      teamIndex: 1,
                      mega: false,
                      giveUp: false,
                      pokemons: team1,
                    }]],
                    board: {
                      weather: 'normal',
                      terrain: 'ground',
                      fields: [
                        {teamId: 0, tiles: [{ x: 0, pokemon: null, nextSendOutPokemonIndex: -1, buff:{}, turnBuff:new Set() }]},
                        {teamId: 1, tiles: [{ x: 0, pokemon: null, nextSendOutPokemonIndex: -1, buff:{}, turnBuff:new Set() }]}
                      ],
                    },
                  }
                  games.push(game)
                  preparedTeams = preparedTeams.filter(t => t.room != clientInfo.room); //remove current
                  send(clientInfo.room, { 
                    head: 'game.start', 
                    gameId: game.id,
                    mode: data.mode,
                    teams: [prepare[0].team.map(p => pokemonData2Sim(p, 0)), prepare[1].team.map(p => pokemonData2Sim(p, 1))]
                  })
                  send(clientInfo.room, {
                    head: 'game.sendOut',
                    nickname: prepare[0].nickname,
                    pokemonname: team0[0].name
                  })
                  send(clientInfo.room, {
                    head: 'game.sendOut',
                    nickname: prepare[1].nickname,
                    pokemonname: team1[0].name
                  })
              } else {
                //clear prepare and notify error
              }
            }
          } else {
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
