using UnityEngine;

public class RequiredThirdStageItem : RequiredItemToCompleteStage
{
    private float _distance;
    public Ray Ray { get; private set; }
    public override int StageNumber { get; protected set; }

    private void Awake()
    {
        StageNumber = 3;
    }

    private void OnMouseDown()
    {
        _distance = Vector3.Distance(transform.position, Camera.main.transform.position);

    }

    private void OnMouseDrag()
    {
        if(gameObject.activeSelf == true)
        {
            Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = Ray.GetPoint(_distance);

            RaycastHit hit;

            if (Physics.Raycast(Ray, out hit))
            {
                if (hit.collider.TryGetComponent(out ThridStageItem clearable))
                {
                    RotateToNormal(hit.normal);
                    transform.position = ClampPosition(rayPoint, clearable, hit.normal);
                }
            }
        }    
    }

    private Vector3 ClampPosition(Vector3 position, ThridStageItem clearabele, Vector3 normal)
    {
        position.x = Mathf.Clamp(position.x, clearabele.MinBorderX, clearabele.MaxBorderX);
        position.y = Mathf.Clamp(position.y, clearabele.MinBorderY, clearabele.MaxBorderY);
        position.z = Mathf.Clamp(position.z, clearabele.MinBorderZ, clearabele.MaxBorderZ);


        if (normal == -Vector3.forward)
            position.z = clearabele.MinBorderZ;
        if(normal == Vector3.up)
            position.y = clearabele.MaxBorderY;

        return new Vector3(position.x, position.y, position.z);
    }

    private void RotateToNormal(Vector3 normal)
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);
    }
}
