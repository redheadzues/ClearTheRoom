using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Outline))]
public abstract class Stage : MonoBehaviour
{
    private Outline _outline;

    public event UnityAction ItemPlaced;
    public abstract int StageNumber { get; protected set; }

    protected void OnItemPlaced()
    {
        ItemPlaced?.Invoke();
    }

    private void OnEnable()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }
}
