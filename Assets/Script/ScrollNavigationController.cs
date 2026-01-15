using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 滾動導航控制器
/// 提供左右按鈕來控制橫向滾動
/// </summary>
public class ScrollNavigationController : MonoBehaviour
{
    [Header("導航按鈕")]
    [Tooltip("向左滾動按鈕")]
    public Button leftButton;

    [Tooltip("向右滾動按鈕")]
    public Button rightButton;

    [Header("滾動控制器")]
    public HorizontalScrollView horizontalScrollView;

    [Header("按鈕顯示設定")]
    [Tooltip("是否根據滾動位置自動隱藏按鈕")]
    public bool autoHideButtons = true;

    [Tooltip("按鈕透明度（完全可見）")]
    [Range(0f, 1f)]
    public float visibleAlpha = 1f;

    [Tooltip("按鈕透明度（不可用時）")]
    [Range(0f, 1f)]
    public float hiddenAlpha = 0.3f;

    private ScrollRect scrollRect;
    private CanvasGroup leftButtonCanvasGroup;
    private CanvasGroup rightButtonCanvasGroup;

    private void Start()
    {
        SetupButtons();
        SetupScrollListener();
    }

    /// <summary>
    /// 設置按鈕事件
    /// </summary>
    private void SetupButtons()
    {
        if (leftButton != null)
        {
            leftButton.onClick.AddListener(OnLeftButtonClick);

            // 添加CanvasGroup用於控制透明度
            leftButtonCanvasGroup = leftButton.GetComponent<CanvasGroup>();
            if (leftButtonCanvasGroup == null && autoHideButtons)
            {
                leftButtonCanvasGroup = leftButton.gameObject.AddComponent<CanvasGroup>();
            }
        }

        if (rightButton != null)
        {
            rightButton.onClick.AddListener(OnRightButtonClick);

            // 添加CanvasGroup用於控制透明度
            rightButtonCanvasGroup = rightButton.GetComponent<CanvasGroup>();
            if (rightButtonCanvasGroup == null && autoHideButtons)
            {
                rightButtonCanvasGroup = rightButton.gameObject.AddComponent<CanvasGroup>();
            }
        }
    }

    /// <summary>
    /// 設置滾動監聽器
    /// </summary>
    private void SetupScrollListener()
    {
        if (horizontalScrollView != null && horizontalScrollView.scrollRect != null)
        {
            scrollRect = horizontalScrollView.scrollRect;
            scrollRect.onValueChanged.AddListener(OnScrollValueChanged);

            // 初始更新按鈕狀態
            UpdateButtonStates(scrollRect.horizontalNormalizedPosition);
        }
    }

    /// <summary>
    /// 左按鈕點擊事件
    /// </summary>
    private void OnLeftButtonClick()
    {
        if (horizontalScrollView != null)
        {
            horizontalScrollView.ScrollLeft();
        }
    }

    /// <summary>
    /// 右按鈕點擊事件
    /// </summary>
    private void OnRightButtonClick()
    {
        if (horizontalScrollView != null)
        {
            horizontalScrollView.ScrollRight();
        }
    }

    /// <summary>
    /// 滾動值變化時的回調
    /// </summary>
    private void OnScrollValueChanged(Vector2 scrollPosition)
    {
        UpdateButtonStates(scrollPosition.x);
    }

    /// <summary>
    /// 更新按鈕顯示狀態
    /// </summary>
    private void UpdateButtonStates(float normalizedPosition)
    {
        if (!autoHideButtons) return;

        // 更新左按鈕狀態（滾動到最左邊時淡出）
        if (leftButtonCanvasGroup != null)
        {
            if (normalizedPosition <= 0.01f)
            {
                leftButtonCanvasGroup.alpha = hiddenAlpha;
                leftButton.interactable = false;
            }
            else
            {
                leftButtonCanvasGroup.alpha = visibleAlpha;
                leftButton.interactable = true;
            }
        }

        // 更新右按鈕狀態（滾動到最右邊時淡出）
        if (rightButtonCanvasGroup != null)
        {
            if (normalizedPosition >= 0.99f)
            {
                rightButtonCanvasGroup.alpha = hiddenAlpha;
                rightButton.interactable = false;
            }
            else
            {
                rightButtonCanvasGroup.alpha = visibleAlpha;
                rightButton.interactable = true;
            }
        }
    }

    /// <summary>
    /// 啟用/禁用導航按鈕
    /// </summary>
    public void SetNavigationEnabled(bool enabled)
    {
        if (leftButton != null)
        {
            leftButton.gameObject.SetActive(enabled);
        }

        if (rightButton != null)
        {
            rightButton.gameObject.SetActive(enabled);
        }
    }

    private void OnDestroy()
    {
        // 清理事件監聽
        if (leftButton != null)
        {
            leftButton.onClick.RemoveListener(OnLeftButtonClick);
        }

        if (rightButton != null)
        {
            rightButton.onClick.RemoveListener(OnRightButtonClick);
        }

        if (scrollRect != null)
        {
            scrollRect.onValueChanged.RemoveListener(OnScrollValueChanged);
        }
    }
}
