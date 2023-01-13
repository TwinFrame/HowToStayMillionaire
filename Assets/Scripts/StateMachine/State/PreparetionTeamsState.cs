using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparetionTeamsState : State
{
	private void OnEnable()
	{
		if (!Game.IsTeamsReady)
			Game.EnterPreparationTeams();
	}

	private void OnDisable()
	{
		Game.ExitPreparationTeams();
	}
}
