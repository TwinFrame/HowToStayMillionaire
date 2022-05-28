using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoLoopPlayer : MonoBehaviour
{
	[SerializeField] private AdvancedVideoPlayer[] _players;
	[SerializeField] CanvasGroup[] _canvases;

	private bool _isLoopState;
	private Coroutine _playLoopedVideoJob;

	private int _currentPlayer;

	public void OnPlayLoopedVideo(VideoClip video, double mixNTime)
	{
		_isLoopState = true;

		foreach (var player in _players)
		{
			player.ReleaseRenderTexture();
			player.gameObject.SetActive(true);
		}

		foreach (var canvas in _canvases)
		{
			canvas.alpha = 1;
			canvas.gameObject.SetActive(true);
		}

		if (_playLoopedVideoJob != null)
			StopCoroutine(_playLoopedVideoJob);
		_playLoopedVideoJob = StartCoroutine(OnPlayLoopedVideoJob(video, mixNTime));
	}

	public void OnStopedLoopedVideo()
	{
		_isLoopState = false;
	}

	public double GetCurrentTime(int numPlayer)
	{
			return _players[numPlayer - 1].NormalizeTime;
	}

	private double ClampDouble(double value, double min, double max)
	{
		if (value < min) { value = min; return value; }

		if (value > max) { value = max; return value; }

		return value;
	}

	private IEnumerator OnPlayLoopedVideoJob(VideoClip video, double mixNTime)
	{
		mixNTime = ClampDouble(mixNTime, 0.5d, 1d);

		foreach (var player in _players)
		{
			player.LoadContent(video);
			player.PreparePlayer();
			player.SetIsLoop(false);
		}

		foreach (var player in _players)
		{
			yield return new WaitUntil(() => player.IsPrepared);
		}

		_currentPlayer = 0;

		while (_isLoopState)
		{
			_players[_currentPlayer].Play();

			yield return new WaitUntil(() => _players[_currentPlayer].NormalizeTime >= mixNTime);

			_currentPlayer++;

			if (_currentPlayer >= _players.Length)
				_currentPlayer = 0;
		}

		foreach (var player in _players)
		{
			if (player.IsPlaying)
				yield return new WaitUntil(() => player.IsDone);
		}

		foreach (var player in _players)
		{
			player.ReleaseRenderTexture();
			player.gameObject.SetActive(false);
		}

		foreach (var canvas in _canvases)
		{
			canvas.alpha = 0;
			canvas.gameObject.SetActive(false);
		}
	}
}
