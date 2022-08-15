using System.Collections;
using UnityEngine;
using TMPro;

public class TextAnimation : MonoBehaviour
{
    private Coroutine _coroutine;
    private TMP_Text _text;
    private CharacterResizer _resizer;
    private float _delay = 0.1f;

    void Awake()
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

        while (counter < totalVisibleCharacters+1)
        {

            _text.maxVisibleCharacters = counter;
            _resizer.ResizeCharacter(counter);

            counter += 1;

            yield return waitingTime;
        }
    }    
}



