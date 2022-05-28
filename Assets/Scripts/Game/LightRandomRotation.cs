using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRandomRotation : MonoBehaviour
{
	[SerializeField] private float _speed;
	[SerializeField] private float _timeToChangeDirection;
	private float _timeTransition;

	private Vector3 _currentRotation;
	private Vector3 _currentRandomRotation;
	private Vector3 _oldRandomRotation;
	private Coroutine _randomRotationJob;
	private float _currentTime;
	private float _currentTimeTransition;
	private float _normalizeCurrentTimeTransition;
	private bool _isTransition;

	public void StartRotating()
	{
		if (_randomRotationJob != null)
			StopCoroutine(_randomRotationJob);

		_randomRotationJob = StartCoroutine(RandomRotationJob());
	}

	public void StopRotating()
	{
		StopCoroutine(_randomRotationJob);
	}

	private IEnumerator RandomRotationJob()
	{
		_timeTransition = _timeToChangeDirection * 0.2f;
		_currentRotation = transform.rotation.eulerAngles;
		_currentTime = 0;

		while (true)
		{
			_currentTime += Time.deltaTime;

			if (_currentTime >= _timeToChangeDirection)
			{
				_currentTime = 0;
				_currentTimeTransition = 0;

				_isTransition = true;

				_oldRandomRotation = _currentRandomRotation;
				_currentRandomRotation = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
			}

			if (_isTransition)
			{
				_normalizeCurrentTimeTransition = _currentTimeTransition / _timeTransition;

				_currentRotation = Vector3.Lerp(_oldRandomRotation, _currentRandomRotation, _normalizeCurrentTimeTransition);

				_currentTimeTransition += Time.deltaTime;
			}
			else
				_currentRotation = _currentRandomRotation;

			if (_currentTimeTransition >= _timeTransition)
				_isTransition = false;

			transform.Rotate(_speed * Time.deltaTime * _currentRotation);

			yield return null;
		}
	}
}
