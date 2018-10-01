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

    bool existing = false;

    public bool add = false;


    public void SetData()
    {
        username = GameManager.manager.playerId;
        password = GameManager.manager.playerPw;
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        yield return StartCoroutine(this.GetComponent<CheckInternet>().CheckDbConnection());
        if (GameManager.manager.dbConnection)
        {
            yield return StartCoroutine(CheckRepeat());
            if (!existing)
                StartCoroutine(AddNewPatient());
        }
        else
            MsgSystem("No es posible conectar a la base de datos, por favor intente más tarde");
    }

    IEnumerator CheckRepeat()
    {
        url = GameManager.manager.dbLink + "LoadData.php";
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("passPost", password);

        WWW pacientData = new WWW(url, form);
        yield return pacientData;
        Debug.Log(pacientData.text);

        if (pacientData.text == "Player not found")
            existing = false;
        else
        {
            existing = true;
            MsgSystem("El usuario ya existe en la base de datos");
        }
    }

    IEnumerator AddNewPatient()
    {
        url = GameManager.manager.dbLink + "InsertPlayer.php";
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("passPost", password);
        form.AddField("mahavirPost", mahavir ? 1:0);

        WWW www = new WWW(url, form);

        yield return www;
        //Debug.Log(www.text);

        if (www.text == "Added")
        {
            MsgSystem("Jugador creado con éxito en la base de datos");
            //tx.text = msg;
        }
        else
        {
            MsgSystem("El jugador no pudo ser agregado a la base de datos");
            //tx.text = msg;
        }
    }

    private void MsgSystem(string msg)
    {
        msgSystem.gameObject.SetActive(true);
        Text tx = msgSystem.transform.Find("MsgText").gameObject.GetComponent<Text>();
        tx.text = msg;
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
