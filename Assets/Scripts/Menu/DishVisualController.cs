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
        _questManager.OnQuestFailed += OnQuestFailed;
        _questManager.OnQuestCompleted += OnQuestCompleted;
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
        // 如果有部分进度
        if (HasPartialProgress())
        {
            dishRenderer.sprite = partialDishSprite;
        }
        
        else
        {
            dishRenderer.sprite = emptyDishSprite;
        }
    }
    private void OnQuestFailed()
    {
        dishRenderer.sprite = failedDishSprite;
        StartCoroutine(AnimatedResetAfterDelay(2f)); // 启动协程，等待两秒后恢复颜色并更新任务内容
    }
    private void OnQuestCompleted()
    {
        dishRenderer.sprite = completedDishSprite;
        StartCoroutine(AnimatedResetAfterDelay(2f)); // 启动协程，等待两秒后恢复颜色并更新任务内容
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
            dishRenderer.sprite = emptyDishSprite;
            dishRenderer.color = originalColor; // 恢复透明度
        
        _resetCoroutine = null;
    }
    private bool AllIngredientsReachTarget()
    {
        foreach (var recipe in _questManager.CurrentQuest)
        {
            // 如果任意一种食材的 currentAmount 不等于 requiredAmount
            if (recipe.currentAmount != recipe.requiredAmount)
            {
                return false;
            }
        }
        return true;
    }

    private bool HasPartialProgress()
    {
        foreach (var recipe in _questManager.CurrentQuest)
        {
            if (recipe.currentAmount > 0 && recipe.currentAmount <= recipe.requiredAmount)
            {
                return true;
            }
        }
        return false;
    }

}
