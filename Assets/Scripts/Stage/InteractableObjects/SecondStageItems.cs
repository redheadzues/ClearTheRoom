using System.Collections;
using UnityEngine;

public class SecondStageItems : Stage
{
    [SerializeField] private Transform _rightPlace;
    [SerializeField] private float _moveSpeed;
    
    private ItemInRightPosition _itemInRightPosition;
    private Coroutine _coroutine;
    private float _rotationSpeed = 60;
    private float _delay = 0.01f;

    private bool isPositionRight => (transform.position == _rightPlace.position) && (transform.rotation == _rightPlace.rotation);

    public override int StageNumber { get; protected set; }

    private void OnValidate()
    {
        if(_rightPlace == null)
            throw new System.Exception($"Не назначена верная позиция на объекте {gameObject}");
        if(_moveSpeed <= 0)
            _moveSpeed = 20;
    }

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

    private void TryToPlayCelebration()
    {
        _itemInRightPosition.transform.position = transform.position;

        if (_itemInRightPosition.gameObject.TryGetComponent<ParticleSystem>(out ParticleSystem particle))
            particle.Play();
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

        TaskComplete();
        TryToPlayCelebration();
    }
}
