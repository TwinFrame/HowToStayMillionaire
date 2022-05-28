using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Transition : MonoBehaviour
{
	[SerializeField] protected State _targetState;

	protected bool IsReadyTransit;
	protected State _targetStateOnStart;

	protected Game Game { get; private set; }

	public bool NeedTransit { get; protected set; }
	public State TargetState => _targetState;

	public abstract void OnNextButton();
	public abstract void OnMainTitleButton();
	public abstract void OnTeamsTitleButton();


	public void Init(Game game)
	{
		Game = game;
	}

	protected virtual void OnEnable()
	{
		NeedTransit = false;
		IsReadyTransit = false;
	}

	private void Start()
	{
		_targetStateOnStart = _targetState;
	}

	protected void Update()
	{
		if (IsReadyTransit)
		{
			NeedTransit = true;
		}
	}
}
