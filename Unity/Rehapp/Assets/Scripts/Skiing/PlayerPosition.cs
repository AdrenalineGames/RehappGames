using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Globalization;

public class PlayerPosition : MonoBehaviour
{

    public float ang;
    public float ang2;
    public float speedUp;
    public Rigidbody rigidbody;

    Vector3 speed = Vector3.zero;
    Vector3 prevSpeed = Vector3.zero;
    Vector3 acc;
    Vector3 gravityAngle;
    Vector3 mean;
    Vector3 pionting;
    float[] accDataX = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    float[] accDataY = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    float[] accDataZ = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    float[] filteredX = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    float[] filteredY = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    float[] filteredZ = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    int init = 0;
    float accMiddle = 0;
    float deltaT;
    float linearSpeed = 0;
    bool accAdjusted = false;
    WebCamTexture cam;
    Texture2D tex;
    Texture2D targetTex;
    byte[] camImg;
    byte[] targetImg;
    int targetCenterX;
    int targetCenterY;
    bool lostTarget = false;
    bool thereIsTarget = false;
    double matchVal;
    int matchPosX;
    int matchPosY;

    private void OnEnable()
    {
        cam = GlobalCam.gameCam;
        tex = new Texture2D(640, 480, TextureFormat.RGB24, false);
    }

    private void FixedUpdate()
    {
        acc.x = Input.acceleration.x;
        acc.y = Input.acceleration.y;
        acc.z = Input.acceleration.z;

        deltaT = Time.deltaTime;


        if (accAdjusted)
        {
            GetAccArray();
            GetOffset();
            GetFilteredAcc();
            PlayerSpeed();
            //GetAngles();
            UpdateCamRot();
            if (init < 10)
                init++;
            else
            {
                //UpdateCamPos();
            }
        }

        if (lostTarget)
        {
            lostTarget = false;

            tex.SetPixels32(cam.GetPixels32());
            tex.Apply();
            camImg = tex.GetRawTextureData();

            OcvMechanics.GetTarget(camImg, cam.width, cam.height, out targetCenterX, out targetCenterY);

            CutTarget();
            thereIsTarget = true;

            transform.position = new Vector3(Math.Abs(targetCenterX - cam.width), targetCenterY, 0);
        }
        if (thereIsTarget)
        {
            tex.SetPixels32(cam.GetPixels32());
            tex.Apply();
            camImg = tex.GetRawTextureData();

            targetImg = targetTex.GetRawTextureData();

            OcvMechanics.MatchTemplateImg(camImg, cam.width, cam.height, targetImg, targetTex.width, targetTex.height, out matchVal, out matchPosX, out matchPosY);

            Debug.Log("Match: " + matchVal);

            transform.position = new Vector3(Math.Abs(matchPosX - cam.width), matchPosY, transform.position.z);
            linearSpeed =  Math.Abs((float)(matchVal / 1000000000)-linearSpeed);
            rigidbody.velocity = Vector3.forward * linearSpeed * speedUp / deltaT;
            Debug.Log("vel: " + rigidbody.velocity.z);
        }

        //Debug.Log("Ang x: " + gravityAngle.x + "Ang y: " + gravityAngle.y + "Ang z: " + gravityAngle.z);
        //Debug.Log("Magnitud: " + Input.acceleration.magnitude);
    }

    private void CutTarget()
    {
        float minX;
        float maxX;
        float minY;
        float maxY;
        float windowSize = 10;
        if (targetCenterX < (cam.width / windowSize))
        {
            minX = 0;
            maxX = 2 * cam.width / windowSize;
        }
        else
        {
            if (targetCenterX > (cam.width - (cam.width / windowSize)))
            {
                maxX = cam.width;
                minX = cam.width - 2 * cam.width / windowSize;
            }
            else
            {
                maxX = targetCenterX + cam.width / windowSize;
                minX = targetCenterX - (cam.width / windowSize);
            }
        }
        if (targetCenterY < (cam.height / windowSize))
        {
            minY = 0;
            maxY = 2 * cam.height / windowSize;
        }
        else
        {
            if (targetCenterY > (cam.height - (cam.height / windowSize)))
            {
                maxY = cam.height;
                minY = cam.height - 2 * cam.height / windowSize;
            }
            else
            {
                maxY = targetCenterY + cam.height / windowSize;
                minY = targetCenterY - (cam.height / windowSize);
            }
        }

        targetTex = new Texture2D((int)(maxX - minX), (int)(maxY - minY), TextureFormat.RGB24, false);
        targetTex.SetPixels(0, 0, targetTex.width, targetTex.height, cam.GetPixels((int)minX, (int)minY, targetTex.width, targetTex.height));
        targetTex.Apply();

        byte[] bytes = targetTex.EncodeToPNG();
        File.WriteAllBytes(Application.persistentDataPath + "/Target.png", bytes);
    }

