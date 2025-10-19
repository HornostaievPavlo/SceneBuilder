using System;
using DG.Tweening;
using Enums;
using Services.SceneObjectsRegistry;
using UnityEngine;
using Zenject;

namespace Gameplay
{
	public class SceneObject : MonoBehaviour
	{
		private SceneObjectTypeId _typeId;
		private string _guid;
		
		private ISceneObjectsRegistry _sceneObjectsRegistry;

		public SceneObjectTypeId TypeId => _typeId;
		public string Id => _guid;

		[Inject]
		private void Construct(ISceneObjectsRegistry sceneObjectsRegistry)
		{
			_sceneObjectsRegistry = sceneObjectsRegistry;
		}

		public void Register(SceneObjectTypeId typeId) 
		{
			GenerateGuid();
			SetTypeId(typeId);
			_sceneObjectsRegistry.Register(this);
		}
		
		public void AnimateScale(bool isScalingUp, Action onComplete = null)
		{
			Vector3 startScale = isScalingUp ? Vector3.zero : Vector3.one;
			Vector3 endScale = isScalingUp ? Vector3.one : Vector3.zero;
			
			transform.localScale = startScale;
			transform.DOKill(true);
			transform.DOScale(endScale, 0.15f)
				.SetEase(Ease.InExpo)
				.OnComplete(() => { onComplete?.Invoke(); });
		}

		private void SetTypeId(SceneObjectTypeId typeId)
		{
			_typeId = typeId;
		}

		private void GenerateGuid()
		{
			_guid = Guid.NewGuid().ToString();
		}
	}
}