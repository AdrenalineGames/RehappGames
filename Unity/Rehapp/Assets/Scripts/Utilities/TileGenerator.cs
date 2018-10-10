using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour {

    public Transform tileFront;
    public Transform tileBack;
    public Transform tile;
    public Transform parent;


	void Update ()
    {
        if (tileFront == null)
        {
            tileFront = tileBack;
            Vector3 newTilePos = new Vector3(0, -1.3f, 41);
            tileBack = Instantiate(tile, newTilePos, Quaternion.Euler(new Vector3(92.15198f, 89.99999f, 89.99999f)), parent);
        }
	}
}
