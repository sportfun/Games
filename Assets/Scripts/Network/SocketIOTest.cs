using System.Collections.Generic;
using UnityEngine;
using Quobject.SocketIoClientDotNet.Client;

public class SocketIOTest : MonoBehaviour
{
    [SerializeField] private string _serverURL = "http://api.sportsfun.shr.ovh:8080/";

    private Socket _socket = null;

    [SerializeField] private FloatVariable  _speed;
    [SerializeField] private GameEvent  _gameEvent;

    [SerializeField] private FloatVariable  _input;
    

    private void OnEnable()
    {
        this.Open();
    }

    private void OnDisable()
    {
        this.Close();
    }

    // Open the socket connection and setup event callbacks
    private void Open()
    {
        if (this._socket == null)
        {
            this._socket = IO.Socket(this._serverURL);
            this._socket.On(Socket.EVENT_CONNECT, (msg) => {
                StartSession();
                Debug.Log("+ Socket.IO connected: " + msg);
            });
            this._socket.On(Socket.EVENT_CONNECT_ERROR, (msg) => {
                Debug.Log("# Socket.IO error connecting: " + msg);
            });
            this._socket.On(Socket.EVENT_CONNECT_TIMEOUT, (msg) => {
                Debug.Log("# Socket.IO timeout connecting: " + msg);
            });
            this._socket.On(Socket.EVENT_DISCONNECT, () => {
                Debug.Log("- Socket.IO disconnected");
            });
            this._socket.On(Socket.EVENT_RECONNECT, (msg) => {
                Debug.Log("# Socket.IO reconnect: " + msg);
            });
            this._socket.On(Socket.EVENT_RECONNECTING, (msg) => {
                Debug.Log("# Socket.IO reconnecting: " + msg);
            });
            this._socket.On(Socket.EVENT_RECONNECT_ATTEMPT, (msg) => {
                Debug.Log("# Socket.IO reconnect attempt: " + msg);
            });
            this._socket.On(Socket.EVENT_RECONNECT_ERROR, (msg) => {
                Debug.Log("# Socket.IO reconnect error: " + msg);
            });
            this._socket.On(Socket.EVENT_RECONNECT_FAILED, (msg) => {
                Debug.Log("# Socket.IO reconnect failed: " + msg);
            });
            this._socket.On(Socket.EVENT_MESSAGE, (msg) => {
                Debug.Log("# Socket.IO message: " + msg);
            });
            this._socket.On(Socket.EVENT_ERROR, (msg) => {
                Debug.Log("# Socket.IO error: " + msg);
            });
            this._socket.On("error", (msg) => {
                Debug.Log("# Server error: " + msg);
            });
            this._socket.On("info", (msg) => {
                Debug.Log("# Socket.IO info: " + msg);
            });
            this._socket.On("data", (msg) => {
                Newtonsoft.Json.Linq.JObject Jmsg = ((Newtonsoft.Json.Linq.JObject)msg);
                Debug.Log(msg.ToString());
                if((string)Jmsg["body"]["module"] == "RPM Plugin")
                    this._speed.Set((float)(Jmsg["body"]["value"]));
                else if ((string)Jmsg["body"]["module"] == "Controller Plugin")
                    this._input.Set((float)Jmsg["body"]["value"]);
            });
        }
    }   

    // Close the socket connection
    private void Close()
    {
        if (this._socket != null)
        {
            this._socket.Disconnect();
            this._socket = null;
        }
    }

    public void StartSession()
    {
        Newtonsoft.Json.Linq.JObject jobject;
        ConnectInfo link =  new ConnectInfo();
        ConnectInfo start_game = new ConnectInfo();

        link.setType("game");
        link.setLink("totor-la-petite-voiture");
        link.setBody(new Dictionary<string, string>());
        link.getBody().Add("command", "link");
        string linkJson = Newtonsoft.Json.JsonConvert.SerializeObject(link);
        this._socket.Emit("command", linkJson);

        start_game.setType("game");
        start_game.setLink("totor-la-petite-voiture");
        start_game.setBody(new Dictionary<string, string>());
        start_game.getBody().Add("command", "start_game");

        string start_gameJson = Newtonsoft.Json.JsonConvert.SerializeObject(start_game);
        jobject = Newtonsoft.Json.Linq.JObject.Parse(start_gameJson);
        this._socket.Emit("command", jobject);
    }

    public void EndSession()
    {
        ConnectInfo stop_game =  new ConnectInfo();
        Newtonsoft.Json.Linq.JObject jobject;

        stop_game.setType("game");
        stop_game.setLink("totor-la-petite-voiture");
        stop_game.setBody(new Dictionary<string, string>());
        stop_game.getBody().Add("command", "end_game");
        string stop_gameJson = Newtonsoft.Json.JsonConvert.SerializeObject(stop_game);
        jobject = Newtonsoft.Json.Linq.JObject.Parse(stop_gameJson);
        this._socket.Emit("command", jobject);
    }
}
