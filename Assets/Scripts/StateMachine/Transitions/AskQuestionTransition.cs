using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AskQuestionTransition : Transition
{
	[SerializeField] private State _mainTitleState;

	public override void OnNextButton()
	{
		if(Game.TryGetIsAnsweredCurrentQuestion())
			IsReadyTransit = true;
	}

	public override void OnMainTitleButton()
	{
		Game.WriteLog("����� � ���������� �������.");
	}

	public override void OnTeamsTitleButton()
	{
		Game.WriteLog("������� ��������� ������� ������.");
	}
}
