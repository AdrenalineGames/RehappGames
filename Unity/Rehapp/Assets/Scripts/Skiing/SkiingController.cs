using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkiingController : MonoBehaviour {

    public Transform flag;
    public Transform flagsContainer;

    float newFlag;

	

	void Update () {
        newFlag += Time.deltaTime;
        if (newFlag > 2)
        {
            newFlag = 0;
            NewFlag();
        }
    }

    public void NewFlag()
    {
        Vector3 newFlagPos = new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), flagsContainer.position.y, flagsContainer.position.z);
        Instantiate(flag, newFlagPos, Quaternion.identity, flagsContainer);
        PlayerPosition.PlayerFrontalSpeed = 10;
    }
}
