using Enums;
using Gameplay;
using UI.Widgets.SceneObjects;
using UnityEngine;

namespace UI.Widgets
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