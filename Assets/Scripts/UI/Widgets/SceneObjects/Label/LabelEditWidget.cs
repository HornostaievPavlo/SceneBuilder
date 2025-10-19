using System;
using DG.Tweening;
using Gameplay;
using Services.SceneObjectSelection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Widgets.SceneObjects.Label
{
	public class LabelEditWidget : MonoBehaviour
	{
		[SerializeField] private Button closeButton;
		[SerializeField] private Button applyButton;

		[SerializeField] private TMP_InputField titleInputField;
		[SerializeField] private TMP_InputField descriptionInputField;
		
		private Gameplay.Label _label;
		private Vector3 _initialPosition;
		
		private ISceneObjectSelectionService _sceneObjectSelectionService;
		
		private const float AnimationDuration = 0.25f;
		private const float TweenPositionOffset = 380f;

		public event Action OnClosed;

		[Inject]
		private void Construct(ISceneObjectSelectionService sceneObjectSelectionService)
		{
			_sceneObjectSelectionService = sceneObjectSelectionService;
		}

		private void Awake()
		{
			_initialPosition = transform.localPosition;
		}

		private void OnEnable()
		{
			_sceneObjectSelectionService.OnObjectSelected += HandleObjectSelected;
			
			closeButton.onClick.AddListener(HandleCloseButtonClicked);
			applyButton.onClick.AddListener(HandleApplyButtonClicked);
		}
		
		private void OnDisable()
		{
			_sceneObjectSelectionService.OnObjectSelected -= HandleObjectSelected;
			
			closeButton.onClick.RemoveListener(HandleCloseButtonClicked);
			applyButton.onClick.RemoveListener(HandleApplyButtonClicked);
		}

		public void Setup(Gameplay.Label label)
		{
			_label = label;

			if (transform.localPosition == _initialPosition && gameObject.activeSelf)
				return;
			
			gameObject.SetActive(true);
			AnimateAppear();
		}

		private void HandleObjectSelected(SceneObject sceneObject)
		{
			if (sceneObject as Gameplay.Label != _label)
			{
				AnimateDisappear();
			}
		}

		private void HandleCloseButtonClicked()
		{
			OnClosed?.Invoke();
			AnimateDisappear();
		}

		private void HandleApplyButtonClicked()
		{
			if (titleInputField.text != string.Empty && titleInputField.text != _label.Title)
			{
				_label.SetTitle(titleInputField.text);
			}
			
			if (descriptionInputField.text != string.Empty && descriptionInputField.text != _label.Description)
			{
				_label.SetDescription(descriptionInputField.text);
			}

			OnClosed?.Invoke();
			gameObject.SetActive(false);
		}
		
		private void AnimateAppear()
		{
			transform.localPosition = GetOHiddenPosition();
			
			transform.DOKill(true);
			transform.DOLocalMoveX(_initialPosition.x, AnimationDuration).SetEase(Ease.OutBack);
		}

		private void AnimateDisappear()
		{
			transform.DOKill(true);
			transform.DOLocalMoveX(GetOHiddenPosition().x, AnimationDuration)
				.SetEase(Ease.InBack)
				.OnComplete(() =>
				{
					gameObject.SetActive(false);
					transform.localPosition = _initialPosition;
				});
		}
		
		private Vector3 GetOHiddenPosition()
		{
			return new Vector3(_initialPosition.x - TweenPositionOffset, _initialPosition.y, _initialPosition.z);
		}
	}
}