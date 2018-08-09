using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidBack : MonoBehaviour {

    public Button backBtn;

    private void OnEnable()
    {
        backBtn = GameObject.FindGameObjectWithTag("BackB").GetComponent<Button>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            backBtn.onClick.Invoke();
        }
    }
}
