using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCam : MonoBehaviour {

    public static WebCamTexture gameCam;

	void Awake () {
        var devices = WebCamTexture.devices;
        var frontCam = "";
        if (devices.Length > 0) frontCam = devices[0].name;
        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing)
            {
                frontCam = devices[i].name;
            }
        }
        gameCam = new WebCamTexture(frontCam, 640, 480, 30);
        gameCam.Play();
    }

    public static Vector2 CamDimensions()
    {
        Vector2 w_h = new Vector2();
        w_h.x = gameCam.width;
        w_h.y = gameCam.height;
        return w_h;
    }

    public static void StopCam()
    {
        gameCam.Stop();
    }
}
