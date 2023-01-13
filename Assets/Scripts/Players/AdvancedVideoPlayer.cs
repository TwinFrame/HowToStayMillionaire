using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(AudioSource))]

public class AdvancedVideoPlayer : MonoBehaviour, IAdvancedPlayer
{
	[SerializeField] private VideoPlayer _videoPlayer;
	[SerializeField] private AudioSource _audioSource;

	private bool _isDone;
	private bool _isPause;

	private double _inaccuracyTime = 0.005f;

	public bool IsPlaying { get { return _videoPlayer.isPlaying; } private set { } }
	public bool IsLooping { get { return _videoPlayer.isLooping; } private set { } }
	public bool IsPrepared { get { return _videoPlayer.isPrepared; } private set { } }
	public bool IsDone { get { return _isDone; } private set { } }
	public double Time { get { return _videoPlayer.time; } private set { } }
	public ulong Duration { get { return (ulong)(_videoPlayer.frameCount / _videoPlayer.frameRate); } private set { } }
	public double NormalizeTime { get { return Time / (double)Duration; } private set { } }

	private Coroutine _playJob;

	private void OnEnable()
	{
		_videoPlayer.errorReceived += ErrorRecieved;
		_videoPlayer.frameReady += FrameReady; //cpu tax is heavy
		_videoPlayer.loopPointReached += LoopPointReached;
		_videoPlayer.prepareCompleted += PrepareCompleted;
		_videoPlayer.seekCompleted += SeekCompleted;
		_videoPlayer.started += Started;
	}

	private void OnDisable()
	{
		_videoPlayer.errorReceived -= ErrorRecieved;
		_videoPlayer.frameReady -= FrameReady;
		_videoPlayer.loopPointReached -= LoopPointReached;
		_videoPlayer.prepareCompleted -= PrepareCompleted;
		_videoPlayer.seekCompleted -= SeekCompleted;
		_videoPlayer.started -= Started;
	}
	
	public void LoadContent(VideoClip video)
	{
		_videoPlayer.clip = video;
	}

	public void PreparePlayer()
	{
		_videoPlayer.Prepare();
		Seek(0);

		//Debug.Log($"Can set direct audio volume: {_video.canSetDirectAudioVolume}");
		//Debug.Log($"Can set playback speed: {_video.canSetPlaybackSpeed}");
		//Debug.Log($"Can set skip on drop: {_video.canSetSkipOnDrop}");
		//Debug.Log($"Can set time: {_video.canSetTime}");
		//Debug.Log($"Can step: {_video.canStep}");
	}

	public void SetupAudio()
	{
		if (_videoPlayer.audioTrackCount <= 0)
			return;

		if (_audioSource == null && _videoPlayer.canSetDirectAudioVolume)
		{
			_videoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;
		}
		else
		{
			_videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
			_videoPlayer.EnableAudioTrack(0, true);
			_videoPlayer.SetTargetAudioSource(0, _audioSource);
		}

	}

	public void Play()
	{
		if (_isPause)
			_isPause = false;

		_videoPlayer.Play();
	}

	public void PlayFull()
	{
		if (_isPause)
		{
			_isPause = false;

			_videoPlayer.Play();
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
			_videoPlayer.Play();
			return;
		}

		if (!IsPlaying && pauseMark > NormalizeTime - _inaccuracyTime && pauseMark < NormalizeTime + _inaccuracyTime)
			pauseMark = NormalizeTime;


		if (_playJob != null)
			StopCoroutine(_playJob);
		_playJob = StartCoroutine(PlayJob(pauseMark, 1));
	}

	public void Pause()
	{
		if (IsPlaying)
		{
			_isPause = true;
			_videoPlayer.Pause();
		}
		else
		{
			_isPause = false;
			_videoPlayer.Play();
		}
	}

	public void Seek(double normalizeTime)
	{
		if (!_videoPlayer.canSetTime)
			return;

		if (!IsPrepared)
			return;

		normalizeTime = ClampDouble(normalizeTime, 0, 1);
		_videoPlayer.time = normalizeTime * Duration;
	}

	public void SetIsLoop(bool isLoop)
	{
		if (!IsPrepared)
			return;

		_videoPlayer.isLooping = isLoop;
	}

	public bool GetIsLoop()
	{
		return _videoPlayer.isLooping;
	}

	public void Restart()
	{
		if (!IsPrepared)
			return;

		_videoPlayer.Pause();
		Seek(0);
	}

	public void ClearPlayer()
	{
		_videoPlayer.clip = null;
		_isDone = false;
		_isPause = false;

		SetIsLoop(false);
	}

	public void IncrementPlaybackSpeed()
	{
		if (!_videoPlayer.canSetPlaybackSpeed)
			return;

		_videoPlayer.playbackSpeed += 0.5f;
		_videoPlayer.playbackSpeed = Mathf.Clamp(_videoPlayer.playbackSpeed, 0, 3);
	}

	public void DecrementPlaybackSpeed()
	{
		if (!_videoPlayer.canSetPlaybackSpeed)
			return;

		_videoPlayer.playbackSpeed -= 0.5f;
		_videoPlayer.playbackSpeed = Mathf.Clamp(_videoPlayer.playbackSpeed, 0, 3);
	}

	public double GetCurrentTime()
	{
		return NormalizeTime;
	}

	public bool GetIsPlaying()
	{
		return _videoPlayer.isPlaying;
	}

	public bool GetIsPause()
	{
		return _isPause;
	}

	private void ErrorRecieved(VideoPlayer source, string message)
	{
		Debug.Log($"{source.name}. Log: {message} ");
	}

	private void Started(VideoPlayer source)
	{
	}

	private void SeekCompleted(VideoPlayer source)
	{
		_isDone = false;
	}

	private void PrepareCompleted(VideoPlayer source)
	{
		_isDone = false;
	}

	private void LoopPointReached(VideoPlayer source)
	{
		_isDone = true;
	}

	private void FrameReady(VideoPlayer source, long frameIdx)
	{
		//cpu tax is heavy
	}

	public void ReleaseRenderTexture()
	{
		_videoPlayer.targetTexture.Release();
	}

	private IEnumerator PlayJob(double startNTime, double endNTime)
	{
		startNTime = ClampDouble(startNTime, 0, 1);
		endNTime = ClampDouble(endNTime, 0, 1);

		if (!IsPrepared)
			PreparePlayer();

		yield return new WaitUntil(() => IsPrepared);

		Seek(startNTime);

		_videoPlayer.Play();

		yield return new WaitUntil(() => NormalizeTime >= endNTime);

		if (IsLooping)
		{
			while (IsLooping)
			{
				Seek(startNTime);

				_videoPlayer.Play();

				yield return new WaitUntil(() => NormalizeTime >= endNTime);
			}
		}

		_videoPlayer.Pause();
	}

	private double ClampDouble(double value, double min, double max)
	{
		if (value < min) { value = min; return value; }

		if (value > max) { value = max; return value; }

		return value;
	}
}
