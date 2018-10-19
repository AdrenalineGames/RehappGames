using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayer : MonoBehaviour {

    public GameObject msgSystem;
    public Toggle mahavirToggle;
    public PwListener pwListener;

    public Transform warningSystem;
    public Transform hyperLink;

    public string url;
    public string username;
    public string password;
    public bool mahavir = false;

    bool existing = false;


    public void SetData()
    {
        username = GameManager.manager.playerId;
        password = GameManager.manager.playerPw;
        StartCoroutine(AcceptPolitics());
    }

    IEnumerator AcceptPolitics()
    {
        Transform warningS = Instantiate(warningSystem, GameObject.Find("Canvas").transform);
        warningS.GetComponent<WarningSystem>().SetMsg("Advertencia:" + Environment.NewLine + "Al presionar Aceptar, usted acepta nuestra políta de manejo de datos");
        Transform hLink = Instantiate(hyperLink, warningS.Find("LinkPanel"));
        hLink.GetComponent<HyperLink>().SetText("Ver Política de manejo de datos");
        hLink.GetComponent<HyperLink>().url = ("https://dizquestudios.000webhostapp.com/Manejo%20de%20datos.pdf");

        yield return new WaitUntil(() => warningS.GetComponent<WarningSystem>().Decision);
        if (warningS.GetComponent<WarningSystem>().Accept)
        {
            GameManager.manager.Save();
            StartCoroutine(Register());
        }
        warningS.GetComponent<WarningSystem>().DestroyMsg();
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
        //Debug.Log(pacientData.text);

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
        Debug.Log(www.text);

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

    public void SetMahavirPatient(bool patient)
    {
        mahavirToggle.isOn = mahavir = patient;
    }
}
