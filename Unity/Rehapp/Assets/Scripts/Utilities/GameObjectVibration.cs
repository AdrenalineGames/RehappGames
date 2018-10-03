using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectVibration : MonoBehaviour {
    public float speedX = 1.0f; //how fast it shakes
    public float amountX = 1.0f; //how much it shakes
    public float speedY = 1.0f; //how fast it shakes
    public float amountY = 1.0f; //how much it shakes
    public float speedZ = 1.0f; //how fast it shakes
    public float amountZ = 1.0f; //how much it shakes
    public bool shake = false;
    public string depencencyBool = "";
    public string depencencyScript = "";
    public GameObject depencencyGo;


    void Update()
    {
        if (depencencyBool != "")
            shake = (bool)depencencyGo.GetComponent(depencencyScript).GetType().GetField(depencencyBool).GetValue(this);
        if (shake)
        {
            transform.position = new Vector3(Mathf.Sin(Time.time * speedX) * amountX, transform.position.y + Mathf.Sin(Time.time * speedY) * amountY,
                transform.position.z + Mathf.Sin(Time.time * speedZ) * amountZ);
        }
    }
}
