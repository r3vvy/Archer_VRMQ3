using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _initialSize = 10;
    private Queue<GameObject> _pool = new Queue<GameObject>();

    private void Start()
    {
        for (int i = 0; i < _initialSize; i++)
        {
            GameObject obj = Instantiate(_prefab);
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if (_pool.Count > 0)
        {
            GameObject obj = _pool.Dequeue();
            ResetObject(obj);
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(_prefab);
            return obj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        _pool.Enqueue(obj);
    }
    private void ResetObject(GameObject obj)
    {
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        foreach (var render in obj.GetComponentsInChildren<Renderer>())
        render.enabled = enabled;
    }
}
