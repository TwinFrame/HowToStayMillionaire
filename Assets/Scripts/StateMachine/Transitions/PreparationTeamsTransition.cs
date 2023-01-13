using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparationTeamsTransition : Transition
{
	public override void OnNextButton()
	{
		if (Game.IsTeamsReady)
			IsReadyTransit = true;
		else
			Game.WriteLog("�� ��� ������� ������.");
	}

	public override void OnMainTitleButton()
	{
		Game.WriteLog("����� � ���������� �������.");
	}

	public override void OnTeamsTitleButton()
	{
		Game.WriteLog("���� �� ������ ��� �������.");
	}
}
