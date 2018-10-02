using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Password : MonoBehaviour {

    string correctPw;
    string inputPw;
    string option = "";
    PwListener caller;
    public GameObject wrongPwPop;
    public GameObject correctPwPop;

    public void SetCaller(PwListener cl)
    {
        caller = cl;
    }

    public void SetCorrectPw(string pw)
    {
        correctPw = pw;
    }

    public void SetInputPw(string pw)
    {
        inputPw = pw;
    }

    public void SetOption(string opt)
    {
        option = opt;
    }

    public void CheckPw()
    {
        bool match = false;
        if (correctPw == inputPw)
            match = true;
        correctPwPop.SetActive(match);
        wrongPwPop.SetActive(!match);
        if (option != "")
            caller.SetOption(option);
        caller.SetPwResult(match);
    }
}
