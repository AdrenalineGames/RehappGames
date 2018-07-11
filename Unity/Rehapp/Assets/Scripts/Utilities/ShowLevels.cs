using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class ShowLevels : MonoBehaviour {

    public string[] gameName;
    public string[] showName;

    private void OnEnable()
    {
        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
        Text text = gameObject.GetComponent<Text>();
        text.text = "Niveles por juego:";
        for (int i = 0; i < gameName.Length; i++)
        {
            int level = (int)GameManager.manager.GetType().GetField(gameName[i]).GetValue(GameManager.manager);
            text.text += System.Environment.NewLine + ti.ToTitleCase(showName[i]) + ": " + level;
        }
    }
}
