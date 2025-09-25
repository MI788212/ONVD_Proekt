using UnityEngine;
using UnityEngine.EventSystems;

public class StartButtonScript : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        SceneControllerScript.instance.StartGame();
    }
}
