﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;
using System.Globalization;

public class PlayerPosition : MonoBehaviour
{
    public float speedUp;
    public float posAmplifier;
    public float matchDivider = 10000000;   //640x480 = 10000000, 160x120 = 500000
    public float targetLimit = 0.6f;    //640x480 = 4, 160x120 = 0.6
    public Text debugTx;
    public SkiingController SK;
    public Animator anim;
    public bool testing;
    public float testingSpeed;

    public static float PlayerFrontalSpeed = 0;
    public float distanceCovered = 0;

    Vector3 acc;
    Vector3 mean;
    float prevMeanX = 0;
    float[] accDataX = { 0, 0, 0, 0, 0, 0};
    float[] accDataY = { 0, 0, 0, 0, 0, 0};
    float[] accDataZ = { 0, 0, 0, 0, 0, 0};
    float[] linearSpeed = { 0, 0, 0, 0, 0 };
    //float linearSpeed = 0;
    int init = 0;
    float lastPos = 0;
    float newPos = 0;
    float moved = 0;
    int targetLostCount = 0;
    int newTargetCount = 0;
    float targetLostTimer = 0;

    public WebCamTexture cam;   // assigned in skiing controller after global cam is setted
    Texture2D tex;
    Texture2D targetTex;

    byte[] camImg;
    byte[] targetImg;
    int targetCenterX;
    int targetCenterY;
    bool lostTarget = true;
    bool thereIsTarget = false;
    double matchVal;
    int matchPosX = 0;
    int matchPosY = 0;
    int camWidth;
    int camHeight;
    int tempCont = 0;
    //float timer;
    //float pTimer = 0;

    //private void OnEnable()
    //{
    //    Debug.Log(Application.persistentDataPath);
    //}

    private void LateUpdate()   // Late update to let the cam update
    {
        //timer += Time.deltaTime;
        SetPlayerPose();

        if (SkiingController.onGame)
        {
            acc.x = Input.acceleration.x;
            acc.y = Input.acceleration.y;
            acc.z = Input.acceleration.z;

            if (acc.x - prevMeanX > 0.3f)   // To avoid increment speed from yawing
                TargetLost();
            prevMeanX = acc.x;

            targetLostTimer += Time.deltaTime;  // For search new target
            distanceCovered += PlayerFrontalSpeed * Time.deltaTime;
            GetAccArray();
            GetOffset();
            if (init < 6)   // Waits until the accArray is filled
                init++;
            else
            {
                PlayerHorizontalPos();
            }

            if (lostTarget)     // Detects and cut the new target
            {
                camWidth = cam.width;
                camHeight = cam.height;
                NewTarget();
                //debugTx.text = "New!";
            }
            if (thereIsTarget && cam.didUpdateThisFrame && Time.deltaTime != 0)     // Match the target and calculate speed
            {
                //tempCont++;
                //Debug.Log(tempCont);
                //debugTx.text = (1/(timer-pTimer)).ToString()+" fps";
                //pTimer = timer;
                TargetMatch();

                DetectLostTarget();

                SetPlayerSpeed();
                //Debug.Log("Distance: " + distanceCovered);
            }
            if (testing)
                PlayerFrontalSpeed = testingSpeed;
        }
    }

