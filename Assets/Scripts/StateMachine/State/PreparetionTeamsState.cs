using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparetionTeamsState : State
{
	//private InputGame _inputs;

	private void OnEnable()
	{
		//_inputs = new InputGame();
		//_inputs.Enable();
		//_inputs.Game.MarkDone.performed += ctx => MarkCurrentTeamIsReady();
		//_inputs.Game.MarkNotDone.performed += ctx => MarkCurrentTeamIsReady();

		if (!Game.IsTeamsReady)
			Game.EnterPreparationTeams();
	}

	private void OnDisable()
	{
		Game.ExitPreparationTeams();
		//_inputs.Game.MarkDone.performed -= ctx => MarkCurrentTeamIsReady();
		//_inputs.Game.MarkNotDone.performed -= ctx => MarkCurrentTeamIsReady();
		//_inputs.Disable();
	}
	/*
	private void MarkCurrentTeamIsReady()
	{

	}
	*/
}
