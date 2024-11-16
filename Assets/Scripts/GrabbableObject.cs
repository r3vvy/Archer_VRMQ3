using System;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    public event Action OnGrab;
    public event Action OnGrabExit;
    private bool isBeingHeld = false;
    private Transform holder;
    void Update()
    {
        if (isBeingHeld && holder != null)
        {
            transform.position = holder.position;
            transform.rotation = holder.rotation;
        }
    }

    public void OnPickup(Transform hand)
    {
        isBeingHeld = true;
        holder = hand;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        OnGrab?.Invoke();
    }

    public void OnRelease()
    {
        isBeingHeld = false;
        holder = null;
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        OnGrabExit?.Invoke();
    }
}
