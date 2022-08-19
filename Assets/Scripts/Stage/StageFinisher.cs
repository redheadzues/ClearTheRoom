using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(StagePreparer))]
public class StageFinisher : MonoBehaviour
{
    [SerializeField] private ParticleSystem _congratulation;

    private StagePreparer _stagePreparer;
    private int _taskCompleted;
    public event UnityAction StageFinished;
    public event UnityAction<int, int> CompletionIncreased;

    private void OnValidate()
    {
        if(_congratulation == null)
            throw new System.Exception($"Не назначен particle на объекте {gameObject}");
    }

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
        _stagePreparer.StageStarted -= OnStageStarted;
    }

    private void OnStageStarted()
    {
        _taskCompleted = 0;
        CompletionIncreased?.Invoke(_taskCompleted, _stagePreparer.CurrentItem.Count);

        for (int i = 0; i < _stagePreparer.CurrentItem.Count; i++)
        {
            _stagePreparer.CurrentItem[i].TaskCompleted += OnTaskCoplete;
        }
    }

    private void OnStageFinished()
    {
        for (int i = 0; i < _stagePreparer.CurrentItem.Count; i++)
        {
            _stagePreparer.CurrentItem[i].TaskCompleted -= OnTaskCoplete;
        }

        StageFinished?.Invoke();
        _congratulation.Play();
    }

    private void OnTaskCoplete()
    {
        _taskCompleted++;
        CompletionIncreased?.Invoke(_taskCompleted, _stagePreparer.CurrentItem.Count);

        if (_taskCompleted == _stagePreparer.CurrentItem.Count)
            OnStageFinished();
    }
}
