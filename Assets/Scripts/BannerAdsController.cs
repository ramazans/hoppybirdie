using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class BannerAdsController : MonoBehaviour {

    public BannerView bannerView;
    static BannerAdsController bannerAdsController;
    int removeAds = 0;

    public void Start () {
        removeAds = PlayerPrefs.GetInt("RemoveAds");
        if (removeAds != 1)
        {
            if (bannerAdsController == null)
            {
                DontDestroyOnLoad(gameObject);
                bannerAdsController = this;
                
                SetApplicationIds();
                RequestBanner();

                Debug.Log("Banner reklamı gösterildi");
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void HideAd()
    {
        bannerView.Hide();
        bannerView.Destroy();
        Debug.Log("Banner reklamı gizlendi");
    }

    //Set Application Ids
    private void SetApplicationIds()
    {
        #if UNITY_ANDROID
            string appId = "ca-app-pub-1741891681659976~7822695685";
        #elif UNITY_IPHONE
            string appId = "ca-app-pub-1741891681659976~3349098203";
        #else
            string appId = "unexpected_platform";
        #endif

        //bannerView.OnAdLoaded += HandleOnAdLoaded;

        MobileAds.Initialize(appId);
    }

    //Request Banner
    private void RequestBanner()
    {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-1741891681659976/1238596720";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-1741891681659976/3512749732";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
        
    }

    //Request Banner
    public void RequestBannerTest()
    {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/6300978111";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/6300978111"; 
        #else
            string adUnitId = "unexpected_platform";
        #endif

        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();
        bannerView.LoadAd(request);
    }

    public float GetBannerHeight()
    {
        return bannerView.GetHeightInPixels();
    }

    //public void HandleOnAdLoaded(object sender, EventArgs args)
    //{
    //    removeAds = GameObject.FindGameObjectWithTag("RemoveAds").gameObject;
    //    removeAds.transform.position = new Vector3(0,65,0);
    //    Debug.Log("reklam yuklendi");
    //}
}
