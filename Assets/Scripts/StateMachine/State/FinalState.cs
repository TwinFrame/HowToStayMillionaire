using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalState : State
{
	private InputGame _inputs;

	private void OnEnable()
	{
		Game.EnterFinalTitle();

		//_inputs = new InputGame();
		//_inputs.Enable();
		//_inputs.Game.Fireworks.performed += ctx => OnFireworks();
		//_inputs.Game.SwitchVisible.performed += ctx => OnSwitchVisibilityObjectsBG();
		//_inputs.Game.Flash.performed += ctx => OnFlashGlow();
		//_inputs.Game.ChangeMainTitleElement.performed += ctx => OnChangeMainTitleElement();
	}

	private void OnDisable()
	{
		Game.ExitFinalTitle();

		//_inputs.Game.Fireworks.performed -= ctx => OnFireworks();
		//_inputs.Game.SwitchVisible.performed -= ctx => OnSwitchVisibilityObjectsBG();
		//_inputs.Game.Flash.performed -= ctx => OnFlashGlow();
		//_inputs.Game.ChangeMainTitleElement.performed -= ctx => OnChangeMainTitleElement();
		//_inputs.Disable();
	}
	/*
	private void OnFireworks()
	{
		Game.OnFireworks();
	}

	private void OnFlashGlow()
	{
		Game.OnFlashGlow();
	}

	private void OnSwitchVisibilityObjectsBG()
	{
		Game.SwitchVisibilityObjectsBG();
	}

	private void OnChangeMainTitleElement()
	{
		Game.ChangeMainTitleElement();
	}
	*/
}
