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
    public GameObject pauseBtn;
    public Text scoreTx;
    public TutorialScript tutoScript;

    public float gameTime = 10;
    public int distanceBetweenFlags;
    public int globalCamWidth;
    public int globalCamHeigth;
    public bool useRaerGlobalCam;
    public float playerLevel;

    public static bool onGame = false;

    float gameTimer;
    float sessionScore;
    float playerDistance;
    char[] ratings = { 'E', 'D', 'C', 'B', 'A', 'S' };


    private void Start()
    {
        if (GameManager.manager.skiingLevel == 0)
            StartTuto();
    }

    public void StartTuto()
    {
        tutoScript.Init();
        GameManager.manager.skiingLevel = 1;
        GameManager.manager.Save();
    }

    private void SetDifficult()
    {
        gameTime = 10+playerLevel;
        //Debug.Log(playerLevel + " " + gameTime);
    }

    void Update () {
        if (onGame)
        {
            gameTimer -= Time.deltaTime;    // For the chrono
            if (gameTimer >= 0)
            {
                timerText.text = ((int)gameTimer).ToString();   // Game chrono
                if ((int)(playerScript.distanceCovered) == playerDistance)      // How often appear flags
                {
                    playerDistance = (int)playerScript.distanceCovered + distanceBetweenFlags;
                    NewFlag();
                }
            }
            else
            {
                if (GameObject.FindGameObjectWithTag("Ball") == null)   // Waits until there is no more flags
                {
                    onGame = false;
                    RateSession();
                }
            }
        }
    }

    private void RateSession()
    {
        float finalScore = (float)Math.Floor(((6 / ((gameTime/distanceBetweenFlags)-(playerLevel/10))) * sessionScore));    // playerLevel/10 = number of flags that can let pass and still score S
        if (finalScore > 5) finalScore = 5;
        float sessionSpeed = playerDistance / gameTime;
        if (finalScore >= GameManager.manager.skiingLevel)
            updateLevel((int)finalScore, sessionSpeed);
        //Debug.Log("Obtuviste una: " + ratings[(int)finalScore]);
        pauseBtn.transform.GetChild(0).gameObject.SetActive(true);
        pauseBtn.GetComponentInChildren<Text>().text = "Obtuviste una: " + ratings[(int)finalScore];
    }

    private void updateLevel(int ss, float speed)
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
        GameManager.manager.SetSkiingLvl(GameManager.manager.skiingLevel += modifyLevel, speed);
    }

    public void PlayerScore(int point)
    {
        sessionScore += point;      // This function is called from the player when touch a flag
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreTx.text = sessionScore.ToString();
    }

    void NewFlag()  // Instanciate a new flag in random position
    {
        Vector3 newFlagPos = new Vector3(UnityEngine.Random.Range(-5.0f, 5.0f), flagsContainer.position.y, flagsContainer.position.z);
        Instantiate(flag, newFlagPos, Quaternion.identity, flagsContainer);
    }

    public void StartGame()
    {
        Application.targetFrameRate = 60;
        GlobalCam.camWidth = globalCamWidth;    // Sets the game camera
        GlobalCam.camHeigth = globalCamHeigth;
        GlobalCam.useRearCam = useRaerGlobalCam;    // Use raer cam in this game
        GlobalCam.SetGlobalCam();
        //if (showCamScript != null)
        //    showCamScript.StartShowCam();
        playerScript.cam = GlobalCam.gameCam;
        ResetLevel();       // Resets game variables
        StartCoroutine(OnGame());
    }

    IEnumerator OnGame()
    {
        yield return new WaitForSeconds(1); // Waits one second to begin the game
        onGame = true;
    }

    void ResetLevel()
    {
        //gameTime = 10 + GameManager.manager.skiingLevel;
        playerLevel = GameManager.manager.skiingLevel;
        SetDifficult();
        gameTimer = gameTime;
        playerDistance = 0;
        sessionScore = 0;
        PlayerPosition.PlayerFrontalSpeed = distanceBetweenFlags;
        playerScript.distanceCovered = 0;
    }
}
