using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class DodgeballMechanics : MonoBehaviour {

    private MeshFilter mf;
    private Mesh mesh;

    byte[] backgroundB;
    byte[] camImg;
    public bool playerDetected = false;
    public int globalCamWidth;
    public int globalCamHeigth;
    public bool useRaerGlobalCam;
    public float backgroundTemporizer = 4;
    Texture2D tex;

    //public Text debugTx;
    int[] hullPoints = new int[120];
    int hullLen = 0;
    bool settingBackground = false;
    float settingBackgroundTemporizer;
    WebCamTexture cam;

    public GameObject instructionsPanel;
    public Text instructionsTxt;
    public GameObject startBtn;
    public GameObject pivot;
    public GameObject initPanel;


    // Use this for initialization
    void OnEnable()
    {
        //debugTx.text = OcvMechanics.DllTest().ToString();
        tex = new Texture2D(640, 480, TextureFormat.RGB24, false);
        mesh = new Mesh();
        mf = GetComponent<MeshFilter>();
        mf.sharedMesh = mesh;
        cam = new WebCamTexture();
        settingBackgroundTemporizer = backgroundTemporizer;
    }

    public void StopCam()
    {
        cam.Stop();
    }

    void CreateMesh()
    {
        // Vertices
        List<Vector3> verticesList = new List<Vector3>();
        for (int i = 0; i < hullLen * 2; i += 2)
        {
            float x = hullPoints[i];
            float y = hullPoints[i + 1];
            verticesList.Add(new Vector3(x, y, 0));
        }
        //verticesList.Add(new Vector3(0, 0, 0));
        //verticesList.Add(new Vector3(10, 0, 0));
        //verticesList.Add(new Vector3(10, 10, 0));
        //verticesList.Add(new Vector3(0, 10, 0));

        // Tiangles
        int triNum = 3 * (hullLen - 2);
        int[] tri = new int[triNum];
        int cont = 0;
        for (int i = 0; i < triNum; i += 3)
        {
            tri[i] = 0;
            tri[i + 1] = cont + 1;
            tri[i + 2] = cont + 2;
            cont++;
        }

        // Normals
        //Vector3[] normals = new Vector3[hullLen];
        //for (int i = 0; i < hullLen; i++)
        //    normals[i] = Vector3.forward;

        // Asign arrays
        mesh.Clear();
        mesh.vertices = verticesList.ToArray();
        //mesh.triangles = tri.Reverse().ToArray();
        mesh.triangles = tri;
        //mesh.normals = normals;
        mesh.RecalculateNormals();

        playerDetected = ((mesh.bounds.max.x - mesh.bounds.min.x) < (cam.width - 5));
        if (playerDetected)
            GetComponent<MeshCollider>().sharedMesh = mesh;
        else
            GetComponent<MeshCollider>().sharedMesh = null;
    }

    public Mesh GetBodyMesh()   // This is called from the player shadow object
    {
        return mesh;
    }

    void LateUpdate ()
    {
        if (settingBackground)  //Temporizer for the player to know how much time ha has to stay in the correct side
        {
            settingBackgroundTemporizer -= Time.deltaTime;
            instructionsTxt.text = "Párate aquí " + ((int)settingBackgroundTemporizer).ToString();
        }
        if (cam.isPlaying && backgroundB != null && cam.didUpdateThisFrame)
        {
            tex.SetPixels32(cam.GetPixels32());
            tex.Apply();
            camImg = tex.GetRawTextureData();            
            OcvMechanics.GetBodyTrack(camImg, cam.width, cam.height, backgroundB, hullPoints, out hullLen);

            if (hullLen > 2)
                CreateMesh();
        }
    }


    public void SetBackground()
    {
        StartCoroutine(SetBg());      
    }

    IEnumerator SetBg()
    {
        if (GameManager.manager.tests)
            backgroundTemporizer = 1;
        instructionsPanel.gameObject.SetActive(true);
        SetPlayCam();   // Actives the game cam
        settingBackground = true;
        yield return new WaitForSeconds(backgroundTemporizer);     // Waits until the player is in the correct position
        Texture2D tex = new Texture2D(cam.width, cam.height, TextureFormat.RGB24, false);   // Takes halve picture of the backgroun
        tex.SetPixels(0, 0, cam.width / 2, cam.height, cam.GetPixels(0, 0, cam.width / 2, cam.height));
        yield return new WaitForSeconds(1);
        pivot.transform.localPosition = new Vector3(cam.width / 2, 0,0);    // Moves the canvas to guide the player
        settingBackgroundTemporizer = backgroundTemporizer;         // Restart background capture temporizer
        yield return new WaitForSeconds(backgroundTemporizer);     // Waits until the player is in the correct position
        tex.SetPixels(cam.width / 2, 0, cam.width / 2, cam.height, cam.GetPixels(cam.width / 2, 0, cam.width / 2, cam.height));  // Takes the other halve picture of the backgroun
        tex.Apply();
        backgroundB = tex.GetRawTextureData();
        settingBackground = false;
        yield return new WaitForSeconds(1);
        instructionsPanel.gameObject.SetActive(false);     // Deactive instructios pannel
        DodgeballController.startGame = true;
        initPanel.gameObject.SetActive(false);
    }

    void SetPlayCam()
    {
        GlobalCam.camWidth = globalCamWidth;
        GlobalCam.camHeigth = globalCamHeigth;
        GlobalCam.useRearCam = useRaerGlobalCam;
        GlobalCam.SetGlobalCam();
        cam = GlobalCam.gameCam;
        ShowCam sc = GameObject.Find("Background").GetComponent<ShowCam>();
        if(sc != null)
            sc.StartShowCam();
    }
}
