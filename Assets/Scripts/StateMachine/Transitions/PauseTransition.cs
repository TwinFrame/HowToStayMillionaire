using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTransition : Transition
{
	[SerializeField] State _teamsTitleState;

	public override void OnMainTitleButton()
	{
		_targetState = _targetStateOnStart;
		IsReadyTransit = true;
	}

	public override void OnNextButton()
	{
		_targetState = _targetStateOnStart;
		IsReadyTransit = true;
	}

	public override void OnTeamsTitleButton()
	{
		_targetState = _teamsTitleState;
		IsReadyTransit = true;
	}
}
