using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class PopUpManager : MonoBehaviour {

    public Transform popUp;
    public int surveyWeight = 0;

    private RewardBasedVideoAd rewardVideo;

    public void Start()
    {
        rewardVideo = RewardBasedVideoAd.Instance;
        RequestAdvertasing();
    }

    private void Update()
    {
        if (GameManager.manager.advertising)
        {
            //popUp.gameObject.SetActive(true);
            GameManager.manager.advertising = false;
            ChooseShow();
        }
    }

    private void ChooseShow()
    {
        if (GameManager.manager.mahavirPatient)
        {
            if ((UnityEngine.Random.Range(0,100) < surveyWeight ? true : false))
                ShowSurvey();
            else
                ShowAds();
        }
        else
            ShowAds();
    }

    private void ShowSurvey()
    {
        throw new NotImplementedException();
    }

    public void RequestAdvertasing()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-9188981930160944/6016335249";
#else
        string adUnitId = "Unespected platform";
#endif

        rewardVideo.LoadAd(new AdRequest.Builder().Build(), adUnitId);
    }

    public void ShowAds()
    {
        if (rewardVideo.IsLoaded())
        {
            rewardVideo.Show();
        }
        else
            Debug.Log("Not loaded");

    }

    public void DisablePopUp()
    {
        popUp.gameObject.SetActive(false);
    }
}
