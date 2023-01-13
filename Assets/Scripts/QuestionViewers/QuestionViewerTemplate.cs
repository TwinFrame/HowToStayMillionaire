using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestionViewerTemplate : MonoBehaviour
{
	[Header("Service")]
	[SerializeField] protected GameProperties _properties;

	protected TypesOfQuestions.Type Type;

	protected WaitForSeconds WaitBetweenViewers;
	protected WaitForSeconds WaitBetweenElements;

	public TypesOfQuestions.Type TypeReadOnly => Type;

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
