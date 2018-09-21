using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour {
	public string command;

	public void setCommand(string command)
	{
		this.command = command;
	}

	public string getBody()
	{
		return (this.command);
	}
}
