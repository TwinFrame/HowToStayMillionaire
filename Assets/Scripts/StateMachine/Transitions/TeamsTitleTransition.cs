using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamsTitleTransition : Transition
{
	[SerializeField] private State _mainTitleState;

	private bool _isNeedChangeTour;

	public override void OnNextButton()
	{
		if (Game.Tours[Game.CurrentNumTour].Questions[Game.CurrentNumQuestion].IsAnswerShowReadOnly
			&& !Game.IsFinished)
		{
			// About Teams
			Game.CurrentTeamIsAsked();

			if (Game.IsAllTeamsIsAsked())
				Game.ResetTeamsIsAsked();

			Game.GetNextTeam();


			// About Tours and Questions
			if (Game.TryGetNextQuestion(out _isNeedChangeTour))
			{
				if (_isNeedChangeTour)
				{
					Game.NextNumTour();

					Game.ResetNumQuestion();
				}
				else
				{
					Game.NextNumQuestion();
				}
			}
			else
			{
				Game.OnIsFinished();
				Game.ChooseTeamWinner();
			}

			IsReadyTransit = true;
		}

		if (Game.IsFinished || _isNeedChangeTour)
			_targetState = _mainTitleState;
		else
			_targetState = _targetStateOnStart;

		IsReadyTransit = true;
	}

	public override void OnMainTitleButton()
	{
		_targetState = _mainTitleState;

		IsReadyTransit = true;
	}
	
	public override void OnTeamsTitleButton()
	{
		Game.WriteLog("Вы уже на слайде о командах.");
	}
}