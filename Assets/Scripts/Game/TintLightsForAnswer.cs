using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TintLightsForAnswer : MonoBehaviour
{
	[SerializeField] private Light _sun;
	[SerializeField] private Light _subSun;
	[Header("Service")]
	[SerializeField] private GameProperties _properties;

	private readonly List<Light> _lights = new List<Light>();
	private float _currentTime;
	private float _normalizeCurrentTime;
	private Color _defaultColor = Color.white;
	private Color _currentColor;
	private Color _targetColor;
	private Coroutine _changingColorJob;
	private WaitForSeconds _waitTintTime;

	private void Awake()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).TryGetComponent<Light>(out Light light))
				_lights.Add(light);
		}

		_lights.Add(_sun);
		_lights.Add(_subSun);

		SetColor(_defaultColor);

		_waitTintTime = new WaitForSeconds(_properties.IlluminationTime);
	}

	public void OnTintingLights(bool isRight)
	{
		if (isRight)
			_targetColor = _properties.GameColorChanger.GetRightColor();
		else
			_targetColor = _properties.GameColorChanger.GetWrongColor();

		if (_changingColorJob != null)
			StopCoroutine(_changingColorJob);

		_changingColorJob = StartCoroutine(ChangingColorJob(_targetColor));
	}

	private IEnumerator ChangingColorJob(Color color)
	{
		_currentColor = _defaultColor;
		_currentTime = 0;

		while (_currentTime <= _properties.TimeOfLightChange)
		{
			_currentTime += Time.deltaTime;

			_normalizeCurrentTime = _currentTime / _properties.TimeOfLightChange;

			_currentColor = Vector4.Lerp(_defaultColor, color, _properties.FadeIn.Evaluate(_normalizeCurrentTime));

			SetColor(_currentColor);

			yield return null;
		}

		yield return _waitTintTime;

		_currentTime = 0;

		while (_currentTime <= _properties.TimeOfLightChange)
		{
			_currentTime += Time.deltaTime;

			_normalizeCurrentTime = _currentTime / _properties.TimeOfLightChange;

			_currentColor = Vector4.Lerp(color, _defaultColor, _properties.FadeOut.Evaluate(_normalizeCurrentTime));

			SetColor(_currentColor);

			yield return null;
		}
	}

	private void SetColor(Color color)
	{
		foreach (var light in _lights)
		{
			light.color = color;
		}
	}
}