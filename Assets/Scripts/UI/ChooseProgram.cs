using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseProgram : MonoBehaviour {

	// Use this for initialization

	private List<Newtonsoft.Json.Linq.JObject>	_trainings;
	[SerializeField]	GameObject	_trainingSaver;
	[SerializeField] private DontDestroyTraining _training;


	void Awake () {
		DontDestroyOnLoad(this._trainingSaver.transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnProgramChoose()
	{
		// Here launch game choosing canvas

		this._trainings = GetComponent<GetTraining>().GetLoadedTrainings();
		//Debug.Log(this._trainings[GetComponentInChildren<Dropdown>().value]);
		this._training.Value = this._trainings[GetComponentInChildren<TMPro.TMP_Dropdown>().value];
		this._training.Text = this._trainings[GetComponentInChildren<TMPro.TMP_Dropdown>().value].ToString();
		// save the sport program
		// and stock it somewhere
	}

	public void LaunchGame(int sceneID)
	{
		SceneManager.LoadScene(sceneID);
	}
}
