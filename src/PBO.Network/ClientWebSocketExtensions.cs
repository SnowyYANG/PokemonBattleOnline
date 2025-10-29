using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonBattleOnline.Network
{
    public static class ClientWebSocketExtensions
    {
        static async Task<string?> ReceiveTextAsync(this ClientWebSocket ws, CancellationToken ct)
        {
            var buffer = new byte[8192];
            using var ms = new MemoryStream();
            while (true)
            {
                var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), ct);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "bye", ct);
                    return null;
                }
                ms.Write(buffer, 0, result.Count);
                if (result.EndOfMessage)
                {
                    if (result.MessageType != WebSocketMessageType.Text)
                        throw new InvalidOperationException("Expected text message");
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }

        static Task SendTextAsync(this ClientWebSocket ws, string text, CancellationToken ct)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            return ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, endOfMessage: true, cancellationToken: ct);
        }
    }
}
