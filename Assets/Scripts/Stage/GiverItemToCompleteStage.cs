using UnityEngine;

[RequireComponent(typeof(StagePreparer))]
[RequireComponent(typeof(StageFinisher))]
public class GiverItemToCompleteStage : MonoBehaviour
{
    private RequiredItemToCompleteStage[] _requiredItems;
    private StagePreparer _preparer;
    private StageFinisher _finisher;

    private void Awake()
    {
        _preparer = GetComponent<StagePreparer>();
        _finisher = GetComponent<StageFinisher>();
        _requiredItems = FindObjectsOfType<RequiredItemToCompleteStage>();
        DeactivateAllItems();
    }

    private void OnEnable()
    {
        _preparer.StageStarted += OnStageStart;
        _finisher.StageFinished += OnStageFinish;
    }

    private void OnDisable()
    {
        _preparer.StageStarted -= OnStageStart;
        _finisher.StageFinished -= OnStageFinish;
    }

    private void OnStageStart()
    {
        ChangeRequiredItemsActiveStatus(true);
    }

    private void OnStageFinish()
    {
        ChangeRequiredItemsActiveStatus(false);
    }

    private void DeactivateAllItems()
    {
        for (int i = 0; i < _requiredItems.Length; i++)
            _requiredItems[i].gameObject.SetActive(false);
    }

    private void ChangeRequiredItemsActiveStatus(bool isActive)
    {
        for (int i = 0; i < _requiredItems.Length; i++)
        {
            if (_requiredItems[i].StageNumber == _preparer.CurrentStage)
                _requiredItems[i].gameObject.SetActive(isActive);
        }
    }
}
