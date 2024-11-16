using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private ObjectPool _objectPool;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Transform[] _movePoints;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private float Speed = 1;
    private List<int> _targetInstances = new List<int>();
    [SerializeField] private float _spawnTime;
    private static System.Random rng = new System.Random();
    private int _scrore = 0;
    private void Start()
    {
        StartCoroutine(SpawnTargets());
    }

    private IEnumerator SpawnTargets()
    {
        while (_targetInstances.Count <= 2)
        {
            var instance = _objectPool.GetObject();
            instance.transform.position = _spawnPoints[rng.Next(0, _spawnPoints.Length - 1)].position;
            var targetHandle = instance.GetComponentInChildren<TargetHandle>();
            targetHandle.SetupSpeed(Speed);
            targetHandle.OnHit += RemoveFromList;
            targetHandle.OnUpdateScore += IncreaseScore;
            Shuffle(_movePoints);
            targetHandle.PointsToMove = _movePoints;
            _targetInstances.Add(instance.GetInstanceID());
            yield return new WaitForSeconds(_spawnTime);
        }
        yield return new WaitUntil(() => _targetInstances.Count <= 2);
        StartCoroutine(SpawnTargets());
    }

    private void RemoveFromList(GameObject target)
    {
        TargetHandle targetHandle =target.GetComponentInChildren<TargetHandle>();
        _targetInstances.Remove(target.GetInstanceID());
        _objectPool.ReturnObject(target.gameObject);
        targetHandle.OnHit -= RemoveFromList;
        targetHandle.OnUpdateScore -= IncreaseScore;
        Debug.Log(_targetInstances.Count);
    }
    private void IncreaseScore()
    {
        _scrore++;
        _scoreText.text = _scrore.ToString();
    }
    public void Shuffle(Transform[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Transform value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }
}
