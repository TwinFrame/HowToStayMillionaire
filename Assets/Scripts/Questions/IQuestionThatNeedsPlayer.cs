using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuestionThatNeedsPlayer
{
	public void SetNormalizedPauseMark(float normalizedPauseMark);

	public float GetNormalizedPauseMark();
}
