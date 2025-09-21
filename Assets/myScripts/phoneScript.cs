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

    //calling number
    private string number;
    private int counter;
    private string lastNumber;

    private void Awake()
    {
        interactingScript = mainCamera.GetComponent<InteractingScript>();
        pickUpScript = mainCamera.GetComponent<PickUpScript>();
        camControllerFPS = mainCamera.GetComponent<CameraControllerFPS>();
        playerMovementBehavior = player.GetComponent<PlayerMovementBehavior>();
        counter = 0;
        lastNumber = "";
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
    }

    // Update is called once per frame
    void Update()
    {
        if(counter == 10)
        {
            lastNumber = number;
            counter = 0;
            interactingScript.callThisNumber(number);
        }
    }

    public void wasClicked(string name)
    {
        Debug.Log(name + " was clicked!");
        if (name == "redialButton")
        {
            Debug.Log("redialing");
            interactingScript.callThisNumber(lastNumber);
            //number = "";
        }
        else if(name == "button1")
        {
            number += "1";
            counter++;
        }
        else if(name == "button2")
        {
            number += "2";
            counter++;
        }
        else if(name == "button3")
        {
            number += "3";
            counter++;
        }
        else if (name == "button4")
        {
            number += "4";
            counter++;
        }
        else if (name == "button5")
        {
            number += "5";
            counter++;
        }
        else if (name == "button6")
        {
            number += "6";
            counter++;
        }
        else if (name == "button7")
        {
            number += "7";
            counter++;
        }
        else if (name == "button8")
        {
            number += "8";
            counter++;
        }
        else if (name == "button9")
        {
            number += "9";
            counter++;
        }
        else if (name == "button0")
        {
            number += "0";
            counter++;
        }
        else if (name == "leftOfPhone" || name == "rightOfPhone")
        {
            gameObject.SetActive(false);
        }
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
    }
}
