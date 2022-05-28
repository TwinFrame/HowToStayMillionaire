using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
	[SerializeField] private List<Transition> _transitions;

	protected Game Game { get; private set; }

	public void Enter(Game game)
	{
		if (enabled == false)
		{
			Game = game;

			enabled = true;

			foreach (var transition in _transitions)
			{
				transition.enabled = true;
				transition.Init(Game);
			}
		}
	}

	public void Exit()
	{
		if (enabled == true)
		{
			foreach (var transition in _transitions)
				transition.enabled = false;

			enabled = false;
		}
	}

	public State GetNextState()
	{
		foreach (var transition in _transitions)
		{
			if (transition.NeedTransit)
				return transition.TargetState;
		}

		return null;
	}
}
