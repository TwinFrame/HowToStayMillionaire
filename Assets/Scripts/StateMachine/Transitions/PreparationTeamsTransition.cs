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
			Game.WriteLog("Не все команды готовы.");
	}

	public override void OnMainTitleButton()
	{
		Game.WriteLog("Пауза с остановкой времени.");
	}

	public override void OnTeamsTitleButton()
	{
		Game.WriteLog("Пока не готовы все команды.");
	}
}
