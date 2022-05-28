using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestionViewerTemplate : MonoBehaviour
{
	[Header("Service")]
	[SerializeField] protected GameProperties _properties;

	protected TypesOfQuestions.Type Type;
	//protected bool IsStartCountdown = false;

	protected WaitForSeconds WaitBetweenViewers;
	protected WaitForSeconds WaitBetweenElements;

	public TypesOfQuestions.Type TypeReadOnly => Type;
	//public bool IsStartCountdownReadOnly => IsStartCountdown;

	//public bool IsHaveVideoReadOnly => IsHaveVideo;

	public abstract void ClearTemplate();

	public abstract void InitTemplate();

	public abstract void PreparationTemplate();

	public abstract void Enter(Question question);

	public abstract void Exit(QuestionViewer questionViewer);

	protected void InitWaitForSeconds()
	{
		WaitBetweenViewers = new WaitForSeconds(_properties.DelayBetweenTitleAnimations);
		WaitBetweenElements = new WaitForSeconds(_properties.DelayBetweenElementAnimations);
	}
}
