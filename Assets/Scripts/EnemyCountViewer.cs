using TMPro;
using UnityEngine;

public class EnemyCountViewer : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI _count;

    public void SetScore(int count)
    {
        _count.text = count.ToString();
    }
}