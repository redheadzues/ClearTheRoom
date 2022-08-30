using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstStageItems : Stage
{
    [SerializeField] private Collider _target;

    private Rigidbody _rigidbody;
    private float _moveSpeed = 0.8f;
    private float _rotationSpeed = 0.8f;
    private float _delay = 0.01f;
    private float _maxLiftUp = 6f;
    private float _axisRotateAngle = 90;
    private bool _isDragable = true;
    private float _distance;
    private Quaternion _rotationAngle;

    public override int StageNumber { get; protected set; }

    private void OnValidate()
    {
        if (_target == null)
            throw new System.Exception($"Не назначен Target на объекте {gameObject}");
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        StageNumber = 1;
        _rotationAngle = new Quaternion(_axisRotateAngle, _axisRotateAngle, _axisRotateAngle, 0);
    }

    private void OnMouseDown()
    {
       _distance = Vector3.Distance(transform.position, Camera.main.transform.position);
    }

    private void OnMouseDrag()
    {
        if(_isDragable)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(_distance);
            transform.position = new Vector3(rayPoint.x, _maxLiftUp, rayPoint.z);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider == _target)
        {
            _isDragable = false;
            _rigidbody.isKinematic = true;
            StartCoroutine(OnTargetFind());
        }
    }

    private void FallInBox()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _moveSpeed);
    }

    private void OnFallRotation()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, _rotationAngle, _rotationSpeed);
    }

    private IEnumerator OnTargetFind()
    {
        var waitingTime = new WaitForSeconds(_delay);

        while (transform.position != _target.transform.position)
        {
            FallInBox();
            yield return waitingTime;
            OnFallRotation();
            yield return waitingTime;
        }

        TaskComplete();
        _target.gameObject.GetComponent<RequiredFirstStageItem>().OnHit();
        gameObject.SetActive(false);
    }
}
