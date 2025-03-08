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
    private float completedShowDuration; // 默认延长至1秒
    [SerializeField][Tooltip("失败状态持续时间（秒）")] 
    private float failedShowDuration;    // 默认延长至1秒

    // 新增渐变动画参数
    [Header("Fade Animation")]
    [SerializeField]
    private float fadeOutDuration = 0.4f;     // 渐出动画时长
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
    }

    private void OnDisable()
    {
        _questManager.OnQuestUpdated -= HandleQuestUpdate;
    }

    private void HandleQuestUpdate()
    {
        // 如果有正在运行的延迟重置协程则停止
        if (_resetCoroutine != null)
        {
            StopCoroutine(_resetCoroutine);
            _resetCoroutine = null;
        }

        // 如果所有任务都完成
        if (_questManager.CheckQuestComplete())
        {
            dishRenderer.sprite = completedDishSprite;
            _resetCoroutine = StartCoroutine(AnimatedResetAfterDelay(completedShowDuration));
        }
        // 如果所有任务都失败
        else if (_questManager.CheckQuestFailed())
        {
            dishRenderer.sprite = failedDishSprite;
            _resetCoroutine = StartCoroutine(AnimatedResetAfterDelay(failedShowDuration));
        }
        // 如果有部分进度
        else if (HasPartialProgress())
        {
            dishRenderer.sprite = partialDishSprite;
        }
        // 如果回到空状态且没有正在显示的状态
        else if (dishRenderer.sprite != emptyDishSprite)
        {
            dishRenderer.sprite = emptyDishSprite;
        }
    }
    private IEnumerator AnimatedResetAfterDelay(float delay)
    {
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
      
            dishRenderer.sprite = emptyDishSprite;
            dishRenderer.color = originalColor; // 恢复透明度
        
        _resetCoroutine = null;
    }

    private bool HasPartialProgress()
    {
        foreach (var recipe in _questManager.CurrentQuest)
        {
            if (recipe.currentAmount < recipe.requiredAmount)
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
