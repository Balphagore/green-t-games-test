using Zenject;
using UniRx;

public class SceneInstaller : MonoInstaller
{
    private CompositeDisposable disposables = new CompositeDisposable();

    public override void InstallBindings()
    {
        Container.Bind<ModelController>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<ModelFactory>().AsSingle();
    }

    public void Awake()
    {
        ModelController modelController = Container.Resolve<ModelController>();
        disposables.Add(modelController);
    }

    public void OnDisable()
    {
        disposables.Dispose();
    }
}