using UnityEngine;

public class CutDetector : MonoBehaviour
{
    [Header("基础设置")]
    [SerializeField] private Collider2D bladeCollider; // 拖入刀尖子物体的碰撞体

    private void Start()
    {
        if (bladeCollider != null)
        {
            bladeCollider.isTrigger = true; // 确保是触发器
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检测到可切割物体时触发
        if (other.TryGetComponent<CuttableObject>(out var cuttable))
        {
            cuttable.HandleCut(bladeCollider.transform.position);
        }
    }
}
