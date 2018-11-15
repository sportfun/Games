using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using UnityEngine;
using UnityEngine.Events;
using SportfunCommand = System.Collections.Generic.Dictionary<string, string>;

public class SocketIO : MonoBehaviour
{
    [Serializable] public class SocketIoEvent : UnityEvent {}
    [Serializable] public class SocketIoDataEvent : UnityEvent<string, object> {}

    private enum SocketState
    {
        None,
        Connected,
        Disconnected,
        Reconnecting
    }

    #region Unity Editor Fields

    [Header("Settings")] 
    [SerializeField] private string _serverUrl = "http://api.sportsfun.shr.ovh:8080/";
    [SerializeField] private string _linkId = "totor-la-petite-voiture";
    [SerializeField] private float _msBeforeUpdate = 0.1f;

    [Header("Input Events")]
    [SerializeField] private SocketIoDataEvent _onDataReceivedEvent = new SocketIoDataEvent();

    [Header("Socket Events")]
    [SerializeField] private SocketIoEvent _onConnectionEvent = new SocketIoEvent();
    [SerializeField] private SocketIoEvent _onDisconnectionEvent = new SocketIoEvent();
    [SerializeField] private SocketIoEvent _onReconnectionEvent = new SocketIoEvent();

    #endregion

    #region Socket.IO Command

    private static readonly SportfunCommand LinkCommand = new SportfunCommand {{"command", "link"}};
    private static readonly SportfunCommand StartGameCommand = new SportfunCommand {{"command", "start_game"}};
    private static readonly SportfunCommand EndGameCommand = new SportfunCommand {{"command", "end_game"}};

    #endregion

    private float _msBeforeNextUpdate;
    private Socket _socket;
    private SocketState _state = SocketState.None;
    private readonly Queue<Tuple<string, object>> _dataQueue = new Queue<Tuple<string, object>>();
 
    private void OnEnable() => Open();
    private void OnDisable() => Close();

    private void Update()
    {
        _msBeforeNextUpdate -= Time.deltaTime;
        if (_msBeforeNextUpdate > 0) return;

        _msBeforeNextUpdate = _msBeforeUpdate;

        lock (_socket)
        {
            if (_state == SocketState.Connected) _onConnectionEvent.Invoke();
            if (_state == SocketState.Disconnected) _onDisconnectionEvent.Invoke();
            if (_state == SocketState.Reconnecting) _onReconnectionEvent.Invoke();
            _state = SocketState.None;
        }

        lock (_dataQueue)
        {
            if (_dataQueue.Count == 0) return;
            foreach (var data in _dataQueue)
                _onDataReceivedEvent.Invoke(data.Item1, data.Item2);
            _dataQueue.Clear();
        }
    }

    #region Socket.io management

    private void Open()
    {
        Debug.Log($"Socket.io: {nameof(StartSession)}: setup socket & handlers");

        if (_socket == null) _socket = IO.Socket(_serverUrl);
        var handlers = new Dictionary<string, Action<object>>
        {
            // Default handlers
            {Socket.EVENT_CONNECT_ERROR, o => Debug.LogError($"Socket.io: error connecting: {o}")},
            {Socket.EVENT_CONNECT_TIMEOUT, o => Debug.LogError($"Socket.io: timeout connecting: {o}")},
            {Socket.EVENT_RECONNECTING, o => Debug.LogError($"Socket.io: reconnecting: {o}")},
            {Socket.EVENT_RECONNECT_ATTEMPT, o => Debug.LogError($"Socket.io: reconnect attempt: {o}")},
            {Socket.EVENT_RECONNECT_ERROR, o => Debug.LogError($"Socket.io: reconnect error: {o}")},
            {Socket.EVENT_RECONNECT_FAILED, o => Debug.LogError($"Socket.io: reconnect failed: {o}")},
            {Socket.EVENT_MESSAGE, o => Debug.Log($"Socket.io: message: {o}")},
            {Socket.EVENT_ERROR, o => Debug.LogError($"Socket.io: error: {o}")},
            {"info", o => Debug.Log($"Socket.io: info: {o}")},

            // Custom handlers
            {Socket.EVENT_CONNECT, OnConnectionHandler},
            {Socket.EVENT_DISCONNECT, OnDisconnectionHandler},
            {Socket.EVENT_RECONNECT, OnReconnectionHandler},
            {"data", OnDataHandler}
        };

        foreach (var handler in handlers)
            _socket.On(handler.Key, handler.Value);
    }

    private void Close()
    {
        if (_socket == null) return;

        _socket.Disconnect();
        _socket = null;
    }

    private void Emit(SportfunCommand command)
    {
        JObject wsPacket = new WebSocketPacket
        {
            Type = "game",
            LinkId = _linkId,
            Body = command
        };
        _socket.Emit("command", wsPacket);

        Debug.Log($"Socket.io: emit '({command["command"]})'");
    }

    #endregion

    #region Session management

    public void StartSession() => Emit(StartGameCommand);
    public void EndSession() => Emit(EndGameCommand);

    #endregion

    #region Socket.io handlers

    private void OnConnectionHandler(object o)
    {
        Debug.LogWarning("Socket.io: new connection");
        lock (_socket) { _state = SocketState.Connected; }

        Emit(LinkCommand);
        StartSession();
    }

    private void OnDisconnectionHandler(object o)
    {
        Debug.LogError("Socket.io: disconnected");
        lock (_socket) { _state = SocketState.Disconnected; }
    }

    private void OnReconnectionHandler(object o)
    {
        Debug.LogWarning("Socket.io: reconnection");
        lock (_socket) { _state = SocketState.Reconnecting; }
    }

    private void OnDataHandler(object message)
    {
        var json = message as JObject;
        if (json?["body"]?["value"] == null)
        {
            Debug.LogError($"Socket.io: received invalid data: {json ?? message}");
            return;
        }

        Debug.Log($"Socket.io: data received: {json}");
        switch ((string) json["body"]["module"])
        {
            case "rpm":
            case "controller":
                var module = (string) json["body"]["module"];
                var value = json["body"]["value"] ?? -1;

                lock (_dataQueue) { _dataQueue.Enqueue(new Tuple<string, object>(module, value)); }

                break;
            case null:
                Debug.LogError("Socket.io: module not defined");
                break;
            default:
                Debug.LogWarning($"Socket.io: unknown module '{json["body"]["module"]}'");
                break;
        }
    }

    #endregion
}