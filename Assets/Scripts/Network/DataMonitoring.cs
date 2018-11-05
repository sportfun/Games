using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DataMonitoring : MonoBehaviour {

	[SerializeField] private string  _server = "http://api.sportsfun.shr.ovh:8080";
    [SerializeField] private TextMeshProUGUI _time;
    [SerializeField] private int    _GameID = 0;
	private string	_userRoute = "/api/user";
    private string _activityRoute = "/api/activity";
	private Dictionary<string, string>	_keyword;
    private TrainingData _dataSaver;
    private string _token;

    void Start()
    {
        this._dataSaver =  GameObject.Find("Training Saver").GetComponent<TrainingData>() as TrainingData;
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

        postHeader.Add("Content-Type", "application/json");
        postHeader.Add("token", this._token);
        if (data.error != null)
            Debug.LogError("Error during user data request. Please try again");
        else
        {
            userData = Newtonsoft.Json.Linq.JObject.Parse(data.text);
            time =  Mathf.RoundToInt(this._time.GetComponent<TimerUI>().GetTime());
            trainingType = this._dataSaver.GetChoosenTraining()["data"]["sequences"][0]["type"].ToString();
            userID = userData["data"]["_id"].ToString();
            jsonStr = "{\"user\":\""+ userID  + "\", \"game\":" + this._GameID +", \"type\":" + trainingType + ", \"timeSpent\":" + time + "}";
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

	public void SendEndSessionData()
	{
		this.GetUserData();
	}

}
