using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTitleTransition : Transition
{
	[SerializeField] private State _finalTitle;
	[SerializeField] private State _preparationTeam;
	[SerializeField] private State _teamsTitle;

	public override void OnNextButton()
	{
		if (!Game.IsTeamsReady)
			_targetState = _preparationTeam;

		else if (Game.IsFinished)
			_targetState = _finalTitle;

		else
			_targetState = _targetStateOnStart;

		IsReadyTransit = true;
	}

	public override void OnMainTitleButton()
	{
		Game.WriteLog("Main Title: You are here.");
	}

	public override void OnTeamsTitleButton()
	{
	
		_targetState = _teamsTitle;

		if (Game.IsTeamsReady)
			IsReadyTransit = true;
		else
			Game.WriteLog("The teams have not been initialized yet.");
	}
}
