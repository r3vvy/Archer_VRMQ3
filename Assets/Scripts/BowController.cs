using Oculus.Interaction.HandGrab;
using System;
using UnityEngine;

public class BowString : MonoBehaviour
{
    public event Action<float> ReleaseArrow;
    private bool _canDraw = false;
    private VRHandController _pullingHand;
    private Transform _handPosition;
    private bool _isDrwing=false;
    private Vector3 _startPosition;
    private float _forceDrawing=0;

    private void Start()
    {
        _startPosition = transform.localPosition;
    }
    void Update()
    {
        if (_canDraw)
        {
            if (_pullingHand.controller == OVRInput.Controller.RTouch)
            {
                if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
                    _isDrwing = true;
                else if (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))
                {
                    ReleaseSpring();
                }
            }
            else if (_pullingHand.controller == OVRInput.Controller.LTouch)
            {
                if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
                    _isDrwing = true;
                else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
                {
                    ReleaseSpring();
                }
            }
        }
      
    }
    private void FixedUpdate()
    {

        if (_isDrwing)
            DrawingString();
        else
            ResetPosition();

    }
    [ContextMenu("Drawing")]
    private void DrawingString()
    {
        Vector3 _handPos = _pullingHand.transform.position;
        transform.position = new Vector3(_handPos.x, _handPos.y, _handPos.z);
    
    }
    private void ResetPosition()
    {
        transform.localPosition= _startPosition;
    }
    private void ReleaseSpring()
    {
        _forceDrawing = (_startPosition - _pullingHand.transform.position).magnitude;
        ReleaseArrow?.Invoke(_forceDrawing);
        _isDrwing=false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           _pullingHand= other.GetComponent<VRHandController>();
            _handPosition = other.transform;
            _canDraw = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canDraw = false;
            ResetPosition();
        }
    }


}
