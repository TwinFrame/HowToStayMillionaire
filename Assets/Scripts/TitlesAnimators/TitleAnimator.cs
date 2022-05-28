using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TitleAnimator : MonoBehaviour
{
	[Header("Service")]
	[SerializeField] protected GameProperties Properties;
	[SerializeField] protected GameEvents GameEvent;

	private bool _isEntering;
	private Coroutine _exitJob;
	private Coroutine _enterJob;
	protected WaitForSeconds WaitBetweenElements;
	protected WaitForSeconds WaitBetweenViewers;

	public bool IsEntering => _isEntering;

	private void Awake()
	{
		WaitBetweenElements = new WaitForSeconds(Properties.DelayBetweenElementAnimations);
		WaitBetweenViewers = new WaitForSeconds(Properties.DelayBetweenTitleAnimations);
	}

	public void Enter(out Coroutine enterJob)
	{
		_isEntering = true;

		if (_enterJob != null)
			StopCoroutine(_enterJob);
		_enterJob = StartCoroutine(EnterJob());

		enterJob = _enterJob;
	}

	public void Exit()
	{
		_isEntering = false;

		if (_exitJob != null)
			StopCoroutine(_exitJob);
		_exitJob = StartCoroutine(ExitJob());
	}

	protected abstract IEnumerator EnterJob();

	protected abstract IEnumerator ExitJob();

	protected abstract void ResetTitle();
}