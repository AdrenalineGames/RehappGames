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
        if (GameManager.manager.skiingLevel > 5)
        {
            correctPw.SetActive(true);
            EnableUnlockButton();
        }
        else
            wrongPw.SetActive(true);
    }

    public void EnableUnlockButton()
    {
        if (GameManager.manager.skiingLevel > 5)
            unlockButton.interactable = false;
        else
            unlockButton.interactable = true;
    }
}
