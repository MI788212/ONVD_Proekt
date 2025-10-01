using System.Xml;
using TMPro;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

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
    public GameObject prozor;
    public GameObject bed;
    public GameObject underBed;
    public GameObject fioka1;
    public GameObject fioka2;
    public GameObject fioka3;
    public GameObject phonebook;
    public GameObject door;
    public GameObject doorKnob;
    public GameObject keyHoleArea;
    public ParticleSystem confetti1;
    public ParticleSystem confetti2;
    public GameObject lightSwitch;
    public GameObject chair;
    public GameObject wallStains;

    public GameObject textGuide;
    public GameObject textBox;

    //the scripts i need to access
    public PickUpScript pickUpScript;
    public CameraControllerFPS cameraControllerFPS;
    private teaCupScript teaCupScript;
    private textGuideScript textGuideScript;
    private DialogueScript dialogueScript;
    private AudioManagerScript audioManagerScript;
    private PlayerMovementBehavior playerMovementBehavior;

    public float rayDistance = 5f;
    public LayerMask interactLayer;

    //teaCup part variables
    private bool inWarmingArea = false;
    private bool warming = false;
    private bool warmedUp = false;
    private float warmingTimeCounter = 0f;
    private float requiredWarmingTime = 5f;
    public float heightAboveCandle = 0.3f;

    //key variable
    private bool canUnlockDoor;
    private bool unlockedDoor;

    //messages
    public GameObject pickUpMess;
    public GameObject whilePickedUpMess;
    public GameObject interactMess;
    public GameObject cookingMess;

    private int choice; // 0: drink, 1: call, 2:take safe out, 3:to enter pass, 4:phonebook, 5:clue paper
    private bool tookSafeOut;
    private bool safeOpened;
    private bool fioka1opened;
    private int fioka2indeks;
    private bool fioka2opened;


    public TextMeshProUGUI text;   // guideMess
    private int guideMessIndex = -1; // 0: try pick up, 1: try rotate..
    public float fadeInTime = 0.5f;     // Duration to fade in
    public float holdTime = 2f;         // How long it stays fully visible, DISABLED
    public float fadeOutTime = 1.5f;      // Duration to fade out

    public TextMeshProUGUI hint;
    private bool calledCorrectNumber = false;
    private bool accessedFioka3 = false;

    //screens
    public GameObject phoneScreen;
    private phoneScript phoneScript;
    public GameObject underBedScreen;
    public RawImage blackScreen;
    public GameObject safeScreen;
    public GameObject phonebookScreen;
    public GameObject cluePaperScreen;

    public GameObject crossHair;

    public GameObject menu;

    //is this script active?
    public bool unresponsive;

    //phone placement relative to the camera and original phone placement
    public Vector3 phoneOffset = new Vector3(-0.038f, -0.107f, 0.267f);
    public Quaternion phoneRotationOffset = new Quaternion(-0.187f, -0.056f, 0.096f, 0.976f);
    private Vector3 phoneOriginalPosition;
    private Quaternion phoneOriginalRotation;


    void Awake()
    {
        unresponsive = false;
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
        phoneScreen.SetActive(false);
        phoneScript = phoneScreen.GetComponent<phoneScript>();
        underBedScreen.SetActive(false);
        blackScreen.enabled = false;
        tookSafeOut = false;
        safeScreen.SetActive(false);
        safeOpened = false;
        fioka1opened = false;
        fioka2indeks = 0;
        fioka2opened = false;
        phonebookScreen.SetActive(false);
        cluePaperScreen.SetActive(false);
        audioManagerScript = GameObject.FindGameObjectWithTag("audioManager").GetComponent<AudioManagerScript>();
        crossHair.SetActive(true);
        playerMovementBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementBehavior>();
        keyHoleArea.SetActive(false);
        canUnlockDoor = false;
        confetti1.Stop();
        confetti2.Stop();
        unlockedDoor = false;
        menu.SetActive(false);
    }

    private void Start()
    {
        ShowMessage("");
        hint.text = "Try warming the tea by placing the mug on the candle.";
    }
    private void Update()
    {
        
        warmingArea.SetActive(pickUpScript.heldObj != null && !warmedUp);
        keyHoleArea.SetActive(pickUpScript.heldObj == key);

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        //whilePickedUpMess.SetActive(pickUpScript.heldObj != null);
        cookingMess.SetActive(warming);
        
        //interactMess.SetActive(Physics.Raycast(ray, out hit, rayDistance, interactLayer) && hit.collider.gameObject.CompareTag("canInteractWith") && pickUpScript.heldObj == null);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(true);
        }

        if (guideMessIndex == 0)
        {
            guideMessIndex=-1;
            ShowMessage("Try picking up an object with <<E>>",3f);
        }
        else if(guideMessIndex == 1)
        {
            guideMessIndex = -1;
            ShowMessage("Rotate an object when picked up, by holding <<R>> and draging your mouse",4f);
        }

        if(pickUpScript.heldObj == fioka2 )
        {
            accessedFioka3 = true;
        }

        if (calledCorrectNumber)
        {
            if (!tookSafeOut)
            {
                hint.text = "Check under the bed.";
            }
            else if (!accessedFioka3)
            {
                hint.text = "See what hides in the third drawer by taking the middle one out.";
            }
            else {
                hint.text = "Use the system written on the paper to decode the call.\n\nX knocks and Y snaps intersect on the table to create one of the digits 1-9. For example:\n'knock knock snap, knock snap snap, knock snap snap snap' -> 4 2 3\n\nDon't forget the exception for digit 0, which is represented by 4 knocks. ";
            }
        }

        if (Physics.Raycast(ray, out hit, rayDistance, interactLayer) && (hit.collider.gameObject.CompareTag("canPickUp")||hit.collider.gameObject.CompareTag("canInteractWith")) && pickUpScript.heldObj == null && !pickUpScript.justThrew && !unresponsive)
        {
            //pickUpMess.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (hit.collider.gameObject == fullTeaCup)
                {
                    if (warmedUp)
                    {
                        dialogueScript.lines.Clear();
                        //dialogueScript.lines.Add("It's warmed up.");
                        dialogueScript.lines.Add("Drink up?");
                        dialogueScript.hasChoice = true;
                        choice = 0;
                        textBox.SetActive(true);
                    }
                    else if (!warming)
                    {
                        dialogueScript.lines.Clear();
                        dialogueScript.lines.Add("Bleghhh...");
                        dialogueScript.lines.Add("You could go for some tea right now, but you refuse to drink it this cold.");
                        dialogueScript.lines.Add("Maybe there's a way to warm it up?");
                        guideMessIndex = 0;
                        textBox.SetActive(true);
                    }
                }
                else if (hit.collider.gameObject == phone)
                {
                    dialogueScript.lines.Clear();
                    dialogueScript.lines.Add("Is there someone you want to call?"); 
                    dialogueScript.hasChoice = true;
                    choice = 1;
                    textBox.SetActive(true);
                }
                else if(hit.collider.gameObject == emptyTeaCup)
                {
                    dialogueScript.lines.Clear();
                    dialogueScript.lines.Add("Still tasted yucky, but it warmed you up.");
                    dialogueScript.lines.Add("And you seem to notice something...");
                    textBox.SetActive(true);
                    guideMessIndex = 1;
                }
                else if(hit.collider.gameObject == prozor)
                {
                    dialogueScript.lines.Clear();
                    dialogueScript.lines.Add("You don't feel compelled to question the void.");
                    textBox.SetActive(true);
                }
                else if(hit.collider.gameObject == bed)
                {
                    dialogueScript.lines.Clear();
                    dialogueScript.lines.Add("You feel compelled to sleep.");
                    textBox.SetActive(true);
                }
                else if(hit.collider.gameObject == underBed && !tookSafeOut)
                {
                    fadeInScreen(underBedScreen);
                    dialogueScript.lines.Clear();
                    dialogueScript.lines.Add("There's a safe safely tucked under the bed.");
                    dialogueScript.lines.Add("Do you take it out?");
                    dialogueScript.hasChoice = true;
                    choice = 2;
                    textBox.SetActive(true);
                }
                else if(hit.collider.gameObject == candle)
                {
                    dialogueScript.lines.Clear();
                    dialogueScript.lines.Add("This candle seems awfully warm.");
                    textBox.SetActive(true);
                }
                else if(hit.collider.gameObject == safe && !safeOpened)
                {
                    dialogueScript.lines.Clear();
                    dialogueScript.lines.Add("Ready to enter your password?");
                    dialogueScript.hasChoice = true;
                    choice = 3;
                    textBox.SetActive(true);
                }
                else if(hit.collider.gameObject == fioka1)
                {
                    if (!fioka1opened)
                    {
                        audioManagerScript.PlaySFX(audioManagerScript.openDrawer);
                        fioka1.transform.localPosition = new Vector3(-1.44603825f, 0.453433216f, 1.26699996f);
                        fioka1opened = true;
                    }
                    else
                    {
                        audioManagerScript.PlaySFX(audioManagerScript.closeDrawer);
                        fioka1.transform.localPosition = new Vector3(-1.44603825f, 0.453433216f, 1.65363026f);
                        fioka1opened = false;
                    }
                }
                else if(hit.collider.gameObject == fioka2)
                {
                    if (!fioka2opened)
                    {
                        audioManagerScript.PlaySFX(audioManagerScript.openDrawer);
                        fioka2opened = true;
                        fioka2.transform.localPosition += new Vector3(0, 0, -0.4f);
                    }
                    else
                    {
                        audioManagerScript.PlaySFX(audioManagerScript.closeDrawer);
                        fioka2opened = false;
                        fioka2.transform.localPosition += new Vector3(0, 0, 0.4f);
                    }
                    //Debug.Log("trying to open fioka2");
                    //if (fioka2indeks <2)
                    //{
                    //    fioka2indeks++;
                    //}
                    //else if ((fioka2indeks >= 2 && fioka2indeks < 5) || fioka2indeks == 6)
                    //{
                    //    fioka2indeks++;
                    //    fioka2.transform.localPosition += new Vector3(0, 0, -0.01f);
                    //}
                    //else if (fioka2indeks == 5 || fioka2indeks == 7)
                    //{
                    //    fioka2indeks++;
                    //    fioka2.transform.localPosition += new Vector3(0, 0, -0.1f);
                    //}
                    //else if(fioka2indeks == 8)
                    //{
                    //    fioka2indeks = 9;
                    //    fioka2.transform.localPosition += new Vector3(0, 0, -0.2f);
                    //}
                    //else if(fioka2indeks == 9)
                    //{
                    //    fioka2indeks = 10;
                    //    fioka2.transform.localPosition = new Vector3(0.25f, -0.0199999996f, -0.529999971f);
                    //    fioka2.transform.localRotation = new Quaternion(-0.00165190746f, -0.0865471512f, -0.000143506564f, 0.996246397f);
                    //}
                }
                else if(hit.collider.gameObject == fioka3 && fioka2indeks!=10 && !accessedFioka3)
                {
                    audioManagerScript.PlaySFX(audioManagerScript.lockedDrawer);
                    unresponsive = true;
                    pickUpScript.enabled = false;
                    cameraControllerFPS.enabled = false;
                    crossHair.SetActive(false);
                    playerMovementBehavior.enabled = false;
                    WaitSeconds(0.5f, () => {
                    dialogueScript.lines.Clear();
                    dialogueScript.lines.Add("This one is completely shut.");
                    textBox.SetActive(true);
                    });
                }
                else if (hit.collider.gameObject == phonebook)
                {
                    dialogueScript.lines.Clear();
                    dialogueScript.lines.Add("It's a phone book.");
                    dialogueScript.lines.Add("Do you open it?");
                    dialogueScript.hasChoice = true;
                    choice = 4;
                    textBox.SetActive(true);
                }
                else if(hit.collider.gameObject == cluePaper)
                {
                    dialogueScript.lines.Clear();
                    dialogueScript.lines.Add("Do you want to see it up close?");
                    dialogueScript.hasChoice = true;
                    choice = 5;
                    textBox.SetActive(true);
                }
                else if(hit.collider.gameObject == door || hit.collider.gameObject == doorKnob)
                {
                    audioManagerScript.PlaySFX(audioManagerScript.lockedDoor);
                    unresponsive = true;
                    pickUpScript.enabled = false;
                    cameraControllerFPS.enabled = false;
                    crossHair.SetActive(false);
                    playerMovementBehavior.enabled = false;
                    WaitSeconds(0.7f, () => {
                        dialogueScript.lines.Clear();
                        dialogueScript.lines.Add("You try the door, but it won't budge.");
                        textBox.SetActive(true);
                    });
                }
                else if(hit.collider.gameObject == lightSwitch)
                {
                    audioManagerScript.PlaySFX(audioManagerScript.lightSwitch);
                    unresponsive = true;
                    pickUpScript.enabled = false;
                    cameraControllerFPS.enabled = false;
                    crossHair.SetActive(false);
                    playerMovementBehavior.enabled = false;
                    WaitSeconds(0.7f, () => {
                        dialogueScript.lines.Clear();
                        dialogueScript.lines.Add("Nothing happened.");
                        textBox.SetActive(true);
                    });
                }
                else if(hit.collider.gameObject == key)
                {
                    dialogueScript.lines.Clear();
                    dialogueScript.lines.Add("It's your only way out.");
                    dialogueScript.lines.Add("Try not to lose it.");
                    textBox.SetActive(true);
                }
                else if(hit.collider.gameObject == chair)
                {
                    dialogueScript.lines.Clear();
                    dialogueScript.lines.Add("Your back hurts just looking at it.");
                    textBox.SetActive(true);
                }
                else if (hit.collider.gameObject == wallStains)
                {
                    dialogueScript.lines.Clear();
                    dialogueScript.lines.Add("You took those picture frames down.");
                    dialogueScript.lines.Add("They were empty anyway.");
                    textBox.SetActive(true);
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
            unresponsive = true;
            pickUpScript.enabled = false;
            warmingTimeCounter += Time.deltaTime;
            if (warmingTimeCounter >= requiredWarmingTime)
            {
                //Debug.Log("Done!! Drink up");
                warming = false;
                unresponsive = false;
                warmedUp = true;
                hint.text = "Drink the tea.";
                cookingMess.SetActive(false);
                pickUpScript.enabled = true;

                dialogueScript.lines.Clear();
                dialogueScript.lines.Add("It warmed up.");
                //dialogueScript.lines.Add("Drink up?");
                //dialogueScript.hasChoice = true;
                //choice = 0;
                textBox.SetActive(true);
            }
        }
        if (canUnlockDoor && pickUpScript.heldObj == null && !pickUpScript.justThrew && !unlockedDoor)
        {
            Debug.Log("Unlocked door");
            canUnlockDoor = false;
            unlockedDoor = true;

            pickUpScript.enabled = false;
            unresponsive = true;
            crossHair.SetActive(false);

            key.transform.SetParent(door.transform);
            key.GetComponent<Rigidbody>().isKinematic = true;
            key.transform.localPosition = new Vector3(0.101000071f, -0.0400000215f, 0.404999733f);
            key.transform.localRotation = Quaternion.Euler(70.5835648f, 253.215179f, 72.235405f);
            audioManagerScript.PlaySFX(audioManagerScript.doorUnlocks);
            WaitSeconds(1, () =>
            {
                key.transform.localRotation = Quaternion.Euler(4.07513857f, 341.392944f, 178.600021f);
                WaitSeconds(1.5f, () =>
                {
                    audioManagerScript.PlaySFX(audioManagerScript.doorOpens);
                    WaitSeconds(0.2f, () =>
                    {
                        door.transform.localPosition = new Vector3(-2.63499999f, 1, -0.272000015f);
                        door.transform.localRotation = Quaternion.Euler(0, 290.67981f, 0);
                        WaitSeconds(2, () => {
                            audioManagerScript.PlaySFX(audioManagerScript.kidsCheer);
                            audioManagerScript.PlaySFX(audioManagerScript.confettiBlast);
                            confetti1.Play();
                            confetti2.Play();
                            WaitSeconds(2, () => {
                                dialogueScript.lines.Clear();
                                dialogueScript.lines.Add("Congratulations!");
                                dialogueScript.lines.Add("You now have access to the whole void, although you may have prefered not to.");
                                textBox.SetActive(true);
                            });
                        });
                    });

                });
            });
        }

    }

    void fadeInScreen(GameObject screenObject)
    {
        screenObject.SetActive(true);
        blackScreen.enabled = true;
        blackScreen.color = new Color(0, 0, 0, 1);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / 1f);
            blackScreen.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        blackScreen.color = new Color(0, 0, 0, 0); 
        blackScreen.enabled=false;
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

    public void KeyEnteredKeyHoleArea()
    {
        Debug.Log("key entered keyHoleArea");
        canUnlockDoor = true;
    }

    public void KeyExitedKeyHoleArea()
    {
        canUnlockDoor = false;
    }

    public void madeChoice(bool yesChoice)
    {
        //audioManagerScript.PlaySFX(audioManagerScript.choice);
        switch (choice)
        {
            case 0:
                if(yesChoice)
                {
                    Debug.Log("drank tea");
                    hint.text = "Try calling the number with the initials written on the bottom of the mug.";
                    audioManagerScript.PlaySFX(audioManagerScript.slurp);
                    WaitFrames(1, () =>
                    {
                        unresponsive = true;
                        pickUpScript.enabled = false;
                        cameraControllerFPS.enabled = false;
                        crossHair.SetActive(false);
                        playerMovementBehavior.enabled = false;
                        fullTeaCup.SetActive(false);
                        emptyTeaCup.transform.position = fullTeaCup.transform.position;
                        emptyTeaCup.transform.rotation = fullTeaCup.transform.rotation;
                        emptyTeaCup.gameObject.SetActive(true);
                        WaitSeconds(2, () => {
                            StartCoroutine(DoAfterFrame());
                        });
                    });
                }
                else
                {
                    Debug.Log("didnt drink tea");
                }
                break;
            case 1:
                if (yesChoice)
                {
                    Debug.Log("wants to call");
                    phoneScreen.SetActive(true);
                }
                else
                {
                    Debug.Log("doesnt want to call");
                }
                break;
            case 2:
                underBedScreen.SetActive(false);
                if (yesChoice)
                {
                    Debug.Log("takes the safe out");
                    tookSafeOut = true;
                    safe.transform.position = new Vector3(1.8526899f, 0.492f, 10.170080f);
                    safe.transform.rotation = Quaternion.Euler(0, 279.273041f, 0);
                    key.transform.position = safe.transform.Find("safe").position;
                    key.transform.rotation = safe.transform.Find("safe").rotation;
                }
                else
                {
                    Debug.Log("doesnt take the safe out");
                }
                break;
            case 3:
                if (yesChoice)
                {
                    Debug.Log("ready to enter password");
                    fadeInScreen(safeScreen);
                }
                else
                {
                    Debug.Log("not ready to enter password");
                }
                break;
            case 4:
                if (yesChoice)
                {
                    Debug.Log("opens phonebook");
                    phonebookScreen.SetActive(true);
                }
                else
                {
                    Debug.Log("doesnt open phonebook");
                }
                break;
            case 5:
                if (yesChoice)
                {
                    Debug.Log("see clue paper up close");
                    audioManagerScript.PlaySFX(audioManagerScript.paper);
                    cluePaperScreen.SetActive(true);
                }
                else
                {
                    Debug.Log("dont see clue paper up close");
                }
                break;
            default: break;
        }
    }

    public void callThisNumber(string number)
    {
        if(number == "5552781022")
        {
            Debug.Log("called the right number.");
            phoneScreen.SetActive(false);
            phoneOriginalPosition = phone.transform.position;
            phoneOriginalRotation = phone.transform.rotation;
            audioManagerScript.PlaySFX(audioManagerScript.phoneCall);
            AttachPhone(21.4f);
            calledCorrectNumber = true;
        }
        else
        {
            Debug.Log("called a wrong number.");
            phoneScreen.SetActive(false);
            phoneOriginalPosition = phone.transform.position;
            phoneOriginalRotation = phone.transform.rotation;
            audioManagerScript.PlaySFX(audioManagerScript.wrongCall);
            AttachPhone(7);
        }
    }

    void AttachPhone(float delay = 1f)
    {
        Rigidbody rb = phone.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        phone.transform.SetParent(gameObject.transform);
        phone.transform.localPosition = phoneOffset;
        phone.transform.localRotation = phoneRotationOffset;

        StartCoroutine(DetachAfterDelay(delay));
    }

    private IEnumerator DetachAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DetachPhone();
    }

    void DetachPhone()
    {
        phone.transform.SetParent(null);

        Rigidbody rb = phone.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;
        phone.transform.position = phoneOriginalPosition;
        phone.transform.rotation = phoneOriginalRotation;
    }

    public void enterThisPin(string number)
    {
        if (number == "2507")
        {
            Debug.Log("entered the right pin.");
            audioManagerScript.PlaySFX(audioManagerScript.correctPin);
            WaitSeconds(0.5f, () =>
            {
                safeScreen.SetActive(false);
                audioManagerScript.PlaySFX(audioManagerScript.openSafe);
                safe.transform.Find("safe_door").transform.localPosition = new Vector3(0.5312f, 0.1019339f, 0.4281f);
                safe.transform.Find("safe_door").transform.localRotation = Quaternion.Euler(0f, -128.281f, -180f);
                safeOpened = true;
                safe.GetComponent<BoxCollider>().enabled = false;
            });
        }
        else
        {
            audioManagerScript.PlaySFX(audioManagerScript.wrongPin);
            Debug.Log("entered the wrong pin.");
        }
    }

    public void ShowMessage(string message, float holdTime = 2f)
    {
        StopAllCoroutines();            
        StartCoroutine(FadeInOut(message,holdTime));
    }

    private IEnumerator FadeInOut(string message, float holdTime)
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

    IEnumerator DoAfterFrame()
    {
        yield return null;
        dialogueScript.lines.Clear();
        dialogueScript.lines.Add("Still tasted yucky, but it warmed you up.");
        dialogueScript.lines.Add("And you seem to notice something...");
        textBox.SetActive(true);
        guideMessIndex = 1;
        Debug.Log("Still tasted yucky, but it warmed you up. And you seem to notice something. Try rotating the cup by holding R and moving your mouse.");
    }

    public void WaitFrames(int frameCount, Action afterWait)
    {
        StartCoroutine(WaitFramesRoutine(frameCount, afterWait));
    }

    private IEnumerator WaitFramesRoutine(int frameCount, Action afterWait)
    {
        for (int i = 0; i < frameCount; i++)
        {
            yield return null; 
        }

        afterWait?.Invoke(); 
    }

    public void WaitSeconds(float seconds, Action afterWait)
    {
        StartCoroutine(WaitSecondsRoutine(seconds, afterWait));
    }

    private IEnumerator WaitSecondsRoutine(float  seconds, Action afterWait)
    {
        yield return new WaitForSeconds(seconds);
        afterWait?.Invoke();
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