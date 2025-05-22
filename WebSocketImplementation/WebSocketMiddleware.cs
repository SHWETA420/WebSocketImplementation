// WebApi/Middleware/WebSocketMiddleware.cs
using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Model;
using Model.Chat;
using Service.Chat;

namespace WebSocketImplementation
{
    public class WebSocketMiddleware
    {
        private static readonly ConcurrentDictionary<string, WebSocket> _sockets = new();
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public WebSocketMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/ws/chat")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    string connectionId = Guid.NewGuid().ToString();
                    _sockets.TryAdd(connectionId, webSocket);

                    try
                    {
                        await HandleWebSocketConnection(connectionId, webSocket);
                    }
                    finally
                    {
                        _sockets.TryRemove(connectionId, out _);
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
                    }
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task HandleWebSocketConnection(string connectionId, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var chatService = scope.ServiceProvider.GetRequiredService<IChatService>();
                    var messageJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var message = JsonSerializer.Deserialize<ChatMessage>(messageJson);

                    if (message != null)
                    {
                        var savedMessage = await chatService.SaveMessageAsync(message.Sender, message.Message);

                        var broadcastMessage = JsonSerializer.Serialize(savedMessage);
                        var broadcastBuffer = Encoding.UTF8.GetBytes(broadcastMessage);

                        foreach (var socket in _sockets.Values)
                        {
                            if (socket.State == WebSocketState.Open)
                            {
                                await socket.SendAsync(
                                    new ArraySegment<byte>(broadcastBuffer, 0, broadcastBuffer.Length),
                                    WebSocketMessageType.Text,
                                    true,
                                    CancellationToken.None);
                            }
                        }
                    }
                }

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
        }
    }
}