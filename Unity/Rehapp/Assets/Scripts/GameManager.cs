using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour {

    public static GameManager manager;

    public string dbLink = "http://localhost/MahavirKmina/";

    public string playerId = "";
    public string playerPw = "";
    public int marathonLevel = 0;
    public float marathonSpeed = 0;
    public int marathonSteps = 0;
    public int skiingLevel = 0;
    public float skiingSpeed = 0;
    public int dodgeballLevel = 0;
    public bool save = false;

    public ulong saveDate = 0;

    string unlockPw = "r3A6";
    string inputPw = "";

    //private void Start()
    //{
    //    ulong date = DateAsLong();
    //    Debug.Log(date);
    //}

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
            marathonLevel = 6;
            skiingLevel = 6;
            Save();
        }
    }

    public void LoginPlayerName(Text name)
    {
        playerId = name.text;
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

    public void SetSkiingLvl(int lvl, float lastSpeed)
    {
        skiingSpeed = lastSpeed;
        skiingLevel = lvl;
        Save();
    }

    public void SetMarathonLvl(int lvl, float lastSpeed, int lastGoal)
    {
        marathonSpeed = lastSpeed;
        marathonSteps = lastGoal;
        marathonLevel = lvl;
        Save();
    }

    // Use this for initialization
    void Awake ()
    {
        Debug.Log(Application.persistentDataPath);
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
        saveDate = DateAsLong();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saves.dat");

        PlayerData data = new PlayerData();
        data.saveDate = saveDate;
        data.playerId = playerId;
        data.playerPw = playerPw;
        data.marathonLevel = marathonLevel;
        data.marathonSpeed = marathonSpeed;
        data.marathonSteps = marathonSteps;
        data.skiingLevel = skiingLevel;
        data.skiingSpeed = skiingSpeed;
        data.dodgeballLevel = dodgeballLevel;

        bf.Serialize(file, data);
        file.Close();

        if (playerId != "")
            this.GetComponentInChildren<DataSaver>().SetData();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/saves.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saves.dat", FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            saveDate = data.saveDate;
            playerId = data.playerId;
            playerPw = data.playerPw;
            marathonLevel = data.marathonLevel;
            marathonSpeed = data.marathonSpeed;
            marathonSteps = data.marathonSteps;
            skiingLevel = data.skiingLevel;
            skiingSpeed = data.skiingSpeed;
            dodgeballLevel = data.dodgeballLevel;
        }
        else Debug.Log("No saved data");
    }

    public void ReiniciarNiveles()
    {
        marathonLevel = 0;
        marathonSpeed = 0;
        marathonSteps = 0;
        skiingLevel = 0;
        skiingSpeed = 0;
        dodgeballLevel = 0;
        Save();
    }

    ulong DateAsLong()
    {
        return((ulong)DateTime.Now.Year * 10000000 + (ulong)DateTime.Now.DayOfYear * 10000
            + (ulong)DateTime.Now.Hour * 100 + (ulong)DateTime.Now.Minute);
    }
}

[Serializable]
class PlayerData
{
    public ulong saveDate;
    public string playerId;
    public string playerPw;
    public int marathonLevel;
    public float marathonSpeed;
    public int marathonSteps;
    public int skiingLevel;
    public float skiingSpeed;
    public int dodgeballLevel;
}
