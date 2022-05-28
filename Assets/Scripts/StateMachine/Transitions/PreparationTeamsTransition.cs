using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparationTeamsTransition : Transition
{
	public override void OnNextButton()
	{
		if (Game.IsTeamsReady)
			IsReadyTransit = true;
		else
			Game.WriteLog("Not all teams are Ready.");
	}

	public override void OnMainTitleButton()
	{
		Game.WriteLog("Pause with stop time.");
	}

	public override void OnTeamsTitleButton()
	{
		Game.WriteLog("Teams is not ready yet.");
	}

	/*
	public void OnFinishedPreparationTeams()
	{
		if (Game.IsTeamsReady)
			IsReadyTransit = true;
	}
	*/
}
