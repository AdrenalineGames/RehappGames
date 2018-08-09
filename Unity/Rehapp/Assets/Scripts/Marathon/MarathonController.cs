using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarathonController : MonoBehaviour {

    public int playerLvl;
    public int maxGoal;
    public int minGoal;
    public float maxStepSpeed;
    public float minStepSpeed;
    public Text goalText;
    public TutorialScript tutoScript;

    int goal;
    int maxLevel = 50;
    bool onGame = false;
    float sessionTime = 0;
    int sessionScore = 0;
    char[] ratings = { 'E', 'D', 'C', 'B', 'A', 'S' };

    public Pedometer pedometer;

    private void Start()
    {
        if(GameManager.manager.firstTimeMarathon)
            StartTuto();
    }

    public void StartTuto()
    {
        tutoScript.Init();
        GameManager.manager.firstTimeMarathon = false;
        GameManager.manager.Save();
    }

    private void ResetLevel()
    {
        if (onGame)
            pedometer.StartCapturing();
        onGame = false;
        sessionTime = 0;
        pedometer.steps = 3;
    }

    public void NewGame () {
        ResetLevel();
        playerLvl = GameManager.manager.marathonLevel;
        //playerLvl = 1;      //Tests
        if (playerLvl == 0)
            playerLvl = 1;
        SetGoal();
        pedometer.sessionGoal = goal;
        pedometer.StartCapturing();
        //Debug.Log("New");
        onGame = true;
	}

    private void SetGoal()
    {
        //goal = (int)(((maxGoal-minGoal)/maxLevel)*playerLvl);
        goal = 5 + playerLvl;      //Tests
        goalText.text = "El objetivo de esta sesión son " + goal + " pasos!";
    }

    // Update is called once per frame
    void Update () {
        if (onGame && pedometer.onPocket && !pedometer.capKernell)
        {
            sessionTime += Time.deltaTime;
            if(pedometer.steps == goal)
            {
                Handheld.Vibrate();
                onGame = false;
                pedometer.StartCapturing();
                rateSession();
                ResetLevel();
            }
        }
	}

    private void rateSession()
    {
        float sessionStepSpeed = sessionTime / goal;
        sessionScore = (int)((5 / (maxStepSpeed - minStepSpeed)) * sessionStepSpeed + 7.5f);     //Min session speed for S is 0.75 and min for E is 1.95 seconds for step
        if (sessionScore > 5)
            sessionScore = 5;
        if (sessionScore < 0)
            sessionScore = 0;
        Debug.Log(sessionScore);
        goalText.text = "Sesión terminada, tu puntuación es: " + ratings[sessionScore];
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
                modifyLevel = 1;
                break;
            case 4:
                modifyLevel = 2;
                break;
            case 5:
                modifyLevel = 3;
                break;
        }
        GameManager.manager.SetMarathonLvl(GameManager.manager.marathonLevel += modifyLevel);
    }
}
