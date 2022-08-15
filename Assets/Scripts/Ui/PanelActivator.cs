using UnityEngine;

public class PanelActivator : MonoBehaviour
{
    [SerializeField] private GameObject _stageCompletePanel;
    [SerializeField] private StageFinisher _stageFinisher;

    private void OnEnable()
    {
        _stageFinisher.StageFinished += OnStageFinished;
    }

    private void OnDisable()
    {
        _stageFinisher.StageFinished -= OnStageFinished;
    }

    private void OnStageFinished()
    {
        _stageCompletePanel.SetActive(true);
    }
}
