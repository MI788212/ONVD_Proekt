using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;

public class CluePaperScript : MonoBehaviour
{
    public GameObject mainCamera;
    private InteractingScript interactingScript;
    private PickUpScript pickUpScript;
    public GameObject player;
    private CameraControllerFPS camControllerFPS;
    private PlayerMovementBehavior playerMovementBehavior;
    public GameObject crosshair;

    public GameObject cluePaper;

    private bool imageWasClicked;


    private void Awake()
    {
        interactingScript = mainCamera.GetComponent<InteractingScript>();
        pickUpScript = mainCamera.GetComponent<PickUpScript>();
        camControllerFPS = mainCamera.GetComponent<CameraControllerFPS>();
        playerMovementBehavior = player.GetComponent<PlayerMovementBehavior>();
    }

    void OnEnable()
    {
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
        cluePaper.SetActive(false);
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    imageWasClicked = true;
    //    Debug.Log("image was clicked1");
    //}

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        StartCoroutine(checkClickAfterFrame());
    //    }
    //}

    //IEnumerator checkClickAfterFrame()
    //{
    //    yield return null;
    //    if (imageWasClicked)
    //    {
    //        imageWasClicked = false;
    //        Debug.Log("image was clicked");
    //    }
    //    else
    //    {
    //        gameObject.SetActive(false);
    //        Debug.Log("clicked outside of image");
    //    }
    //}

    public void closeScreen()
    {
        gameObject.SetActive(false);
    }

    public void OnDisable()
    {
        interactingScript.unresponsive = false;
        pickUpScript.enabled = true;
        camControllerFPS.enabled = true;
        playerMovementBehavior.enabled = true;
        crosshair.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cluePaper.SetActive(true);
    }
}
