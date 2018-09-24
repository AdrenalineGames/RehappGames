using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPatient : MonoBehaviour {

    public string url;
    public string username;
    public string password;

    public bool add = false;

    public void SetData()
    {
        url = GameManager.manager.dbLink + "InsertPatient.php";
        username = GameManager.manager.playerId;
        password = GameManager.manager.playerPw;
        AddNewPatient();
    }

	public void AddNewPatient()
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("passPost", password);

        WWW www = new WWW(url, form);
    }

    private void Update()
    {
        if (add)
        {
            AddNewPatient();
            add = false;
        }
    }
}
