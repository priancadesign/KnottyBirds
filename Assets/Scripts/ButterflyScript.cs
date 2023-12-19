using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButterflyScript : MonoBehaviour
{
    [SerializeField] private RectTransform butterflyArea;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private RectTransform coinSpawnArea;
    [SerializeField] private GameObject[] Startpoints;
    private Vector2 randomPoint;
    void Start()
    {
        randomPoint = Startpoints[Random.Range(0,Startpoints.Length-1)].transform.localPosition;
        gameObject.transform.localPosition = randomPoint;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.x - randomPoint.x < 1 && transform.localPosition.y - randomPoint.y < 1)
        {
            randomPoint = GetRandomPosition();
        if (transform.localPosition.x < randomPoint.x)
            transform.localScale=new Vector3(-1, 1, 1);
        else
            transform.localScale=new Vector3(1, 1, 1);
        }


        transform.localPosition = Vector3.MoveTowards(transform.localPosition, randomPoint, 100*Time.deltaTime);
    }
    Vector2 GetRandomPosition()
    {
        // Get the boundaries of the spawnArea
        float minX = butterflyArea.rect.xMin;
        float maxX = butterflyArea.rect.xMax;
        float minY = butterflyArea.rect.yMin;
        float maxY = butterflyArea.rect.yMax;

        // Generate random x and y coordinates within the boundaries
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        // Return a Vector2 representing the random position
        return new Vector2(randomX, randomY);
    }
    public void pointerDown()
    {
        GameObject coin =Instantiate(coinPrefab,transform.localPosition,transform.localRotation);
        coin.SetActive(true);
        coin.transform.SetParent(coinSpawnArea, false);

        VibrationControls.instance.Vibrate();
        Destroy(this.gameObject);
    }
    
}
