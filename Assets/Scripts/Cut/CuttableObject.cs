using UnityEngine;

public class CuttableObject : MonoBehaviour
{
    [Header("生成设置")]
    [SerializeField] private GameObject[] spawnPrefabs; // 可生成的预制体数组
    [SerializeField] private int maxCuts = 3; // 最大可被切割次数
    [SerializeField] private Vector2 spawnOffset = new Vector2(0, 0.1f); // 生成位置偏移

    private int cutCount; // 当前切割次数

    public void HandleCut(Vector2 cutPosition)
    {
        if (cutCount >= maxCuts) return;

        // 生成新物体
        if (spawnPrefabs.Length > 0)
        {
            int index = Random.Range(0, spawnPrefabs.Length);
            Instantiate(
                spawnPrefabs[index],
                cutPosition + spawnOffset,
                Quaternion.identity
            );
        }

        // 更新切割次数
        cutCount++;

        // 达到最大次数后销毁
        if (cutCount >= maxCuts)
        {
            Destroy(gameObject);
        }
    }
}

