using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AskQuestionTransition : Transition
{
	[SerializeField] private State _mainTitleState;

	private bool _isRightAnswer;
	private bool _isNeedChangeTour;

	public override void OnNextButton()
	{
		//if (Game.IsPauseWithTimeStop)
		//	return;

		if(Game.TryGetIsAnsweredCurrentQuestion())
			IsReadyTransit = true;
	}

	public override void OnMainTitleButton()
	{
		Game.WriteLog("Pause with stop time.");
	}

	public override void OnTeamsTitleButton()
	{
		Game.WriteLog("Need exit from question state.");
	}
}
