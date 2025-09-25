using UnityEngine;
using UnityEngine.EventSystems;

public class ExitButtonScript : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Quit game requested");
        Application.Quit();
    }
}
