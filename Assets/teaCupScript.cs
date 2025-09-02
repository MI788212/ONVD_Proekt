using UnityEngine;


public class teaCupScript : MonoBehaviour
{
    private bool inWarmingArea = false;
    private float timeWarming = 0f;
    public float expectedWarmingTime = 5f; // seconds required
    public GameObject Candle;
    private PickUpScript pickUpScript;
    public GameObject camera;
    public float heightAboveCandle = 0.3f;
    private bool warming = false;
    private bool warmedUp = false;
    public GameObject emptyTeaCup;

    private void Start()
    {
        pickUpScript= camera.GetComponent<PickUpScript>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("warmingArea"))
        {
            Debug.Log("Press I to warm it up!");
            inWarmingArea = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("warmingArea"))
        {
            inWarmingArea = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inWarmingArea)
        {
            if (Input.GetKeyDown(KeyCode.I)&&!warming&&!warmedUp)
            {
                pickUpScript.DropObject();
                transform.position = Candle.transform.position + Vector3.up * heightAboveCandle;
                transform.rotation = Quaternion.identity;
                warming = true;
            }
            if (warming)
            {
                timeWarming += Time.deltaTime;

                if (timeWarming >= expectedWarmingTime)
                {
                    Debug.Log("Object warmed up!");
                    warming = false;
                    warmedUp = true;
                    timeWarming = 0f;
                    GameObject.FindWithTag("warmingArea").SetActive(false);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (warmedUp)
            {
                Debug.Log("Still yucky, but at least it warmed you up.");
                gameObject.SetActive(false);
                emptyTeaCup.transform.position = transform.position;
                emptyTeaCup.transform.rotation = transform.rotation;
                emptyTeaCup.gameObject.SetActive(true);
                pickUpScript.PickUpObject(emptyTeaCup);
            }
            else
            {
                Debug.Log("Bleghhh, you refuse to drink cold tea.");
            }
        }
            
    }
    public void teaCupJustPickedUp()
    {
        Debug.Log("Click I to drink up!");
    }

}
