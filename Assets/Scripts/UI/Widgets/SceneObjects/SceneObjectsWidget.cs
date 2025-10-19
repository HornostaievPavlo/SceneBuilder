using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets.SceneObjects
{
    public class SceneObjectsWidget : MonoBehaviour
    {
        [Header("Content")]
        [SerializeField] private Toggle openToggle;
        [SerializeField] private Toggle expandToggle;
        [SerializeField] private RectTransform content;
    
        [Header("Tabs")]
        [SerializeField] private SceneObjectsTabsWidget tabsWidget;
        [SerializeField] private ToggleGroup tabsToggleGroup;
        [SerializeField] private GameObject lineSeparator;

        [Header("Header")]
        [SerializeField] private Image headerImage;
        [SerializeField] private Sprite collapsedSprite;
        [SerializeField] private Sprite openedSprite;

        private float _initialBackgroundWidth;
        private float _expandedBackgroundHeight;

        private Tween _sizeTween;
        private Toggle[] _tabsToggles;

        private const float AnimationDuration = 0.3f;
        private const float ExpandedStateBottomOffset = 190f;
        
        public ToggleGroup TabsToggleGroup => tabsToggleGroup;
        public Toggle OpenToggle => openToggle;
    
        private void Awake()
        {
            _initialBackgroundWidth = content.sizeDelta.x;
            _expandedBackgroundHeight = CalculateExpandedHeight();
        
            content.sizeDelta = new Vector2(_initialBackgroundWidth, 0f);
        
            expandToggle.gameObject.SetActive(false);
            lineSeparator.SetActive(false);

            _tabsToggles = tabsToggleGroup.gameObject.GetComponentsInChildren<Toggle>(true);
        }

        private void OnEnable()
        {
            openToggle.onValueChanged.AddListener(HandleOpenToggleValueChanged);
            expandToggle.onValueChanged.AddListener(HandleExpandToggleValueChanged);
        }
    
        private void OnDisable()
        {
            openToggle.onValueChanged.RemoveListener(HandleOpenToggleValueChanged);
            expandToggle.onValueChanged.RemoveListener(HandleExpandToggleValueChanged);
            
            _sizeTween?.Kill();
        }

        private void HandleOpenToggleValueChanged(bool value)
        {
            if (value == false)
            {
                HandleCollapsed();
            }

            RefreshOnOpenStateChange(isOpened: value);

            foreach (Toggle toggle in _tabsToggles)
            {
                toggle.interactable = value;
            }
            
            tabsWidget.Setup(value);
        }

        private void HandleExpandToggleValueChanged(bool value)
        {
            float targetHeight = value 
                ? _expandedBackgroundHeight 
                : _expandedBackgroundHeight / 2;

            AnimateSizeDelta(targetHeight);
            expandToggle.transform.eulerAngles = new Vector3(0, 0, value ? 180 : 0);
        }

        private void RefreshOnOpenStateChange(bool isOpened)
        {
            float targetHeight = isOpened 
                ? _expandedBackgroundHeight / 2 
                : 0f;

            if (isOpened)
            {
                RefreshHeaderSprite(true);
            }

            RefreshVisuals(isOpened);
            
            var toggleTargetRotation = new Vector3(0, 0, isOpened ? 180f : 0f);
            
            openToggle.transform.DOKill(true);
            openToggle.transform.DORotate(toggleTargetRotation, AnimationDuration).SetEase(Ease.OutBack);

            AnimateSizeDelta(targetHeight, onCompleted: () => RefreshHeaderSprite(isOpened));
        }

        private void RefreshVisuals(bool isOpened)
        {
            expandToggle.gameObject.SetActive(isOpened);
            lineSeparator.SetActive(isOpened);
        }
        
        private void RefreshHeaderSprite(bool isOpened)
        {
            headerImage.sprite = isOpened 
                ? openedSprite 
                : collapsedSprite;
        }

        private void AnimateSizeDelta(float targetHeight, Action onCompleted = null)
        {
            Vector2 targetSize = new Vector2(_initialBackgroundWidth, targetHeight);
            
            _sizeTween?.Kill();
            _sizeTween = DOTween.To(() => content.sizeDelta, x => content.sizeDelta = x, targetSize, AnimationDuration)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => onCompleted?.Invoke());
        }

        private void HandleCollapsed()
        {
            expandToggle.SetIsOnWithoutNotify(false);
            expandToggle.transform.eulerAngles = Vector3.zero;
            expandToggle.gameObject.SetActive(false);
        }

        private float CalculateExpandedHeight()
        {
            RectTransform canvasRect = GetComponentInParent<Canvas>()?.GetComponent<RectTransform>();
            
            if (canvasRect == null)
            {
                return Screen.height - ExpandedStateBottomOffset;
            }
            
            float canvasHeight = canvasRect.rect.height;
            float contentTopY = content.anchoredPosition.y + canvasHeight / 2;
            
            float canvasBottomY = -canvasHeight / 2;
            float availableHeight = contentTopY - canvasBottomY - ExpandedStateBottomOffset;
            
            return Mathf.Max(0, availableHeight);
        }
    }
}
