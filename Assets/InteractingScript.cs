using System.Xml;
using TMPro;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class InteractingScript : MonoBehaviour
{

    //the objects i need to interact with
    public GameObject candle;
    public GameObject warmingArea;
    public GameObject fullTeaCup;
    public GameObject emptyTeaCup;
    public GameObject phone;
    public GameObject cluePaper;
    public GameObject safe;
    public GameObject key;

    public GameObject textGuide;
    public GameObject textBox;

    //the scripts i need to access
    public PickUpScript pickUpScript;
    public CameraControllerFPS cameraControllerFPS;
    private teaCupScript teaCupScript;
    private textGuideScript textGuideScript;
    private DialogueScript dialogueScript;

    public float rayDistance = 5f;
    public LayerMask interactLayer;

    //teaCup part variables
    private bool inWarmingArea = false;
    private bool warming = false;
    private bool warmedUp = false;
    private float warmingTimeCounter = 0f;
    private float requiredWarmingTime = 5f;
    public float heightAboveCandle = 0.3f;

    //messages
    public GameObject pickUpMess;
    public GameObject whilePickedUpMess;
    public GameObject interactMess;
    public GameObject cookingMess;

    private int choice; // 0: drink, 1: call...

    public TextMeshProUGUI text;   // guideMess
    private int guideMessIndex = -1; // 0: try pick up, 1: try rotate..
    public float fadeInTime = 0.5f;     // Duration to fade in
    public float holdTime = 2f;         // How long it stays fully visible
    public float fadeOutTime = 1.5f;      // Duration to fade out


    void Awake()
    {
        teaCupScript = fullTeaCup.GetComponent<teaCupScript>();
        textGuideScript = textGuide.GetComponent<textGuideScript>();
        dialogueScript = textBox.GetComponent<DialogueScript>();
        textBox.SetActive(false);
        pickUpMess.SetActive(false);
        whilePickedUpMess.SetActive(false);
        interactMess.SetActive(false);
        cookingMess.SetActive(false);
        dialogueScript.hasChoice = false;
        guideMessIndex = -1;

    }

    private void Start()
    {
        ShowMessage("");
    }
    private void Update()
    {
        
        warmingArea.SetActive(pickUpScript.heldObj != null && !warmedUp);

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        whilePickedUpMess.SetActive(pickUpScript.heldObj != null);
        cookingMess.SetActive(warming);

        //interactMess.SetActive(Physics.Raycast(ray, out hit, rayDistance, interactLayer) && hit.collider.gameObject.CompareTag("canInteractWith") && pickUpScript.heldObj == null);

        if(guideMessIndex == 0)
        {
            guideMessIndex++;
            ShowMessage("Try picking up an object with <<E>>");
        }

        if (Physics.Raycast(ray, out hit, rayDistance, interactLayer) && hit.collider.gameObject.CompareTag("canPickUp") && pickUpScript.heldObj == null && !pickUpScript.justThrew)
        {
            //pickUpMess.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(hit.collider.gameObject == fullTeaCup)
                {
                    if(warmedUp)
                    {
                        dialogueScript.lines.Clear();
                        dialogueScript.lines.Add("It's warmed up.");
                        dialogueScript.lines.Add("Drink up?");
                        dialogueScript.hasChoice = true;
                        choice = 0;
                        textBox.SetActive(true);
                    }
                    else if(!warming)
                    {
                        dialogueScript.lines.Clear();
                        dialogueScript.lines.Add("Bleghhh...");
                        dialogueScript.lines.Add("You could go for some tea right now, but you refuse to drink it this cold.");
                        dialogueScript.lines.Add("Maybe there's a way to warm it up?");
                        guideMessIndex = 0;
                        textBox.SetActive(true);
                    }
                }
            }
        }
        else
        {
            //pickUpMess.SetActive(false);
        }

        if (inWarmingArea && pickUpScript.heldObj == null && !pickUpScript.justThrew && !warming && !warmedUp)
        {
            warmingTimeCounter = 0f;
            fullTeaCup.GetComponent<Rigidbody>().isKinematic = true;
            fullTeaCup.transform.position = candle.transform.position + Vector3.up * heightAboveCandle;
            fullTeaCup.transform.rotation = Quaternion.identity;
            warming = true;
            Debug.Log("It's cookin. Give it a sec.");
        }
        if (warming)
        {   
            pickUpScript.enabled = false;
            warmingTimeCounter += Time.deltaTime;
            if (warmingTimeCounter >= requiredWarmingTime)
            {
                Debug.Log("Done!! Drink up");
                warming = false;
                warmedUp = true;
                cookingMess.SetActive(false);
                pickUpScript.enabled = true;

                dialogueScript.lines.Clear();
                dialogueScript.lines.Add("It's warmed up.");
                dialogueScript.lines.Add("Drink up?");
                dialogueScript.hasChoice = true;
                choice = 0;
                textBox.SetActive(true);
            }
        }

    }

    public void CupInWarmingArea()
    {
        inWarmingArea = true;
    }
    public void CupExitedInWarmingArea()
    {
        inWarmingArea = false;
        warming = false;
    }

    public void madeChoice(bool yesChoice)
    {
        switch (choice)
        {
            case 0:
                if(yesChoice)
                {
                    Debug.Log("drank tea");
                    Debug.Log("Still tasted yucky, but it warmed you up. And you seem to notice something. Try rotating the cup by holding R and moving your mouse.");
                    fullTeaCup.SetActive(false);
                    emptyTeaCup.transform.position = fullTeaCup.transform.position;
                    emptyTeaCup.transform.rotation = fullTeaCup.transform.rotation;
                    emptyTeaCup.gameObject.SetActive(true);

                    dialogueScript.lines.Clear();
                    dialogueScript.lines.Add("Still tasted yucky, but it warmed you up. And you seem to notice something.");
                    textBox.SetActive(true);
                }
                else
                {
                    Debug.Log("didnt drink tea");
                }
                break;

            default: break;
        }
    }

    public void ShowMessage(string message)
    {
        StopAllCoroutines();            
        StartCoroutine(FadeInOut(message));
    }

    private IEnumerator FadeInOut(string message)
    {
        text.text = message;

        // Initialize
        text.alpha = 0f;

        float elapsed = 0f;

        // --- Fade in ---
        while (elapsed < fadeInTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeInTime);
            text.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }
        text.alpha = 1f;

        // --- Hold ---
        yield return new WaitForSeconds(holdTime);

        // --- Fade out ---
        elapsed = 0f;
        while (elapsed < fadeOutTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeOutTime);
            text.alpha = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }
        text.alpha = 0f;
    }
}





//OLD SCRIPT//

/*    void Start()
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
}*/





////////////////////////////////////////
//OLD TEACUP SCRIPT
////////////////////////////////////////




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