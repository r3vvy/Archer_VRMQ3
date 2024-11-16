using UnityEngine;


public class VRHandController : MonoBehaviour
{
    public OVRInput.Controller controller;
    public OVRInput.Button grabButton = OVRInput.Button.PrimaryHandTrigger; // или "SecondaryHandTrigger"

    private GrabbableObject currentGrabbable;

    void Update()
    {
        if (OVRInput.GetDown(grabButton, controller) && currentGrabbable != null)
        {
            currentGrabbable.OnPickup(transform);
        }
        else if (OVRInput.GetUp(grabButton, controller) && currentGrabbable != null)
        {
            currentGrabbable.OnRelease();
            currentGrabbable= null;
        }
        if (currentGrabbable!= null) 
        GetComponent<Collider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GrabbableObject>())
        {
            currentGrabbable = other.GetComponent<GrabbableObject>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<GrabbableObject>() == currentGrabbable)
        {
            currentGrabbable = null;
        }
    }
}
