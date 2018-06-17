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
    public int playerLevel;
    public int marathonLevel;
    public int skiingLevel;

    public void LoginPlayerName(Text name)
    {
        playerName = name.text;
    }

    public void LoginPlayerPw(Text pw)
    {
        playerPw = pw.text;
    }

    public void SetPlayerLvl(int lvl)
    {
        playerLevel = lvl;
        Save();
    }

    public void SetSkiingvl(int lvl)
    {
        skiingLevel = lvl;
        Save();
    }

    // Use this for initialization
    void Awake () {
        if (manager == null)
            manager = this;
        else if (manager != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        Load();
	}

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saves.dat");

        PlayerData data = new PlayerData();
        data.playerName = "";
        data.playerPw = "";
        data.playerLevel = playerLevel;
        data.skiingLevel = skiingLevel;
        data.marathonLevel = marathonLevel;

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
            playerLevel = data.playerLevel;
            skiingLevel = data.skiingLevel;
            marathonLevel = data.marathonLevel;
        }
        else Debug.Log("No saved data");
    }


}

[Serializable]
class PlayerData
{
    public string playerName;
    public string playerPw;
    public int playerLevel;
    public int marathonLevel;
    public int skiingLevel;
}
