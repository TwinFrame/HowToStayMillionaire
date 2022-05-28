using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AskQuestionState : State
{
	private InputGame _inputs;

	private int _currentChooseOption;
	private bool _isOnLoopVideo;

	private void OnEnable()
	{
		Game.EnterQuestion();
	}

	private void OnDisable()
	{
		Game.ExitQuestion();
	}
}
