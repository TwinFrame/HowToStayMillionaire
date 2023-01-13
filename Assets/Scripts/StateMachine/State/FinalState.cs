using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalState : State
{
	private void OnEnable()
	{
		Game.EnterFinalTitle();
	}

	private void OnDisable()
	{
		Game.ExitFinalTitle();
	}
}
