using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsScript : MonoBehaviour
{
    [SerializeField] private GameObject score;
    [SerializeField] private TextMeshProUGUI scoreText;

    void Update()
    {
        if (transform.position.x - score.transform.position.x < 1 && transform.position.x - score.transform.position.x > -1)
        {
            ScoreIncrement();
            Destroy(gameObject);
            //gameObject.SetActive(false);
        }

            transform.position=Vector3.MoveTowards(transform.position,score.transform.position,2500*Time.deltaTime);
    }
    private void ScoreIncrement()
    {
        int score = PlayerPrefs.GetInt("Score");
        score += 5;
        PlayerPrefs.SetInt("Score", score);
        scoreText.text = "" + score;
    }
}
