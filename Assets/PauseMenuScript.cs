using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    private InteractingScript interactingScript;
    private PickUpScript pickUpScript;
    private CameraControllerFPS camControllerFPS;
    private PlayerMovementBehavior playerMovementBehavior;
    public GameObject crosshair;
    public GameObject menuButtons;

    private bool IS, PUS, CCFPS, PMB, CH, LS;

    private void Awake()
    {
        interactingScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<InteractingScript>();
        pickUpScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PickUpScript>();
        camControllerFPS = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControllerFPS>();
        playerMovementBehavior = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementBehavior>();
    }
    private void OnEnable()
    {
        IS = interactingScript.unresponsive;
        interactingScript.unresponsive = true;

        PUS = pickUpScript.enabled;
        pickUpScript.enabled = false;

        CCFPS = camControllerFPS.enabled;
        camControllerFPS.enabled = false;

        PMB = playerMovementBehavior.enabled;
        playerMovementBehavior.enabled = false;

        CH = crosshair.activeSelf;
        crosshair.SetActive(false);


        Time.timeScale = 0f;
        AudioListener.pause = true;

        if(Cursor.lockState == CursorLockMode.Locked) LS = true;
        else LS = false;
            Cursor.lockState = CursorLockMode.Confined;

        Cursor.visible = true;
        DeactivateDirectChildren();
        menuButtons.SetActive(true);
    }

    private void OnDisable()
    {
        interactingScript.unresponsive = IS;
        pickUpScript.enabled = PUS;
        camControllerFPS.enabled = CCFPS;
        playerMovementBehavior.enabled = PMB;
        crosshair.SetActive(CH);
        Time.timeScale = 1f;   
        AudioListener.pause = false;
        if (LS)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void DeactivateDirectChildren()
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
