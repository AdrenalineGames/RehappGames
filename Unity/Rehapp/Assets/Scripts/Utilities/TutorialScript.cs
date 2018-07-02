using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour {

    public GameObject tutorialPanel;
    public GameObject[] tutos;

    int tutoNum;

    // Use this for initialization
    public void Init ()
    {
        tutorialPanel.SetActive(true);
        GUI.BringWindowToFront(tutorialPanel.GetInstanceID());
        tutoNum = 0;
        tutos[tutoNum].SetActive(true);
	}

    public void EndTutorial()
    {
        tutos[tutoNum].SetActive(false);
        tutorialPanel.SetActive(false);
    }

    public void NextTuto()
    {
        tutos[tutoNum].SetActive(false);
        tutoNum++;
        if (tutoNum == tutos.Length)
            EndTutorial();
        else
            tutos[tutoNum].SetActive(true);
    }
}
