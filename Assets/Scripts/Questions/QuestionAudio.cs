using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionAudio : Question, IQuestionThatNeedsPlayer
{
	[TextArea]
	[SerializeField] private string _question;
	[SerializeField] private AudioClip _audio;
	[Range(0, 1)] [SerializeField] private float _normalizedPauseTime;

	public string Question => _question;
	public AudioClip Audio => _audio;
	public float NormalizedPauseTime => _normalizedPauseTime;

	public override void InitQuestion()
	{
		IsAnswerShow = false;
		IsAsked = false;
		Type = TypesOfQuestions.Type.audio;
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
