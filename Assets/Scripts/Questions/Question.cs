using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Question : MonoBehaviour
{
	protected TypesOfQuestions.Type Type;
	protected bool IsAsked;
	protected bool IsRightAnswer;
	protected bool IsAnswerShow;

	public TypesOfQuestions.Type TypeReadOnly => Type;
	public bool IsAskedReadOnly => IsAsked;
	public bool IsRightAnswerReadOnly => IsRightAnswer;
	public bool IsAnswerShowReadOnly => IsAnswerShow;

	public abstract void InitQuestion();

	public void OnRightAnswerAndAsked()
	{
		IsAsked = true;
		IsRightAnswer = true;
	}

	public void OnWrongAnswerAndAsked()
	{
		IsAsked = true;
		IsRightAnswer = false;
	}

	public void OnAnswerShowed()
	{
		IsAnswerShow = true;
	}

	public void ResetQuestion()
	{
		IsAnswerShow = false;
		IsAsked = false;
	}
}
