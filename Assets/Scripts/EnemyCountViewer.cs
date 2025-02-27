using TMPro;
using UnityEngine;

public class EnemyCountViewer : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI _count;

    public void OnCountChanged(int count)
    {
        _count.text = count.ToString();
    }
}