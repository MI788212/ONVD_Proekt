using System.Xml;
using TMPro;
using UnityEngine;

public class InteractingScript : MonoBehaviour
{

    //the objects i need to interact with
    public GameObject candle;
    public GameObject WarmingArea;
    public GameObject fullTeaCup;
    public GameObject emptyTeaCup;
    public GameObject phone;
    public GameObject cluePaper;
    public GameObject safe;
    public GameObject key;

    public GameObject textGuide;

    //the scripts i need to access
    public PickUpScript pickUpScript;
    public CameraControllerFPS cameraControllerFPS;
    private teaCupScript teaCupScript;
    private textGuideScript textGuideScript;

    public float rayDistance = 5f;
    public LayerMask interactLayer;
    public float dia = 0; //its time for dialogue 0,1,2...

    //teaCup part variables
    private bool inWarmingArea = false;
    private bool warming = false;
    private bool warmedUp = false;
    private float warmingTimeCounter = 0f;
    private float requiredWarmingTime = 5f;
    public float heightAboveCandle = 0.3f;

    void Start()
    {
        teaCupScript = fullTeaCup.GetComponent<teaCupScript>();
        textGuideScript = textGuide.GetComponent<textGuideScript>();
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactLayer))
        {
            if (hit.collider.gameObject == fullTeaCup && dia==0)
            {
                Debug.Log("You could go for some tea right now. Pick it up (E)");
                dia=1;
               
            }
        }
        if(dia>=1&&dia<2 && pickUpScript.heldObj == fullTeaCup)
        {
            dia = 2;
            Debug.Log("Bleghhh, you refuse to drink cold tea. Maybe there's a way to warm it up?");
        }
        if(dia >= 2 && dia < 3 && inWarmingArea)
        {
            dia = 3;
            Debug.Log("Yesss, this candle will do nicely.\nPut the cup over it (F)");
        }
        if(dia >= 3 && dia < 4 && inWarmingArea && Input.GetKeyDown(KeyCode.F))
        {
            dia = 4;
            pickUpScript.StopClipping();
            pickUpScript.DropObject();
            fullTeaCup.transform.position = candle.transform.position + Vector3.up * heightAboveCandle;
            fullTeaCup.transform.rotation = Quaternion.identity;
            Debug.Log("It's cookin. Give it a sec.");
        }
        if (dia == 4.4f)
        {
            warmingTimeCounter += Time.deltaTime;
        }
        if (dia >= 4 && dia < 5 && warmingTimeCounter >= requiredWarmingTime)
        {
            dia = 5;
            Debug.Log("Done!! Drink up (F)");
            warming = false;
            warmedUp = true;
            warmingTimeCounter = 0f;
            WarmingArea.SetActive(false);
        }
        if(dia >= 5 && dia < 6 && Input.GetKeyDown(KeyCode.F))
        {
            dia = 6;
            Debug.Log("Still tasted yucky, but it warmed you up. And you seem to notice something. Try rotating the cup by holding R and moving your mouse.");
            fullTeaCup.SetActive(false);
            emptyTeaCup.transform.position = fullTeaCup.transform.position;
            emptyTeaCup.transform.rotation = fullTeaCup.transform.rotation;
            emptyTeaCup.gameObject.SetActive(true);
        }
    }

    public void CupInWarmingArea()
    {
        inWarmingArea = true;
    }

    public void CupExitedInWarmingArea()
    {
        inWarmingArea = false;
    }
}

/*
 using UnityEngine;


public class teaCupScript : MonoBehaviour
{
    private bool inWarmingArea = false;
    private float timeWarming = 0f;
    public float expectedWarmingTime = 5f; // seconds required
    public GameObject Candle;
    private PickUpScript pickUpScript;
    public GameObject mainCamera;
    public float heightAboveCandle = 0.3f;
    private bool warming = false;
    private bool warmedUp = false;
    public GameObject emptyTeaCup;

    private void Start()
    {
        pickUpScript= mainCamera.GetComponent<PickUpScript>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("warmingArea"))
        {
            Debug.Log("Press F to warm it up!");
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
            if (Input.GetKeyDown(KeyCode.F)&&!warming&&!warmedUp)
            {
                pickUpScript.StopClipping();
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
        if (Input.GetKeyDown(KeyCode.F))
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
        Debug.Log("Click F to drink up!");
    }

}

 */