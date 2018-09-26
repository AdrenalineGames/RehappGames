using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DataLoader : MonoBehaviour
{
    public GameObject msgSystem;

    public string dbLink = "";
    public string username;
    public string pass;
    public bool connect = false;

    public string[] data;


    public void LoadFromDb()
    {
        StartCoroutine(Connect());
    }

    IEnumerator Connect () {
        dbLink = GameManager.manager.dbLink + "PatientsData.php";
        username = GameManager.manager.playerId;
        pass = GameManager.manager.playerPw;

        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("passPost", pass);

        WWW pacientData = new WWW(dbLink, form);
        yield return pacientData;
        //Debug.Log(pacientDataString);

        if (pacientData.text != "Player not found" && pacientData.text != "Wrong password")
        {
            data = pacientData.text.Split('|');

            GameManager.manager.marathonLevel = Convert.ToInt32(data[3]);
            GameManager.manager.marathonSpeed = (float)Convert.ToDouble(data[4]);
            GameManager.manager.marathonSteps = Convert.ToInt32(data[5]);
            GameManager.manager.skiingLevel = Convert.ToInt32(data[6]);
            GameManager.manager.skiingSpeed = (float)Convert.ToDouble(data[7]);
            GameManager.manager.dodgeballLevel = Convert.ToInt32(data[8]);
            GameManager.manager.saveDate = (ulong)Convert.ToInt64(data[9]);
            GameManager.manager.dbLoading = true;

            GameManager.manager.Save();
        }

        MsgSystem(pacientData.text);
    }

    private void MsgSystem(string msg)
    {
        msgSystem.gameObject.SetActive(true);
        Text tx = msgSystem.transform.Find("MsgText").gameObject.GetComponent<Text>();
        if (msg == "Player not found")
        {
            tx.text = "El jugador no se encuentra en la base de datos de la fundación Mahavir Kmina";
            //tx.text = msg;
        }
        else if(msg == "Wrong password")
        {
            tx.text = "¡Contraseña incorrecta!";
        }
        else
        {
            tx.text = "Progreso cargado correctamente";
            //tx.text = msg;
        }
    }

    void Update()
    {
        if (connect)
        {
            StartCoroutine(Connect());
            connect = false;
        }
    }
}
