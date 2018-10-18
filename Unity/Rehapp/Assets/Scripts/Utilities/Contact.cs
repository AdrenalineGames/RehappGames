using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Contact : MonoBehaviour {


    string techEmail = "develop.info.de@gmail.com";
    string mahavirEmail = "Jesusplata@mahavir-kmina.org";
    string subject = "Contacto App Juegos Interactivos";
    string msgTxt;
    int contactOption;

    public InputField message;
    public Button sendButton;

    public void EnableInputField(int d)
    {
        contactOption = d;
        if (d > 0)
        {
            message.interactable = true;
            sendButton.interactable = true;
        }
        else
        {
            message.interactable = false;
            sendButton.interactable = false;
        }
    }

    public void SetMsg(string msg)
    {
        msgTxt = msg;
    }

    public void SendMail()
    {
        if (contactOption == 1)
        {
            Application.OpenURL("mailto:" + mahavirEmail + "?subject=" + subject + "&body=" + msgTxt);
        }else if(contactOption == 2)
        {
            Application.OpenURL("mailto:" + techEmail + "?subject=" + subject + "&body=" + msgTxt);
        }
    }
}
