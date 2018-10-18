using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInternet : MonoBehaviour {

    public IEnumerator CheckDbConnection()
    {
        string url = GameManager.manager.dbLink + "connectionPDO.php";
        WWW connect = new WWW(url);
        yield return connect;
        //Debug.Log(connect.text);
        if (connect.error != null)
        {
            Debug.Log("Error. Check internet connection!");
            Debug.Log(connect.error);
            GameManager.manager.dbConnection = false;
        }
        else if (connect.text == "Error")
        {
            Debug.Log("Error. Database offline");
            GameManager.manager.dbConnection = false;
        }
        else
        {
            GameManager.manager.dbConnection = true;
            Debug.Log(connect.text);
        }
    }
}
