using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCam : MonoBehaviour {

    public static WebCamTexture gameCam;
    static public int camWidth = 640;
    static public int camHeigth = 480;
    static public bool useRearCam = false;

    public static void SetGlobalCam () {
        var devices = WebCamTexture.devices;
        var cam = "";
        if (devices.Length > 0) cam = devices[0].name;
        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing && !useRearCam)
            {
                cam = devices[i].name;
            }
            if (!devices[i].isFrontFacing && useRearCam)
            {
                cam = devices[i].name;
            }
        }
        gameCam = new WebCamTexture(cam, camWidth, camHeigth, 30);
        gameCam.requestedWidth = camWidth;
        gameCam.Play();
    }

    public void StopCam()
    {
        gameCam.Stop();
        Debug.Log("Global cam stopped");
    }
}
