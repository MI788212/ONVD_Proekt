using UnityEngine;


public class teaCupScript : MonoBehaviour
{
    //contact with my manager
    public GameObject MainCamera;
    private InteractingScript interactingScript;

    //other
    public GameObject WarmingArea;

    

    private void Start()
    {
        interactingScript = MainCamera.GetComponent<InteractingScript>();
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject == WarmingArea)
        {
            interactingScript.CupInWarmingArea();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == WarmingArea)
        {
            interactingScript.CupExitedInWarmingArea();
        }
    }


}