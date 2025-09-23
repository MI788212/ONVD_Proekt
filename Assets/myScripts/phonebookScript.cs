using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class phonebookScript : MonoBehaviour
{
    public GameObject mainCamera;
    private InteractingScript interactingScript;
    private PickUpScript pickUpScript;
    public GameObject player;
    private CameraControllerFPS camControllerFPS;
    private PlayerMovementBehavior playerMovementBehavior;
    public GameObject crosshair;
    public GameObject phonebook;

    private AudioManagerScript audioManagerScript;

    public Sprite[] images;
    private int currentIndex = 0;
    public UnityEngine.UI.Image uiImage;   

    private void Awake()
    {
        interactingScript = mainCamera.GetComponent<InteractingScript>();
        pickUpScript = mainCamera.GetComponent<PickUpScript>();
        camControllerFPS = mainCamera.GetComponent<CameraControllerFPS>();
        playerMovementBehavior = player.GetComponent<PlayerMovementBehavior>();
        audioManagerScript = GameObject.FindGameObjectWithTag("audioManager").GetComponent<AudioManagerScript>();
    }

    void OnEnable()
    {
        uiImage.sprite = images[currentIndex];
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
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        UnityEngine.Cursor.visible = true;
        phonebook.SetActive(false);
    }


    public void NextImage()
    {
        if(currentIndex < images.Length-1)
        {
            audioManagerScript.PlaySFX(audioManagerScript.paper);
            currentIndex++;
            uiImage.sprite = images[currentIndex];
        }
    }

    public void PreviousImage()
    {
        if(currentIndex > 0)
        {
            audioManagerScript.PlaySFX(audioManagerScript.paper);
            currentIndex--;
            uiImage.sprite = images[currentIndex];
        }
    }

    public void closeBook()
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
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        phonebook.SetActive(true);
    }
}
