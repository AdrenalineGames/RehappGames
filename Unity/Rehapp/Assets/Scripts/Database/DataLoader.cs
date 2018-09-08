using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour {
    public string dbLink = "";
    public bool connect = false;

    public string[] data;

	// Use this for initialization
	IEnumerator Connect () {
        WWW pacientData = new WWW(dbLink);
        yield return pacientData;
        string pacientDataString = pacientData.text;
        //Debug.Log(pacientDataString);

        data = pacientDataString.Split('|');
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
