using System.Collections;
using UnityEngine;

public class SecondStageItems : Stage
{
    [SerializeField] private Transform _rightPlace;
    
    private ItemInRightPosition _itemInRightPosition;
    private float _moveSpeed = 20;
    private float _rotationSpeed = 150;
    private float _delay = 0.1f;
    private Coroutine _coroutine;
    private bool isPositionRight => transform.position == _rightPlace.position && transform.rotation == _rightPlace.rotation;

    public override int StageNumber { get; protected set; }

    private void Awake()
    {        
        StageNumber = 2;
        _itemInRightPosition = GameObject.FindObjectOfType<ItemInRightPosition>();
    }

    private void OnMouseDown()
    {
        if(_coroutine == null && enabled == true)
            _coroutine = StartCoroutine(OnMoveToRightPosition());
    }

    private IEnumerator OnMoveToRightPosition()
    {
        var waitingTime = new WaitForSeconds(_delay);

        while (isPositionRight != true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _rightPlace.position, _moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, _rightPlace.rotation, _rotationSpeed * Time.deltaTime);

            yield return waitingTime;
        }

        OnItemPlaced();
        _itemInRightPosition.transform.position = transform.position;
        _itemInRightPosition.gameObject.GetComponent<ParticleSystem>().Play();
    }
}
