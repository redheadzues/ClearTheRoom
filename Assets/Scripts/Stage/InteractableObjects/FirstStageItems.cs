using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstStageItems : Stage
{
    [SerializeField] private Collider _target;

    private Rigidbody _rigidbody;
    private float _moveSpeed = 0.15f;
    private float _delay = 0.03f;
    private bool _isDragable = true;
    private float _distance;

    public override int StageNumber { get; protected set; }


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        StageNumber = 1;
    }

    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
            transform.position = new Vector3(rayPoint.x, 0.7f, rayPoint.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other == _target)
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

    private IEnumerator OnTargetFind()
    {
        var waitingTime = new WaitForSeconds(_delay);

        while (transform.position != _target.transform.position)
        {
            FallInBox();
            //transform.rotation = Quaternion.Lerp(transform.rotation, _rightPlace.rotation, _rotationSpeed * Time.deltaTime);

            yield return waitingTime;
        }

        OnItemPlaced();
        _target.gameObject.GetComponent<BoxAnimator>().OnHit();
        gameObject.SetActive(false);
    }
}
