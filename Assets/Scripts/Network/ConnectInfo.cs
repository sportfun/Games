using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectInfo {

	public string type;
    public string link_id;
    public Dictionary<string, string> body;

	public void setType(string type)
	{
		this.type = type;
	}

	public void setLink(string link)
	{
		this.link_id = link;
	}

	public void setBody(Dictionary<string, string> body)
	{
		this.body = body;
	}

	public Dictionary<string, string>	getBody()
	{
		return (this.body);
	}
}
