using UnityEngine;
using UnityEngine.EventSystems;
public class WasClickedScript : MonoBehaviour, IPointerClickHandler
{
    private phoneScript phoneScript;

    void Awake()
    {
        phoneScript = GameObject.FindWithTag("phoneScreen").GetComponent<phoneScript>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        phoneScript.wasClicked(gameObject.name);
    }
}
