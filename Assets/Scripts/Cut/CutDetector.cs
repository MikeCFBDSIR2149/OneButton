using UnityEngine;

public class CutDetector : MonoBehaviour
{
    private Knife knife;
    private TrailRenderer trailRenderer;
    private bool isCutting = false; // 是否正在切割

    private void Start()
    {
        knife = GetComponent<Knife>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();
    }

    private void Update()
    {
        // 检测刀是否开始切割
        if (trailRenderer.emitting && !isCutting)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);

            if (hit.collider != null)
            {
                CuttableObject cuttableObject = hit.collider.GetComponent<CuttableObject>();
                if (cuttableObject != null)
                {
                    isCutting = true; //  
                    cuttableObject.OnCut(hit.point); // 调用被切物体的 OnCut 方法
                }
            }
        }
        // 检测刀是否结束切割
        else if (!trailRenderer.emitting && isCutting)
        {
            isCutting = false; // 重置切割状态
        }
    }
}
