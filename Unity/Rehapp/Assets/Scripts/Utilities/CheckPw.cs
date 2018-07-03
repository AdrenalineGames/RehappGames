using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPw : MonoBehaviour
{
    public GameObject wrongPw;
    public GameObject correctPw;

    public void CheckUnlockPw()
    {
        if (GameManager.manager.unlockGames)
            correctPw.SetActive(true);
        else
            wrongPw.SetActive(true);
    }
}
