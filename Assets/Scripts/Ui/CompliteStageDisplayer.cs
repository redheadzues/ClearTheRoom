using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompliteStageDisplayer : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _text;    
    [SerializeField] private StageFinisher _stageFinisher;

    private int _precent = 100;
    private string _percentString = "%";
    private int _DontDivisionByZero = 0;

    private void OnValidate()
    {
        if(_slider == null)
            throw new System.Exception($"Не назначен слайдер на объекте {gameObject}");
        if(_text == null)
            throw new System.Exception($"Не назначен текст на объекте {gameObject}");
        if(_stageFinisher == null)
            throw new System.Exception($"Не назначен StageFinisher на объекте {gameObject}");
    }

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
        if (maxValue != _DontDivisionByZero)
            _slider.value = (float)value / maxValue;
    }

    private void DisplyaPercent(int value, int maxValue)
    {
        if (maxValue != _DontDivisionByZero)
            _text.text = (value * _precent / maxValue).ToString() + _percentString;
    }
}
