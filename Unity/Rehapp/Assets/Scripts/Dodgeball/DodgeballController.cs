using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class DodgeballController : MonoBehaviour {

    public GameObject pauseBtn;
    public Text scoreTx;
    public Text timeTx;
    public float sessionTime;
    public static bool startGame = false;
    public GameObject pitchersPrefab;
    public TutorialScript tutoScript;


    GameObject pitchers;
    private int globalScore = 0;
    float gameTime;
    public static bool onGame = false;
    int maxPoints;
    int minPoints;
    char[] ratings = { 'E', 'D', 'C', 'B', 'A', 'S'};
    int sessionScore;
    int sessionLevel;

    private void Start()
    {
        if (GameManager.manager.firstTimeDodgeball)
            StartTuto();
    }

    public void StartTuto()
    {
        tutoScript.Init();
        GameManager.manager.firstTimeDodgeball = false;
        GameManager.manager.Save();
    }

    void OnEnable() {
        timeTx.text = ((int)gameTime).ToString();
        sessionLevel = GameManager.manager.dodgeballLevel;
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
            gameTime -= Time.deltaTime;     // Game chrono
            timeTx.text = ((int)gameTime).ToString();
            if(gameTime <= 0)          
                onGame = false;       
        }
        if (pitchers.GetComponent<Pitcher>().shootOver)     // Waits until ther is no balls in game
        {
            pitchers.GetComponent<Pitcher>().shootOver = false;
            Finish();
        }
    }

    public void SetSessionLevel(float lvl)      // This game allows to modify the dificult from settings
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
        if(sessionLevel >= GameManager.manager.dodgeballLevel)
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
                modifyLevel = 1 + sessionLevel - GameManager.manager.dodgeballLevel;
                break;
            case 4:
                modifyLevel = 2 + sessionLevel - GameManager.manager.dodgeballLevel;
                break;
            case 5:
                modifyLevel = 3 + sessionLevel - GameManager.manager.dodgeballLevel;
                break;
        }
        GameManager.manager.SetDodgeballLvl(GameManager.manager.dodgeballLevel += modifyLevel);
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
