using UnityEngine;

public class CuttableObject : MonoBehaviour
{
    public GameObject[] spawnPrefabs; // 被切时生成的预制体数组
    public int maxCuts = 3; // 物体最多可以被切几次
    public Vector3 spawnOffset = new Vector3(0, 0.1f, 0); // 生成物体的偏移量

    private int cutCount = 0; // 记录被切的次数

    public void OnCut(Vector3 cutPosition)
    {
        if (cutCount >= maxCuts) return; // 如果已经达到最大切割次数，则不再处理

        cutCount++;

        // 随机选择一个预制体生成
        if (spawnPrefabs.Length > 0)
        {
            int index = Random.Range(0, spawnPrefabs.Length);
            GameObject spawnedObject = Instantiate(spawnPrefabs[index], cutPosition + spawnOffset, Quaternion.identity);
            spawnedObject.transform.parent = null; // 确保生成的物体不继承父物体的变换
        }

        // 如果达到最大切割次数，销毁当前物体
        if (cutCount >= maxCuts)
        {
            Destroy(gameObject);
        }
    }
}

