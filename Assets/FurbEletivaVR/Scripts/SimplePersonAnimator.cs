using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SimplePersonAnimator : MonoBehaviour
{
    public int AnimationToPlay;
    public bool PlayFromStart;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (PlayFromStart)
            animator.SetInteger("Animation_int", AnimationToPlay);
    }

    public IEnumerator Play(float secDelay = 0)
    {
        yield return new WaitForSeconds(secDelay);

        animator.SetInteger("Animation_int", AnimationToPlay);

        yield return new WaitForSeconds(1f);

        animator.SetInteger("Animation_int", 0);
    }
}
