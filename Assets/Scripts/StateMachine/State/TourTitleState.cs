using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourTitleState : State
{
	private void OnEnable()
	{
		Game.EnterTourTitle();
	}

	private void OnDisable()
	{
		Game.ExitTourTitle();
	}
}
