using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamsTitleState : State
{
	private void OnEnable()
	{
		Game.EnterTeamsTitle();
	}

	private void OnDisable()
	{
		Game.ExitTeamsTitle();
	}
}