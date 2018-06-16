using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPos : MonoBehaviour {
	// Update is called once per frame
	void Update () {
        gameObject.transform.position = new Vector3(gameObject.transform.parent.transform.position.x, gameObject.transform.parent.transform.position.y,
            GameObject.FindGameObjectWithTag("Player").transform.position.z);
	}
}
