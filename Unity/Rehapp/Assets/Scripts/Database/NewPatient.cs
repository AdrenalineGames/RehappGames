using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPatient : MonoBehaviour {

    public string url;
    public string mahavirId;
    public string username;
    public string password;

    public bool add = false;

	public void AddNewPatient(string mID, string un, string pass, string u)
    {
        WWWForm form = new WWWForm();
        form.AddField("mahavirIDPost", mID);
        form.AddField("usernamePost", un);
        form.AddField("passPost", pass);

        WWW www = new WWW(u, form);
    }

    private void Update()
    {
        if (add)
        {
            AddNewPatient(mahavirId, username, password, url);
            add = false;
        }
    }
}
