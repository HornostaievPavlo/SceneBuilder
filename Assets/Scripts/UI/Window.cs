using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Window : MonoBehaviour
    {
        [SerializeField] private Toggle contentToggle;
        [SerializeField] private GameObject content;
        [SerializeField] private CanvasGroup contentCanvasGroup;

        private void OnEnable()
        {
            contentToggle.onValueChanged.AddListener(ToggleContent);
        }

        private void OnDisable()
        {
            contentToggle.onValueChanged.RemoveListener(ToggleContent);
        }

        private void ToggleContent(bool value)
        {
            if (value)
            {
                content.SetActive(true);
            }
            
            AnimateToggle(value);
        }
        
        private void AnimateToggle(bool value)
        {
            contentCanvasGroup.alpha = value ? 0f : 1f;
            float endValue = value ? 1f : 0f;

            contentCanvasGroup.DOKill(true);
            var alphaTween = DOTween.To(() => contentCanvasGroup.alpha, x => contentCanvasGroup.alpha = x, endValue, 0.2f);
            
            if (value == false)
            {
                alphaTween.OnComplete(() => content.SetActive(false));
            }
        
            var targetRotation = new Vector3(0, 0, value ? 0f : 180f);
        
            contentToggle.transform.DOKill(true);
            contentToggle.transform.DORotate(targetRotation, 0.5f).SetEase(Ease.OutBack);
        }
    }
}