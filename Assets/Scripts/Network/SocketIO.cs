using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using SportfunCommand = System.Collections.Generic.Dictionary<string, string>;

public class SocketIO : MonoBehaviour
{
    [Serializable]
    public class SocketIOEvent : UnityEvent<string, object>
    {
    }

    #region Unity Editor Fields

    [Header("Settings")] [SerializeField] private string _serverUrl = "http://api.sportsfun.shr.ovh:8080/";
    [SerializeField] private string _linkId = "totor-la-petite-voiture";

    [SerializeField] private FloatVariable _speed;
    [SerializeField] private FloatVariable _input;

    [Header("Events")] [SerializeField] private SocketIOEvent _onConnection = new SocketIOEvent();
    [SerializeField] private SocketIOEvent _onDisconnection = new SocketIOEvent();
    [SerializeField] private SocketIOEvent _onReconnection = new SocketIOEvent();

    #endregion

    #region Socket.IO Command

    private static readonly SportfunCommand LinkCommand = new SportfunCommand {{"command", "link"}};
    private static readonly SportfunCommand StartGameCommand = new SportfunCommand {{"command", "start_game"}};
    private static readonly SportfunCommand EndGameCommand = new SportfunCommand {{"command", "end_game"}};

    #endregion

    private Socket _socket;

    private void OnEnable() => Open();
    private void OnDisable() => Close();

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
        _onConnection.Invoke(Socket.EVENT_CONNECT, o);

        Emit(LinkCommand);
        StartSession();
    }

    private void OnDisconnectionHandler(object o)
    {
        Debug.LogError("Socket.io: disconnected");
        _onDisconnection.Invoke(Socket.EVENT_DISCONNECT, o);
    }

    private void OnReconnectionHandler(object o)
    {
        Debug.LogWarning("Socket.io: reconnection");
        _onReconnection.Invoke(Socket.EVENT_RECONNECT, o);
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
                _speed.Set((float) (json["body"]["value"] ?? 0));
                break;
            case "controller":
                _input.Set((float) (json["body"]["value"] ?? 0));
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