using TMPro;
using UnityEngine;

namespace Gameplay
{
	public class Label : SceneObject
	{
		[SerializeField] private TMP_Text titleText;
		[SerializeField] private TMP_Text descriptionText;
		
		private string _title;
		private string _description;
		
		public string Title => _title;
		public string Description => _description;

		private void OnEnable()
		{
			_title = "Label";
			_description = "Description";
		}

		public void SetTitle(string title)
		{
			_title = title;
			titleText.text = title;
		}
		
		public void SetDescription(string description)
		{
			_description = description;
			descriptionText.text = description;
		}
	}
}