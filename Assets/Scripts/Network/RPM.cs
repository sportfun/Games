using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPM : MonoBehaviour {
	public string link_id;
    public string rpm;

	public void setLink(string link)
	{
		this.link_id = link;
	}

	public void setRPM(string rpm)
	{
		this.rpm = rpm;
	}

	public string getRpm()
	{
		return (this.rpm);
	}
}
