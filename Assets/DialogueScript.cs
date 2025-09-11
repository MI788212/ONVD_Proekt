using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class DialogueScript : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public List<string> lines = new List<string>();
    public float textSpeed = 0.2f;
    private int index;
    public GameObject mainCamera;
    private InteractingScript interactingScript;
    private PickUpScript pickUpScript;
    public GameObject player;
    private CameraControllerFPS camControllerFPS;
    private PlayerMovementBehavior playerMovementBehavior;
    public GameObject crosshair;
    public bool hasChoice = false;
    public GameObject choiceBoxes;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        interactingScript = mainCamera.GetComponent<InteractingScript>();
        pickUpScript= mainCamera.GetComponent<PickUpScript>();
        camControllerFPS = mainCamera.GetComponent<CameraControllerFPS>();
        playerMovementBehavior = player.GetComponent<PlayerMovementBehavior>();
    }
    void OnEnable()
    {
        textComponent.text = string.Empty;
        StartDialogue();
        interactingScript.enabled = false;
        pickUpScript.enabled = false;
        camControllerFPS.enabled = false;
        playerMovementBehavior.enabled = false;
        crosshair.SetActive(false);
        choiceBoxes.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (index == lines.Count - 1 && hasChoice)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            choiceBoxes.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if(index < lines.Count - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            interactingScript.enabled = true;
            pickUpScript.enabled = true;
            camControllerFPS.enabled = true;
            playerMovementBehavior.enabled = true;
            crosshair.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            hasChoice = false;
        }
    }

    public void choseYes()
    {
        interactingScript.madeChoice(true);
        gameObject.SetActive(false);
        interactingScript.enabled = true;
        pickUpScript.enabled = true;
        camControllerFPS.enabled = true;
        playerMovementBehavior.enabled = true;
        crosshair.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        hasChoice = false;

    }
    public void choseNo() 
    {
        interactingScript.madeChoice(false);
        gameObject.SetActive(false);
        interactingScript.enabled = true;
        pickUpScript.enabled = true;
        camControllerFPS.enabled = true;
        playerMovementBehavior.enabled = true;
        crosshair.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        hasChoice = false;
    }
}
