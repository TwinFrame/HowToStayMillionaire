using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
	[Header("Main Title")]
	public UnityEvent OnMainTitleEnter;
	public UnityEvent OnMainTitleExit;
	[Header("Teams Title")]
	public UnityEvent OnTeamsTitleEnter;
	public UnityEvent OnTeamsTitleExit;
	[Header("Tour Title")]
	public UnityEvent OnTourTitleEnter;
	public UnityEvent OnTourTitleExit;
	[Header("Preparation Title")]
	public UnityEvent OnPreparationTitleEnter;
	public UnityEvent OnPreparationTitleExit;
	[Header("Final Title")]
	public UnityEvent OnFinalTitleEnter;
	public UnityEvent OnFinalTitleExit;
	[Header("Question Title")]
	[Header("Start Question")]
	public UnityEvent OnStartQuestion;
	public UnityEvent<int> OnStartQuestionWithOptions;
	public UnityEvent<IAdvancedPlayer, float> OnStartQuestionWithPlayer;
	[Header("Stop Question")]
	public UnityEvent OnStopQuestion;
	public UnityEvent OnStopQuestionWithOptions;
	public UnityEvent OnStopQuestionWithPlayer;
	[Header("Answer")]
	public UnityEvent OnRightAnswer;
	public UnityEvent OnWrongAnswer;
	[Space]
	public UnityEvent OnFlash;
	[Header("Change money to Team")]
	public UnityEvent OnStartChangeMoneyOfTeam;
	public UnityEvent OnStopChangeMoneyOfTeam;
	[Header("Countdown")]
	public UnityEvent OnStartCountdown;
	public UnityEvent OnStopCountdown;
	[Header("Service")]
	[SerializeField] private QuestionViewer _questionViewer;

	private void OnEnable()
	{
		_questionViewer.StartQuestion += StartQuestion;
		_questionViewer.StopQuestion += StopQuestion;
		_questionViewer.StartQuestionWithOptions += (count) => StartQuestionWithOptions(count);
		_questionViewer.StopQuestionWithOptions += StopQuestionWithOptions;
		_questionViewer.StartQuestionWithPlayer += (player, pauseMark) => StartQuestionWithPlayer(player, pauseMark);
		_questionViewer.StopQuestionWithPlayer += StopQuestionWithPlayer;
	}

	private void OnDisable()
	{
		_questionViewer.StartQuestion -= StartQuestion;
		_questionViewer.StopQuestion -= StopQuestion;
		_questionViewer.StartQuestionWithOptions -= (count) => StartQuestionWithOptions(count);
		_questionViewer.StopQuestionWithOptions -= StopQuestionWithOptions;
		_questionViewer.StartQuestionWithPlayer -= (player, pauseMark) => StartQuestionWithPlayer(player, pauseMark);
		_questionViewer.StopQuestionWithPlayer -= StopQuestionWithPlayer;
	}

	/* —транные два метода DepositeMoney, возможно переписать
	public UnityEvent StartDepositMoney()
	{
		return OnStartDepositMoney;
	}

	public UnityEvent FinishedDepositMoney()
	{
		return OnFinishedDepositMoney;
	}
	*/

	public void StartQuestion()
	{
		OnStartQuestion?.Invoke();
	}

	public void StopQuestion()
	{
		OnStopQuestion?.Invoke();
	}

	public void StartQuestionWithOptions(int optionsCount)
	{
		OnStartQuestionWithOptions?.Invoke(optionsCount);
	}

	public void StopQuestionWithOptions()
	{
		OnStopQuestionWithOptions?.Invoke();
	}

	public void StartQuestionWithPlayer(IAdvancedPlayer player, float currentPauseMark)
	{
		OnStartQuestionWithPlayer?.Invoke(player, currentPauseMark);
	}

	public void StopQuestionWithPlayer()
	{
		OnStopQuestionWithPlayer?.Invoke();
	}

	public void StartCountdown()
	{
		OnStartCountdown?.Invoke();
	}

	public void StopCountdown()
	{
		OnStopCountdown?.Invoke();
	}
	/*
	public void SetNormalizedPauseMark(float normalizedPauseMark)
	{
		OnSetNormalizedPauseMark?.Invoke(normalizedPauseMark);
	}
	*/
}
