using Services.Input;
using Services.Instantiation;
using Services.Loading;
using Services.LocalSavesRepository;
using Services.Painting;
using Services.Saving;
using Services.SceneObjectCopying;
using Services.SceneObjectSelection;
using Services.SceneObjectsRegistry;
using Zenject;

namespace Installer
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
			Container.BindInterfacesAndSelfTo<SaveService>().AsSingle();
			Container.BindInterfacesAndSelfTo<LoadService>().AsSingle();
			Container.BindInterfacesAndSelfTo<LocalSavesRepository>().AsSingle();
			Container.BindInterfacesAndSelfTo<SceneObjectCopyService>().AsSingle();
		}
	}
}