using UnityEngine;
using UnityEngine.Video;

public class CoinFallsLoopedVideo : MonoBehaviour
{
	[SerializeField] private VideoClip _video;
	[Range(0.5f, 1f)] [SerializeField] private double _mixNTime;

	[Header("Service")]
	[SerializeField] VideoLoopPlayer _videoLoopPlayer;

	public void PlayVideo()
	{
		_videoLoopPlayer.OnPlayLoopedVideo(_video, _mixNTime);
	}

	public void StopVideo()
	{
		_videoLoopPlayer.OnStopedLoopedVideo();
	}
}
