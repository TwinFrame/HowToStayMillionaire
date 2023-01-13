using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeamsTitleAnimator : TitleAnimator
{
	[Space]
	[SerializeField] private MainPlate _mainPlate;
	[SerializeField] private TeamFields _teamFieldsTemplate;
	[SerializeField] private Transform _container;
	[Header("Service")]
	[SerializeField] private Game _game;
	[SerializeField] private ConvertToMoneyFormat _convertMoney;

	private Coroutine _exitPlateJob;
	private Coroutine _changeTeamMoneyJob;
	private Coroutine _changeTeamNameJob;
	private Coroutine _fadeInNameJob;
	private Coroutine _transformNameJob;
	private Coroutine _scaleTeamFieldJob;
	private Coroutine _preparationTeamJob;
	private Coroutine _selectTeamJob;
	private Coroutine _deselectTeamJob;

	private float _currentScalingTime;
	private float _normalizeScalingTime;
	private Vector3 _startScale;
	private Vector3 _endScale;

	private float _currentChangeNameTime;
	private float _normalizeCurrentChangeNameTime;
	private Vector3 _startNamePosition;
	private Vector3 _endNamePosition;

	private float _currentChangeMoneyTime;
	private float _normalizeChangeMoneyTime;
	private int _startMoney;
	private int _endMoney;
	private int _currentBank;

	private TeamFields _currentTeamTemplate;
	private List<TeamFields> _teamFieldsList = new List<TeamFields>();

	protected override IEnumerator EnterJob()
	{
		InitTeamsContent();

		yield return WaitBetweenViewers;

		GameEvent.OnTeamsTitleEnter?.Invoke();

		_mainPlate.gameObject.SetActive(true);
		_mainPlate.enabled = true;

		ResetTitle();

		_mainPlate.Enter(out Coroutine enterJob);

		yield return enterJob;
	}

	protected override IEnumerator ExitJob()
	{
		GameEvent.OnTeamsTitleExit?.Invoke();

		_mainPlate.Exit(out _exitPlateJob);

		yield return _exitPlateJob;

		_mainPlate.enabled = false;
		_mainPlate.gameObject.SetActive(false);

		ClearTeamsContent();
	}

	protected override void ResetTitle()
	{
		_mainPlate.Reset();
	}

	public void PreparationTeam(int numTeam, int initialCapital, out Coroutine preparationTeamJob)
	{
		if (_preparationTeamJob != null)
			StopCoroutine(_preparationTeamJob);
		_preparationTeamJob = StartCoroutine(PreparationTeamJob(numTeam, initialCapital));

		preparationTeamJob = _preparationTeamJob;
	}

	public void ChangeTeamMoney(int numTeam, int money, bool isWithScaling, out Coroutine changeTeamMoneyJob)
	{
		if (_changeTeamMoneyJob != null)
			StopCoroutine(_changeTeamMoneyJob);
		_changeTeamMoneyJob = StartCoroutine(ÑhangeTeamMoneyJob(numTeam, money, isWithScaling));

		changeTeamMoneyJob = _changeTeamMoneyJob;
	}

	public void ChangeTeamName(int numTeam, string newName)
	{
		if (_changeTeamNameJob != null)
			StopCoroutine(_changeTeamNameJob);
		_changeTeamNameJob = StartCoroutine(ChangeTeamNameJob(numTeam, newName));
	}

	private void InitTeamsContent()
	{
		_teamFieldsList.Clear();

		for (int i = 0; i < _game.Teams.Count; i++)
		{
			_currentTeamTemplate = Instantiate(_teamFieldsTemplate, _container);

			_currentTeamTemplate.SetName(_game.Teams[i].Name);

			_currentTeamTemplate.SetBank(_convertMoney.MoneyFormat(_game.Teams[i].Bank.ToString(), _game.CurrentMonetaryUnit));

			_teamFieldsList.Add(_currentTeamTemplate);
		}
	}

	private void ClearTeamsContent()
	{
		for (int i = 0; i < _container.childCount; i++)
		{
			Destroy(_container.GetChild(i).gameObject);
		}
	}

	private IEnumerator PreparationTeamJob(int numTeam, int initialCapital)
	{
		SelectTeam(numTeam, out _selectTeamJob);

		yield return new WaitUntil(() => _game.Teams[numTeam].IsConfirmNameWhenPreparing);

		ChangeTeamMoney(numTeam, initialCapital, false, out _changeTeamMoneyJob);

		yield return new WaitUntil(() => _game.Teams[numTeam].IsReady);

		DeselectTeam(numTeam, out _deselectTeamJob);

		yield return _deselectTeamJob;
	}

	private IEnumerator ÑhangeTeamMoneyJob(int numTeam, int money, bool isWithScaling)
	{
		GameEvent.OnStartChangeMoneyOfTeam?.Invoke();

		_startMoney = _game.Teams[numTeam].Bank;
		_endMoney = _startMoney + money;

		_game.ChangeTeamMoney(numTeam, money, true);

		_currentChangeMoneyTime = 0;

		if (isWithScaling)
		{
			SelectTeam(numTeam, out _selectTeamJob);

			yield return _selectTeamJob;
		}

		while (_currentChangeMoneyTime <= Properties.ChangeTeamMoneyTime)
		{
			_normalizeChangeMoneyTime = _currentChangeMoneyTime / Properties.ChangeTeamMoneyTime;

			_currentBank = Mathf.RoundToInt(Mathf.Lerp(_startMoney, _endMoney,
				Properties.EaseSoftInOut.Evaluate(_normalizeChangeMoneyTime)));

			_teamFieldsList[numTeam].SetBank(_convertMoney.MoneyFormat(_currentBank.ToString(), _game.CurrentMonetaryUnit));

			_currentChangeMoneyTime += Time.deltaTime;

			yield return null;
		}

		_teamFieldsList[numTeam].SetBank(_convertMoney.MoneyFormat(_endMoney.ToString(), _game.CurrentMonetaryUnit));

		GameEvent.OnStopChangeMoneyOfTeam?.Invoke();

		if (isWithScaling)
		{
			DeselectTeam(numTeam, out _deselectTeamJob);

			yield return _deselectTeamJob;
		}
	}

	private IEnumerator ChangeTeamNameJob(int numTeam, string newName)
	{
		TMP_Text name = _teamFieldsList[numTeam].Name;

		bool isNeedScaling = _teamFieldsList[numTeam].transform.localScale.x < Properties.ScaleSelectedTeamFields;

		if (isNeedScaling)
		{
			SelectTeam(numTeam, out _selectTeamJob);

			yield return _selectTeamJob;
		}

		_startNamePosition = name.transform.position;
		_endNamePosition = _startNamePosition + Properties.OffsetPosition;

		if (_transformNameJob != null)
			StopCoroutine(_transformNameJob);
		_transformNameJob = StartCoroutine(MoveNameJob(name.transform, _startNamePosition, _endNamePosition));

		yield return _transformNameJob;

		name.text = newName;

		if (_transformNameJob != null)
			StopCoroutine(_transformNameJob);
		_transformNameJob = StartCoroutine(MoveNameJob(name.transform, _endNamePosition, _startNamePosition));

		yield return _transformNameJob;

		if (isNeedScaling)
		{
			DeselectTeam(numTeam, out _deselectTeamJob);

			yield return _deselectTeamJob;
		}
	}

	private IEnumerator MoveNameJob(Transform transform, Vector3 _startPosition, Vector3 _endPosition)
	{
		_currentChangeNameTime = 0;

		while (_currentChangeNameTime <= Properties.FadeInOutUIElements)
		{
			_currentChangeNameTime += Time.deltaTime;

			_normalizeCurrentChangeNameTime = _currentChangeNameTime / Properties.FadeInOutUIElements;

			transform.position = Vector3.Lerp(_startPosition, _endPosition,
				Properties.EaseSoftInOut.Evaluate(_normalizeCurrentChangeNameTime));

			yield return null;
		}

		transform.position = _endPosition;
	}

	private void SelectTeam(int numTeam, out Coroutine selectTeamJob)
	{
		if (_scaleTeamFieldJob != null)
			StopCoroutine(_scaleTeamFieldJob);
		_scaleTeamFieldJob = StartCoroutine(ScaleTeamFieldJob(numTeam, true));

		selectTeamJob = _scaleTeamFieldJob;
	}

	private void SelectTeam(int numTeam)
	{
		if (_scaleTeamFieldJob != null)
			StopCoroutine(_scaleTeamFieldJob);
		_scaleTeamFieldJob = StartCoroutine(ScaleTeamFieldJob(numTeam, true));
	}

	private void DeselectTeam(int numTeam, out Coroutine deselectTeamJob)
	{
		if (_scaleTeamFieldJob != null)
			StopCoroutine(_scaleTeamFieldJob);
		_scaleTeamFieldJob = StartCoroutine(ScaleTeamFieldJob(numTeam, false));

		deselectTeamJob = _scaleTeamFieldJob;
	}

	private IEnumerator ScaleTeamFieldJob(int numTeam, bool isZoomIn)
	{
		if (isZoomIn)
		{
			_startScale = Vector3.one;
			_endScale = Vector3.one * Properties.ScaleSelectedTeamFields;
		}
		else
		{
			_startScale = Vector3.one * Properties.ScaleSelectedTeamFields;
			_endScale = Vector3.one;
		}

		_currentScalingTime = 0;

		while (_currentScalingTime <= Properties.ScalingTime)
		{
			_normalizeScalingTime = _currentScalingTime / Properties.ScalingTime;

			_teamFieldsList[numTeam].transform.localScale = Vector3.Lerp(_startScale, _endScale,
				Properties.EaseSoftInOut.Evaluate(_normalizeScalingTime));

			_currentScalingTime += Time.deltaTime;

			yield return null;
		}

		_teamFieldsList[numTeam].transform.localScale = _endScale;
	}
}
