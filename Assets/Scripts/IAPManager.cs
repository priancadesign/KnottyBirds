using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : MonoBehaviour
{
    private string points100="com.priyankapanjwani.knottybirds.100points";
    private string removeAds= "com.priyankapanjwani.knottybirds.noads";
    private string removeAdsBanner= "com.priyankapanjwani.knottybirds.noadsbanner";

    [SerializeField] private Button removeAdButton;

    private void Start()
    {
        if (PlayerPrefs.GetInt("RemoveAds", 0) == 1)
            removeAdButton.interactable = false;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureReason)
    {
        Debug.Log(product.definition.id + " failed because " + failureReason);
    }

    public void OnPurchaseComplete(Product product)
    {
        if (product.definition.id == points100)
        {
            Debug.Log("100 coins rewarded");
            int score = PlayerPrefs.GetInt("Score", 0);
            score += 100;
            PlayerPrefs.SetInt("Score", score);

            FindObjectOfType<LevelManager>().scoreText.text = "" + score;
        }
        if(product.definition.id == removeAds || product.definition.id == removeAdsBanner)
        {
            Debug.Log("Ads Removed");
            removeAdButton.interactable = false;
            PlayerPrefs.SetInt("RemoveAds", 1);
            if(AdmobADs.instance!=null)
                 AdmobADs.instance.removeAds = true;
            FindObjectOfType<MenuManager>().ReloadCurrentScene();
        }
    }
    public void OnProductFetched(Product product)
    {
      
    }
    
}
