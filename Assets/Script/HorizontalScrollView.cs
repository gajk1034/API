using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 橫向滑動控制器
/// 用於管理Amiibo列表的橫向滾動顯示
/// </summary>
public class HorizontalScrollView : MonoBehaviour
{
    [Header("滾動設定")]
    [Tooltip("滾動速度")]
    public float scrollSpeed = 0.1f;

    [Tooltip("是否啟用慣性滑動")]
    public bool enableInertia = true;

    [Tooltip("慣性減速率")]
    [Range(0.01f, 1f)]
    public float decelerationRate = 0.135f;

    [Header("組件引用")]
    public ScrollRect scrollRect;
    public HorizontalLayoutGroup horizontalLayoutGroup;

    private void Start()
    {
        SetupScrollRect();
        SetupHorizontalLayout();
    }

    /// <summary>
    /// 設置ScrollRect組件
    /// </summary>
    private void SetupScrollRect()
    {
        if (scrollRect == null)
        {
            scrollRect = GetComponent<ScrollRect>();
        }

        if (scrollRect != null)
        {
            // 設置為橫向滾動
            scrollRect.horizontal = true;
            scrollRect.vertical = false;

            // 慣性設定
            scrollRect.inertia = enableInertia;
            scrollRect.decelerationRate = decelerationRate;

            // 滾動敏感度
            scrollRect.scrollSensitivity = scrollSpeed * 100;
        }
    }

    /// <summary>
    /// 設置橫向布局組件
    /// </summary>
    private void SetupHorizontalLayout()
    {
        if (horizontalLayoutGroup == null)
        {
            horizontalLayoutGroup = scrollRect.content.GetComponent<HorizontalLayoutGroup>();
        }

        if (horizontalLayoutGroup != null)
        {
            // 設置子物件對齊方式
            horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
            horizontalLayoutGroup.childControlWidth = false;
            horizontalLayoutGroup.childControlHeight = false;
            horizontalLayoutGroup.childForceExpandWidth = false;
            horizontalLayoutGroup.childForceExpandHeight = false;

            // 設置間距
            horizontalLayoutGroup.spacing = 20f;
            horizontalLayoutGroup.padding = new RectOffset(20, 20, 10, 10);
        }
    }

    /// <summary>
    /// 滾動到指定位置 (0-1之間)
    /// </summary>
    public void ScrollToPosition(float normalizedPosition)
    {
        if (scrollRect != null)
        {
            scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(normalizedPosition);
        }
    }

    /// <summary>
    /// 平滑滾動到指定位置
    /// </summary>
    public void SmoothScrollToPosition(float targetPosition, float duration = 0.5f)
    {
        StartCoroutine(SmoothScrollCoroutine(targetPosition, duration));
    }

    private IEnumerator SmoothScrollCoroutine(float targetPosition, float duration)
    {
        float elapsedTime = 0f;
        float startPosition = scrollRect.horizontalNormalizedPosition;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / duration;
            float smoothPosition = Mathf.Lerp(startPosition, targetPosition, normalizedTime);
            scrollRect.horizontalNormalizedPosition = smoothPosition;
            yield return null;
        }

        scrollRect.horizontalNormalizedPosition = targetPosition;
    }

    /// <summary>
    /// 滾動到最左邊
    /// </summary>
    public void ScrollToStart()
    {
        ScrollToPosition(0f);
    }

    /// <summary>
    /// 滾動到最右邊
    /// </summary>
    public void ScrollToEnd()
    {
        ScrollToPosition(1f);
    }

    /// <summary>
    /// 向左滾動一個單位
    /// </summary>
    public void ScrollLeft()
    {
        if (scrollRect != null)
        {
            float newPosition = scrollRect.horizontalNormalizedPosition - scrollSpeed;
            SmoothScrollToPosition(Mathf.Clamp01(newPosition), 0.3f);
        }
    }

    /// <summary>
    /// 向右滾動一個單位
    /// </summary>
    public void ScrollRight()
    {
        if (scrollRect != null)
        {
            float newPosition = scrollRect.horizontalNormalizedPosition + scrollSpeed;
            SmoothScrollToPosition(Mathf.Clamp01(newPosition), 0.3f);
        }
    }
}
