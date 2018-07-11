using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPw : MonoBehaviour
{
    public GameObject wrongPw;
    public GameObject correctPw;
    public Button unlockButton;

    public void CheckUnlockPw()
    {
        if (GameManager.manager.unlockGames)
        {
            correctPw.SetActive(true);
            EnableUnlockButton();
        }
        else
            wrongPw.SetActive(true);
    }

    public void EnableUnlockButton()
    {
        if (GameManager.manager.unlockGames)
            unlockButton.interactable = false;
        else
            unlockButton.interactable = true;
    }
}
