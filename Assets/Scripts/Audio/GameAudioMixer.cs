using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameAudioMixer : MonoBehaviour
{

	[Header("Audio Mixer")]
	[SerializeField] private AudioMixerGroup _masterMixer;
	[SerializeField] private AudioMixerGroup _fxMixer;
	[SerializeField] private AudioMixerGroup _countdownMixer;
	[SerializeField] private AudioMixerGroup _questionMixer;
	[SerializeField] private AudioMixerGroup _musicMixer;
	[Header("The law of volume change")]
	[SerializeField] private AnimationCurve _curve;

	public float NormalizeMasterVolume { get { _masterMixer.audioMixer.GetFloat("MasterVolume", out float volume);
			return GetNormalizeValue(volume); } }
	public float NormalizeFxVolume { get { _fxMixer.audioMixer.GetFloat("FxVolume", out float volume);
			return GetNormalizeValue(volume); } }
	public float NormalizeCountdownVolume { get { _countdownMixer.audioMixer.GetFloat("CountdownVolume", out float volume);
			return GetNormalizeValue(volume); } }
	public float NormalizeQuestionVolume
	{
		get
		{
			_questionMixer.audioMixer.GetFloat("QuestionVolume", out float volume);
			return GetNormalizeValue(volume);
		}
	}

	public float NormalizeMusicVolume { get { _musicMixer.audioMixer.GetFloat("MusicVolume", out float volume);
			return GetNormalizeValue(volume); } }

	private float _maxValue = 20;
	private float _minValue = -80;
	private float _maxNormalizeValue = 1;
	private float _minNormalizeValue = 0;
	private float _currentValueIndB;
	private float _currentNormalizeValue;

	public UnityAction<TypesOfAudioChannel, float> OnChangedChannelEvent;

	public float GetNormalizeValue(float valueIndB)
	{
		valueIndB = Mathf.Clamp(valueIndB, _minValue, _maxValue);

		_currentNormalizeValue = _minNormalizeValue + ((valueIndB - _minValue) * (_maxNormalizeValue - _minNormalizeValue)) / (_maxValue - _minValue);

		return _currentNormalizeValue;
	}

	public void SetChannel(TypesOfAudioChannel channel, float normalizeValue)
	{
		switch (channel)
		{
			case TypesOfAudioChannel.Master:
				SetMasterChannel(normalizeValue);
				break;
			case TypesOfAudioChannel.Fx:
				SetFxChannel(normalizeValue);
				break;
			case TypesOfAudioChannel.Countown:
				SetCountdownChannel(normalizeValue);
				break;
			case TypesOfAudioChannel.Question:
				SetQuestionChannel(normalizeValue);
				break;
			case TypesOfAudioChannel.Music:
				SetMusicChannel(normalizeValue);
				break;
			default:
				break;
		}
	}

	private void SetMasterChannel(float normalizeValue)
	{
		_masterMixer.audioMixer.SetFloat("MasterVolume", GetValueIndB(normalizeValue));

		OnChangedChannelEvent?.Invoke(TypesOfAudioChannel.Master, normalizeValue);
	}

	private void SetFxChannel(float normalizeValue)
	{
		_fxMixer.audioMixer.SetFloat("FxVolume", GetValueIndB(normalizeValue));

		OnChangedChannelEvent?.Invoke(TypesOfAudioChannel.Fx, normalizeValue);

	}

	private void SetCountdownChannel(float normalizeValue)
	{
		_countdownMixer.audioMixer.SetFloat("CountdownVolume", GetValueIndB(normalizeValue));

		OnChangedChannelEvent?.Invoke(TypesOfAudioChannel.Countown, normalizeValue);
	}

	private void SetQuestionChannel(float normalizeValue)
	{
		_questionMixer.audioMixer.SetFloat("QuestionVolume", GetValueIndB(normalizeValue));

		OnChangedChannelEvent?.Invoke(TypesOfAudioChannel.Question, normalizeValue);
	}

	private void SetMusicChannel(float normalizeValue)
	{
		_musicMixer.audioMixer.SetFloat("MusicVolume", GetValueIndB(normalizeValue));

		OnChangedChannelEvent?.Invoke(TypesOfAudioChannel.Music, normalizeValue);
	}

	private float GetValueIndB(float normalizeValue)
	{
		normalizeValue = Mathf.Clamp01(normalizeValue);

		_currentValueIndB = Mathf.Lerp(_minValue, _maxValue, normalizeValue);

		return _currentValueIndB;
	}
}
