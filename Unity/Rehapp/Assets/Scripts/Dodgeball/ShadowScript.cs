using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour {
    // Update is called once per frame

    private void OnEnable()
    {
        Color parentColor = GetComponentInParent<Renderer>().material.color;
        var renderer = GetComponent<Renderer>();
        renderer.material.SetFloat("_Mode", 3);
        renderer.material.color = new Color(parentColor.r, parentColor.g, parentColor.b, parentColor.a/2);
    }

    void Update () {
        gameObject.transform.position = new Vector3(gameObject.transform.parent.transform.position.x, gameObject.transform.parent.transform.position.y,
            GameObject.FindGameObjectWithTag("Player").transform.position.z);
	}
}
