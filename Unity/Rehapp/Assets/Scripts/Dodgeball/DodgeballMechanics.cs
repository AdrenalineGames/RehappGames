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
    public float moveSpeed;
    public bool playerDetected = false;
    Texture2D tex;

    public Text debugTx;
    int[] hullPoints = new int[120];
    int hullLen = 0;
    WebCamTexture cam;

    public GameObject instructionsTx;
    public GameObject startBtn;
    public GameObject pivot;
    public GameObject initPanel;


    // Use this for initialization
    void OnEnable()
    {
        debugTx.text = OcvMechanics.DllTest().ToString();
        tex = new Texture2D(640, 480, TextureFormat.RGB24, false);
        mesh = new Mesh();
        mf = GetComponent<MeshFilter>();
        mf.sharedMesh = mesh;
    }

    private void Start()
    {
        cam = GlobalCam.gameCam;
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

    public Mesh GetBodyMesh()
    {
        return mesh;
    }

    void Update () {
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
        instructionsTx.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        Texture2D tex = new Texture2D(cam.width, cam.height, TextureFormat.RGB24, false);
        tex.SetPixels(0, 0, cam.width / 2, cam.height, cam.GetPixels(0, 0, cam.width / 2, cam.height));
        yield return new WaitForSeconds(1);
        pivot.transform.localPosition = new Vector3(cam.width / 2, 0,0);
        yield return new WaitForSeconds(3);
        tex.SetPixels(cam.width / 2, 0, cam.width / 2, cam.height, cam.GetPixels(cam.width / 2, 0, cam.width / 2, cam.height));
        tex.Apply();
        backgroundB = tex.GetRawTextureData();
        yield return new WaitForSeconds(1);
        instructionsTx.gameObject.SetActive(false);
        GameController.startGame = true;
        initPanel.gameObject.SetActive(false);
    }
}
