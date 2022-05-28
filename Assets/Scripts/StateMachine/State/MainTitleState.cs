using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTitleState : State
{
	private InputGame _inputs;

	private void OnEnable()
	{
		Game.EnterMainTitle();

		//_inputs = new InputGame();
		//_inputs.Enable();
		//_inputs.Game.SwitchVisibleBGElements.performed += ctx => OnSwitchVisibilityObjectsBG();
	}

	private void OnDisable()
	{
		Game.ExitMainTitle();

		//_inputs.Game.SwitchVisibleBGElements.performed -= ctx => OnSwitchVisibilityObjectsBG();
		//_inputs.Disable();
	}

	private void OnSwitchVisibilityObjectsBG()
	{
		//Game.SwitchVisibilityObjectsBG();
	}
}
