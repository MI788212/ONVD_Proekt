using UnityEngine;
using System.Collections;

public class phoneScript : MonoBehaviour
{
    public GameObject mainCamera;
    private InteractingScript interactingScript;
    private PickUpScript pickUpScript;
    public GameObject player;
    private CameraControllerFPS camControllerFPS;
    private PlayerMovementBehavior playerMovementBehavior;
    public GameObject crosshair;
    public GameObject phone;

    private AudioManagerScript audioManagerScript;

    //calling number
    private string number;
    private int counter;
    private string lastNumber;
    private bool dialingDisabled;

    private void Awake()
    {
        interactingScript = mainCamera.GetComponent<InteractingScript>();
        pickUpScript = mainCamera.GetComponent<PickUpScript>();
        camControllerFPS = mainCamera.GetComponent<CameraControllerFPS>();
        playerMovementBehavior = player.GetComponent<PlayerMovementBehavior>();
        counter = 0;
        lastNumber = "";
        audioManagerScript = GameObject.FindGameObjectWithTag("audioManager").GetComponent<AudioManagerScript>();
        dialingDisabled = false;
    }
    //void OnEnable()
    //{
    //    Debug.Log("OnEnable phoneScript start");
    //    interactingScript.enabled = false;
    //    pickUpScript.enabled = false;
    //    camControllerFPS.enabled = false;
    //    playerMovementBehavior.enabled = false;
    //    crosshair.SetActive(false);
    //    Debug.Log("OnEnable phoneScript end");
    //}

    void OnEnable()
    {
        //Debug.Log("OnEnable phoneScript start");
        number = "";
        StartCoroutine(DisableNextFrame());
        dialingDisabled = false;
    }

    IEnumerator DisableNextFrame()
    {
        yield return null; // wait one frame
        //interactingScript.enabled = false;
        interactingScript.unresponsive = true;
        pickUpScript.enabled = false;
        camControllerFPS.enabled = false;
        playerMovementBehavior.enabled = false;
        crosshair.SetActive(false);
        Debug.Log("Actually disabled after one frame");
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        phone.SetActive(false);
        audioManagerScript.PlayLoop(audioManagerScript.dialTone);
    }

    // Update is called once per frame
    void Update()
    {
        if(counter == 10)
        {
            dialingDisabled = true;
            lastNumber = number;
            counter = 0;
            StartCoroutine(WaitTime(0.2f));
        }
    }

    public void wasClicked(string name)
    {
        if (!dialingDisabled)
        {
            Debug.Log(name + " was clicked!");
            if (name == "redialButton")
            {
                Debug.Log("redialing");
                audioManagerScript.PlaySFX(audioManagerScript.redialButton);
                interactingScript.callThisNumber(lastNumber);
                //number = "";
            }
            else if (name == "button1")
            {
                audioManagerScript.PlaySFX(audioManagerScript.button1);
                number += "1";
                counter++;
            }
            else if (name == "button2")
            {
                audioManagerScript.PlaySFX(audioManagerScript.button2);
                number += "2";
                counter++;
            }
            else if (name == "button3")
            {
                audioManagerScript.PlaySFX(audioManagerScript.button3);
                number += "3";
                counter++;
            }
            else if (name == "button4")
            {
                audioManagerScript.PlaySFX(audioManagerScript.button4);
                number += "4";
                counter++;
            }
            else if (name == "button5")
            {
                audioManagerScript.PlaySFX(audioManagerScript.button5);
                number += "5";
                counter++;
            }
            else if (name == "button6")
            {
                audioManagerScript.PlaySFX(audioManagerScript.button6);
                number += "6";
                counter++;
            }
            else if (name == "button7")
            {
                audioManagerScript.PlaySFX(audioManagerScript.button7);
                number += "7";
                counter++;
            }
            else if (name == "button8")
            {
                audioManagerScript.PlaySFX(audioManagerScript.button8);
                number += "8";
                counter++;
            }
            else if (name == "button9")
            {
                audioManagerScript.PlaySFX(audioManagerScript.button9);
                number += "9";
                counter++;
            }
            else if (name == "button0")
            {
                audioManagerScript.PlaySFX(audioManagerScript.button0);
                number += "0";
                counter++;
            }
            else if (name == "leftOfPhone" || name == "rightOfPhone")
            {
                gameObject.SetActive(false);
            }
        }
    }

    IEnumerator WaitTime(float time = 0.2f)
    {
        yield return new WaitForSeconds(time); // wait exactly 0.2 seconds
        Debug.Log("0.2 seconds passed!");

        interactingScript.callThisNumber(number);
        number = "";
        dialingDisabled = false;
    }

    public void OnDisable()
    {
        number = "";
        interactingScript.unresponsive = false;
        pickUpScript.enabled = true;
        camControllerFPS.enabled = true;
        playerMovementBehavior.enabled = true;
        crosshair.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        phone.SetActive(true);
        audioManagerScript.StopLoop();
    }
}
