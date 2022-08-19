using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RequiredFirstStageItem : RequiredItemToCompleteStage
{
    [SerializeField] private ParticleSystem _dustParticle;

    private Animator _animator;

    public override int StageNumber { get; protected set; }

    private void OnValidate()
    {
        if(_dustParticle == null )
            throw new System.Exception($"Не назначен particle на объекте {gameObject}");
    }

    private void Awake()
    {
        StageNumber = 1;
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _dustParticle.Stop();
    }

    public void OnHit()
    {
        _dustParticle.Play();
        _animator.SetTrigger("Hit");
    }
}
