using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour {

    public static GameManager manager;
    public string playerName;
    public string playerPw;
    public int dodgeballLevel = 0;
    public int marathonLevel = 0;
    public int skiingLevel = 0;
    public bool firstTimeMarathon = true;
    public bool firstTimeSki = true;
    public bool firstTimeDodgeball = true;
    public bool unlockGames = false;
    public bool save = false;

    string unlockPw = "r3A6";
    string inputPw = "";

    private void Update()
    {
        if (save)
        {
            save = false;
            Save();
        }
    }

    public void SetInputPw(string ipw)
    {
        inputPw = ipw;
    }

    public void UnlockGames()
    {
        if (unlockPw == inputPw)
        {
            unlockGames = true;
            Save();
        }
    }

    public void LoginPlayerName(Text name)
    {
        playerName = name.text;
    }

    public void LoginPlayerPw(Text pw)
    {
        playerPw = pw.text;
    }

    public void SetDodgeballLvl(int lvl)
    {
        dodgeballLevel = lvl;
        Save();
    }

    public void SetSkiingLvl(int lvl)
    {
        skiingLevel = lvl;
        Save();
    }

    public void SetMarathonLvl(int lvl)
    {
        marathonLevel = lvl;
        Save();
    }

    // Use this for initialization
    void Awake ()
    {
        //Debug.Log(Application.persistentDataPath);
        if (manager == null)
            manager = this;
        else if (manager != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        Load();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saves.dat");

        PlayerData data = new PlayerData();
        data.playerName = "";
        data.playerPw = "";
        data.dodgeballLevel = dodgeballLevel;
        data.skiingLevel = skiingLevel;
        data.marathonLevel = marathonLevel;
        data.firstTimeDodgeball = firstTimeDodgeball;
        data.firstTimeMarathon = firstTimeMarathon;
        data.firstTimeSki = firstTimeSki;
        data.unlockGames = unlockGames;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/saves.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saves.dat", FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            playerName = data.playerName;
            playerPw = data.playerPw;
            dodgeballLevel = data.dodgeballLevel;
            skiingLevel = data.skiingLevel;
            marathonLevel = data.marathonLevel;
            firstTimeDodgeball = data.firstTimeDodgeball;
            firstTimeMarathon = data.firstTimeMarathon;
            firstTimeSki = data.firstTimeSki;
            unlockGames = data.unlockGames;
        }
        else Debug.Log("No saved data");
    }

    public void ReiniciarNiveles()
    {
        marathonLevel = 1;
        skiingLevel = 1;
        dodgeballLevel = 1;
        unlockGames = false;
        Save();
    }

}

[Serializable]
class PlayerData
{
    public string playerName;
    public string playerPw;
    public int dodgeballLevel;
    public int marathonLevel;
    public int skiingLevel;
    public bool firstTimeMarathon;
    public bool firstTimeSki;
    public bool firstTimeDodgeball;
    public bool unlockGames;
}
