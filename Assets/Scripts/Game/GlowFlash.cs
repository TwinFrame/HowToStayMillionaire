using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]

public class GlowFlash : MonoBehaviour
{
	[Header("Default")]
	[SerializeField] private float _startThreshold;
	[SerializeField] private float _startIntensity;
	[Header("Default Glow")]
	[SerializeField] private float _glowDefaultThreshold;
	[SerializeField] private float _glowDefaultIntensity;
	[Header("When right answer")]
	[SerializeField] private float _glowRightThreshold;
	[SerializeField] private float _glowRightIntensity;
	[Header("When wrong answer")]
	[SerializeField] private float _glowWrongThreshold;
	[SerializeField] private float _glowWrongIntensity;
	[Header("Service")]
	[SerializeField] private Volume _volume;
	[SerializeField] private GameProperties _properties;
	
	private float _currentTime;
	private float _normalizeCurrentTime;
	private float _currentGlowThreshold;
	private float _currentGlowIntensity;

	private Bloom _bloom;
	private Coroutine _glowingJob;
	
	public void GlowingDefault()
	{
		if (_glowingJob != null)
			StopCoroutine(_glowingJob);

		_glowingJob = StartCoroutine(GlowingJob(_glowDefaultThreshold, _glowDefaultIntensity, _properties.GlowColor,
			_properties.GlowColor, _properties.FadeInBurstTimeByDefault, _properties.FadeOutBurstTimeByDefault));
	}

	public void GlowingRightAnswer()
	{
		if (_glowingJob != null)
			StopCoroutine(_glowingJob);

		_glowingJob = StartCoroutine(GlowingJob(_glowRightThreshold, _glowRightIntensity, _properties.GlowColor,
			_properties.GameColorChanger.GetRightColor(), _properties.FadeInBurstTime, _properties.FadeOutBurstTime));
	}

	public void GlowingWrongAnswer()
	{
		if (_glowingJob != null)
			StopCoroutine(_glowingJob);

		_glowingJob = StartCoroutine(GlowingJob(_glowWrongThreshold, _glowWrongIntensity, _properties.GlowColor,
			_properties.GameColorChanger.GetWrongColor(), _properties.FadeInBurstTime, _properties.FadeOutBurstTime));
	}
	
	private IEnumerator GlowingJob(float glowThreshold, float glowIntensity, Color initialColor, Color glowColor,
		float timeIn, float timeOut)
	{
		if (_volume.profile.TryGet<Bloom>(out _bloom))
		{
			_currentGlowThreshold = _bloom.threshold.GetValue<float>();
			_currentGlowIntensity = _bloom.intensity.GetValue<float>();
		}
		else
			Debug.Log("Dont Find Bloom in Post Process");

		_currentTime = 0;

		while (_currentTime <= timeIn)
		{
			_normalizeCurrentTime = _currentTime / timeIn;

			_bloom.threshold.value = Mathf.Lerp(_currentGlowThreshold, glowThreshold, _properties.FadeIn.Evaluate(_normalizeCurrentTime));
			_bloom.intensity.value = Mathf.Lerp(_currentGlowIntensity, glowIntensity, _properties.FadeIn.Evaluate(_normalizeCurrentTime));
			_bloom.tint.value = Vector4.Lerp(initialColor, glowColor, _properties.FadeIn.Evaluate(_normalizeCurrentTime));

			_currentTime += Time.deltaTime;

			yield return null;
		}

		_currentTime = 0;

		while (_currentTime <= timeOut)
		{
			_normalizeCurrentTime = _currentTime / timeOut;

			_bloom.threshold.value = Mathf.Lerp(glowThreshold, _startThreshold, _properties.EaseHardInOut.Evaluate(_normalizeCurrentTime));
			_bloom.intensity.value = Mathf.Lerp(glowIntensity, _startIntensity, _properties.EaseHardInOut.Evaluate(_normalizeCurrentTime));
			_bloom.tint.value = Vector4.Lerp(glowColor, initialColor, _properties.EaseHardInOut.Evaluate(_normalizeCurrentTime));

			_currentTime += Time.deltaTime;

			yield return null;
		}

		_bloom.threshold.value = _startThreshold;
		_bloom.intensity.value = _startIntensity;
		_bloom.tint.value = initialColor;
	}
}
