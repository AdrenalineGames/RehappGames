using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contact : MonoBehaviour {


    string email = "sympatheticgames@gmail.com";
    string subject = "Rehapp contact";
    string msgTxt;

    public void SetMsg(string msg)
    {
        msgTxt = msg;
    }

    public void SendMail()
    {
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + msgTxt);
    }
}
