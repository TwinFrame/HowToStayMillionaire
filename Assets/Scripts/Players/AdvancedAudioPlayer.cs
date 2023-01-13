using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class AdvancedAudioPlayer : MonoBehaviour, IAdvancedPlayer
{
	[SerializeField] private AudioSource _audio;

	private bool _isDone;
	private bool _isPause;

	private double _inaccuracyTime = 0.005f;

	public AudioClip Clip { get { return _audio.clip; } private set { } }
	public bool IsPlaying { get { return _audio.isPlaying; } private set { } }
	public bool IsLooping { get { return _audio.loop; } private set { } }
	public bool IsPrepared { get { return _audio.clip.preloadAudioData; } private set { } }
	public bool IsDone { get { return _isDone; } private set { } }
	public float Time { get { return _audio.time; } private set { } }
	public float Duration { get { return (_audio.clip.length); } private set { } }
	public double NormalizeTime { get { return (double)(Time / Duration); } private set { } }

	private Coroutine _playJob;
	private Coroutine _playLoopInMiddleJob;

	public void LoadContent(AudioClip audio)
	{
		_audio.clip = audio;
	}

	public void PreparePlayer()
	{
		_audio.Stop();
		Seek(0);
	}

	public void Play()
	{
		if (!IsPrepared)
			_audio.clip.LoadAudioData();

		if (_isPause)
			_isPause = false;

		_audio.Play();
	}

	public void PlayFull()
	{
		if (_isPause)
		{
			_isPause = false;

			_audio.Play();
		}
		else
		{
			if (_playJob != null)
				StopCoroutine(_playJob);
			_playJob = StartCoroutine(PlayJob(0, 1));
		}
	}

	public void PlayUntilPauseMark(double pauseMark)
	{
		if (_playJob != null)
			StopCoroutine(_playJob);
		_playJob = StartCoroutine(PlayJob(0, pauseMark));
	}

	public void PlayAfterPauseMark(double pauseMark)
	{
		if (pauseMark >= 1 - _inaccuracyTime)
		{
			Seek(0);
			Play();
			return;
		}

		if (!IsPlaying && pauseMark > NormalizeTime - _inaccuracyTime && pauseMark < NormalizeTime + _inaccuracyTime)
			pauseMark = NormalizeTime;


		if (_playJob != null)
			StopCoroutine(_playJob);
		_playJob = StartCoroutine(PlayJob(pauseMark, 1));
	}

	public void PlayLoopInMiddle(AudioClip audio, double loopStartNormalizeTime, double loopStopNormalizeTime)
	{
		LoadContent(audio);

		PreparePlayer();

		SetIsLoop(true);

		if (_playLoopInMiddleJob != null)
			StopCoroutine(_playLoopInMiddleJob);
		_playLoopInMiddleJob = StartCoroutine(PlayLoopInMiddleJob(loopStartNormalizeTime, loopStopNormalizeTime));
	}

	public void Pause()
	{
		if (IsPlaying)
		{
			_isPause = true;
			_audio.Pause();
		}
		else
		{
			_isPause = false;
			_audio.Play();
		}
	}

	public void Seek(double normalizeTime)
	{
		if (!IsPrepared)
			return;

		normalizeTime = ClampDouble(normalizeTime, 0, 1);
		_audio.time = (float)(normalizeTime * Duration);
	}

	public void SetIsLoop(bool isLoop)
	{
		if (!IsPrepared)
			return;

		_audio.loop = isLoop;
	}

	public bool GetIsLoop()
	{
		return _audio.loop;
	}

	public void Restart()
	{
		if (!IsPrepared)
			return;

		_audio.Pause();
		Seek(0);
	}


	public void ClearPlayer()
	{
		SetIsLoop(false);
		_audio.clip.UnloadAudioData();
		_audio.clip = null;
		_isDone = false;
		_isPause = false;
	}

	public void IncrementPlaybackSpeed()
	{
		_audio.pitch += 0.25f;
		_audio.pitch = Mathf.Clamp(_audio.pitch, 0.5f, 2f);
	}

	public void DecrementPlaybackSpeed()
	{
		_audio.pitch -= 0.25f;
		_audio.pitch = Mathf.Clamp(_audio.pitch, 0.5f, 2f);
	}

	public double GetCurrentTime()
	{
		return NormalizeTime;
	}

	public bool GetIsPlaying()
	{
		return _audio.isPlaying;
	}

	public bool GetIsPause()
	{
		return _isPause;
	}

	private IEnumerator PlayJob(double startNTime, double endNTime)
	{
		startNTime = ClampDouble(startNTime, 0, 1);
		endNTime = ClampDouble(endNTime, 0, 1);

		if (!IsPrepared)
		{
			_audio.clip.LoadAudioData();
			PreparePlayer();
		}

		yield return new WaitUntil(() => IsPrepared);

		Seek(startNTime);

		_audio.Play();

		yield return new WaitUntil(() => NormalizeTime + _inaccuracyTime >= endNTime);

		if (IsLooping)
		{
			while (IsLooping)
			{
				Seek(startNTime);

				_audio.Play();

				yield return new WaitUntil(() => NormalizeTime + _inaccuracyTime >= endNTime);
			}
		}

		_audio.Pause();
	}

	private IEnumerator PlayLoopInMiddleJob(double loopStartNormalizeTime, double loopStopNormalizeTime)
	{
		loopStartNormalizeTime = ClampDouble(loopStartNormalizeTime, 0, 1);
		loopStopNormalizeTime = ClampDouble(loopStopNormalizeTime, 0, 1);

		if (loopStartNormalizeTime >= loopStopNormalizeTime)
			loopStartNormalizeTime = 0;

		if (!IsPrepared)
		{
			_audio.clip.LoadAudioData();
			PreparePlayer();
		}

		yield return new WaitUntil(() => IsPrepared);

		Seek(0);

		_audio.Play();

		yield return new WaitUntil(() => NormalizeTime - _inaccuracyTime >= loopStopNormalizeTime);

		if (IsLooping)
		{
			while (IsLooping)
			{
				Seek(loopStartNormalizeTime);

				_audio.Play();

				yield return new WaitUntil(() => NormalizeTime - _inaccuracyTime >= loopStopNormalizeTime);
			}
		}
	}

	private double ClampDouble(double value, double min, double max)
	{
		if (value < min) { value = min; return value; }

		if (value > max) { value = max; return value; }

		return value;
	}
}
