using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject levelAndTimer;

    [SerializeField] private GameObject musicButtonOn;
    [SerializeField] private GameObject musicButtonOff;

    [SerializeField] private GameObject hapticsButtonOn;
    [SerializeField] private GameObject hapticsButtonOff;

    [SerializeField] private GameObject UICam;
    [SerializeField] private GameObject bannerBar;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private string link= "market://details?id=com.PriyankaPanjwani.KnottyBirds";

    private float scale1;
    private float scale2;
    private Canvas canvas;
    private float timer;
    private float timerCounter;
    private void Awake()
    {
        /* canvas = GetComponent<Canvas>();

         canvas.renderMode = RenderMode.ScreenSpaceOverlay;

         float x = canvas.gameObject.GetComponent<RectTransform>().position.x;
         float y = canvas.gameObject.GetComponent<RectTransform>().position.y;
         scale1 = canvas.gameObject.GetComponent<RectTransform>().localScale.x;


         UICam.transform.position=new Vector3(x,y,-100);

         canvas.renderMode = RenderMode.ScreenSpaceCamera;
         scale2 = canvas.gameObject.GetComponent<RectTransform>().localScale.x;*/
        shopPanel.SetActive(true);
    }
    private void Start()
    {
        shopPanel.SetActive(false);
        timer = 1.5f;
        timerCounter = timer;
        if (AdmobADs.instance != null && !AdmobADs.instance.removeAds && PlayerPrefs.GetInt("RemoveAds", 0) == 0)
            AdmobADs.instance.LoadBannerAd();
        else
        {
            bannerBar.SetActive(false);
        }
    }
    private void Update()
    {
       /* scale2 = canvas.gameObject.GetComponent<RectTransform>().localScale.x;
        if (scale1 != scale2 && timerCounter>0)
        {
            timerCounter -= Time.deltaTime;
            FindObjectOfType<LevelManager>().CalculatePositions();
            FindObjectOfType<LevelManager>().UpdateLine();
            if (scale1 > scale2)
            {
                UICam.GetComponent<Camera>().fieldOfView += Time.deltaTime;
            }
            else if (scale1 < scale2)
            {
                UICam.GetComponent<Camera>().fieldOfView -= Time.deltaTime;
            }
        }*/


        if (Input.GetKeyDown(KeyCode.R))
            ReloadCurrentScene();

        if (PlayerPrefs.GetInt("Audio", 1) == 1)
        {
            musicButtonOn.SetActive(true);
            musicButtonOff.SetActive(false);
        }
        else
        {
            musicButtonOn.SetActive(false);
            musicButtonOff.SetActive(true);
        }

        if (PlayerPrefs.GetInt("Haptics", 1) == 1)
        {
            hapticsButtonOn.SetActive(true);
            hapticsButtonOff.SetActive(false);
        }
        else
        {
            hapticsButtonOn.SetActive(false);
            hapticsButtonOff.SetActive(true);
        }

    }
    public void ShowInterstitialAds()
    {
        if (AdmobADs.instance != null && !AdmobADs.instance.removeAds && PlayerPrefs.GetInt("RemoveAds", 0) == 0)
            AdmobADs.instance.ShowInterstitial();
    }
    public void ShowRewardedAd(int rewardNumber)
    {
        if (AdmobADs.instance != null)
            AdmobADs.instance.ShowRewardedAd(rewardNumber);
    }
    public void ReloadCurrentScene()
    {
        Invoke("LoadScene", 0.4f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void AudioOn()
    {
        PlayerPrefs.SetInt("Audio", 1);
    }
    public void AudioOff()
    {
        PlayerPrefs.SetInt("Audio", 0);
    }
    public void HapticsOn()
    {
        PlayerPrefs.SetInt("Haptics", 1);
    }
    public void HapticsOff()
    {
        PlayerPrefs.SetInt("Haptics", 0);
    }
    public void RateUS()
    {
        Application.OpenURL(link);       
    }

    public void LoadCustomLevel(int level)
    {
        PlayerPrefs.SetInt("CurrentLevel",level);
        ReloadCurrentScene();
    }
}
