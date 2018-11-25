using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LauncherScript : MonoBehaviour
{
    [SerializeField] private string  _server = "http://api.sportsfun.shr.ovh:8080";
    [SerializeField] private Token  _saveToken;
    private string  _loginRoute;
    private string  _gameIDRoute;
    private string  _jsonData;
    private string _token;
    [SerializeField] private TMPro.TMP_InputField  _login;
    [SerializeField] private TMPro.TMP_InputField  _password;
    [SerializeField] private Canvas _trainningsCanvas;
    [SerializeField] private ReactionUnityEvent _onSuccessReaction;

    private Text login;
    void Start()
    {
        this._loginRoute = "/api/user/login";
        this._gameIDRoute = "/api/game";
        this._jsonData = "";
    }


    public WWW Login()
    {
        WWW www;
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");
        ConnectUser credential = new ConnectUser();
        credential.setLogin(this._login.text);
        credential.setPassword(this._password.text);
        string jsonStr = "{\"username\":\""+ credential.getLogin() +"\", \"password\":\"" + credential.getPassword() + "\"}";
        var formData = System.Text.Encoding.UTF8.GetBytes(jsonStr);
        www = new WWW(this._server + this._loginRoute, formData, postHeader);
        StartCoroutine(WaitForRequest(www));
        return www;
    }

    IEnumerator WaitForRequest(WWW data)
{
    yield return data;
    if (data.error != null)
    {
        Debug.Log("There was an error sending request: " + data.error);
    }
    else
    {


        this._token = data.text;
        this._saveToken.token = data.text;
        this._trainningsCanvas.gameObject.SetActive(true);
        if (this._onSuccessReaction != null)
            this._onSuccessReaction.React();
        else
            this.gameObject.SetActive(false);
    }
}

    public void SendData()
    {
       Login();
    }

    public string getToken()
    {
        return (this._token);
    }
}
