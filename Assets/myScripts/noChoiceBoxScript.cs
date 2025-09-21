using UnityEngine;
using UnityEngine.EventSystems;

public class noChoiceBoxScript : MonoBehaviour, IPointerClickHandler
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
        dialogueScript.choseNo();
    }
}
