using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAdvancedPlayer
{
	public void Play();
	public void PlayFull();
	public void PlayUntilPauseMark(double pauseMark);
	public void PlayAfterPauseMark(double pauseMark);
	public void Pause();
	public bool GetIsPause();
	public void Seek(double normalizeTime);
	public void SetIsLoop(bool isLoop);
	public bool GetIsLoop();
	public void Restart();
	public void ClearPlayer();
	public double GetCurrentTime();
	public bool GetIsPlaying();
	public void IncrementPlaybackSpeed();
	public void DecrementPlaybackSpeed();
}
