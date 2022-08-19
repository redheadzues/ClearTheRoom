using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StagePreparer : MonoBehaviour
{
    [SerializeField] private float _highlightsTime;

    private Stage[] _stagesItems;
    private List<Stage> _currentStageItems;
    private int _currentStage;

    public event UnityAction StageStarted;
    public IReadOnlyList<Stage> CurrentItem => _currentStageItems;
    public int CurrentStage => _currentStage;

    private void OnValidate()
    {
        if(_highlightsTime <= 0)   
            _highlightsTime = 1.5f;
    }

    private void Awake()
    {
        _stagesItems = GameObject.FindObjectsOfType<Stage>();
        DeactivateScripts();
    }

    private void Start()
    {
        _currentStage = 0;
        StartNextStage();
    }

    public void StartNextStage()
    {
        _currentStage++;
        FindCurrentStageItems();
        ActivateScripts();
        StartCoroutine(OnHighlightsOutline());
        StageStarted?.Invoke();
    }

    private void DeactivateScripts()
    {
        for(int i = 0; i < _stagesItems.Length; i++)
            _stagesItems[i].enabled = false;
    }

    private void ActivateScripts()
    {
        for(int i = 0; i < _currentStageItems.Count; i++)
            _currentStageItems[i].enabled = true;         
    }

    private void FindCurrentStageItems()
    {
        _currentStageItems = new List<Stage>();

        for (int i = 0; i < _stagesItems.Length; i++)
        {
            if (_stagesItems[i].StageNumber == _currentStage)
                _currentStageItems.Add(_stagesItems[i]);
        }
    }
    private void HighlightsOutline(bool isHighlighs)
    {
        for (int i = 0; i < _currentStageItems.Count; i++)
        {
            var outline = _currentStageItems[i].gameObject.GetComponent<Outline>();
            outline.enabled = isHighlighs;
        }
    }

    private IEnumerator OnHighlightsOutline()
    {
        var waitingTime = new WaitForSeconds(_highlightsTime);

        HighlightsOutline(true);

        yield return waitingTime;

        HighlightsOutline(false);
    }
}
