using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EffectorForPrimitives))]

public class PrimitiveObjects : MonoBehaviour
{
	[SerializeField] private float _startForce;
	[SerializeField] private float _zoomOutTime;
	[Header("Autopilot")]
	[SerializeField] private float _autopilotForce;
	[SerializeField] private float _impulseEverySeconds;
	[SerializeField] private float _halfLoopTime;
	[Header("Service")]
	[SerializeField] private GameProperties _properties;
	[SerializeField] private GameSpace _gameSpace;
	
	private EffectorForPrimitives _effectorForPrimitives;

	private List<Rigidbody> _rigidbodies = new List<Rigidbody>();
	private List<Vector3> _startScales = new List<Vector3>();
	private List<Vector3> _startPositions = new List<Vector3>();

	private Coroutine _zoomOutPrimitives;
	private float _currentZoomOutTime;
	private float _currentNormalizeZoomOutTime;
	private Coroutine _autopilotModeJob;
	private float _currentAutopilotTime;
	private float _currentLoopTime;
	private Coroutine _waitExitJob;

	private bool _isExiting = false;

	private void Awake()
	{
		_effectorForPrimitives = GetComponent<EffectorForPrimitives>();

		_rigidbodies = CollectChildRigidbodies();
		_startScales = CollectStartScales(_rigidbodies);
	}

	public void EnterPrimitives()
	{
		if (_isExiting)
		{
			_isExiting = false;

			if (_waitExitJob != null)
				StopCoroutine(_waitExitJob);
			_waitExitJob = StartCoroutine(WaitExitJob());
		}
		else
		{
			PlaceOnEndZone();

			EnableAllPrimitives();

			_effectorForPrimitives.ResetVelocity(_rigidbodies);

			_effectorForPrimitives.AttractionToCenter(_rigidbodies, _startForce);

			StartAutopilotMode();
		}
	}

	public void ExitPrimitives()
	{
		ExitAutopilotMode();

		_effectorForPrimitives.AttractionInDirection(_rigidbodies, _startForce, -Vector3.forward);

		if (_zoomOutPrimitives != null)
			StopCoroutine(_zoomOutPrimitives);
		_zoomOutPrimitives = StartCoroutine(ZoomOutPrimitives());
	}

	public void AddForce(float value, bool isInside = true)
	{
		if (!isInside)
			value *= -1;

		_effectorForPrimitives.AttractionToCenter(_rigidbodies, value);
	}

	public void Restart()
	{
		DisableAllPrimitives();
		EnterPrimitives();
	}

	private void PlaceOnEndZone()
	{
		_startPositions = _gameSpace.GetPointsOnEndZone(_rigidbodies.Count);

		for (int i = 0; i < _startPositions.Count; i++)
		{
			_rigidbodies[i].transform.SetPositionAndRotation(_startPositions[i],
				Quaternion.Euler(Random.Range(0, 359), Random.Range(0, 359), Random.Range(0, 359)));
		}
	}

	private void EnableAllPrimitives()
	{
		foreach (var rigidbody in _rigidbodies)
			rigidbody.gameObject.SetActive(true);
	}

	private void DisableAllPrimitives()
	{
		foreach (var rigidbody in _rigidbodies)
			rigidbody.gameObject.SetActive(false);
	}

	private List<Rigidbody> CollectChildRigidbodies()
	{
		List<Rigidbody> rigidbodies = new List<Rigidbody>();

		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
			{
				rigidbodies.Add(rigidbody);
			}
		}

		return rigidbodies;
	}

	private List<Vector3> CollectStartScales(List<Rigidbody> rigidbodies)
	{
		List<Vector3> scales = new List<Vector3>();

		foreach (var rigidbody in rigidbodies)
		{
			scales.Add(rigidbody.transform.localScale);
		}

		return scales;
	}

	private void StartAutopilotMode()
	{
		if (_autopilotModeJob != null)
			StopCoroutine(_autopilotModeJob);

		_autopilotModeJob = StartCoroutine(AutopilotModeJob());
	}

	private void ExitAutopilotMode()
	{
		if (_autopilotModeJob != null)
			StopCoroutine(_autopilotModeJob);
	}

	private void SetPrimitivesStartScale()
	{
		for (int i = 0; i < _rigidbodies.Count; i++)
			_rigidbodies[i].transform.localScale = _startScales[i];
	}

	private IEnumerator AutopilotModeJob()
	{
		_currentAutopilotTime = 0;
		_currentLoopTime = 0;

		while (true)
		{
			_currentAutopilotTime += Time.deltaTime;
			_currentLoopTime += Time.deltaTime;

			if (_currentAutopilotTime >= _impulseEverySeconds)
			{
				_currentAutopilotTime = 0;
				_effectorForPrimitives.AttractionToCenter(_rigidbodies, _autopilotForce, true);
			}

			if (_currentLoopTime >= _halfLoopTime)
			{
				_currentLoopTime = 0;
				_autopilotForce *= -1;
			}

			yield return null;
		}
	}

	private IEnumerator ZoomOutPrimitives()
	{
		_isExiting = true;

		_currentZoomOutTime = 0;

		while (_currentZoomOutTime <= _zoomOutTime)
		{
			_currentNormalizeZoomOutTime = _currentZoomOutTime / _zoomOutTime;

			for (int i = 0; i < _rigidbodies.Count; i++)
			{
				_rigidbodies[i].transform.localScale = Vector3.Lerp(_startScales[i], Vector3.zero,
					_properties.EaseSoftInOut.Evaluate(_currentNormalizeZoomOutTime));
			}

			_currentZoomOutTime += Time.deltaTime;

			yield return null;
		}

		DisableAllPrimitives();
		SetPrimitivesStartScale();

		_isExiting = false;
	}

	private IEnumerator WaitExitJob()
	{
		yield return _zoomOutPrimitives;

		SetPrimitivesStartScale();

		DisableAllPrimitives();

		EnableAllPrimitives();

		_effectorForPrimitives.ResetVelocity(_rigidbodies);

		PlaceOnEndZone();

		_effectorForPrimitives.AttractionToCenter(_rigidbodies, _startForce);

		StartAutopilotMode();
	}
}
