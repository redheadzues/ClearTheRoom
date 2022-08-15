using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StagePreparer))]
public class StageFinisher : MonoBehaviour
{
    [SerializeField] private ParticleSystem _congratulation;

    private StagePreparer _stagePreparer;
    private int _placedItemCount;
    public event UnityAction StageFinished;
    public event UnityAction<int, int> CompletionIncreased;

    private void Awake()
    {
        _stagePreparer = GetComponent<StagePreparer>();
    }

    private void OnEnable()
    {
        _stagePreparer.StageStarted += OnStageStarted;
        _congratulation.Stop();
    }

    private void OnDisable()
    {
        _stagePreparer.StageStarted += OnStageStarted;
    }

    private void OnStageStarted()
    {
        _placedItemCount = 0;
        CompletionIncreased?.Invoke(_placedItemCount, _stagePreparer.CurrentItem.Count);

        for (int i = 0; i < _stagePreparer.CurrentItem.Count; i++)
        {
            _stagePreparer.CurrentItem[i].ItemPlaced += OnItemPlaced;
        }
    }

    private void OnStageFinished()
    {
        for (int i = 0; i < _stagePreparer.CurrentItem.Count; i++)
        {
            _stagePreparer.CurrentItem[i].ItemPlaced -= OnItemPlaced;
        }

        StageFinished?.Invoke();
        _congratulation.Play();
    }

    private void OnItemPlaced()
    {
        _placedItemCount++;
        CompletionIncreased?.Invoke(_placedItemCount, _stagePreparer.CurrentItem.Count);

        if(_placedItemCount == _stagePreparer.CurrentItem.Count)
            OnStageFinished();
    }
}
