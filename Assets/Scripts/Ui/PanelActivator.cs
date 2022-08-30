using UnityEngine;

public class PanelActivator : MonoBehaviour
{
    [SerializeField] private GameObject _stageCompletePanel;
    [SerializeField] private StageFinisher _stageFinisher;

    private void OnValidate()
    {
        if (_stageFinisher == null)
            throw new System.Exception($"Не назначен StageFinisher на объекте {gameObject}");
        if(_stageCompletePanel == null)
            throw new System.Exception($"Не назначена панель завершения стадии на объекте {gameObject}");
    }

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
