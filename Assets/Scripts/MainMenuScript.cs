using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject[] loadingScreen;
    [SerializeField] private GameObject Tutorial;
    [SerializeField] private GameObject termsAndPolicies;
    [SerializeField] private string levelScene;

    public string termsAndConditions = "https://docs.google.com/document/d/1h9jmIKWsyxpX65uet5-CwplfVF5KV-tzMahGZfR9fTQ/edit?usp=sharing";
    public string privacyPolicy = "https://docs.google.com/document/d/1gnkFgSxBnBvwBaZMnAxfhfXMurrG9upvQsKSHifKZgQ/edit?usp=sharing";

    void Start()
    {
        for (int i = 0; i < loadingScreen.Length; i++)
        {
            loadingScreen[i].SetActive(false);
        }
        loadingScreen[0].SetActive(true);
        Invoke("Loading1",1.2f);
        Invoke("Loading2",2.3f);
        Invoke("CloseLoadingScreen", 3.6f);
        Tutorial.SetActive(false);
        termsAndPolicies.SetActive(false);

    }
    private void Loading1()
    {
        loadingScreen[1].SetActive(true);
    }
    private void Loading2()
    {
        loadingScreen[2].SetActive(true);
    }

    private void CloseLoadingScreen() {

        for (int i = 0; i < loadingScreen.Length; i++)
        {
            loadingScreen[i].SetActive(false);
        }

        if (PlayerPrefs.GetInt("Terms", 0) == 0)
        {
            termsAndPolicies.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("TutorialPlay", 0) == 0)
        {
            Tutorial.SetActive(true);
        }
        else
        {
            LoadLevel();
        }
    }

    public void TermsContinue()
    {
        PlayerPrefs.SetInt("Terms", 1);
        termsAndPolicies.SetActive(false);
        if (PlayerPrefs.GetInt("TutorialPlay", 0) == 0)
        {
            Tutorial.SetActive(true);
        }
    }
    public void TutotiralContinue()
    {
        Tutorial.SetActive(false);
        PlayerPrefs.SetInt("TutorialPlay", 1);
        LoadLevel();
    } 

    public void LoadLevel()
    {
            SceneManager.LoadScene(levelScene);
    }

    public void openTerms()
    {
        Application.OpenURL(termsAndConditions);
    }
    public void openPrivacyPolicy()
    {
        Application.OpenURL(privacyPolicy);
    }
}
