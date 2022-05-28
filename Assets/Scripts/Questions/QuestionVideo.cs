using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class QuestionVideo : Question, IQuestionThatNeedsPlayer
{
	[TextArea]
	[SerializeField] private string _question;
	[SerializeField] private VideoClip _video;
	[Range(0, 1)] [SerializeField] private float _normalizedPauseTime;

	public string Question => _question;
	public VideoClip Video => _video;
	public float NormalizedPauseTime => _normalizedPauseTime;

	public override void InitQuestion()
	{
		IsAnswerShow = false;
		IsAsked = false;
		Type = TypesOfQuestions.Type.video;
	}

	public void SetNormalizedPauseMark(float normalizedPauseTime)
	{
		_normalizedPauseTime = normalizedPauseTime;
	}

	public float GetNormalizedPauseMark()
	{
		return _normalizedPauseTime;
	}
}
