using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class LevelManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] public  TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI movesText;
    [SerializeField] private GameObject[] Failedtext;
    [SerializeField] private GameObject movesArea;
    [SerializeField] private GameObject scoreArea;
    [SerializeField] private Image clock;

    [SerializeField] public float timerTimeSec = 60;
    [SerializeField] public float timerTimeMin = 60;

    [SerializeField] private Sprite[] birdsImage;
    [SerializeField] private Sprite powerBirdsImage;
    [SerializeField] private GameObject birdPrefeb;
    [SerializeField] private GameObject butterflyPrefeb;
    [SerializeField] private GameObject beePrefeb;
    [SerializeField] private int noOfBirds; 
    [SerializeField] private int currentLevel;
    [SerializeField] private int score;
    [SerializeField] private RectTransform spawnArea;
    [SerializeField] private RectTransform dragArea;
    [SerializeField] private RectTransform birdSpawnArea;
    [SerializeField] private RectTransform lineSpawnArea;
    [SerializeField] private RectTransform butterflySpawnArea;
    [SerializeField] private RectTransform beeSpawnArea;
    [SerializeField] private GameObject[] totalBirds;
    [SerializeField] private GameObject[] polygonSpawn;
    
    [SerializeField] private RectTransform linePrefab;

    [SerializeField] public GameObject[] completePanel;
    [SerializeField] public GameObject ratePanel;
    [SerializeField] public GameObject gameOverPanel;
    [SerializeField] public GameObject settingsPanel;
    [SerializeField] public GameObject howToPanel;
    [SerializeField] public GameObject removeAdsPanel;
    [SerializeField] public GameObject confetti;


    private RectTransform[] noOflines; // Reference to the current line being drawn
    private Vector2[] startPoints; // Starting point of the line
    private Vector2[] endPoints;
    private bool levelComplete;
    private bool overlap;
    private bool canCheckoverlaps;

    private int[] randomMovement;
    public int noOfMoves;

    private List<Vector3> usedPositions = new List<Vector3>();

    private float butterflySpawnTime;
    private float beeSpawnTime;
    private float butterflySpawnTimeCounter;
    private float beeSpawnTimeCounter;
    private int totalButterflies;
    public int totalBees;

    private float width;
    private float height;


    private void Awake()
    {
        Time.timeScale = 1;
    }
    private void Start()
    {

        RectTransform rectTransform = GetComponent<RectTransform>();
        width = rectTransform.rect.width;
        height = rectTransform.rect.height;


        canCheckoverlaps = false;
        Invoke("TurnCheckingOn", 1f);
           score = PlayerPrefs.GetInt("Score", 0);
        scoreText.text = "" + score;

        
        noOfBirds = 6;

        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        levelText.text = "Level " + currentLevel;

        if (currentLevel <= 14)
            noOfBirds += currentLevel;
        else
            noOfBirds = 20;

        if(currentLevel<5)
        {
            timerTimeSec = 60;
            timerTimeMin = 0;
        }
        else if (currentLevel < 10)
        {
            timerTimeSec = 60;
            timerTimeMin = 0;
        }
        else if(currentLevel < 15)
        {
            timerTimeSec = 60;
            timerTimeMin = 0;
        }
        else
        {
            timerTimeSec = 0;
            timerTimeMin = 2;
        }

        totalBirds = new GameObject[noOfBirds];
        startPoints = new Vector2[noOfBirds];
        endPoints = new Vector2[noOfBirds];
        noOflines = new RectTransform[noOfBirds];

        GenerateRandomPositions(noOfBirds);

        CalculatePositions();
        CreateLines();

        butterflySpawnTime = 5f;
        beeSpawnTime = 8f;
        butterflySpawnTimeCounter = butterflySpawnTime;
        beeSpawnTimeCounter = beeSpawnTime;

        if (currentLevel > 4)
        {
            totalButterflies = currentLevel - 4;
            if (totalButterflies > 5)
                totalButterflies = 5;
        }
        else
            totalButterflies = 0;

        if (currentLevel > 9)
        {
            totalBees = currentLevel - 9;
            if (totalBees > 10)
                totalBees = 10;
        }
        else
            totalBees = 0;

      
        noOfMoves = 25;
        if (currentLevel < 15)
        {
            movesArea.SetActive(false);
            scoreArea.transform.localPosition=new Vector2(0,scoreArea.transform.localPosition.y);
        }
        else
        {
            movesArea.SetActive(true);
            scoreArea.transform.localPosition=new Vector2(-180,scoreArea.transform.localPosition.y);
        }


        levelComplete = false;

        for (int i = 0; i < completePanel.Length; i++)
        {
            completePanel[i].SetActive(false);
        }
        gameOverPanel.SetActive(false);
        settingsPanel.SetActive(false);
        confetti.SetActive(false);
        ratePanel.SetActive(false);

        Vector2 size = spawnArea.rect.size;
        spawnArea.gameObject.GetComponent<BoxCollider2D>().size = size;
        spawnArea.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0,0);

        Vector2 size2 = dragArea.rect.size;
        dragArea.gameObject.GetComponent<BoxCollider2D>().size = size2;
        dragArea.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0,0);

        clock.fillAmount = 1;


        randomMovement = new int[totalBirds.Length];
        for (int i = 0; i < totalBirds.Length; i++)
        {
            randomMovement[i] = Random.Range(-200, 200);
        }
    }

    private void SpawnBee()
    {
        GameObject bee = Instantiate(beePrefeb, transform.position, transform.rotation);
        bee.SetActive(true);
        bee.transform.SetParent(beeSpawnArea, false);
    }

    private void spawnButterfly()
    {
        GameObject butterfly = Instantiate(butterflyPrefeb, transform.position, transform.rotation);
        butterfly.SetActive(true);
        butterfly.transform.SetParent(butterflySpawnArea, false);
    }

    public void CalculatePositions()
    {
        for (int i = 0; i < totalBirds.Length; i++)
        {
            startPoints[i] = totalBirds[i].transform.localPosition;
            if (i + 1 < totalBirds.Length)
            {
                endPoints[i] = totalBirds[i + 1].transform.localPosition;
            }
            else
            {
                endPoints[i] = totalBirds[0].transform.localPosition;
            }
        }
    }

    
    private void Update()
    {
        if (totalButterflies > 0 && butterflySpawnArea.transform.childCount<3)
        {
            spawnButterfly();
            totalButterflies--;
        }

        /*if (butterflySpawnTimeCounter <= 0 && totalButterflies > 0)
        {
            spawnButterfly();
            totalButterflies--;
            butterflySpawnTime = Random.Range(0, 20);
            butterflySpawnTimeCounter = butterflySpawnTime;
        }
        else if (butterflySpawnTimeCounter > 0)
            butterflySpawnTimeCounter -= Time.deltaTime;*/

        if (beeSpawnTimeCounter <= 0 && totalBees> 0)
        {
            for (int i = 0; i < Random.Range(1,3); i++)
            {
                if (totalBees > 0)
                {
                SpawnBee();
                totalBees--;
                }
            }
            beeSpawnTime = Random.Range(15, 20);
            beeSpawnTimeCounter = beeSpawnTime;
        }
        else if (beeSpawnTimeCounter > 0)
            beeSpawnTimeCounter -= Time.deltaTime;



        if (levelComplete)
        {
            for (int i = 0; i < totalBirds.Length; i++)
            {
                totalBirds[i].transform.position += new Vector3(randomMovement[i], 200, 0)*Time.deltaTime;
                Color currentColor = totalBirds[i].GetComponent<Image>().color;
                totalBirds[i].GetComponent<Image>().color = new Color(currentColor.r,currentColor.g,currentColor.b,currentColor.a-0.0025f);
            }
        }

        movesText.text = "" + noOfMoves;

        if(currentLevel>14 && noOfMoves==0)
        {
            for (int i = 0; i < Failedtext.Length; i++)
            {
                Failedtext[i].SetActive(false);
            }
            Failedtext[2].SetActive(true);

            timerTimeMin = 0;
            timerTimeSec = 0;
        }

        if (timerTimeSec <= 0 && timerTimeMin>0)
        {
            timerTimeMin--;
            timerTimeSec = 60;
        }

        if(timerTimeSec > 0 || timerTimeMin > 0)
        {
            if(!levelComplete && !settingsPanel.activeInHierarchy && !howToPanel.activeInHierarchy && !removeAdsPanel.activeInHierarchy)
            {
            timerTimeSec -= Time.deltaTime;
                if(currentLevel<5)
                    clock.fillAmount = (timerTimeSec +(60*timerTimeMin))/(60);
                else if (currentLevel < 10)
                    clock.fillAmount = (timerTimeSec + (60 * timerTimeMin)) / (60);
                else if (currentLevel < 15)
                    clock.fillAmount = (timerTimeSec + (60 * timerTimeMin)) / (60);
                else
                    clock.fillAmount = (timerTimeSec + (60 * timerTimeMin)) / (120);
            }
            
            timerText.text ="0"+ (int)timerTimeMin +":"+(int)timerTimeSec;
         
             overlap = IsOverlap();
            if (!overlap && !levelComplete && canCheckoverlaps &&birdsIsPlaced())
            {
                levelComplete = true;

                if (currentLevel == 4)
                    completePanel[1].SetActive(true);
                else if (currentLevel == 9)
                    completePanel[2].SetActive(true);
                else if (currentLevel == 14)
                    completePanel[3].SetActive(true);
                else if (currentLevel == 25)
                    completePanel[4].SetActive(true);
                else
                    completePanel[0].SetActive(true);

                if (currentLevel == 10 || currentLevel == 25)
                    ratePanel.SetActive(true);

                confetti.SetActive(true);
                PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
                PlayerPrefs.SetInt("Score",score+10);
                Debug.Log("comp");
                for (int i = 0; i < noOflines.Length; i++)
                {
                    noOflines[i].gameObject.SetActive(false);
                }
                FindObjectOfType<AudioManager>().PlayBirdsChirpingSound();

            }
            else if(overlap)
            {
                for (int i = 0; i < completePanel.Length; i++)
                {
                    completePanel[i].SetActive(false);
                }
                confetti.SetActive(false);
                levelComplete = false;
            }
        }
        else
        {
            if(!gameOverPanel.activeInHierarchy)
            FindObjectOfType<AudioManager>().PlayFailedSound();
            gameOverPanel.SetActive(true);
            //Time.timeScale = 0;
        }
        
    }
    Vector2 GetRandomPosition()
    {
        // Get the boundaries of the spawnArea
        float minX = spawnArea.rect.xMin +150;
        float maxX = spawnArea.rect.xMax -150;
        float minY = spawnArea.rect.yMin +150;
        float maxY = spawnArea.rect.yMax -150;

        // Generate random x and y coordinates within the boundaries
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        // Return a Vector2 representing the random position
        return new Vector2(randomX, randomY);
    }
    Vector2 GetPolygonSpawnPos()
    {
        Transform polygonSpawnTransform;

        if (currentLevel > 14)
        {
        polygonSpawnTransform= polygonSpawn[polygonSpawn.Length-1].transform;
        }
        else
        {
        polygonSpawnTransform= polygonSpawn[currentLevel - 1].transform;
        }
        
        return polygonSpawnTransform.GetChild(Random.Range(0,polygonSpawnTransform.childCount)).transform.localPosition;
    }


    void CreateLines()
    {
        for (int i = 0; i < noOfBirds; i++)
        {
            noOflines[i] = Instantiate(linePrefab, lineSpawnArea);
            noOflines[i].anchorMin = noOflines[i].anchorMax = Vector2.zero;
            noOflines[i].transform.localPosition = startPoints[i];
            //currentLine.anchoredPosition = startPosition;
        }
            UpdateLine();
    }
    public void UpdateLine()
    {
        for (int i = 0; i < noOfBirds; i++)
        {
            Vector2 direction;
            direction.x = endPoints[i].x - startPoints[i].x;
            direction.y = endPoints[i].y - startPoints[i].y;

            float distance = direction.magnitude;
            Vector2 normalizedDirection = direction.normalized;

            noOflines[i].sizeDelta = new Vector2(distance, 10f); // Adjust line thickness as needed

            float angle = Mathf.Atan2(normalizedDirection.y, normalizedDirection.x) * Mathf.Rad2Deg;
            noOflines[i].rotation = Quaternion.Euler(0f, 0f, angle);

            noOflines[i].transform.localPosition = (endPoints[i] + startPoints[i]) / 2f;
            // currentLine.anchoredPosition = (endPosition + startPoint) / 2f;

            Vector2 S = noOflines[i].rect.size;
            S.x -= 18;
            noOflines[i].GetComponent<BoxCollider2D>().size = S;
            noOflines[i].GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
        }
    }

    private bool IsOverlap()
    {
        for (int i = 0; i < noOflines.Length; i++)
        {
            bool overlap = noOflines[i].GetComponent<LineScript>().isOverlap;
            if (overlap)
            {
                return true;
            }
            else if(i==noOflines.Length-1 && !overlap)
            {
                return false;
            }
        }
        return true;
    }
    private void TurnCheckingOn()
    {
        canCheckoverlaps = true;
    }
    private bool birdsIsPlaced()
    {
        for (int i = 0; i < totalBirds.Length; i++)
        {
            bool isplaced = totalBirds[i].GetComponent<BirdScript>().isPlaced;
            if (!isplaced)
                return false;
        }
        return true;
    }

    private bool IsOverlapping(Vector3 position)
    {
        foreach (Vector3 usedPos in usedPositions)
        {
            //float distance = Vector3.Distance(position, usedPos);
            //if (distance < 1) // Adjust the minimum distance to prevent overlap previuos was 125.5
           // {
                //return true;
            // }

            if (position == usedPos)
                return true;
        }
        return false;
    }

    private void GenerateRandomPositions(int count)
    {
        for (int i = 0; i < count; i++)
        {
            //Vector3 randomPos = GetRandomPosition();
            Vector3 randomPos = GetPolygonSpawnPos();
            while (IsOverlapping(randomPos))
             {
                //randomPos = GetRandomPosition(); // Generate a new random position if overlapping
               randomPos = GetPolygonSpawnPos(); // Generate a new random position if overlapping
             }
            Sprite birdImage = birdsImage[Random.Range(0, birdsImage.Length)];
            birdPrefeb.GetComponent<Image>().sprite = birdImage;
            GameObject birdObject = Instantiate(birdPrefeb, randomPos, transform.rotation);
            birdObject.transform.SetParent(birdSpawnArea.transform, false);
            //birdObject.transform.localPosition -= new Vector3(0, 50,0);

            totalBirds[i] = birdObject;
            usedPositions.Add(randomPos);
        }

        if (currentLevel > 25)
        {
            int powerBird = Random.Range(0, totalBirds.Length - 1);
            totalBirds[powerBird].GetComponent<Image>().sprite = powerBirdsImage;
            totalBirds[powerBird].GetComponent<BirdScript>().isPowerBird=true;
        }
    }
}
