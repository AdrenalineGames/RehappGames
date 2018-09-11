using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : MonoBehaviour {

    public string loginUrl;
    public string username;
    public string password;

    public bool login = false;

	IEnumerator PatientLogin (string un, string pw, string db) {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", un);
        form.AddField("passPost", pw);

        WWW www = new WWW(db, form);
        yield return www;

        Debug.Log(www.text);
    }
	

	void Update () {
        if (login)
        {
            StartCoroutine(PatientLogin(username, password, loginUrl));
            login = false;
        }
	}
}
