using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public abstract class BasePlate : MonoBehaviour
{
	[SerializeField] protected Animator _animator;

	private Coroutine _enterJob;
	private Coroutine _exitJob;

	private int _introHash;
	private int _outroHash;

	private void Awake()
	{
		_introHash = Animator.StringToHash("Intro");
		_outroHash = Animator.StringToHash("Outro");
	}

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
		_animator.ResetTrigger(_outroHash);
		_animator.ResetTrigger(_introHash);
	}

	private IEnumerator EnterJob()
	{
		_animator.SetTrigger(_introHash);

		yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
	}

	private IEnumerator ExitJob()
	{
		_animator.SetTrigger(_outroHash);
		_animator.ResetTrigger(_introHash);

		yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("StartPosition"));
	}
}
