using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
	[SerializeField] private List<Transform> _indicators;
	[Header("Service")]
	[SerializeField] private GameEvents _gameEvents;

	private readonly List<Vector3> _startScales = new List<Vector3>();
	private readonly List<Vector3> _endScales = new List<Vector3>();
	private Vector3 _currentEndScale = new Vector3();

	private Coroutine _countdownJob;
	private float _currentTime;
	private float _normalizeCurrentTime;


	private void Awake()
	{
		InitStartAndEndScales();
	}

	public void StartCountdown(float timeToQuestion)
	{
		if (_countdownJob != null)
			StopCoroutine(_countdownJob);
		_countdownJob = StartCoroutine(CountdownJob(timeToQuestion));
	}

	public void ResetIndicator()
	{
		if (_countdownJob != null)
			StopCoroutine(_countdownJob);

		for (int i = 0; i < _indicators.Count; i++)
			_indicators[i].localScale = _startScales[i];
	}

	private IEnumerator CountdownJob(float timeToQuestion)
	{
		_currentTime = 0;

		_gameEvents.StartCountdown();

		while (_currentTime <= timeToQuestion)
		{
			_currentTime += Time.deltaTime;

			_normalizeCurrentTime = _currentTime / timeToQuestion;

			for (int i = 0; i < _indicators.Count; i++)
			{
				_indicators[i].localScale = Vector3.Lerp(_startScales[i], _endScales[i], _normalizeCurrentTime);
			}

			yield return null;
		}

		_gameEvents.StopCountdown();
	}

	private void InitStartAndEndScales()
	{
		for (int i = 0; i < _indicators.Count; i++)
		{
			_startScales.Add(_indicators[i].localScale);

			_currentEndScale.Set(0, _indicators[i].localScale.y, _indicators[i].localScale.z);
			_endScales.Add(_currentEndScale);
		}
	}
}
