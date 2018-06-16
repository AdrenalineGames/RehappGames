using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentMesh : MonoBehaviour {
    public DodgeballMechanics dM;

	void Update () {
        if (dM.playerDetected)
            GetComponent<MeshFilter>().sharedMesh = dM.GetBodyMesh();
        else
            GetComponent<MeshFilter>().sharedMesh = null;

    }
}
