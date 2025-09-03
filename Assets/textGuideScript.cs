using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class textGuideScript : MonoBehaviour
{
    public GameObject mainCamera;
    private InteractingScript interactingScript;
    private TMP_Text tmpText;
    private RawImage textBox;
    

    void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        interactingScript = mainCamera.GetComponent<InteractingScript>();
        textBox = gameObject.transform.parent.gameObject.GetComponent<RawImage>();
    }

    void Update()
    {
        switch (interactingScript.dia)
        {
            case 0:
                tmpText.enabled = false;
                textBox.enabled = false;
                break;
            case 1:
                tmpText.text = "You could go for some tea right now.\n\n(Press spacebar to continue this conversation)";
                tmpText.enabled = true;
                textBox.enabled = true;
                if (Input.GetKeyDown(KeyCode.Space)) interactingScript.dia = 1.1f;
                break;
            case 1.1f:
                tmpText.text = "Press E to pick it up!";
                tmpText.enabled = true;
                textBox.enabled = true;
                break;
            case 2:
                tmpText.text = "Bleghhh\n\n(Press spacebar to continue this conversation)";
                tmpText.enabled = true;
                textBox.enabled = true;
                if (Input.GetKeyDown(KeyCode.Space)) interactingScript.dia = 2.1f;
                break;
            case 2.1f:
                tmpText.text = "That's cold tea.";
                tmpText.enabled = true;
                textBox.enabled = true;
                if (Input.GetKeyDown(KeyCode.Space)) interactingScript.dia = 2.2f;
                break;
            case 2.2f:
                tmpText.text = "You refuse to drink cold tea.";
                tmpText.enabled = true;
                textBox.enabled = true;
                if (Input.GetKeyDown(KeyCode.Space)) interactingScript.dia = 2.3f;
                break;
            case 2.3f:
                tmpText.text = "Maybe there's a way to warm it up?";
                tmpText.enabled = true;
                textBox.enabled = true;
                if (Input.GetKeyDown(KeyCode.Space)) interactingScript.dia = 2.4f;
                break;
            case 2.4f:
                tmpText.enabled = false;
                textBox.enabled = false;
                break;
            case 3:
                tmpText.text = "Took you long enough.";
                tmpText.enabled = true;
                textBox.enabled = true;
                if (Input.GetKeyDown(KeyCode.Space)) interactingScript.dia = 3.1f;
                break;
            case 3.1f:
                tmpText.text = "But yess, this candle will do nicely.";
                tmpText.enabled = true;
                textBox.enabled = true;
                if (Input.GetKeyDown(KeyCode.Space)) interactingScript.dia = 3.2f;
                break;
            case 3.2f:
                tmpText.text = "Press F to put the cup over it.";
                tmpText.enabled = true;
                textBox.enabled = true;
                break;
            case 4:
                tmpText.text = "It's cookin, alright!";
                tmpText.enabled = true;
                textBox.enabled = true;
                if (Input.GetKeyDown(KeyCode.Space)) interactingScript.dia = 4.1f;
                break;
            case 4.1f:
                tmpText.text = "Give it a sec.";
                tmpText.enabled = true;
                textBox.enabled = true;
                if (Input.GetKeyDown(KeyCode.Space)) interactingScript.dia = 4.2f;
                break;
            case 4.2f:
                tmpText.text = "Impatient much?";
                tmpText.enabled = true;
                textBox.enabled = true;
                if (Input.GetKeyDown(KeyCode.Space)) interactingScript.dia = 4.3f;
                break;
            case 4.3f:
                tmpText.text = "Alright alright, give it 5 more seconds.";
                tmpText.enabled = true;
                textBox.enabled = true;
                if (Input.GetKeyDown(KeyCode.Space)) interactingScript.dia = 4.4f;
                break;
            case 4.4f:
                tmpText.enabled = false;
                textBox.enabled = false;
                break;
            case 5:
                tmpText.text = "Done.";
                tmpText.enabled = true;
                textBox.enabled = true;
                if (Input.GetKeyDown(KeyCode.Space)) interactingScript.dia = 5.1f;
                break;
            case 5.1f:
                tmpText.text = "You can pick it up and drink it up.";
                tmpText.enabled = true;
                textBox.enabled = true;
                if (Input.GetKeyDown(KeyCode.Space)) interactingScript.dia = 5.2f;
                break;
            case 5.2f:
                tmpText.text = "(E to pick up and F to drink up)";
                tmpText.enabled = true;
                textBox.enabled = true;
                if (Input.GetKeyDown(KeyCode.Space)) interactingScript.dia = 5.3f;
                break;
            case 5.3f:
                tmpText.enabled = false;
                textBox.enabled = false;
                break;
            case 6:
                tmpText.text = "Still tasted yucky, but it warmed you up.\nAnd you seem to notice something.\nTry rotating the cup by holding R and moving your mouse.";
                tmpText.enabled = true;
                textBox.enabled = true;
                break;
            case 7:
                tmpText.text = "Done!! Drink up (F)";
                tmpText.enabled = true;
                textBox.enabled = true;
                break;
            case 8:
                tmpText.text = "Done!! Drink up (F)";
                tmpText.enabled = true;
                textBox.enabled = true;
                break;
            default:
                tmpText.text = "this isnt supposed to happen tf";
                tmpText.enabled = true;
                textBox.enabled = true;
                break;
        }
    }

    
}
