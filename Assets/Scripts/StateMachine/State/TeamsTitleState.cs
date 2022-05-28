using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamsTitleState : State
{

	//private InputGame _inputs;

	private void OnEnable()
	{
		Game.EnterTeamsTitle();

		//_inputs = new InputGame();
		//_inputs.Enable();
		//_inputs.Game.Fireworks.performed += ctx => OnFireworks();
		//_inputs.Game.SwitchVisible.performed += ctx => OnSwitchVisible();
	}

	private void OnDisable()
	{
		Game.ExitTeamsTitle();
		//_inputs.Game.Fireworks.performed -= ctx => OnFireworks();
		//_inputs.Game.SwitchVisible.performed -= ctx => OnSwitchVisible();
		//_inputs.Disable();
	}

	private void OnFireworks()
	{
		Game.OnFireworks();
	}
}