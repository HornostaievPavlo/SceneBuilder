using Enums;
using Gameplay;
using UnityEngine;

namespace UI.Widgets.SceneObjects.Label
{
	public class LabelInfoLayoutWidget : SceneObjectInfoLayoutWidget
	{
		[SerializeField] private LabelEditWidget labelEditWidget;
		
		protected override SceneObjectInfoWidget CreateInfoWidget(SceneObject sceneObject)
		{
			if (sceneObject.TypeId != SceneObjectTypeId.Label)
				return null;
			
			LabelInfoWidget labelInfoWidget = base.CreateInfoWidget(sceneObject) as LabelInfoWidget;
			labelInfoWidget.SetEditWidget(labelEditWidget);
			
			return labelInfoWidget;
		}
	}
}