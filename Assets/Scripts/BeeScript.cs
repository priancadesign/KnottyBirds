using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeScript : MonoBehaviour
{
    [SerializeField] private RectTransform beeArea;
    [SerializeField] private GameObject[] points;
    [SerializeField] private GameObject[] Failedtext;


    private Vector2 startPoint;
    private Vector2 Point2;
    private Vector2 endPoint;
    private bool canRandomlyMove;
    public bool canMove;
    // Start is called before the first frame update
    void Start()
    {
        startPoint = points[Random.Range(0,points.Length-1)].transform.localPosition;
        Point2 = points[Random.Range(0,points.Length-1)].transform.localPosition;
        endPoint= GetRandomPosition();
        gameObject.transform.localPosition = startPoint;
        canRandomlyMove = false;
        canMove = false;
        Invoke("reAppear", 3);
        Invoke("removeBee",Random.Range(15,20));
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.localPosition.x - endPoint.x < 1 && transform.localPosition.y - endPoint.y < 1 &&canRandomlyMove )
        {
            endPoint= GetRandomPosition();

            if (transform.localPosition.x < endPoint.x)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);

        }
        if(canMove)
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPoint, 150 * Time.deltaTime);
    }

    Vector2 GetRandomPosition()
    {
        // Get the boundaries of the spawnArea
        float minX = beeArea.rect.xMin;
        float maxX = beeArea.rect.xMax;
        float minY = beeArea.rect.yMin;
        float maxY = beeArea.rect.yMax;

        // Generate random x and y coordinates within the boundaries
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        // Return a Vector2 representing the random position
        return new Vector2(randomX, randomY);
    }

    public void pointerDown()
    {
        VibrationControls.instance.Vibrate();
        canMove = false;
        StartCoroutine(StingingEffect());
        Invoke("GameOver", 0.5f);
        //GameOver();
    }

    private void GameOver()
    {
        for (int i = 0; i < Failedtext.Length; i++)
        {
            Failedtext[i].SetActive(false);
        }
        Failedtext[1].SetActive(true);

        FindObjectOfType<LevelManager>().timerTimeSec = 0;
        FindObjectOfType<LevelManager>().timerTimeMin = 0;
    }

    private IEnumerator StingingEffect()
    {
        transform.position += new Vector3(8, 1, 0);
        yield return new WaitForSeconds(0.05f);

        transform.position += new Vector3(-8, 0, 0);
        yield return new WaitForSeconds(0.05f);
        
        transform.position += new Vector3(8, 1, 0);
        yield return new WaitForSeconds(0.05f);
        
        transform.position += new Vector3(-8, 0, 0);
        yield return new WaitForSeconds(0.05f);

        transform.position += new Vector3(8, 1, 0);
        yield return new WaitForSeconds(0.05f);

        transform.position += new Vector3(-8, 0, 0);
        yield return new WaitForSeconds(0.05f);

        transform.position += new Vector3(8, 1, 0);
        yield return new WaitForSeconds(0.05f);

        transform.position += new Vector3(-8, 0, 0);
        yield return new WaitForSeconds(0.05f);
    }

    private void removeBee()
    {
        canRandomlyMove = false;
        endPoint = Point2;
        if (transform.localPosition.x < endPoint.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    private void reAppear()
    {
        canRandomlyMove = true;
        canMove = true;
        endPoint = GetRandomPosition();
        if (transform.localPosition.x < endPoint.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }
}
