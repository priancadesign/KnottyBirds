using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;
public class AdmobADs : MonoBehaviour
{
    /*[SerializeField] TextMeshProUGUI initialComp;
    [SerializeField] TextMeshProUGUI interLoaded;
    [SerializeField] TextMeshProUGUI interShowed;
    [SerializeField] TextMeshProUGUI rewardLoaded;
    [SerializeField] TextMeshProUGUI rewardShowed;*/



         string appId = "ca-app-pub-1602266538594449~4301754819"; //Place your Android app id here
         string iOSAppId = "ca-app-pub-1602266538594449~4789985184"; //Place your iOS app id here

#if UNITY_ANDROID
              string bannerId = "ca-app-pub-1602266538594449/5103700593";
        string interstitialId = "ca-app-pub-1602266538594449/8955369599"; // Place your Android interstitial ad id here
            string rewardedId = "ca-app-pub-1602266538594449/9850749911";   // Place your Android rewarded ad id here
#elif UNITY_IOS
        string bannerId = "ca-app-pub-1602266538594449/6425095514";
        string interstitialId="ca-app-pub-1602266538594449/6082885554";
        string rewardedId = "ca-app-pub-1602266538594449/1742510823";

#endif


    InterstitialAd interstitialAd;
     RewardedAd rewardedAd;
    BannerView bannerView;
     

    public static AdmobADs instance;
    public bool removeAds;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);


        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initstatus =>
        {
            Debug.Log("ADmob ADs Initialized");
            // initialComp.text = "Initialized!";

            LoadRewardedAd();
            if(!removeAds && PlayerPrefs.GetInt("RemoveAds", 0)==0)
            LoadInterstital();
            //LoadBannerAd();
        });

    }

    #region Banner

    public void LoadBannerAd()
    {
        //create a banner
        CreateBannerView();

        //listen to banner events
        ListenToBannerEvents();

        //load the banner
        if (bannerView == null)
        {
            CreateBannerView();
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        print("Loading banner Ad !!");
        bannerView.LoadAd(adRequest);//show the banner on the screen
    }
    void CreateBannerView()
    {

        if (bannerView != null)
        {
            DestroyBannerAd();
        }
        int width = Screen.width;
        bannerView = new BannerView(bannerId, AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth), AdPosition.Bottom);
    }
    void ListenToBannerEvents()
    {
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Banner view paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }
    public void DestroyBannerAd()
    {

        if (bannerView != null)
        {
            print("Destroying banner Ad");
            bannerView.Destroy();
            bannerView = null;
        }
    }
    #endregion

    #region interstital

    public void LoadInterstital()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");
        InterstitialAd.Load(interstitialId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.Log("Intertstial AD Failed to Load");
                // interLoaded.text = "" + error;
                return;
            }
            Debug.Log("Interstital AD Loaded " + ad.GetResponseInfo());
             // interLoaded.text = "inter Loaded";
            interstitialAd = ad;
            InterstitialEvents(interstitialAd);

        });
    }
    public void ShowInterstitial()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
            Debug.Log("Interstital Ad Showed!!");
            //  interShowed.text = "Inter Showed";
        }
        else
        {
            Debug.Log("AD is not Ready");
        }
    }
    public void InterstitialEvents(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Interstitial ad paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            if(!removeAds && PlayerPrefs.GetInt("RemoveAds", 0) == 0)
            LoadInterstital();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            if(!removeAds && PlayerPrefs.GetInt("RemoveAds", 0) == 0)
            LoadInterstital();
        };

    }

    #endregion

    #region Rewarded
    
        public void LoadRewardedAd()
        {

            if (rewardedAd != null)
            {
                rewardedAd.Destroy();
                rewardedAd = null;
            }
            var adRequest = new AdRequest();
            adRequest.Keywords.Add("unity-admob-sample");

            RewardedAd.Load(rewardedId, adRequest, (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.Log("Rewarded failed to load" + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded !!");
                //rewardLoaded.text = "Reward Loaded";
                rewardedAd = ad;
                RewardedAdEvents(rewardedAd);
            });
        }
        public void ShowRewardedAd(int rewardNumber)
        {

            if (rewardedAd != null && rewardedAd.CanShowAd())
            {
                rewardedAd.Show((Reward reward) =>
                {
                    Debug.Log("Rewarded AD Showed!!");
                    // rewardShowed.text = "Reward Showed";

                    if (rewardNumber == 1)
                        MovesIncrement();
                    else if (rewardNumber == 2)
                        RemoveTimer();
                    else if (rewardNumber == 3)
                        LevelIncrease();

                });
            }
            else
            {
                Debug.Log("Rewarded ad not ready");
            }
        }
        public void RewardedAdEvents(RewardedAd ad)
        {
            // Raised when the ad is estimated to have earned money.
            ad.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log("Rewarded ad paid {0} {1}." +
                    adValue.Value +
                    adValue.CurrencyCode);
            };
            // Raised when an impression is recorded for an ad.
            ad.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Rewarded ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            ad.OnAdClicked += () =>
            {
                Debug.Log("Rewarded ad was clicked.");
            };
            // Raised when an ad opened full screen content.
            ad.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Rewarded ad full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded ad full screen content closed.");
                LoadRewardedAd();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content " +
                               "with error : " + error);
                LoadRewardedAd();
            };
        }

    #endregion

    #region extra
    void MovesIncrement() //reward 1
    {
        FindObjectOfType<LevelManager>().noOfMoves += 1;
        Debug.Log("Player Rewarded");
    }

    void RemoveTimer() //reward 2
    {
        FindObjectOfType<LevelManager>().timerTimeMin = 99999999;
        FindObjectOfType<MenuManager>().levelAndTimer.SetActive(false);
    }

    void LevelIncrease() //reward 3
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        currentLevel += 1;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        FindObjectOfType<MenuManager>().ReloadCurrentScene();
    }
    #endregion
}
