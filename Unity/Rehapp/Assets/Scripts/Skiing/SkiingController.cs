using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkiingController : MonoBehaviour {

    public Transform flag;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NewFlag()
    {
        Instantiate(flag, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
