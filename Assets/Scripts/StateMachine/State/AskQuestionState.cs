using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AskQuestionState : State
{
	private void OnEnable()
	{
		Game.EnterQuestion();
	}

	private void OnDisable()
	{
		Game.ExitQuestion();
	}
}
