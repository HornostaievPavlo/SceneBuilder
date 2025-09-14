using Services.Instantiation;
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
		}
	}
}