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

        private Toggle[] _tabsToggles;
    
        private float _initialBackgroundWidth;
        private float _expandedBackgroundHeight;

        private Tween _sizeTween;
    
        private void Awake()
        {
            _initialBackgroundWidth = content.sizeDelta.x;
            _expandedBackgroundHeight = content.sizeDelta.y;
        
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

            RefreshContentSize(isOpened: value);

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

        private void RefreshVisuals(bool value)
        {
            openToggle.transform.eulerAngles = new Vector3(0, 0, value ? 180 : 0);
            expandToggle.gameObject.SetActive(value);
            lineSeparator.SetActive(value);
        
            headerImage.sprite = value 
                ? openedSprite 
                : collapsedSprite;
        }

        private void RefreshContentSize(bool isOpened)
        {
            float targetHeight = isOpened 
                ? _expandedBackgroundHeight / 2 
                : 0f;

            if (isOpened)
            {
                RefreshVisuals(true);
            }

            AnimateSizeDelta(targetHeight, onCompleted: () => RefreshVisuals(isOpened));
        }

        private void AnimateSizeDelta(float targetHeight, Action onCompleted = null)
        {
            _sizeTween?.Kill();
            
            Vector2 targetSize = new Vector2(_initialBackgroundWidth, targetHeight);
            
            _sizeTween = DOTween.To(() => content.sizeDelta, x => content.sizeDelta = x, targetSize, 0.3f)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => onCompleted?.Invoke());
        }

        private void HandleCollapsed()
        {
            expandToggle.SetIsOnWithoutNotify(false);
            expandToggle.transform.eulerAngles = Vector3.zero;
            expandToggle.gameObject.SetActive(false);
        }
    }
}
