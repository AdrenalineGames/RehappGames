using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaver : MonoBehaviour {

    public string url;
    public string username;
    public int marathonLevel;
    public float marathonSpeed;
    public int marathonSteps;
    public int skiingLevel;
    public float skiingSpeed;
    public int dodgeballLevel;
    ulong saveDate;

    public void SetData()
    {
        url = GameManager.manager.dbLink + "SaveData.php";
        username = GameManager.manager.playerId;
        marathonLevel = GameManager.manager.marathonLevel;
        marathonSpeed = GameManager.manager.marathonSpeed;
        marathonSteps = GameManager.manager.marathonSteps;
        skiingLevel = GameManager.manager.skiingLevel;
        skiingSpeed = GameManager.manager.skiingSpeed;
        dodgeballLevel = GameManager.manager.dodgeballLevel;
        saveDate = GameManager.manager.saveDate;
        StartCoroutine(SaveDataDb());
    }

    IEnumerator SaveDataDb()
    {
        yield return StartCoroutine(this.GetComponent<CheckInternet>().CheckDbConnection());

        if (GameManager.manager.dbConnection)
        { 
            WWWForm form = new WWWForm();
            form.AddField("usernamePost", username);
            form.AddField("marathonLevelPost", marathonLevel);
            form.AddField("marathonSpeedPost", marathonSpeed.ToString());
            form.AddField("marathonStepsPost", marathonSteps);
            form.AddField("skiingLevelPost", skiingLevel);
            form.AddField("skiingSpeedPost", skiingSpeed.ToString());
            form.AddField("dodgeballLevelPost", dodgeballLevel);
            form.AddField("saveDatePost", saveDate.ToString());

            WWW www = new WWW(url, form);

            yield return www;

            Debug.Log(www.text);
        }
    }
}
