using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompliteStageDisplayer : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _text;    
    [SerializeField] private StageFinisher _stageFinisher;

    private int _precent = 100;

    private void OnEnable()
    {
        _stageFinisher.CompletionIncreased += DisplayCompleteness;
    }

    private void OnDisable()
    {
        _stageFinisher.CompletionIncreased -= DisplayCompleteness;
    }

    private void DisplayCompleteness(int value, int maxValue)
    {
        FillSlider(value, maxValue);
        DisplyaPercent(value, maxValue);
    }

    private void FillSlider(int value, int maxValue)
    {
        if (maxValue != 0)
            _slider.value = (float)value / maxValue;
    }

    private void DisplyaPercent(int value, int maxValue)
    {
        if (maxValue != 0)
            _text.text = (value * _precent / maxValue).ToString() + "%";
    }
}
