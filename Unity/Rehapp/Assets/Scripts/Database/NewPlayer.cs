using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayer : MonoBehaviour {

    public GameObject msgSystem;
    public Toggle mahavirToggle;
    public PwListener pwListener;

    public string url;
    public string username;
    public string password;
    public bool mahavir = false;

    public bool add = false;


    public void SetData()
    {
        url = GameManager.manager.dbLink + "InsertPlayer.php";
        Debug.Log(url);
        username = GameManager.manager.playerId;
        password = GameManager.manager.playerPw;
        StartCoroutine(AddNewPatient());
    }

    IEnumerator AddNewPatient()
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("passPost", password);
        form.AddField("mahavirPost", mahavir ? 1:0);

        WWW www = new WWW(url, form);

        yield return www;
        Debug.Log(www.text);

        MsgSystem(www.text);
    }

    private void MsgSystem(string msg)
    {
        msgSystem.gameObject.SetActive(true);
        Text tx = msgSystem.transform.Find("MsgText").gameObject.GetComponent<Text>();
        if (msg == "Added")
        {
            tx.text = "Jugador creado con éxito en la base de datos";
            //tx.text = msg;
        }
        else
        {
            tx.text = "El jugador no pudo ser agregado a la base de datos";
            //tx.text = msg;
        }
    }

    private void Update()
    {
        mahavirToggle.isOn = pwListener.result;
        mahavir = pwListener.result;
        if (add)
        {
            StartCoroutine(AddNewPatient());
            add = false;
        }
    }
}
