using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultadTxt : MonoBehaviour {

    public void ShowDifficult(float d)
    {
        GetComponent<Text>().text = "Selecciona la dificultad: " + d;
    }
}
