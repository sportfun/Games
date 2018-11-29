using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft;

public class GetTraining : MonoBehaviour
{
    [SerializeField] Canvas _loginCanvas;
    [SerializeField] Token _token;
    private string _tokenParsed;
    private Dictionary<string, string> _routes;
    private int _id;
    private Newtonsoft.Json.Linq.JObject _training;
    private List<Newtonsoft.Json.Linq.JObject> _loadedTrainings;

    private TMPro.TMP_Dropdown _optionsDatas;
    private List<TMPro.TMP_Dropdown.OptionData> _optionsDataList;



    void Start()
    {
        this._loadedTrainings = new List<Newtonsoft.Json.Linq.JObject>();
    }

    void Awake()
    {
        this._routes = new Dictionary<string, string>();
        this._routes.Add("server", "http://149.202.41.22:8080");
        this._routes.Add("user", "/api/user/");
        this._routes.Add("trainingId", "/api/training/");
        this._optionsDataList = new List<TMPro.TMP_Dropdown.OptionData>();
        this._optionsDatas = this.gameObject.GetComponentInChildren<TMPro.TMP_Dropdown>();
        this._optionsDatas.ClearOptions();
        this._optionsDatas.AddOptions(this._optionsDataList);
    }

    public void OnEnable()
    {
        Dictionary<string, string> getHeader = new Dictionary<string, string>();
        if (this._token.token.Contains(":") || this._token.token.Contains("\"") || this._token.token.Contains("}"))
            this._tokenParsed = this._token.token.Split(':', '\"', '}')[15];
        else // Connected with QR Code
            this._tokenParsed = this._token.token;
        getHeader.Add("Content-Type", "application/json");
        getHeader.Add("token", this._tokenParsed);
        WWW www = new WWW(this._routes["server"] + this._routes["user"], null, getHeader);
        StartCoroutine(WaitForRequest(www));
    }


    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        this._optionsDatas.ClearOptions();

        if (www.error == null)
        {
            Newtonsoft.Json.Linq.JObject userDatas = Newtonsoft.Json.Linq.JObject.Parse(www.text);
            Dictionary<string, string> getHeader = new Dictionary<string, string>();
            getHeader.Add("Content-Type", "application/json");
            getHeader.Add("token", this._tokenParsed);
            foreach (string id in userDatas["data"]["trainings"])
            {
                WWW GetTraining = new WWW(this._routes["server"] + this._routes["trainingId"] + id, null, getHeader);
                StartCoroutine(GetOneTraining(GetTraining));
            }
        }
        else
        {
            Debug.LogError("WWW Error: " + www.error);
        }
    }

    IEnumerator GetOneTraining(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            this._training = Newtonsoft.Json.Linq.JObject.Parse(www.text);
			if (this._training ["data"].ToString() != "") 
			{
           		this._loadedTrainings.Add(this._training);
            	List<TMPro.TMP_Dropdown.OptionData> item = new List<TMPro.TMP_Dropdown.OptionData>();
				item.Add (new TMPro.TMP_Dropdown.OptionData ((this._training ["data"] ["description"].ToString())));
				this._optionsDatas.AddOptions (item);
				this._optionsDatas.RefreshShownValue ();
			}
        }
        else
        {
            Debug.LogError(www.error);
        }
    }

    public List<Newtonsoft.Json.Linq.JObject> GetLoadedTrainings()
    {
        return (this._loadedTrainings);
    }
}
