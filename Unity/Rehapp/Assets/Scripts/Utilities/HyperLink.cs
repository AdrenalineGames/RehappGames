using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HyperLink : MonoBehaviour {

    public string url;

    public Text textTx;

    public void OpenUrl()
    {
        System.Diagnostics.Process.Start(url);
    }

    public void SetText(string tx)
    {
        textTx.text = tx;
    }
}
