using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PhonebookBackgroundScript : MonoBehaviour, IPointerClickHandler
{
    private phonebookScript phonebookScript;

    void Awake()
    {
        phonebookScript = GameObject.FindWithTag("phonebookScreen").GetComponent<phonebookScript>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        phonebookScript.closeBook();
    }
}
