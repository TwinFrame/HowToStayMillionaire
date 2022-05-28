using System.Collections;
using TMPro;
using UnityEngine;

public class FinalTitleAnimator : TitleAnimator
{
	[Header("Texts")]
	[SerializeField] private TMP_Text _finalName;
	[Space]
	[SerializeField] private TitlePlate _titlePlate;

	private Coroutine _exitPlateJob;

	public void SetName(string name)
	{
		_finalName.text = name;
	}

	protected override IEnumerator EnterJob()
	{
		yield return WaitBetweenViewers;

		GameEvent.OnFinalTitleEnter?.Invoke();

		_titlePlate.gameObject.SetActive(true);
		_titlePlate.enabled = true;

		ResetTitle();

		_titlePlate.Enter(out Coroutine enterJob);

		yield return enterJob;
	}

	protected override IEnumerator ExitJob()
	{
		GameEvent.OnFinalTitleExit?.Invoke();

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