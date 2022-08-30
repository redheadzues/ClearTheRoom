using System.Collections.Generic;
using UnityEngine;

public class RequiredThirdStageItem : RequiredItemToCompleteStage
{
    [SerializeField] private float _speed;

    private float _distance;
    private ThridStageItem[] _stageItems;
    private List<Collider> _colliders;
    private RaycastHit hit;

    public Ray Ray { get; private set; }
    public override int StageNumber { get; protected set; }

    private void Awake()
    {
        StageNumber = 3;
        _stageItems = GameObject.FindObjectsOfType<ThridStageItem>();
        _colliders = new List<Collider>();
        AddCollidersInArray();
    }    

    private void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Mouse0))
        {
            _distance = Vector3.Distance(transform.position, Camera.main.transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = Ray.GetPoint(_distance);

            if (Physics.Raycast(ray, out hit))
            {
                if (CheckCollider(hit.collider, out ThridStageItem clearable))
                {
                    Ray = ray;
                    RotateToNormal(hit.normal);
                    var target = ClampPosition(rayPoint, clearable, hit.normal);

                    transform.position = Vector3.Lerp(transform.position, target,  _speed * Time.deltaTime);

                }
            }
        }
    }

    private void AddCollidersInArray()
    {
        for (int i = 0; i < _stageItems.Length; i++)
        {
            var collider = _stageItems[i].gameObject.GetComponent<Collider>();

            _colliders.Add(collider);
        }
    }

    private bool CheckCollider(Collider collider, out ThridStageItem clearable)
    {
        clearable = null;

        for(int i = 0; i < _colliders.Count; i++)
        {
            if (collider == _colliders[i])
                clearable = _stageItems[i];
        }

        return clearable != null;
    }

    private Vector3 ClampPosition(Vector3 position, ThridStageItem clearabele, Vector3 normal)
    {
        position.x = Mathf.Clamp(position.x, clearabele.MinBorderX, clearabele.MaxBorderX);
        position.y = Mathf.Clamp(position.y, clearabele.MinBorderY, clearabele.MaxBorderY);
        position.z = Mathf.Clamp(position.z, clearabele.MinBorderZ, clearabele.MaxBorderZ);


        if (normal == -Vector3.forward)
            position.z = clearabele.MinBorderZ;
        if (normal == Vector3.up)
            position.y = clearabele.MaxBorderY;

        return new Vector3(position.x, position.y, position.z);
    }

    private void RotateToNormal(Vector3 normal)
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);
    }
}