using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour {
    public void Pause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0f;
        }
        else
            Time.timeScale = 1f;
    }
}
