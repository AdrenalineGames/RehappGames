using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCanvasEnable : MonoBehaviour {

    public GameObject[] childrenInit;

    private void OnEnable()
    {
        for (int i = 0; i < childrenInit.Length; i++)
        {
            childrenInit[i].SetActive(true);
        }
    }
}
