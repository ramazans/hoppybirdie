using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.SceneManagement;

public class InterAdsController : MonoBehaviour {

    private InterstitialAd interstitial;
    static InterAdsController interAdsController;
    int removeAds = 0;

    void Start () {
        removeAds = PlayerPrefs.GetInt("RemoveAds");
        if (removeAds != 1)
        {
            if (interAdsController == null)
            {
                DontDestroyOnLoad(gameObject);
                interAdsController = this;
                Debug.Log("Interstitial controller oluşturuldu");

                SetApplicationIds();
                RequestInterstitial();

                interstitial.OnAdClosed += HandleOnAdClosed;
            }
            else
            {
                Destroy(gameObject);
                Debug.Log("null check Destroy");
            }
        }
    }

    private void HandleOnAdClosed(object sender, EventArgs e)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowAds()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
            Debug.Log("Interstitial reklamı gösterildi");
        }
        interAdsController = null;
        Destroy(gameObject);
    }

    //Set Application Id
    private void SetApplicationIds()
    {
        #if UNITY_ANDROID
            string appId = "ca-app-pub-1741891681659976~7822695685";
        #elif UNITY_IPHONE
            string appId = "ca-app-pub-1741891681659976~3349098203";
        #else
            string appId = "unexpected_platform";
        #endif

        MobileAds.Initialize(appId);
    }

    private void RequestInterstitial()
    {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-1741891681659976/1598577428";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-1741891681659976/8082550132";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
    }

    private void RequestInterstitialTest()
    {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();
        interstitial.LoadAd(request);
    }
}
