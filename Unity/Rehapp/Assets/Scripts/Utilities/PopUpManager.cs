using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour {

    public Transform popUp;

    private void Update()
    {
        if (GameManager.manager.advertising)
        {
            popUp.gameObject.SetActive(true);
            GameManager.manager.advertising = false;
        }
    }

    public void DisablePopUp()
    {
        popUp.gameObject.SetActive(false);
    }
}
