using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PwListener : MonoBehaviour
{
    string option;

    public void SetOption(string opt)
    {
        option = opt;
    }

    public void SetPwResult(bool match)
    {
        switch (option)
        {
            case "paciente":
                GameManager.manager.GetComponent<NewPlayer>().SetMahavirPatient(match);
                break;
            case "unlock":
                GameManager.manager.UnlockGames(match);
                break;
            default:
                break;
        }
    }
}
