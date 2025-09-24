using UnityEngine;

public class KeyScript : MonoBehaviour
{
    InteractingScript interactingScript;
    public GameObject keyHoleArea;
    private void Start()
    {
        interactingScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<InteractingScript>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == keyHoleArea)
        {
            interactingScript.KeyEnteredKeyHoleArea();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == keyHoleArea)
        {
            interactingScript.KeyExitedKeyHoleArea();
        }
    }
}
