using System;
using System.Collections.Concurrent;
using WebSocketSharp;

namespace Argen.Demo;

public class SocketWrapper
{
    private WebSocket _webSocket;
    private readonly ConcurrentQueue<Action> _events = new();

    public event Action OnConnected;
    public event Action OnDisconnected;
    public event Action<string> OnDataReceived;

    public SocketWrapper()
    {
        
    }

    public void ConnectToHost(string url, int port)
    {
        _webSocket = new WebSocket($"ws://{url}:{port}");
        _webSocket.OnClose += _webSocket_OnClose;
        _webSocket.OnError += _webSocket_OnError;
        _webSocket.OnOpen += _webSocket_OnOpen;
        _webSocket.OnMessage += _webSocket_OnMessage;

        _webSocket.Connect();
    }

    private void _webSocket_OnMessage(object sender, MessageEventArgs e)
    {
        if (e.IsText)
        {
            //OnDataReceived?.Invoke(e.Data);
            _events.Enqueue(() => OnDataReceived?.Invoke(e.Data));
        }
    }

    private void _webSocket_OnError(object sender, ErrorEventArgs e)
    {
        _events?.Enqueue(() => OnDisconnected?.Invoke()); 
    }

    private void _webSocket_OnOpen(object sender, System.EventArgs e)
    {
        _events?.Enqueue(() => OnConnected?.Invoke());
    }

    private void _webSocket_OnClose(object sender, CloseEventArgs e)
    {
        _events?.Enqueue(() => OnDisconnected?.Invoke());
    }

    public void DisconnectFromHost()
    {
        _webSocket?.Close();
    }

    public void SendData(string data)
    {
        _webSocket?.Send(data);
    }

    public void PoolEvents()
    {
        while (_events.TryDequeue(out var action))
        {
            action.Invoke();
        }
    }
}
