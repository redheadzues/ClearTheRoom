using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Clearer))]
public class ThridStageItem : Stage
{
    private float _delay = 0.5f;
    private float _allowableClearValue = 0.9f;
    private Clearer _clearer;
    private Coroutine _coroutine;

    public float MaxBorderX => transform.position.x + transform.localScale.x/2;
    public float MinBorderX => transform.position.x - transform.localScale.x/2;
    public float MaxBorderY => transform.position.y + transform.localScale.y/2;
    public float MinBorderY => transform.position.y - transform.localScale.y/2;
    public float MaxBorderZ => transform.position.z + transform.localScale.z/2;
    public float MinBorderZ => transform.position.z - transform.localScale.z/2;

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
