using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour {
    // Update is called once per frame
    public Renderer parentRend;

    private void OnEnable()
    {
        Color parentColor = parentRend.material.color;
        var renderer = GetComponent<Renderer>();
        renderer.material.SetFloat("_Mode", 3);
        renderer.material.SetColor("_Color", new Color(parentColor.r, parentColor.g, parentColor.b, parentColor.a/2));
    }

    void Update () {
        gameObject.transform.position = new Vector3(gameObject.transform.parent.transform.position.x, gameObject.transform.parent.transform.position.y,
            GameObject.FindGameObjectWithTag("Player").transform.position.z);
	}
}
