using System.Collections;
using TMPro;
using UnityEngine;

public class MainTitleAnimator : TitleAnimator
{
	[Header("Texts")]
	[SerializeField] private TMP_Text _gameName;
	[Space]
	[SerializeField] private TitlePlate _titlePlate;

	private Coroutine _exitPlateJob;

	public void SetName(string name)
	{
		_gameName.text = name;
	}

	protected override IEnumerator EnterJob()
	{
		yield return WaitBetweenViewers;

		GameEvent.MainTitleEnteredEvent?.Invoke();
		_titlePlate.gameObject.SetActive(true);
		_titlePlate.enabled = true;

		ResetTitle();

		_titlePlate.Enter(out Coroutine enterJob);

		yield return enterJob;
	}

	protected override IEnumerator ExitJob()
	{
		GameEvent.MainTitleExitedEvent?.Invoke();

		_titlePlate.Exit(out _exitPlateJob);

		yield return _exitPlateJob;

		_titlePlate.enabled = false;
		_titlePlate.gameObject.SetActive(false);
	}

	protected override void ResetTitle()
	{
		_titlePlate.Reset();
	}
}