    private void UpdateCamPos()
    {
        //float filteredAccZ = -((int)(filteredZ.First() * 100)) / 100;
        //float filteredAccZ = -filteredZ.First();
        rigidbody.velocity = Vector3.forward * speed.x * speedUp;
        //rigidbody.AddForce(new Vector3(0, 0, speed.x/deltaT), ForceMode.Acceleration);
        //Debug.Log("acc: " + -filteredZ.First());
        Debug.Log("vel: " + rigidbody.velocity.z);


    }

    private void PlayerSpeed()
    {
        speed.x = speed.x + filteredX[0] * deltaT;
        speed.y = speed.y + filteredY[0] * deltaT;
        speed.z = speed.z + filteredZ[0] * deltaT;
        //Debug.Log("acc: " + filteredAccZ);
    }

    private void GetFilteredAcc()
    {
        for (int i = 0; i < filteredX.Length; i++)
            filteredX[i] = accDataX[i] - mean.x;
        for (int i = 0; i < filteredY.Length; i++)
            filteredY[i] = accDataY[i] - mean.y;
        for (int i = 0; i < filteredZ.Length; i++)
            filteredZ[i] = accDataZ[i] - mean.z;
        //Debug.Log(string.Join(" ", filteredZ.Select(x => x.ToString()).ToArray()));
    }

    private void GetOffset()
    {
        mean.x = accDataX.Average();
        mean.y = accDataY.Average();
        mean.z = accDataZ.Average();
        //Debug.Log("x: " + mean.x + "y: " + mean.y + "z: " + mean.z);
    }

    private void UpdateCamRot()
    {
        //transform.eulerAngles = new Vector3(0, 0, gravityAngle.z);
        //transform.eulerAngles = new Vector3(gravityAngle.x,0, gravityAngle.y);
        //transform.eulerAngles = new Vector3(gravityAngle.x, ang2, ang);
        //transform.eulerAngles = gravityAngle;

        Vector3 pointing = new Vector3(0, mean.z, -mean.y);
        Vector3 relativePos = pointing - transform.position;
        Quaternion verticalRot = Quaternion.LookRotation(relativePos);
        transform.rotation = verticalRot;
        Debug.Log("Y: " + mean.z + "Z" + -mean.y);

        //cameraRotationAccelerometer();
    }

    private void GetAngles()
    {
        Vector3 ijk = Vector3.zero;

        if (mean.y < 0) ijk.x += 180;
        if (mean.z < 0) ijk.y += 180;
        if (mean.x < 0) ijk.z += 180;

        gravityAngle.x = (float)((Math.Atan(mean.z / mean.y)) * 180 / Math.PI) + ijk.x - 180;
        gravityAngle.y = (float)((Math.Atan(mean.x / mean.z)) * 180 / Math.PI) + ijk.y;
        gravityAngle.z = (float)((Math.Atan(mean.y / mean.x)) * 180 / Math.PI) + ijk.z;
    }

    void GetAccArray()
    {
        Array.Copy(accDataX, 0, accDataX, 1, accDataX.Length - 1);
        accDataX[0] = acc.x - accMiddle;
        Array.Copy(accDataY, 0, accDataY, 1, accDataY.Length - 1);
        accDataY[0] = acc.y - accMiddle;
        Array.Copy(accDataZ, 0, accDataZ, 1, accDataZ.Length - 1);
        accDataZ[0] = acc.z - accMiddle;
        //Debug.Log("G: " + (acc.z - accMiddle));
        //Debug.Log(string.Join(" ", accDataZ.Select(x => x.ToString()).ToArray()));
    }

    public void StartAccAdjust()
    {
        //StartCoroutine(AdjustAccelerometer());
        //accAdjusted = true;
        lostTarget = true;
        thereIsTarget = false;
    }

    private IEnumerator AdjustAccelerometer()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("G: " + (-acc.magnitude));
        float minAcc = -acc.magnitude;
        Debug.Log("turn it");
        yield return new WaitForSeconds(3);
        Debug.Log("G: " + (acc.magnitude));
        float maxAcc = acc.magnitude;
        accMiddle = (maxAcc + minAcc) / 2;
        accAdjusted = true;
        Debug.Log("Middle: " + accMiddle);
    }
}