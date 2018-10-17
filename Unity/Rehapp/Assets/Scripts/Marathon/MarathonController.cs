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
    public bool onPocket = false;

    public Text state;
    public Text stepsTx;

    public GameObject lockPanel;
    public TutorialScript tutoScript;

    int goal;
    int steps = 0;
    int maxLevel = 50;
    int minLevel = 1;
    bool onGame = false;
    float sessionTime = 0;
    float sessionStepSpeed = 0;
    int sessionScore = 0;
    char[] ratings = { 'E', 'D', 'C', 'B', 'A', 'S' };

    public Pedometer pedometer;

    private void Start()
    {
        PAProximity.messageReceiver = gameObject;
        if (GameManager.manager.marathonLevel == 0)
            StartTuto();
    }

    public void StartTuto()
    {
        tutoScript.Init();
        GameManager.manager.marathonLevel = 1;
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
        pedometer.StartCapturing();
        state.text = ("Guarda el celular en el bolsillo");
        //Debug.Log("New");
        onGame = true;
	}

    public void StopGame()
    {
        onGame = false;
        pedometer.StartCapturing();
        GameManager.manager.advertising = true;

        if (steps > goal * 0.7)
        {
            rateSession();
            state.text = "Sesión terminada, tu puntuación es: " + ratings[sessionScore];
        }
        else
            state.text = "Sesión incompleta ";

        ResetLevel();
    }

    private void SetGoal()
    {
        playerLvl = GameManager.manager.marathonLevel;
        goal = ((playerLvl-minLevel)*((maxGoal-minGoal)/(maxLevel-minLevel))+minGoal);
        if (GameManager.manager.tests)
        {
            goal = 5 + playerLvl;      //Tests
        }
        Debug.Log(goal);
        stepsTx.text = "Tu nuevo objetivo son " + goal + " pasos!";
    }

    // Update is called once per frame
    void Update () {
        if (onGame && pedometer.greenLight && !pedometer.capKernell)
        {
            sessionTime += Time.deltaTime;
            if(pedometer.steps == goal)
            {
                Handheld.Vibrate();
                rateSession();
                Debug.Log("Goal");
                state.text = "Nivel completado, tu puntuación es: " + ratings[sessionScore];
                SetGoal();
            }

            ShowSteps();
        }

        OnProximityChange(PAProximity.proximity);
        pedometer.greenLight = onPocket;
        TurnScreenOnOff(onPocket);
    }

    private void ShowSteps()
    {
        if (steps != pedometer.steps)
        {
            steps = pedometer.steps;
            stepsTx.text = steps + " de " + goal;
        }
    }

    private void TurnScreenOnOff(bool condition)
    {
        if (condition)
            lockPanel.SetActive(true);        //Apagar pantalla
        else
            lockPanel.SetActive(false);       //Encender pantalla
    }

    void OnProximityChange(PAProximity.Proximity proximity)
    {
#if UNITY_EDITOR
#else
        onPocket = ((proximity == PAProximity.Proximity.NEAR) ? true : false);
#endif
    }

    private void rateSession() 
    {
        sessionStepSpeed = sessionTime / pedometer.steps;
        sessionScore = (int)((5 / (maxStepSpeed - minStepSpeed)) * sessionStepSpeed + 7.5f);     //Min session speed for S is 0.75 and min for E is 1.95 seconds for step

        if (sessionScore > 5)
            sessionScore = 5;
        if (sessionScore < 0)
            sessionScore = 0;

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
            case 4:
                modifyLevel = 1;
                break;
            case 5:
                modifyLevel = 2;
                break;
        }
        GameManager.manager.SetMarathonLvl(GameManager.manager.marathonLevel += modifyLevel, sessionStepSpeed, goal);
    }
}
