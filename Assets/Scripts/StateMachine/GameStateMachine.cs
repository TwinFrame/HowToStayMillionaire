using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Game))]

public class GameStateMachine : MonoBehaviour
{
	[SerializeField] private State _firstState;
	[Header("Service")]
	[SerializeField] private Game _game;

	private State _currentState;

	private void Start()
	{
		ResetState(_firstState);
	}

	public State CurrentState => _currentState;

	private void Update()
	{
		if (_currentState == null)
			return;

		var nextState = _currentState.GetNextState();

		if (nextState != null)
			Transit(nextState);
	}

	private void ResetState(State startState)
	{
		_currentState = startState;

		if (_currentState != null)
			_currentState.Enter(_game);
	}

	private void Transit(State nextState)
	{
		if (_currentState != null)
			_currentState.Exit();

		_currentState = nextState;

		if (_currentState != null)
			_currentState.Enter(_game);
	}
}
