using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPosition : MonoBehaviour {

    private void Update()
    {
        Position();
    }

    private void Position()
    {
        transform.position -= new Vector3(0, 0, PlayerPosition.PlayerFrontalSpeed * Time.deltaTime);
        if (transform.position.z < 0)
            Destroy(gameObject);
    }
}
