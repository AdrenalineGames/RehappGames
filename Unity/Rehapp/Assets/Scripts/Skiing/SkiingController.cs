using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkiingController : MonoBehaviour {

    public Transform flag;
    public Transform flagsContainer;
    public Text timerText;
    public ShowCam showCamScript = null;
    public PlayerPosition playerScript;

    public float gameTime = 10;
    public int distanceBetweenFlags;
    public int globalCamWidth;
    public int globalCamHeigth;
    public bool useRaerGlobalCam;

    public static bool onGame = false;
    public bool onGameB = false;

    float gameTimer;
    static float sessionScore;
    int playerDistance;
    char[] ratings = { 'E', 'D', 'C', 'B', 'A', 'S' };


    void Update () {
        //onGame = onGameB;
        onGameB = onGame;
        if (onGame)
        {
            gameTimer -= Time.deltaTime;
            if (gameTimer >= 0)
            {
                timerText.text = ((int)gameTimer).ToString();
                if ((int)(playerScript.distanceCovered) == playerDistance)
                {
                    playerDistance = (int)playerScript.distanceCovered + distanceBetweenFlags;
                    NewFlag();
                }
            }
            else
            {
                if (GameObject.FindGameObjectWithTag("Ball") == null)
                {
                    onGame = false;
                    RateSession();
                }
            }
        }
    }

    private void RateSession()
    {
        sessionScore = (float)Math.Floor(((6 / gameTime) * sessionScore));
        if (sessionScore > 5) sessionScore = 5;
        Debug.Log("Obtuviste una: " + ratings[(int)sessionScore]);
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
                modifyLevel = 1 + (int)sessionScore - GameManager.manager.skiingLevel;
                break;
            case 4:
                modifyLevel = 2 + (int)sessionScore - GameManager.manager.skiingLevel;
                break;
            case 5:
                modifyLevel = 3 + (int)sessionScore - GameManager.manager.skiingLevel;
                break;
        }
        GameManager.manager.SetSkiingvl(GameManager.manager.skiingLevel += modifyLevel);
    }

    public static void PlayerScore(int point)
    {
        sessionScore += point;
    }

    void NewFlag()
    {
        Vector3 newFlagPos = new Vector3(UnityEngine.Random.Range(-9.0f, 9.0f), flagsContainer.position.y, flagsContainer.position.z);
        Instantiate(flag, newFlagPos, Quaternion.identity, flagsContainer);
    }

    public void StartGame()
    {
        //Application.targetFrameRate = 30;
        GlobalCam.camWidth = globalCamWidth;
        GlobalCam.camHeigth = globalCamHeigth;
        GlobalCam.useRearCam = useRaerGlobalCam;
        GlobalCam.SetGlobalCam();
        //if (showCamScript != null)
        //    showCamScript.StartShowCam();
        playerScript.cam = GlobalCam.gameCam;
        ResetLevel();
        StartCoroutine(OnGame());
    }

    IEnumerator OnGame()
    {
        yield return new WaitForSeconds(2); //Avoid error with first fotogram
        onGame = true;
    }

    void ResetLevel()
    {
        //gameTime = 10 + GameManager.manager.skiingLevel;
        gameTimer = gameTime;
        playerDistance = 0;
        PlayerPosition.PlayerFrontalSpeed = 3;
        playerScript.distanceCovered = 0;
    }
}
