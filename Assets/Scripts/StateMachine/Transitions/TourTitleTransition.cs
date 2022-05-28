using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourTitleTransition : Transition
{
	[SerializeField] private State _teamsTitleState;
	[SerializeField] private State _mainTitleState;

	public override void OnNextButton()
	{

		if (!Game.IsPlaying && !Game.IsFinished)
			Game.OnIsPlaying();

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
		_targetState = _teamsTitleState;

		IsReadyTransit = true;
	}
}
