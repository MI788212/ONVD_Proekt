using UnityEngine;
using UnityEngine.EventSystems;

public class CluePaperBackgroundScript : MonoBehaviour, IPointerClickHandler
{
    private CluePaperScript cluePaperScript;

    void Awake()
    {
        cluePaperScript = GameObject.FindWithTag("cluePaperScreen").GetComponent<CluePaperScript>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        cluePaperScript.closeScreen();
    }
}
