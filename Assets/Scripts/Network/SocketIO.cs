using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using UnityEngine;
using UnityEngine.Events;
using SportfunCommand = System.Collections.Generic.Dictionary<string, object>;

public class SocketIO : MonoBehaviour
{
    [Serializable] public class SocketIoEvent : UnityEvent {}
    [Serializable] public class SocketIoReceivedEvent : UnityEvent<string, JObject> {}

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

    [Header("Socket Callbacks")]
    [SerializeField] private string[] _boundChannels = {"data", "qr"};
    [SerializeField] private SocketIoReceivedEvent _onReceivedEvent = new SocketIoReceivedEvent();

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
    private readonly Queue<Tuple<string, JObject>> _receptionQueue = new Queue<Tuple<string, JObject>>();
 
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

        lock (_receptionQueue)
        {
            if (_receptionQueue.Count == 0) return;
            foreach (var data in _receptionQueue)
                _onReceivedEvent.Invoke(data.Item1, data.Item2);
            _receptionQueue.Clear();
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
        };

        foreach (var handler in handlers)
            _socket.On(handler.Key, handler.Value);

        foreach (var channel in _boundChannels)
            if (handlers.ContainsKey(channel))
                throw new Exception($"channel '{channel}' already used and can't be bound");
            else
                _socket.On(channel, o => OnReceptionHandler(channel, o));
    }

    private void Close()
    {
        if (_socket == null) return;

        _socket.Disconnect();
        _socket = null;
    }

    public void Emit<T>(string channel, T value) where T : struct
    {
        _socket.Emit(channel, value);
        Debug.Log($"Socket.io: emit '{value}' on '{channel}'");
    }

    public void Emit(string channel, JObject value)
    {
        _socket.Emit(channel, value);
        Debug.Log($"Socket.io: emit '{value}' on '{channel}'");
    }

    private void Emit(SportfunCommand command)
    {
        JObject wsPacket = new WebSocketPacket
        {
            Type = "game",
            LinkId = _linkId,
            Body = command
        };
        Emit("command", wsPacket);
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

    private void OnReceptionHandler(string channel, object message)
    {
        lock (_receptionQueue)
        {
            _receptionQueue.Enqueue(new Tuple<string, JObject>(channel, message as JObject));
        }
    }

    #endregion
}