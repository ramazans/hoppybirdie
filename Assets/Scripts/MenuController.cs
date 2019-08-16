using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using UnityEngine.UI;
using UnityEditor;

public class MenuController : MonoBehaviour {

    public GameObject getReadyMenu;
    public GameObject gameOverMenu;
    public GameObject startMenu;
    public GameObject onGameMenu;
    public GameObject settingsMenu;
    public GameObject introductionMenu;
    public GameObject tutorialMenu;

    public enum MenuStates { GetReadyMenu, GameOverMenu, StartMenu, OnGameMenu, SettingsMenu, IntroductionMenu, TutorialMenu };
    public static MenuStates currentMenu;

    GameObject character;
    AudioSource mic;
    Assets.Scripts.CharacterController characterController;
    BannerAdsController bannerAdsController;

    Text gameScoreText;
    Text boardScoreText;
    Text boardBestScoreText;
    Image newBestScoreImage;

    GameObject mainGameUI;
    GameObject removeAdsButton1;
    GameObject removeAdsButton2;
    GameObject removeAdsButton3;

    public static int score = 0;
    public static bool newBestScore = false;
    public static bool firstGameMenu = false;

    //Rate
    private const string AndroidRatingURI = "http://play.google.com/store/apps/details?id={0}";
    private const string iOSRatingURI = "itms://itunes.apple.com/us/app/apple-store/{0}?mt=8";

    [Tooltip("iOS App ID (number), example: 1122334455")]
    private string iOSAppID = "1448203386";
    private string url;

    private int removeAds = 0;
    float ss;

    float removeAdsY = 0, removeAdsX = 0;

    void Start() {

        //TVOS Ads Control
#if UNITY_TVOS
            PlayerPrefs.SetInt("RemoveAds", 1);
#endif


        //PlayerPrefs.SetInt("RemoveAds", 1);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        settingsMenu.GetComponent<RectTransform>().sizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;

        introductionMenu.GetComponent<RectTransform>().sizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;
        gameObject.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;

        tutorialMenu.GetComponent<RectTransform>().sizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;
        gameObject.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;


        //if (IsIPad() || isAdroidTablet())
        //    Screen.orientation = ScreenOrientation.Landscape;
        //else
        Screen.orientation = ScreenOrientation.Portrait;

        character = GameObject.Find("Character");
        mic = character.GetComponent<AudioSource>();
        characterController = character.GetComponent<Assets.Scripts.CharacterController>();

        //mainGameUI = GameObject.Find("MainGameUI");

        ss = PlayerPrefs.GetFloat("Sensitive");
        if (ss != 0)
        {
            Assets.Scripts.CharacterController.sensitivity = ss;
        }
        else
        {
            ss = Assets.Scripts.CharacterController.sensitivity;
        }
        gameObject.transform.GetChild(4).gameObject.transform.GetChild(2).gameObject.GetComponent<Slider>().value = ss;

        gameScoreText = gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        boardScoreText = gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>();
        boardBestScoreText = gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        newBestScoreImage = gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();

        removeAds = PlayerPrefs.GetInt("RemoveAds");
        removeAdsButton1 = gameObject.transform.GetChild(1).gameObject.transform.GetChild(3).gameObject;
        removeAdsButton2 = gameObject.transform.GetChild(2).gameObject.transform.GetChild(2).gameObject;
        removeAdsButton3 = gameObject.transform.GetChild(4).gameObject.transform.GetChild(4).gameObject;
        

        if (firstGameMenu)
        {
            MenuGameOver();
        }
        else
        {
            currentMenu = MenuStates.StartMenu;
            firstGameMenu = true;
        }
        score = 0;

        bannerAdsController = GameObject.FindGameObjectWithTag("BannerAds").GetComponent<BannerAdsController>();
        removeAdsY = removeAdsButton1.transform.position.y;
        removeAdsX = removeAdsButton1.transform.position.x;

        #if UNITY_ANDROID
            url = AndroidRatingURI.Replace("{0}", Application.identifier);
        #elif UNITY_IOS
            url = iOSRatingURI.Replace("{0}", iOSAppID);
        #endif

        
    }

