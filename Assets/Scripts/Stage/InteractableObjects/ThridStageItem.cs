using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Clearer))]
public class ThridStageItem : Stage
{
    private float _delay = 0.5f;
    private float _allowableClearValue = 0.9f;
    private Clearer _clearer;
    private Coroutine _coroutine;
    private float _half = 0.5f;

    public float MaxBorderX => transform.position.x + transform.localScale.x * _half;
    public float MinBorderX => transform.position.x - transform.localScale.x * _half;
    public float MaxBorderY => transform.position.y + transform.localScale.y * _half;
    public float MinBorderY => transform.position.y - transform.localScale.y * _half;
    public float MaxBorderZ => transform.position.z + transform.localScale.z * _half;
    public float MinBorderZ => transform.position.z - transform.localScale.z * _half;

    public override int StageNumber { get; protected set; }

    private void Awake()
    {
        StageNumber = 3;
        _clearer = GetComponent<Clearer>();
    }

    private void OnEnable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(TryCompleteTask());
    }


    private IEnumerator TryCompleteTask()
    {
        var waitingTime = new WaitForSeconds(_delay);

        while(_clearer.TotalMaskPixels * _allowableClearValue> _clearer.TotalClearedPixels)
        {
            yield return waitingTime;
        }

        TaskComplete();
    }
}
