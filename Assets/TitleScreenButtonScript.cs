using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleScreenButtonScript : MonoBehaviour, IPointerClickHandler
{
    public GameObject menu;
    public void OnPointerClick(PointerEventData eventData)
    {
        menu.SetActive(false);
        SceneControllerScript.instance.BackToStart();
    }
}
