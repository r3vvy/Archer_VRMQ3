using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

public class TargetHandle : MonoBehaviour
{
    public Action<GameObject> OnHit;
    public Action OnUpdateScore;
    [HideInInspector] public Transform[] PointsToMove;
    [SerializeField] private ParticleSystem _impactFX;
    [SerializeField] private bool _lookAtTarget = false;
    private int _pointIndex = 0;
    private Transform _currentPoint;
    private Rigidbody _rb;
    private readonly string _arrowTag = "Arrow";
    private bool _canMove=true;
    private Random _random= new Random();
    private float _speed;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        MoveToNext();
    }
    private void OnEnable()
    {
        if(_random.Next(0,10)>5)
        _canMove = true;
        else 
        _canMove = false;
    }
    public void SetupSpeed(float speed)
    {
        _speed = speed;
    }
    private void Update()
    {
        if (!_canMove)
            return;
        transform.position = Vector3.MoveTowards(transform.position, _currentPoint.position,_speed * Time.deltaTime) ;
        if (Vector3.Distance(transform.position, _currentPoint.position) < 1)
            MoveToNext();
    }
    private void MoveToNext()
    {
        _currentPoint = PointsToMove[_pointIndex];
        _pointIndex = (_pointIndex + 1) % PointsToMove.Length;
        if (_lookAtTarget)
            transform.LookAt(_currentPoint.position,Vector3.up);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != _arrowTag)
            return;
            _impactFX.Play();
            _canMove= false;
            collision.gameObject.SetActive(false);
            foreach(var render in GetComponentsInChildren<MeshRenderer>())
            render.enabled = false;
             OnUpdateScore.Invoke();
            StartCoroutine(DelayDestroy(2));
        
    }
    private IEnumerator DelayDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        OnHit.Invoke(transform.parent.gameObject);
   
    }
}
