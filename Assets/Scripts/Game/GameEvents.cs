using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
	[Header("Main Title")]
	public UnityEvent MainTitleEnteredEvent;
	public UnityEvent MainTitleExitedEvent;
	[Header("Teams Title")]
	public UnityEvent TeamsTitleEnteredEvent;
	public UnityEvent TeamsTitleExitedEvent;
	[Header("Tour Title")]
	public UnityEvent TourTitleEnteredEvent;
	public UnityEvent TourTitleExitedEvent;
	[Header("Preparation Title")]
	public UnityEvent PreparationTitleEnteredEvent;
	public UnityEvent PreparationTitleExitedEvent;
	[Header("Final Title")]
	public UnityEvent FinalTitleEnteredEvent;
	public UnityEvent FinalTitleExitedEvent;
	[Header("Question Title")]
	[Header("Start Question")]
	public UnityEvent StartedQuestionEvent;
	public UnityEvent<int> StartedQuestionWithOptionsEvent;
	public UnityEvent<IAdvancedPlayer, float> StartedQuestionWithPlayerEvent;
	[Header("Stop Question")]
	public UnityEvent StopedQuestionEvent;
	public UnityEvent StopedQuestionWithOptionsEvent;
	public UnityEvent StopedQuestionWithPlayerEvent;
	[Header("Answer")]
	public UnityEvent RightAnsweredEvent;
	public UnityEvent WrongAnsweredEvent;
	[Space]
	public UnityEvent FlashedEvent;
	[Header("Change money to Team")]
	public UnityEvent StartedChangeMoneyOfTeamEvent;
	public UnityEvent StopedChangeMoneyOfTeamEvent;
	[Header("Countdown")]
	public UnityEvent StartedCountdownEvent;
	public UnityEvent StopedCountdownEvent;
	[Header("Service")]
	[SerializeField] private QuestionViewer _questionViewer;

	private void OnEnable()
	{
		_questionViewer.StartedQuestionEvent += StartQuestion;
		_questionViewer.StoppedQuestionEvent += StopQuestion;
		_questionViewer.StartedQuestionWithOptionsEvent += (count) => StartQuestionWithOptions(count);
		_questionViewer.StoppedQuestionWithOptionsEvent += StopQuestionWithOptions;
		_questionViewer.StartedQuestionWithPlayerEvent += (player, pauseMark) => StartQuestionWithPlayer(player, pauseMark);
		_questionViewer.StoppedQuestionWithPlayerEvent += StopQuestionWithPlayer;
	}

	private void OnDisable()
	{
		_questionViewer.StartedQuestionEvent -= StartQuestion;
		_questionViewer.StoppedQuestionEvent -= StopQuestion;
		_questionViewer.StartedQuestionWithOptionsEvent -= (count) => StartQuestionWithOptions(count);
		_questionViewer.StoppedQuestionWithOptionsEvent -= StopQuestionWithOptions;
		_questionViewer.StartedQuestionWithPlayerEvent -= (player, pauseMark) => StartQuestionWithPlayer(player, pauseMark);
		_questionViewer.StoppedQuestionWithPlayerEvent -= StopQuestionWithPlayer;
	}

	public void StartQuestion()
	{
		StartedQuestionEvent?.Invoke();
	}

	public void StopQuestion()
	{
		StopedQuestionEvent?.Invoke();
	}

	public void StartQuestionWithOptions(int optionsCount)
	{
		StartedQuestionWithOptionsEvent?.Invoke(optionsCount);
	}

	public void StopQuestionWithOptions()
	{
		StopedQuestionWithOptionsEvent?.Invoke();
	}

	public void StartQuestionWithPlayer(IAdvancedPlayer player, float currentPauseMark)
	{
		StartedQuestionWithPlayerEvent?.Invoke(player, currentPauseMark);
	}

	public void StopQuestionWithPlayer()
	{
		StopedQuestionWithPlayerEvent?.Invoke();
	}

	public void StartCountdown()
	{
		StartedCountdownEvent?.Invoke();
	}

	public void StopCountdown()
	{
		StopedCountdownEvent?.Invoke();
	}
}
