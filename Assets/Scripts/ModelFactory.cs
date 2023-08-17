using Zenject;

public class ModelFactory : IFactory<Model>
{
    private int modelsCount;

    public Model Create()
    {
        Model model = new Model(modelsCount);
        modelsCount++;
        return model;
    }
}
