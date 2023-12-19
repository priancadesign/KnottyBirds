using UnityEngine;

public class BirdScript : MonoBehaviour
{
    [SerializeField] private bool isTrigger = false;
    [SerializeField] public bool isPlaced;
    [SerializeField] public bool birdOverlap;
    [SerializeField] private GameObject completePanel;
    [SerializeField] private GameObject completePanel4;
    [SerializeField] private GameObject completePanel9;
    [SerializeField] private GameObject completePanel14;
    [SerializeField] private GameObject completePanel25;
    [SerializeField] private GameObject gmeoverPanel;
    [SerializeField] private GameObject targetObject1;
    [SerializeField] private GameObject beeSpawnArea;
    [SerializeField] private RectTransform lightningSpawnArea;
    [SerializeField] private GameObject powerBirdAnim;

    [SerializeField] private GameObject[] polygons;
    [SerializeField] private GameObject[] corners;
    [SerializeField] public bool isPowerBird;
    //[SerializeField] private GameObject targetObject2;

    /*private float minx;
    private float maxx;
    private float miny;
    private float maxy;*/

    private Vector3 difference;
    private Vector2 tempPos;

    private bool canVibrate;
    private bool canMove;

    private Animator anim;
    private void Start()
    {
        powerBirdAnim.SetActive(false);
        canMove = true;
        anim = gameObject.GetComponent<Animator>();
        canVibrate = true;

        /*minx = targetObject.GetComponent<Collider2D>().bounds.min.x;
        maxx = targetObject.GetComponent<Collider2D>().bounds.max.x;
        miny = targetObject.GetComponent<Collider2D>().bounds.min.y;
        maxy = targetObject.GetComponent<Collider2D>().bounds.max.y;*/

        tempPos = gameObject.transform.position;

        int currentPoly = PlayerPrefs.GetInt("CurrentLevel", 1) - 1;
        if (currentPoly > 13)
            currentPoly = 13;

        corners = new GameObject[polygons[currentPoly].transform.childCount];

        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = polygons[currentPoly].transform.GetChild(i).gameObject;
        }
    }
    private void Update()
    {
        anim.SetBool("isTrigger", isTrigger);
        if (isTrigger && !gmeoverPanel.activeInHierarchy && !completePanel.activeInHierarchy && !completePanel4.activeInHierarchy && !completePanel9.activeInHierarchy && !completePanel14.activeInHierarchy && !completePanel25.activeInHierarchy)
        {
            if (IsMouseWithinObject(gameObject.transform.position, targetObject1))
            {
                // Debug.Log("Mouse is within the area of the target object!");
                tempPos = gameObject.transform.position;
                gameObject.transform.position = Input.mousePosition;
                FindObjectOfType<LevelManager>().CalculatePositions();
                FindObjectOfType<LevelManager>().UpdateLine();
            }
            else
            {

                /*if (gameObject.transform.position.x < minx)
                    tempPos.x = gameObject.transform.position.x + 1;
                if (gameObject.transform.position.x > maxx)
                    tempPos.x = gameObject.transform.position.x - 1;
                if (gameObject.transform.position.x < miny)
                    tempPos.x = gameObject.transform.position.y + 1;
                if (gameObject.transform.position.x > maxy)
                    tempPos.x = gameObject.transform.position.y - 1;

                gameObject.transform.position = tempPos;*/
            }
        }
        else
        {
            if (!completePanel.activeInHierarchy && !completePanel4.activeInHierarchy && !completePanel9.activeInHierarchy && !completePanel14.activeInHierarchy && !completePanel25.activeInHierarchy)
                gameObject.transform.position = tempPos;
        }

        for (int i = 0; i < corners.Length; i++)
        {
            difference = gameObject.transform.position - corners[i].transform.position;
            if (difference.x < 50f && difference.x > -50f && difference.y < 50f && difference.y > -50f)
            {
                isPlaced = true;
                if(!completePanel.activeInHierarchy && !completePanel4.activeInHierarchy && !completePanel9.activeInHierarchy && !completePanel14.activeInHierarchy && !completePanel25.activeInHierarchy)
                gameObject.transform.position = corners[i].transform.position;
                FindObjectOfType<LevelManager>().CalculatePositions();
                FindObjectOfType<LevelManager>().UpdateLine();
                if (canVibrate && !isTrigger)
                {
                    canVibrate = false;
                    if (isPowerBird)
                    {
                        canMove = false;
                        for (int j = 2; j < beeSpawnArea.transform.childCount; j++)
                        {
                            GameObject lightning= Instantiate(powerBirdAnim, beeSpawnArea.transform.GetChild(j).transform.localPosition, transform.rotation);
                            lightning.transform.position+=new Vector3(40,40,0);
                            lightning.SetActive(true);
                            lightning.transform.SetParent(lightningSpawnArea,false);
                            
                            beeSpawnArea.transform.GetChild(j).GetComponent<BeeScript>().canMove=false;
                        }
                        Invoke("Removebees",1.2f);
                    }
                    VibrationControls.instance.Vibrate();
                }
                break;
            }
            else
            {
                isPlaced = false;
            }
        }
    }
    public void TriggerDown()
    {
        if(canMove)
        isTrigger = true;
    }
    public void TriggerUp()
    {
        if (canMove)
        {
        isTrigger = false;
        canVibrate = true;
        FindObjectOfType<LevelManager>().noOfMoves--;
        }
    }

    bool IsMouseWithinObject(Vector3 mousePosition, GameObject gameObject1)
    {
        // Get the bounds of the target GameObject
        Collider2D collider1 = gameObject1.GetComponent<Collider2D>();
        //Collider2D collider2 = gameObject2.GetComponent<Collider2D>();
        if (collider1 != null)
        {
            // Check if the mouse position is within the bounds of the GameObject
            // if (collider1.bounds.Contains(mousePosition) || collider2.bounds.Contains(mousePosition))
            //   return true;
            // else
            //   return false;

            return collider1.bounds.Contains(mousePosition);
        }
        else
        {
            Debug.LogWarning("Target GameObject does not have a Collider2D component!");
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Butterfly" && isTrigger)
        {
            collision.GetComponent<ButterflyScript>().pointerDown();
        }
        else if(collision.gameObject.tag == "Bee" && isTrigger && !isPowerBird)
        {
            collision.GetComponent<BeeScript>().pointerDown();
        }
    }
    private void Removebees()
    {
        for (int j = 2; j < beeSpawnArea.transform.childCount; j++)
        {
            beeSpawnArea.transform.GetChild(j).gameObject.SetActive(false);
        }
    }
}
