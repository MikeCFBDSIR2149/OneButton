using UnityEngine;
using System.Collections;

public class DishVisualController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer dishRenderer;

    [Header("Visual States")]
    [SerializeField] private Sprite emptyDishSprite;
    [SerializeField] private Sprite partialDishSprite;
    [SerializeField] private Sprite completedDishSprite;
    [SerializeField] private Sprite failedDishSprite;

    [Header("Timing")]
    [SerializeField][Tooltip("完成状态持续时间（秒）")] 
    private float completedShowDuration = 3f; // 默认延长至3秒
    [SerializeField][Tooltip("失败状态持续时间（秒）")] 
    private float failedShowDuration = 2f;    // 默认延长至2秒

    // 新增渐变动画参数
    [Header("Fade Animation")]
    [SerializeField][Range(0.1f, 2f)] 
    private float fadeOutDuration = 0.8f;     // 渐出动画时长
    [SerializeField] 
    private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    private Coroutine _resetCoroutine;
    private QuestManager _questManager;

    private void Awake()
    {
        _questManager = FindFirstObjectByType<QuestManager>();
        dishRenderer.sprite = emptyDishSprite;
    }

    private void OnEnable()
    {
        _questManager.OnQuestUpdated += HandleQuestUpdate;
        _questManager.OnQuestCompleted += HandleQuestCompleted;
        _questManager.OnQuestFailed += HandleQuestFailed;
    }

    private void OnDisable()
    {
        _questManager.OnQuestUpdated -= HandleQuestUpdate;
        _questManager.OnQuestCompleted -= HandleQuestCompleted;
        _questManager.OnQuestFailed -= HandleQuestFailed;
    }

    private void HandleQuestUpdate()
    {
        // 如果有正在运行的延迟重置协程则停止
        if (_resetCoroutine != null)
        {
            StopCoroutine(_resetCoroutine);
            _resetCoroutine = null;
        }

        // 如果当前有任意进度且未完成
        if (HasPartialProgress())
        {
            dishRenderer.sprite = partialDishSprite;
        }
        // 如果回到空状态且没有正在显示的状态
        else if (dishRenderer.sprite != emptyDishSprite)
        {
            dishRenderer.sprite = emptyDishSprite;
        }
    }

    private void HandleQuestCompleted()
    {
        if (_resetCoroutine != null) StopCoroutine(_resetCoroutine);
        dishRenderer.sprite = completedDishSprite;
        _resetCoroutine = StartCoroutine(AnimatedResetAfterDelay(completedShowDuration));
    }

    private void HandleQuestFailed()
    {
        if (_resetCoroutine != null) StopCoroutine(_resetCoroutine);
        dishRenderer.sprite = failedDishSprite;
        _resetCoroutine = StartCoroutine(AnimatedResetAfterDelay(failedShowDuration));
    }

    private IEnumerator AnimatedResetAfterDelay(float delay)
    {
        // 等待完整显示时间
        yield return new WaitForSeconds(delay);

        // 渐出动画
        float elapsed = 0;
        Color originalColor = dishRenderer.color;
        while (elapsed < fadeOutDuration)
        {
            float alpha = fadeCurve.Evaluate(elapsed / fadeOutDuration);
            dishRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 重置状态
        if (!HasPartialProgress())
        {
            dishRenderer.sprite = emptyDishSprite;
            dishRenderer.color = originalColor; // 恢复透明度
        }
        _resetCoroutine = null;
    }

    private bool HasPartialProgress()
    {
        foreach (var recipe in _questManager.CurrentQuest)
        {
            if (recipe.currentAmount > 0 && recipe.currentAmount < recipe.requiredAmount)
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // 只有在没有新进度时才重置为空
        if (!HasPartialProgress())
        {
            dishRenderer.sprite = emptyDishSprite;
        }
        _resetCoroutine = null;
    }
}