    void Update() {
        
        if (removeAds == 1)
        {
            removeAdsButton1.SetActive(false);
            removeAdsButton2.SetActive(false);
            removeAdsButton3.SetActive(false);
        }
        else
        {
            float bannerHeight = bannerAdsController.GetBannerHeight();
            if (DeviceHasSafeArea())
            {
                float safeArea = (bannerHeight / 100) * 68;
                removeAdsButton1.transform.position = new Vector3(removeAdsX, (removeAdsY + bannerHeight + safeArea), 0);
                removeAdsButton2.transform.position = new Vector3(removeAdsX, (removeAdsY + bannerHeight + safeArea), 0);
                removeAdsButton3.transform.position = new Vector3(removeAdsX, (removeAdsY + bannerHeight + safeArea), 0);
            }
            else
            {
                removeAdsButton1.transform.position = new Vector3(removeAdsX, (removeAdsY + bannerHeight), 0);
                removeAdsButton2.transform.position = new Vector3(removeAdsX, (removeAdsY + bannerHeight), 0);
                removeAdsButton3.transform.position = new Vector3(removeAdsX, (removeAdsY + bannerHeight), 0);
            }
        }

        gameScoreText.text = score.ToString();
        if (characterController.loudness > 1)
            Go();

        switch (currentMenu)
        {
            case MenuStates.GetReadyMenu:
                getReadyMenu.SetActive(true);
                gameOverMenu.SetActive(false);
                startMenu.SetActive(false);
                onGameMenu.SetActive(false);
                settingsMenu.SetActive(false);
                introductionMenu.SetActive(false);
                tutorialMenu.SetActive(false);
                break;
            case MenuStates.GameOverMenu:
                getReadyMenu.SetActive(false);
                gameOverMenu.SetActive(true);
                startMenu.SetActive(false);
                onGameMenu.SetActive(false);
                settingsMenu.SetActive(false);
                introductionMenu.SetActive(false);
                tutorialMenu.SetActive(false);
                break;
            case MenuStates.StartMenu:
                getReadyMenu.SetActive(false);
                gameOverMenu.SetActive(false);
                startMenu.SetActive(true);
                onGameMenu.SetActive(false);
                settingsMenu.SetActive(false);
                introductionMenu.SetActive(false);
                tutorialMenu.SetActive(false);
                break;
            case MenuStates.OnGameMenu:
                getReadyMenu.SetActive(false);
                gameOverMenu.SetActive(false);
                startMenu.SetActive(false);
                onGameMenu.SetActive(true);
                settingsMenu.SetActive(false);
                introductionMenu.SetActive(false);
                tutorialMenu.SetActive(false);
                break;
            case MenuStates.SettingsMenu:
                getReadyMenu.SetActive(false);
                gameOverMenu.SetActive(false);
                startMenu.SetActive(false);
                onGameMenu.SetActive(false);
                settingsMenu.SetActive(true);
                introductionMenu.SetActive(false);
                tutorialMenu.SetActive(false);
                break;
            case MenuStates.IntroductionMenu:
                getReadyMenu.SetActive(false);
                gameOverMenu.SetActive(false);
                startMenu.SetActive(false);
                onGameMenu.SetActive(false);
                settingsMenu.SetActive(false);
                introductionMenu.SetActive(true);
                tutorialMenu.SetActive(false);
                break;
            case MenuStates.TutorialMenu:
                getReadyMenu.SetActive(false);
                gameOverMenu.SetActive(false);
                startMenu.SetActive(false);
                onGameMenu.SetActive(false);
                settingsMenu.SetActive(false);
                introductionMenu.SetActive(false);
                tutorialMenu.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void Rate()
    {
        if (!string.IsNullOrEmpty(url))
        {
            Application.OpenURL(url);
        }
        else
        {
            Debug.LogWarning("Unable to open URL, invalid OS");
        }
    }

    public void RemoveAds()
    {
        Purchaser.Instance.BuyRemoveAds();
    }

    public void Back()
    {
        currentMenu = MenuStates.StartMenu;
    }

    public void SkipIntroduction()
    {
        currentMenu = MenuStates.GetReadyMenu;
        mic.mute = false;
        Assets.Scripts.CharacterController.tutorialTiming = 0;
        PlayerPrefs.SetInt("Introduction", 1);
    }

    public void OptionsMenu()
    {
        currentMenu = MenuStates.SettingsMenu;
    }

    public void Play() //go getready
    {
        Assets.Scripts.CharacterController.tutorialTiming = 0;
        if (PlayerPrefs.GetInt("Introduction") == 1)
        {
            currentMenu = MenuStates.GetReadyMenu;
            mic.mute = false;
        }
        else
        {
            currentMenu = MenuStates.IntroductionMenu;
        }
    }

    public void MenuGameOver() //game over
    {
        currentMenu = MenuStates.GameOverMenu;
        boardScoreText.text = score.ToString();
        boardBestScoreText.text = PlayerPrefs.GetInt("BestScore").ToString();
        newBestScoreImage.enabled = newBestScore;
        newBestScore = false;
    }

    public void Go() //game started
    {
        currentMenu = MenuStates.OnGameMenu;
    }

    bool IsIPad()
    {
    #if UNITY_IOS
                return UnityEngine.iOS.Device.generation.ToString().IndexOf("iPad") > -1;
    #else
            return false;
    #endif
    }

    bool isAdroidTablet()
    {
    #if UNITY_ANDROID
            return (Application.platform == RuntimePlatform.Android && DeviceDiagonalSizeInInches() > 6.5f);
    #else
                return false;
    #endif
    }

    public static float DeviceDiagonalSizeInInches()
    {
        float screenWidth = Screen.width / Screen.dpi;
        float screenHeight = Screen.height / Screen.dpi;
        float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));

        Debug.Log("Getting device inches: " + diagonalInches);

        return diagonalInches;
    }

    public void Slider_Changed(float val)
    {
        PlayerPrefs.SetFloat("Sensitive", val);
        Assets.Scripts.CharacterController.sensitivity = val;
    }

    public bool DeviceHasSafeArea()
    {
        float screenRatio = ((1.0f * Screen.height) / (1.0f * Screen.width));
        #if UNITY_IOS
            return ((screenRatio > 2.1 && screenRatio < 2.2)
                    || (screenRatio > 1.3 && screenRatio < 1.4));
        #else
            return false;
        #endif

        //1.4 < screenRatio < 1.6   // 3:2 iPhones - models 4 and earlier
        //1.7 < screenRatio < 1.8   // 16:9 iPhones - models 5, SE, up to 8+
        //2.1 < screenRatio < 2.2   // 19.5:9 iPhones - models X, Xs,  Xr, Xsmax

        //1.3 < screenRatio < 1.4   // 4:3 iPad - models without home button
    }

}
