using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class ModelController : IDisposable
{
    private CompositeDisposable disposables = new CompositeDisposable();

    [Inject]
    public void Construct(ModelFactory modelFactory)
    {
        IEnumerable<Model> models = Enumerable.Range(0, 5).Select(_ => modelFactory.Create());

        IObservable<int>[] streams = models
            .Select((modelData, index) =>
            {
                return modelData.GetStream();
            })
            .ToArray();

        IObservable<int> mergedStream = Observable.Merge(streams);

        mergedStream.Subscribe(
            value => Debug.Log("ModelId: " + value),
            error => Debug.LogError("Error: " + error),
            () => Debug.LogWarning("Stream completed")
        ).AddTo(disposables);



        var modelStreams = models.ToObservable()
            .Select(model => model.GetStream());

        modelStreams.Subscribe(modelStream =>
        {
            IDisposable modelStreamDisposable = modelStream
                .Subscribe(
                    value => Debug.Log("ModelId: " + value),
                    error => Debug.LogError("Error: " + error),
                    () => Debug.LogWarning("Stream completed")
                );
        });



        streams = models
            .Select((modelData, index) =>
            {
                return modelData.GetStream().Where(model => model % 2 == 0);
            })
            .ToArray();

        mergedStream = Observable.Merge(streams);

        mergedStream.Subscribe(
            value => Debug.Log("ModelId: " + value),
            error => Debug.LogError("Error: " + error),
            () => Debug.LogWarning("Stream completed")
        ).AddTo(disposables);
    }

    public void Dispose()
    {
        disposables.Dispose();
    }
}
