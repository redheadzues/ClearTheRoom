using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Outline))]
public abstract class Stage : MonoBehaviour
{
    private Outline _outline;

    public virtual event UnityAction TaskCompleted;
    public abstract int StageNumber { get; protected set; }

    protected void TaskComplete()
    {
        TaskCompleted?.Invoke();
    }

    private void OnEnable()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }
}
