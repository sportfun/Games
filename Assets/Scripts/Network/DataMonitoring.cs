using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DataMonitoring : MonoBehaviour {


	[SerializeField] private string  _server = "http://api.sportsfun.shr.ovh:8080";
    [SerializeField] private TextMeshProUGUI _time;
    [SerializeField] private int _gameChoosedID = 0;
    private string    _gameID;
    [SerializeField] private IntVariable _score;
	private string	_userRoute = "/api/user";
    private string _activityRoute = "/api/activity";
    private string _gameIDRoute = "/api/game";
	private Dictionary<string, string>	_keyword;
    private TrainingData _dataSaver;
    private string _token;
	[HideInInspector] public static bool _areDataSent = false;

    void Start()
    {
        this._dataSaver =  GameObject.Find("Training Saver").GetComponent<TrainingData>() as TrainingData;
        this.GetGameID();
		_areDataSent = false;
    }

	private WWW GetUserData()
    {
        WWW www;
        Dictionary<string, string> postHeader = new Dictionary<string, string>();
        Newtonsoft.Json.Linq.JObject    tokenData;
        string JsonToken;

        postHeader.Add("Content-Type", "application/json");
        JsonToken = this._dataSaver.GetToken();
        tokenData  = Newtonsoft.Json.Linq.JObject.Parse(JsonToken);
        this._token = tokenData["data"]["token"].ToString();
        if (this._token.Length == 0)
        {
            Debug.LogError("Token not found pls verify your network connection.");
        }
        postHeader.Add("token", this._token);
        www = new WWW(this._server + this._userRoute, null, postHeader);
        StartCoroutine(WaitForRequest(www));
        return www;
    }


    private WWW GetGameID()
    {
        WWW www;
        Dictionary<string, string> postHeader = new Dictionary<string, string>();
        Newtonsoft.Json.Linq.JObject    tokenData;
        string JsonToken;
        postHeader.Add("Content-Type", "application/json");
        JsonToken = this._dataSaver.GetToken();
        tokenData  = Newtonsoft.Json.Linq.JObject.Parse(JsonToken);
        this._token = tokenData["data"]["token"].ToString();
        if (this._token.Length == 0)
        {
            Debug.LogError("Token not found pls verify your network connection.");
        }
        postHeader.Add("token", this._token);
        www = new WWW(this._server + this._gameIDRoute, null, postHeader);
        StartCoroutine(WaitForGameID(www));
        return www;
    }

    private IEnumerator SendDataRequestReturn(WWW data)
    {
        yield return data;
        if (data.error != null)
        {
            Debug.LogError(data.error);
            Debug.LogError("Couldn't send End session Data, verify your network connexion.");
        }
        else
        {
            Debug.Log(data.text);
			this.disconnect ();
			_areDataSent = true;
        }
    }
    
    private WWW SendData(WWW data)
    {
        WWW www;
        Newtonsoft.Json.Linq.JObject    userData = null;
        Dictionary<string, string>      postHeader = new Dictionary<string, string>();
        string  jsonStr;
        byte[]  formatData;
        float time = 0.0f;
        string trainingType = "";
        string userID = "";

		Debug.Log ("IS LAUNCHED " + LauncherScript._isLaunched);

        postHeader.Add("Content-Type", "application/json");
        postHeader.Add("token", this._token);
        if (data.error != null || this._gameID == null)
            Debug.LogError("Error during user data request. Please try again");
        else
        {
            userData = Newtonsoft.Json.Linq.JObject.Parse(data.text);
            time =  Mathf.RoundToInt(this._time.GetComponent<TimerUI>().GetTime());
            trainingType = this._dataSaver.GetChoosenTraining()["data"]["sequences"][0]["type"].ToString();
            userID = userData["data"]["_id"].ToString();
            jsonStr = "{\"user\":\""+ userID  + "\", \"game\":\"" + this._gameID +"\", \"type\":" + trainingType + ", \"timeSpent\":" + time +  ", \"score\":" + this._score.Value + "}";
            Debug.Log(jsonStr);
            formatData = System.Text.Encoding.UTF8.GetBytes(jsonStr);
            www = new WWW(this._server + this._activityRoute, formatData, postHeader);
            StartCoroutine(SendDataRequestReturn(www));
            return www;
        }
        return null;
    }
    private IEnumerator WaitForRequest(WWW data)
    {
        yield return data;
        if (data.error != null)
        {
            Debug.Log("There was an error sending request end session data: " + data.ToString());
        }
        else
        {
            this.SendData(data);
            Debug.Log("Sent");
        }
    }

    private IEnumerator WaitForGameID(WWW data)
    {
        yield return data;
        if (data.error != null)
        {
            Debug.Log("There was an error sending request GameID: " + data.ToString());
        }
        else
        {
            Newtonsoft.Json.Linq.JObject jsonID   = Newtonsoft.Json.Linq.JObject.Parse(data.text);
            this._gameID = jsonID["data"][this._gameChoosedID]["_id"].ToString();
        }
    }

	public void SendEndSessionData()
	{
		this.GetUserData ();
	}

	public void disconnect()
	{
		GameObject.DestroyImmediate (GameObject.Find ("Training Saver"));
		Debug.Log ("oui");
		LauncherScript._isLaunched = false;
		SceneManager.LoadScene (0, LoadSceneMode.Single);
	}

}
