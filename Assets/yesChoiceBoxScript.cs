using UnityEngine;
using UnityEngine.EventSystems;

public class yesChoiceBoxScript : MonoBehaviour, IPointerClickHandler
{
    public GameObject textBox;
    private DialogueScript dialogueScript;

    void Awake()
    {
        dialogueScript = textBox.GetComponent<DialogueScript>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + " was clicked!");
        dialogueScript.choseYes();
    }
}
