using System;
using UniRx;

public class Model : IDisposable
{
    private int modelId;
    private IDisposable disposableResource;

    public Model(int modelId)
    {
        this.modelId = modelId;
    }

    public IObservable<int> GetStream()
    {
        return Observable.Create<int>(observer =>
        {
            observer.OnNext(modelId);
            observer.OnCompleted();

            return disposableResource = Disposable.Create(() =>
            {
                Dispose();
            });
        });
    }

    public void Dispose()
    {
        disposableResource?.Dispose();
    }
}
