using UnityEngine;
using UnityEngine.EventSystems;

public class safeButtonWasClickedScript : MonoBehaviour, IPointerClickHandler
{
    private SafeScript safeScript;

    void Awake()
    {
        safeScript = GameObject.FindWithTag("safeScreen").GetComponent<SafeScript>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        safeScript.wasClicked(gameObject.name);
    }
}
