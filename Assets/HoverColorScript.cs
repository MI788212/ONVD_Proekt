using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverColorScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RawImage rawImage;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.gray;

    void Start()
    {
        if (rawImage == null)
            rawImage = GetComponent<RawImage>();

        rawImage.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rawImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rawImage.color = normalColor;
    }
}
