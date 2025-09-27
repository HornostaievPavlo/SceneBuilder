using Services.Input;
using Services.Instantiation;
using Services.Painting;
using Services.SceneObjectSelection;
using Services.SceneObjectsRegistry;
using Zenject;

namespace Installers
{
	public class BootstrapInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<InstantiateService>().AsSingle();
			Container.BindInterfacesAndSelfTo<SceneObjectsRegistry>().AsSingle();
			Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
			Container.BindInterfacesAndSelfTo<SceneObjectSelectionService>().AsSingle();
			Container.BindInterfacesAndSelfTo<ModelPaintingService>().AsSingle();
		}
	}
}