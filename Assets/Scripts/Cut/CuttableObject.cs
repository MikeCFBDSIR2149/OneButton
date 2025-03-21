using UnityEngine;

public class CuttableObject : MonoBehaviour
{
    [Header("生成设置")]
    [SerializeField] private GameObject[] spawnPrefabs; // 可生成的预制体数组
    [SerializeField] private int maxCuts = 3; // 最大可被切割次数
    [SerializeField] private Vector2 spawnOffset = new Vector2(0, 0.1f); // 生成位置偏移
    [SerializeField] private Sprite cutSprite; // 第一次切割后的贴图
    [SerializeField] private ParticleSystem cutParticleEffect; // 切割时的粒子效果

    private int cutCount; // 当前切割次数
    private bool hasChangedSprite; // 是否已经改变过贴图

    public void HandleCut(Vector2 cutPosition)
    {
        if (cutCount >= maxCuts) return;

        // 第一次切割时改变贴图
        if (!hasChangedSprite && cutSprite)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = cutSprite;
            }
            hasChangedSprite = true;
        }

        // 触发粒子效果
        if (cutParticleEffect)
        {
            ParticleSystem particleInstance = Instantiate(cutParticleEffect, cutPosition, Quaternion.identity);
            particleInstance.Play(); // 播放粒子效果
            Destroy(particleInstance.gameObject, particleInstance.main.duration); // 播放完后销毁
        }

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
        
        AudioManager.Instance.PlaySFX(2);

        // 更新切割次数
        cutCount++;

        // 达到最大次数后销毁
        if (cutCount >= maxCuts)
        {
            Destroy(gameObject);
        }
    }
}

