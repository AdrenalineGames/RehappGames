using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningSystem : MonoBehaviour {

    public Text warningTx;

    bool accept = false;
    bool decision = false;

    public bool Decision
    {
        get
        {
            return decision;
        }

        set
        {
            decision = value;
        }
    }

    public bool Accept
    {
        get
        {
            return accept;
        }

        set
        {
            accept = value;
        }
    }

    public void SetMsg(string msg)
    {
        warningTx.text = msg;
    }

    public void DestroyMsg()
    {
        Destroy(this.gameObject);
    }
}
