using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CharacterResizer))]
[RequireComponent(typeof(TMP_Text))]
public class TextAnimator : MonoBehaviour
{
    private const int _one = 1;
    private TMP_Text _text;
    private CharacterResizer _resizer;
    private Coroutine _coroutine;
    private float _delay = 0.06f;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _resizer = GetComponent<CharacterResizer>();
        _text.enableWordWrapping = true;
        _text.alignment = TextAlignmentOptions.Top;
    }

    private void OnEnable()
    {
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(InstantiateCharacters());
    }

    private IEnumerator InstantiateCharacters()
    {
        var waitingTime = new WaitForSeconds(_delay);

        _text.ForceMeshUpdate();

        int totalVisibleCharacters = _text.textInfo.characterCount; 
        int counter = 0;

        while (counter < totalVisibleCharacters + _one)
        {
            _text.maxVisibleCharacters = counter;
            _resizer.ResizeCharacter(counter - _one);

            counter++;

            yield return waitingTime;
        }
    }    
}



