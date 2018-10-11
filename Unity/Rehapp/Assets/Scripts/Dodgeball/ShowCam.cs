using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCam : MonoBehaviour {

    WebCamTexture cam;

    public void StartShowCam()
    {
        var renderer = GetComponent<Renderer>();
        renderer.material.SetFloat("_Mode", 3);
        renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        renderer.material.SetInt("_ZWrite", 0);
        renderer.material.DisableKeyword("_ALPHATEST_ON");
        renderer.material.EnableKeyword("_ALPHABLEND_ON");
        renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        renderer.material.renderQueue = 3000;
        cam = GlobalCam.gameCam;
        renderer.material.mainTexture = cam;
        gameObject.transform.localScale = new Vector3(-(float)GlobalCam.gameCam.width, (float)GlobalCam.gameCam.height, 1);
        gameObject.transform.position = new Vector3(GlobalCam.gameCam.width / 2, GlobalCam.gameCam.height / 2, transform.position.z);
    }

    //private void Update()
    //{
        //if (GlobalCam.gameCam != null)
        //    if (!GlobalCam.gameCam.isPlaying)
        //        cam.Stop();
        //    Texture2D tex = new Texture2D(640, 480, TextureFormat.BGRA32, false);
        //    tex.SetPixels32(CameraTexture.GetPixels32());
        //    tex.Apply();
        //    GetComponent<Renderer>().material.mainTexture = tex;
    //}

    //public void StopCamRecord()
    //{
    //    CameraTexture.Stop();
    //}
}
