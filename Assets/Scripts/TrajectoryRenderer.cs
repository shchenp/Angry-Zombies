using System.Collections.Generic;
using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{
    [SerializeField] 
    private SpriteRenderer _dotPrefab;
    
    [SerializeField]
    private int _dotsCount = 30;
    
    [SerializeField]
    private float _simulationTime = 2f;
    
    private List<SpriteRenderer> _dots = new();
    private float _gravity;

    private void Start()
    {
        _gravity = Mathf.Abs(Physics2D.gravity.y);

        for (int i = 0; i < _dotsCount; i++)
        {
            var dot = Instantiate(_dotPrefab, transform);
            dot.gameObject.SetActive(false);
            _dots.Add(dot);
        }
    }
    
    public void DrawTrajectory(Vector2 startPosition, Vector2 velocity)
    {
        for (int i = 0; i < _dotsCount; i++)
        {
            var t = (i / (float)_dotsCount) * _simulationTime;
            var x = startPosition.x + velocity.x * t;
            var y = startPosition.y + velocity.y * t - 0.5f * _gravity * t * t;

            _dots[i].transform.position = new Vector2(x, y);
            _dots[i].gameObject.SetActive(true);
        }
    }

    public void HideTrajectory()
    {
        foreach (var dot in _dots)
            dot.gameObject.SetActive(false);
    }
}