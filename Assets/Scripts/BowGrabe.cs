using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowGrabe : MonoBehaviour
{
    [SerializeField] private SphereCollider _stringTrigger;
    private GrabbableObject _grabbableObject;
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private Transform _arrowSpawnPoint;
    [SerializeField] private BowString _bowString;
    private GameObject _arrow;
    private void Awake()
    {
        _grabbableObject= GetComponent<GrabbableObject>();
        _grabbableObject.OnGrab += OnGrabBow; 
        _grabbableObject.OnGrabExit+= OnGrabBowExit;
        _bowString.ReleaseArrow += ShootArrow;
    }
  
    private void OnGrabBow()
    {
        GetComponent<BoxCollider>().enabled= false;
        _stringTrigger.enabled=true;
        StartCoroutine(SpawnArrow(0));
    }
    private IEnumerator SpawnArrow(float Delay)
    {
       yield return new WaitForSeconds(Delay);
       _arrow=Instantiate(_arrowPrefab, _arrowSpawnPoint); 
    }
    private void ShootArrow(float force)
    {
        _arrow.transform.parent.DetachChildren();
        Rigidbody rb= _arrow.GetComponent<Rigidbody>();
        rb.isKinematic=false;
        rb.AddForce(-_arrow.transform.up*5*force, ForceMode.Impulse);
        var collider = _arrow.GetComponent<Collider>();
        collider.enabled = true;
        collider.isTrigger = false;
        StartCoroutine(SpawnArrow(1));
    }
    private void OnGrabBowExit()
    {
        GetComponent<BoxCollider>().enabled = true;
        _stringTrigger.enabled=false;
    }
    
    private void OnDestroy()
    {
        _grabbableObject.OnGrab-= OnGrabBow;
        _grabbableObject.OnGrabExit-= OnGrabBowExit; ;
        _bowString.ReleaseArrow -= ShootArrow;
    }
    private void OnDrawGizmos()
    {
        if(_arrow!=null)
        Gizmos.DrawRay(_arrow.transform.position, -_arrow.transform.up);   
    }

}
