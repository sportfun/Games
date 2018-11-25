using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingData : MonoBehaviour {

	[SerializeField] private DontDestroyTraining	_training; 
	[SerializeField] private Token	_token;

	private	UserData	_userData;

	public Newtonsoft.Json.Linq.JObject	GetChoosenTraining()
	{
		return (this._training.Value);
	}

	public string GetToken()
	{
		return (this._token.token);
	}

	public void setUserData(UserData datas)
	{
		this._userData = datas;

	}

	public UserData getUserData()
	{
		return (this._userData);
	
	}
}
