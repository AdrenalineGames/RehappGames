using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPatient : MonoBehaviour {

    public GameObject msgSystem;

    public string url;
    public string username;
    public string password;

    public bool add = false;

    public void SetData()
    {
        url = GameManager.manager.dbLink + "InsertPatient.php";
        username = GameManager.manager.playerId;
        password = GameManager.manager.playerPw;
        StartCoroutine(AddNewPatient());
    }

    IEnumerator AddNewPatient()
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("passPost", password);

        WWW www = new WWW(url, form);

        yield return www;

        MsgSystem(www.text);
    }

    private void MsgSystem(string msg)
    {
        msgSystem.gameObject.SetActive(true);
        Text tx = msgSystem.transform.Find("MsgText").gameObject.GetComponent<Text>();
        if (msg == "Added")
        {
            tx.text = "Paciente creado con éxito";
            //tx.text = msg;
        }
        else
        {
            tx.text = "El paciente no pudo ser creado";
            //tx.text = msg;
        }
    }

    private void Update()
    {
        if (add)
        {
            StartCoroutine(AddNewPatient());
            add = false;
        }
    }
}
