using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : State
{
	private void OnEnable()
	{
		Game.EnterMainTitle();
	}

	private void OnDisable()
	{
		Game.ExitMainTitle();
	}
}