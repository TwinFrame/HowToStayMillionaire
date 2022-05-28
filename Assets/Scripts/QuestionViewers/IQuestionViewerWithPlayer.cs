using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuestionViewerWithPlayer
{
	public IAdvancedPlayer GetPlayer();
	public void Play();
	public void PlayFull();
	public void PlayUntilPauseMark();
	public void PlayAfterPauseMark();
	public void Pause();
	public void SetLoop(bool isOn);
	public bool GetIsOnLoop();
	public double GetCurrentTime();
}
