using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Outline))]
public abstract class Stage : MonoBehaviour
{
    private Outline _outline;

    public virtual event UnityAction TaskCompleted;
    public abstract int StageNumber { get; protected set; }

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    protected void TaskComplete()
    {
        TaskCompleted?.Invoke();
    }
}
