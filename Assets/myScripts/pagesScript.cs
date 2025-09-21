using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class pagesScript : MonoBehaviour, IPointerClickHandler
{
    private phonebookScript phonebookScript;

    void Awake()
    {
        phonebookScript = GameObject.FindWithTag("phonebookScreen").GetComponent<phonebookScript>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        RectTransform rt = GetComponent<RectTransform>();

        // Convert screen click to local coords of the image
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out localPoint);

        // Compare with pivot (center at 0,0)
        if (localPoint.x < 0)
        {
            Debug.Log("Clicked LEFT side");
            phonebookScript.PreviousImage();
        }
        else
        {
            Debug.Log("Clicked RIGHT side");
            phonebookScript.NextImage();
        }
    }
}