using UnityEngine;
using System.Collections;
using System.Linq;

public class DishVisualController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer dishRenderer;
    [SerializeField] private SpriteRenderer dishPartialRenderer;

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

    private Coroutine resetCoroutine;
    private QuestManager questManager;

    private void Awake()
    {
        questManager = FindFirstObjectByType<QuestManager>();
        dishRenderer.sprite = emptyDishSprite;
        questManager.OnQuestFailed += OnQuestFailed;
        questManager.OnQuestCompleted += OnQuestCompleted;
    }

    private void OnEnable()
    {
        questManager.OnQuestUpdated += HandleQuestUpdate;
    }

    private void OnDisable()
    {
        questManager.OnQuestUpdated -= HandleQuestUpdate;
    }

    private void HandleQuestUpdate()
    {
        // 如果有部分进度
        dishRenderer.sprite = HasPartialProgress() ? partialDishSprite : emptyDishSprite;
    }
    private void OnQuestFailed()
    {
        // 如果有正在运行的延迟重置协程则停止
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
            dishRenderer.color = Color.white;
            resetCoroutine = null;
        }
        dishRenderer.sprite = failedDishSprite;
        resetCoroutine = StartCoroutine(AnimatedResetAfterDelay()); // 启动协程，等待两秒后恢复颜色并更新任务内容
    }
    private void OnQuestCompleted()
    {
        // 如果有正在运行的延迟重置协程则停止
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
            dishRenderer.color = Color.white;
            resetCoroutine = null;
        }
        dishRenderer.sprite = completedDishSprite;
        resetCoroutine = StartCoroutine(AnimatedResetAfterDelay()); // 启动协程，等待两秒后恢复颜色并更新任务内容
    }
    
    private IEnumerator AnimatedResetAfterDelay()
    {
        // 渐出动画
        float elapsed = 0;
        Color originalColor = dishRenderer.color;
        dishPartialRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        while (elapsed < fadeOutDuration)
        {
            float alpha = fadeCurve.Evaluate(elapsed / fadeOutDuration);
            dishRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        dishRenderer.sprite = emptyDishSprite;
        dishRenderer.color = originalColor; // 恢复透明度
        dishPartialRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
        
        resetCoroutine = null;
    }
    
    // private bool AllIngredientsReachTarget()
    // {
    //     return questManager.CurrentQuest.All(recipe => recipe.currentAmount == recipe.requiredAmount);
    // }

    private bool HasPartialProgress()
    {
        foreach (var recipe in questManager.CurrentQuest)
        {
            if (recipe.currentAmount > 0 && recipe.currentAmount <= recipe.requiredAmount)
            {
                return true;
            }
        }
        return false;
    }

}
