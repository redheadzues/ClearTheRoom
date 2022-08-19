using UnityEngine;

public abstract class RequiredItemToCompleteStage : MonoBehaviour
{
    public abstract int StageNumber { get; protected set; }
}
