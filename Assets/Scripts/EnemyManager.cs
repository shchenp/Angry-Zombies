using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    public UnityEvent<int> SetEnemyCountEvent;
    public UnityEvent OnEnemiesDied;

    [SerializeField] 
    private List<Enemy> _enemies;
    
    private int _count;

    private void Awake()
    {
        foreach (var enemy in _enemies)
        {
            enemy.EnemyDied.AddListener(DecreaseCount);
        }
    }

    private void Start()
    {
        _count = transform.childCount;
        SetEnemyCountEvent?.Invoke(_count);
    }

    private void DecreaseCount()
    {
        _count--;
        SetEnemyCountEvent?.Invoke(_count);

        if (_count == 0)
        {
            OnEnemiesDied?.Invoke();
        }
    }
}