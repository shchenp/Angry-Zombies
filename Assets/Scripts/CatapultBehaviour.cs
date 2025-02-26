using UnityEngine;

public class CatapultBehaviour : MonoBehaviour
{
    [SerializeField]
    private LineRenderer[] _bowstringLineRenderers;
    
    [SerializeField]
    private Transform _endPoint;

    [SerializeField]
    private Skull _skull;

    private bool _isScullDragging;

    private void Awake()
    {
        _skull.OnDragging += SetDraggingFlag;
    }

    private void OnDestroy()
    {
        _skull.OnDragging -= SetDraggingFlag;
    }

    private void Update()
    {
        if (!_isScullDragging)
        {
            foreach (var lineRenderer in _bowstringLineRenderers)
            {
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, lineRenderer.GetPosition(0));
            }
            
            return;
        }
        
        foreach (var lineRenderer in _bowstringLineRenderers)
        {
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, _endPoint.position);
        }
    }

    private void SetDraggingFlag(bool flag)
    {
        _isScullDragging = flag;
    }
}