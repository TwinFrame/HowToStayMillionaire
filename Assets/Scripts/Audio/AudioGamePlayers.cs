using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGamePlayers : MonoBehaviour
{
	[Header("Audio Clips")]
	[SerializeField] private AudioClip _startCountdownAudio;
	[SerializeField] private AudioClip _stopCountdownAudio;
	[SerializeField] private AudioClip _rightAnswer;
	[SerializeField] private AudioClip _wrongAnswer;
	[SerializeField] private AudioClip _mainTitleInOut;
	[SerializeField] private AudioClip _singleFadeInOutTitle;
	[SerializeField] private AudioClip _doubleFadeInOutTitle;
	[Header("Looped Audio")]
	[SerializeField] private AudioClip _cashCount;
	[SerializeField] private double _cashCountInLoopNormTime;
	[SerializeField] private double _cashCountOutLoopNormTime;
	[Space]
	[Header("Audio Source")]
	[SerializeField] private AudioSource _masterSource;
	[SerializeField] private AudioSource _fxSource;
	[SerializeField] private AudioSource _countdownSource;
	[SerializeField] private AudioSource _questionSource;
	[SerializeField] private AudioSource _musicSource;
	[SerializeField] private AdvancedAudioPlayer _loopInMiddlePlayer;

	public void PlayStartCountdown()
	{
		_countdownSource.PlayOneShot(_startCountdownAudio);
	}

	public void PlayStopCountdown()
	{
		_countdownSource.PlayOneShot(_stopCountdownAudio);
	}

	public void PlayRightAnswer()
	{
		_fxSource.PlayOneShot(_rightAnswer);
	}

	public void PlayWrongAnswer()
	{
		_fxSource.PlayOneShot(_wrongAnswer);
	}

	public void PlayMainTitle()
	{
		_fxSource.PlayOneShot(_mainTitleInOut);
	}

	public void PlayCashCounterLoopInMiddle()
	{
		_loopInMiddlePlayer.PlayLoopInMiddle(_cashCount, _cashCountInLoopNormTime, _cashCountOutLoopNormTime);
	}

	public void StopCashCounterLoopInMiddle()
	{
		_loopInMiddlePlayer.SetIsLoop(false);
	}

	public void PlaySingleFadeInOutTitle()
	{
		_fxSource.PlayOneShot(_singleFadeInOutTitle);
	}

	public void PlayDoubleFadeInOutTitle()
	{
		_fxSource.PlayOneShot(_doubleFadeInOutTitle);
	}
}
