using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionAnimator : TitleAnimator
{
	[Space]
	[SerializeField] private QuestionViewer _questionViewer;
	[Space]
	[SerializeField] private MainPlate _mainPlate;
	[Space]
	[SerializeField] private Countdown _countdown;

	public MainPlate MainPlate => _mainPlate;

	private Coroutine _exitPlateJob;

	protected override IEnumerator EnterJob()
	{
		_questionViewer.Activate();

		yield return null;

		_questionViewer.Preparation();

		GameEvent.OnStartQuestion?.Invoke();
		
		yield return WaitBetweenViewers;

		if (_mainPlate.isActiveAndEnabled != true)
			SetActivityOnPlate(true);

		ResetTitle();

		_mainPlate.Enter(out Coroutine enterJob);

		yield return enterJob;

		_questionViewer.Enter();
	}

	protected override IEnumerator ExitJob()
	{
		GameEvent.OnStopQuestion?.Invoke();

		_mainPlate.Exit(out _exitPlateJob);

		yield return _exitPlateJob;

		_questionViewer.Exit();

		_countdown.ResetIndicator();
		//_mainPlate.gameObject.SetActive(false);
		//_mainPlate.enabled = false;
	}

	protected override void ResetTitle()
	{
		_mainPlate.Reset();
	}

	public void SetActivityOnPlate(bool isActive)
	{
		_mainPlate.gameObject.SetActive(isActive);
		_mainPlate.enabled = isActive;
	}

	public void StartCountdown(float timeToQuestion)
	{
		_countdown.StartCountdown(timeToQuestion);
	}

	public bool TryActivationViewer(TypesOfQuestions.Type type, out QuestionViewerTemplate questionType)
	{
		if (_questionViewer.TryActivationViewer(type, out questionType))
			return true;
		else
			return false;
	}

	public void SetAndFillQuestionTemplate(Question question, QuestionViewerTemplate questionViewer)
	{
		_questionViewer.SetAndFillQuestionTemplate(question, questionViewer);
	}

	public QuestionViewerTemplate GetCurrentQuestionViewer()
	{
		return _questionViewer.GetCurrentQuestionViewer();
	}

	public int GetCurrentChoosedOption()
	{
		return _questionViewer.GetCurrentChoosedOption();
	}

	public void ChangeCurrentOption(int newOptionNumber)
	{
		_questionViewer.ChangeCurrentOption(newOptionNumber);
	}

	public void SetChooseOption(int numberOption)
	{
		_questionViewer.SetChooseOption(numberOption);
	}

	public bool TryChoosedOption(int numberOption)
	{
		if (_questionViewer.TryChoosedOption(numberOption))
			return true;
		else
			return false;
	}

	public void ResetViewers()
	{
		_questionViewer.ResetViewers();
	}
}