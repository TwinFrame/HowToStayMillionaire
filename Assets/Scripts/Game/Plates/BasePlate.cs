using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public abstract class BasePlate : MonoBehaviour
{
	[SerializeField] protected Animator _animator;

	private Coroutine _enterJob;
	private Coroutine _exitJob;

	public void Enter(out Coroutine enterJob)
	{
		if (_enterJob != null)
			StopCoroutine(_enterJob);
		_enterJob = StartCoroutine(EnterJob());

		enterJob = _enterJob;
	}

	public void Exit(out Coroutine exitJob)
	{
		if (_exitJob != null)
			StopCoroutine(_exitJob);
		_exitJob = StartCoroutine(ExitJob());

		exitJob = _exitJob;
	}

	public void Reset()
	{
		_animator.ResetTrigger("Outro");
		_animator.ResetTrigger("Intro");
	}

	private IEnumerator EnterJob()
	{
		_animator.SetTrigger("Intro");

		yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
	}

	private IEnumerator ExitJob()
	{
		_animator.SetTrigger("Outro");
		_animator.ResetTrigger("Intro");

		yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("StartPosition"));
	}

}
