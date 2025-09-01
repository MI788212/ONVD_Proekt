using UnityEngine;

public class teaCupScript : MonoBehaviour
{
 
    Camera cam;

    void Start()
    {
        cam = Camera.main; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // left mouse click
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition); // from camera through mouse
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("Raycast hit: " + hit.transform.name); // <--- add this
                }
                // check if THIS object was hit
                if (hit.transform == transform)
                {
                    Debug.Log("You clicked on " + gameObject.name);
                }

            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("warmingArea"))
        {
            Debug.Log("Touched the warming area!");
        }
    }
}
