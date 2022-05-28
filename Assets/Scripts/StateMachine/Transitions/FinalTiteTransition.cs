using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTiteTransition : Transition
{
	[SerializeField] private State _teamsTitleAnimator;

	public override void OnNextButton()
	{
		_targetState = _targetStateOnStart;

		IsReadyTransit = true;
	}

	public override void OnMainTitleButton()
	{
		_targetState = _targetStateOnStart;

		IsReadyTransit = true;
	}

	public override void OnTeamsTitleButton()
	{
		_targetState = _teamsTitleAnimator;

		IsReadyTransit = true;
	}
}
