using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class GameController : MonoBehaviour {

    public GameObject pauseBtn;
    public Text scoreTx;
    public Text timeTx;
    public float sessionTime;
    public static bool startGame = false;
    public GameObject pitchersPrefab;


    GameObject pitchers;
    private int globalScore = 0;
    float gameTime;
    public static bool onGame = false;
    int maxPoints;
    int minPoints;
    char[] ratings = { 'E', 'D', 'C', 'B', 'A', 'S'};
    int sessionScore;
    int sessionLevel;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void OnEnable() {
        timeTx.text = ((int)gameTime).ToString();
        sessionLevel = GameManager.manager.playerLevel;
        pitchers = Instantiate(pitchersPrefab, new Vector3(320, 240, 2000), 
            Quaternion.Euler(180,0,0));
        ResetLevel();
    }
	
	void Update () {
        if (startGame)
        {
            startGame = false;
            pitchers.GetComponent<Pitcher>().StartShooting();
        }
        if (onGame)
        {
            gameTime -= Time.deltaTime;
            timeTx.text = ((int)gameTime).ToString();
            if(gameTime <= 0)          
                onGame = false;       
        }
        if (pitchers.GetComponent<Pitcher>().shootOver)
        {
            pitchers.GetComponent<Pitcher>().shootOver = false;
            Finish();
        }
    }
    
    public void SetSessionLevel(float lvl)
    {
        sessionLevel = (int)lvl;
        pitchers.GetComponent<Pitcher>().SetDifficult(sessionLevel);
    }

    void Finish()
    {
        minPoints = pitchers.GetComponent<Pitcher>().minPoints;
        maxPoints = pitchers.GetComponent<Pitcher>().maxPoints;
        rateSession();
        pauseBtn.transform.GetChild(0).gameObject.SetActive(true);
        pauseBtn.GetComponentInChildren<Text>().text = "Obtuviste una: " + ratings[sessionScore];
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(3);
    }

    private void rateSession()
    {
        sessionScore = (int)Math.Round((5d* ((double)(globalScore - minPoints) / (maxPoints - minPoints))));
        if(sessionLevel >= GameManager.manager.playerLevel)
            updateLevel(sessionScore);
    }

    private void updateLevel(int ss)
    {
        int modifyLevel = 0;
        switch (ss)
        {
            case 0:
                //modifyLevel = -1;
                //break;
            case 1:
            case 2:
                modifyLevel = 0;
                break;
            case 3:
                modifyLevel = 1 + sessionLevel - GameManager.manager.playerLevel;
                break;
            case 4:
                modifyLevel = 2 + sessionLevel - GameManager.manager.playerLevel;
                break;
            case 5:
                modifyLevel = 3 + sessionLevel - GameManager.manager.playerLevel;
                break;
        }
        GameManager.manager.SetPlayerLvl(GameManager.manager.playerLevel += modifyLevel);
    }

    public void AddScore(int score)
    {
        globalScore += score;
        UpdateScore();
    }
    
    void UpdateScore()
    {
        scoreTx.text = "Score: " + globalScore;
    }

    public void ResetLevel()
    {
        gameTime = sessionTime;
        onGame = false;
        startGame = false;
        maxPoints = 0;
        minPoints = 0;
        sessionScore = 0;
        UpdateScore();
        pitchers.GetComponent<Pitcher>().SetDifficult(sessionLevel);
    }
}
