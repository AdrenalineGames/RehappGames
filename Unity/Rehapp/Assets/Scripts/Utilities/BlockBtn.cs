using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockBtn : MonoBehaviour
{

    public GameObject normalText;
    public GameObject blockedText;

    public string restriction;
    public string unlockCondition;
    public int minimunLvl;

    void Start()
    {
        int restrictionLvl = (int)GameManager.manager.GetType().GetField(restriction).GetValue(GameManager.manager);
        bool unlock = (bool)GameManager.manager.GetType().GetField(unlockCondition).GetValue(GameManager.manager);
        if (restrictionLvl < minimunLvl && !unlock)
        {
            normalText.SetActive(false);
            blockedText.SetActive(true);
            gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            normalText.SetActive(true);
            blockedText.SetActive(false);
            gameObject.GetComponent<Button>().interactable = true;
        }
    }
}
