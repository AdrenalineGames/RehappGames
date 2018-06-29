using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentMesh : MonoBehaviour {
    public DodgeballMechanics dM;

	void Update () {
        if (dM.playerDetected)
            GetComponent<MeshFilter>().sharedMesh = dM.GetBodyMesh();   // This mesh shows the player position, is in another GO because this must face the other side of the players collider
        else
            GetComponent<MeshFilter>().sharedMesh = null;

    }
}
