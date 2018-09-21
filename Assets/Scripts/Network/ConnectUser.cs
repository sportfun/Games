using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectUser {
    public string login;
    public string password;

    public void setLogin(string login)
    {
        this.login = login;
    }

    public void setPassword(string password)
    {
        this.password = password;
    }

    public string getLogin()
    {
        return (this.login);
    }

    public string getPassword()
    {
        return (this.password);
    }
}