    private void SetPlayerPose()
    {
        anim.SetFloat("position", Math.Abs(transform.position.x / 5.1f));
        transform.localScale = new Vector3((transform.position.x >= 0 ? 1 : -1) * Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        anim.SetFloat("speed", PlayerFrontalSpeed);
    }

    public void PlayerSpeedMultiplier(float mD)
    {
        speedUp = mD;
    }

    public void SetMatchLimit(float ml)
    {
        targetLimit = ml;   // Minimum match to set a target as lost
    }

    private void SetPlayerSpeed()
    {
        //Debug.Log("Match: " + matchVal);
        newPos = (float)(matchVal);
        moved = newPos - lastPos;
        lastPos = newPos;
        Array.Copy(linearSpeed, 0, linearSpeed, 1, linearSpeed.Length - 1);
        linearSpeed[0] = Math.Abs(moved/Time.deltaTime);
        //linearSpeed = Math.Abs(moved/Time.deltaTime);

        if (linearSpeed[0] < 0.1)// || linearSpeed[0] > 0.4)
            PlayerFrontalSpeed = 0.1f;
        else
            PlayerFrontalSpeed = linearSpeed.Average() * speedUp;

        //Debug.Log("Speed: " + PlayerFrontalSpeed);
        //debugTx.text = PlayerFrontalSpeed.ToString();
    }

    void TargetLost()
    {
        lostTarget = true;
        thereIsTarget = false;
    }

    private void DetectLostTarget()     // A target is los if the match is below the limito or if has passed 4 seconds
    {
        if (targetLostTimer > 2)
        {
            targetLostTimer = 0;
            targetLostCount = 0;
            newTargetCount++;
            if (newTargetCount == 2)
            {
                //Debug.Log("New target!");
                newTargetCount = 0;
                TargetLost();
            }
        }
        //Debug.Log("NewPos" + newPos);
        if (newPos < targetLimit)
        {
            targetLostCount++;
            if (targetLostCount > 15)
            {
                tempCont++;
                newTargetCount = 0;
                targetLostCount = 0;
                //Debug.Log("Lost!");
                debugTx.text = tempCont.ToString();
                TargetLost();
            }
        }
    }

    private void TargetMatch()
    {
        TakePic();

        targetImg = targetTex.GetRawTextureData();

        if(SkiingController.onGame)
            OcvMechanics.MatchTemplateImg(camImg, camWidth, camHeight, targetImg, targetTex.width, targetTex.height, out matchVal, out matchPosX, out matchPosY);
    }

    private void NewTarget()
    {
        TakePic();
        //byte[] bytes = tex.EncodeToPNG();
        //File.WriteAllBytes(Application.persistentDataPath + "/frame.png", bytes);

        if (SkiingController.onGame)
            OcvMechanics.GetTarget(camImg, camWidth, camHeight, out targetCenterX, out targetCenterY);

        CutTarget();
        lostTarget = false;
        thereIsTarget = true;

        //transform.position = new Vector3(Math.Abs(targetCenterX - cam.width), targetCenterY, 0);
    }

    private void TakePic()
    {
        tex = new Texture2D(camWidth, camHeight, TextureFormat.RGB24, false);
        tex.SetPixels32(cam.GetPixels32());
        tex.Apply();
        camImg = tex.GetRawTextureData();
    }

    private void PlayerHorizontalPos()
    {
        float horizontalPos = mean.x * posAmplifier;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        horizontalPos = transform.position.x + Input.GetAxis("Horizontal") * 0.1f;
#endif
        if (horizontalPos > 5) horizontalPos = 5;
        if (horizontalPos < -5) horizontalPos = -5;
        transform.position = new Vector3(horizontalPos, transform.position.y, transform.position.z);
    }

    private void CutTarget()    // Cuts the target avoiding overflow
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

        //byte[] bytes = targetTex.EncodeToPNG();
        //File.WriteAllBytes(Application.persistentDataPath + "/Target.png", bytes);
    }

    private void GetOffset()
    {
        mean.x = accDataX.Average();
        mean.y = accDataY.Average();
        mean.z = accDataZ.Average();
        //Debug.Log("x: " + mean.x + "y: " + mean.y + "z: " + mean.z);
    }

    void GetAccArray()
    {
        Array.Copy(accDataX, 0, accDataX, 1, accDataX.Length - 1);
        accDataX[0] = acc.x;
        Array.Copy(accDataY, 0, accDataY, 1, accDataY.Length - 1);
        accDataY[0] = acc.y;
        Array.Copy(accDataZ, 0, accDataZ, 1, accDataZ.Length - 1);
        accDataZ[0] = acc.z;
        //Debug.Log("G: " + (acc.z - accMiddle));
        //Debug.Log(string.Join(" ", accDataZ.Select(x => x.ToString()).ToArray()));
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Yay!");
        if(other.tag == "Ball")
            SK.PlayerScore(1);
    }
}