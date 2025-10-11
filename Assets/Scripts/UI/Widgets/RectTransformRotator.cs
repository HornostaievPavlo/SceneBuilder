using UnityEngine;

namespace UI.Widgets
{
	public class RectTransformRotator : MonoBehaviour
	{
		private RectTransform _rectTransform;

		private void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();
		}

		private void OnEnable()
		{
			_rectTransform.eulerAngles = Vector3.zero;
		}

		private void Update()
		{
			_rectTransform.eulerAngles -= new Vector3(0, 0, 150f * Time.deltaTime);
		}
	}
}