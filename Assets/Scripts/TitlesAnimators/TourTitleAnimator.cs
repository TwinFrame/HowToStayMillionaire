using System.Collections;
//using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TourTitleAnimator : TitleAnimator
{
	[Header("Texts")]
	[SerializeField] private TMP_Text _tourName;
	[SerializeField] private TMP_Text _teamName;
	[Space]
	[SerializeField] private TitlePlate _titlePlate;

	private Coroutine _enterPlateJob;
	private Coroutine _exitPlateJob;
	private Coroutine _changeCurrentTeamJob;
	private Coroutine _moveNameJob;

	private float _moveTextCurrentTime;
	private float _moveTextCurrentTimeNormalize;
	private Vector3 _startTeamNamePosition;
	private Vector3 _endTeamNamePosition;

	protected override IEnumerator EnterJob()
	{
		yield return WaitBetweenViewers;

		//_gameEvent.OnMainTitleEnter?.Invoke();

		_titlePlate.gameObject.SetActive(true);
		_titlePlate.enabled = true;

		ResetTitle();

		_titlePlate.Enter(out _enterPlateJob);

		yield return _enterPlateJob;
	}

	protected override IEnumerator ExitJob()
	{
		//_gameEvent.OnMainTitleExit?.Invoke();

		_titlePlate.Exit(out _exitPlateJob);

		yield return _exitPlateJob;

		_titlePlate.enabled = false;
		_titlePlate.gameObject.SetActive(false);
	}

	protected override void ResetTitle()
	{
		_titlePlate.Reset();
	}

	public void ActualizeTexts(string tourName, string teamName)
	{
		_tourName.text = tourName;
		_teamName.text = teamName;
	}

	public void ChangeCurrentTeam(string text)
	{
		if (_changeCurrentTeamJob != null)
			StopCoroutine(_changeCurrentTeamJob);
		_changeCurrentTeamJob = StartCoroutine(ChangeCurrentTeamJob(text));
	}
	
	protected IEnumerator ChangeCurrentTeamJob(string text)
	{
		_startTeamNamePosition = _teamName.transform.localPosition;
		_endTeamNamePosition = _startTeamNamePosition - Properties.OffsetPosition;

		if (_moveNameJob != null)
			StopCoroutine(_moveNameJob);
		_moveNameJob = StartCoroutine(MoveTextJob(_teamName.transform, _startTeamNamePosition, _endTeamNamePosition));

		yield return _moveNameJob;

		_endTeamNamePosition = _startTeamNamePosition + Properties.OffsetPosition;

		_teamName.text = text;

		if (_moveNameJob != null)
			StopCoroutine(_moveNameJob);
		_moveNameJob = StartCoroutine(MoveTextJob(_teamName.transform, _endTeamNamePosition, _startTeamNamePosition));

		yield return _moveNameJob;
	}

	private IEnumerator MoveTextJob(Transform transform, Vector3 _startPosition, Vector3 _endPosition)
	{
		_moveTextCurrentTime = 0;

		while (_moveTextCurrentTime <= Properties.FadeInOutUIElements)
		{
			_moveTextCurrentTime += Time.deltaTime;

			_moveTextCurrentTimeNormalize = _moveTextCurrentTime / Properties.FadeInOutUIElements;

			transform.localPosition = Vector3.Lerp(_startPosition, _endPosition,
				Properties.EaseSoftInOut.Evaluate(_moveTextCurrentTimeNormalize));

			yield return null;
		}

		transform.localPosition = _endPosition;
	}
}
