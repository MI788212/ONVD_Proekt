using UnityEngine;
using UnityEngine.EventSystems;

public class OnButtonClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject ObjectToDeactivate;
    public GameObject ObjectToActivate;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (ObjectToActivate != null)
        {
            ObjectToActivate.SetActive(true);
        }
        if (ObjectToDeactivate != null)
        {
            ObjectToDeactivate.SetActive(false);
        }
    }
}
