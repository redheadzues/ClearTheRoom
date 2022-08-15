using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BoxAnimator : MonoBehaviour
{
    [SerializeField] private ParticleSystem _dustParticle;

    private Animator _animator;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _dustParticle.Stop();
    }

    public void OnHit()
    {
        _dustParticle.Play();
        _animator.SetTrigger("Hit");
    }
}
