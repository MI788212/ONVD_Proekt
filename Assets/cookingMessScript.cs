using UnityEngine;
using TMPro;
using System.Collections;

public class cookingMessScript : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float interval = 0.5f;

    private string baseText = "Cooking";
    private int dotCount = 0;
    private Coroutine cycleRoutine;

    public GameObject crosshair;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        cycleRoutine = StartCoroutine(CycleText());
        crosshair.SetActive(false);
    }

    void OnDisable()
    {
        if (cycleRoutine != null)
        {
            StopCoroutine(cycleRoutine);
        }
        crosshair.SetActive(true);
    }

    IEnumerator CycleText()
    {
        while (true)
        {
            // make the text with the right amount of dots
            textComponent.text = baseText + new string('.', dotCount);

            // cycle dot count between 0–3
            dotCount = (dotCount + 1) % 4;

            // wait
            yield return new WaitForSeconds(interval);
        }
    }
}
