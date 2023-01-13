using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]

public class QuestionViewer : MonoBehaviour
{
	[SerializeField] private List<QuestionViewerTemplate> _questionTemplates;

	private CanvasGroup _canvasGroup;
	private QuestionViewerTemplate _currentQuestionViewer;
	private Question _currentQuestion;

	public UnityAction StartQuestion;
	public UnityAction StopQuestion;
	public UnityAction<int> StartQuestionWithOptions;
	public UnityAction StopQuestionWithOptions;
	public UnityAction<IAdvancedPlayer, float> StartQuestionWithPlayer;
	public UnityAction StopQuestionWithPlayer;

	private void Awake()
	{
		_canvasGroup = GetComponent<CanvasGroup>();

		InitTemplates();
	}

	public void Enter()
	{
		if (_currentQuestionViewer is IQuestionViewerWithPlayer && _currentQuestion is IQuestionThatNeedsPlayer)
		{
			var currentQuestionViewer = _currentQuestionViewer as IQuestionViewerWithPlayer;
			var currentQuestion = _currentQuestion as IQuestionThatNeedsPlayer;

			var currentPlayer = currentQuestionViewer.GetPlayer();
			var currentPauseMark = currentQuestion.GetNormalizedPauseMark();

			StartQuestionWithPlayer?.Invoke(currentPlayer, currentPauseMark);
		}

		if (_currentQuestionViewer is QuestionViewerTemplateWithOptions)
		{
			var currentQuestionViewer = _currentQuestionViewer as QuestionViewerTemplateWithOptions;
			StartQuestionWithOptions?.Invoke(currentQuestionViewer.GetOptionsCount());
		}

		_currentQuestionViewer.Enter(_currentQuestion);
	}

	public void Exit()
	{
		if (_currentQuestionViewer == null)
			return;

		_currentQuestionViewer.Exit(this);

		if (_currentQuestionViewer is IQuestionViewerWithPlayer)
			StopQuestionWithPlayer?.Invoke();

		if (_currentQuestionViewer is QuestionViewerTemplateWithOptions)
			StopQuestionWithOptions?.Invoke();
	}

	public void ResetViewers()
	{
		_canvasGroup.alpha = 0;

		foreach (var viewer in _questionTemplates)
		{
			viewer.gameObject.SetActive(false);
		}
	}
	
	public void SetAndFillQuestionTemplate(Question question, QuestionViewerTemplate questionViewer)
	{
		_currentQuestionViewer = questionViewer;

		_currentQuestion = question;

		FillTemplate(_currentQuestion);
	}

	public void Activate()
	{
		_canvasGroup.alpha = 1;

		_currentQuestionViewer.gameObject.SetActive(true);

		_currentQuestionViewer.enabled = true;
	}

	public void Preparation()
	{
		_currentQuestionViewer.PreparationTemplate();
	}

	public bool TryChoosedOption(int numberOption)
	{
		if (_currentQuestionViewer is QuestionViewerTemplateWithOptions)
		{
			var currentQuestionViewer = _currentQuestionViewer as QuestionViewerTemplateWithOptions;
			if (currentQuestionViewer.TryChoosedOption(numberOption))
				return true;
		}

		return false;
	}

	public int GetCurrentChoosedOption()
	{
		if (_currentQuestionViewer is QuestionViewerTemplateWithOptions)
		{
			var currentQuestionViewer = _currentQuestionViewer as QuestionViewerTemplateWithOptions;
			return currentQuestionViewer.GetCurrentChoosedOption();
		}

		return 0;
	}

	public void SetChooseOption(int numberOption)
	{
		var currentQuestionViewer = _currentQuestionViewer as QuestionViewerTemplateWithOptions;
		currentQuestionViewer.SetChooseOption(numberOption);
	}

	public QuestionViewerTemplate GetCurrentQuestionViewer()
	{
		return _currentQuestionViewer;
	}

	public void ChangeCurrentOption(int newOptionNumber)
	{
		var currentQuestionViewer = _currentQuestionViewer as QuestionViewerTemplateWithOptions;
		currentQuestionViewer.ChangeCurrentOption(newOptionNumber);
	}

	public bool PlayUntilPauseMark()
	{
		if (_currentQuestionViewer is IQuestionViewerWithPlayer)
		{
			var currentQuestionViewer = _currentQuestionViewer as IQuestionViewerWithPlayer;
			currentQuestionViewer.PlayUntilPauseMark();
			return true;
		}
		else
			return false;
	}

	public bool PlayAfterPauseMark()
	{
		if (_currentQuestionViewer is IQuestionViewerWithPlayer)
		{
			var currentQuestionViewer = _currentQuestionViewer as IQuestionViewerWithPlayer;
			currentQuestionViewer.PlayAfterPauseMark();
			return true;
		}
		else
			return false;
	}

	public bool PlayFull()
	{
		if (_currentQuestionViewer is IQuestionViewerWithPlayer)
		{
			var currentQuestionViewer = _currentQuestionViewer as IQuestionViewerWithPlayer;
			currentQuestionViewer.PlayFull();
			return true;
		}
		else
			return false;
	}

	public bool Pause()
	{
		if (_currentQuestionViewer is IQuestionViewerWithPlayer)
		{
			var currentQuestionViewer = _currentQuestionViewer as IQuestionViewerWithPlayer;
			currentQuestionViewer.Pause();
			return true;
		}
		else
			return false;
	}

	public bool Loop(bool isOn)
	{
		if (_currentQuestionViewer is IQuestionViewerWithPlayer)
		{
			var currentQuestionViewer = _currentQuestionViewer as IQuestionViewerWithPlayer;
			currentQuestionViewer.SetLoop(isOn);
			return true;
		}
		else
			return false;
	}

	public bool TryGetIsOnLoop(out bool isOn)
	{
		if (_currentQuestionViewer is IQuestionViewerWithPlayer)
		{
			var currentQuestionViewer = _currentQuestionViewer as IQuestionViewerWithPlayer;

			isOn = currentQuestionViewer.GetIsOnLoop();
			return true;
		}

		isOn = false;
		return false;
	}
	
	public bool TryActivationViewer(TypesOfQuestions.Type type, out QuestionViewerTemplate questionType)
	{
		foreach (var item in _questionTemplates)
		{
			if (item.TypeReadOnly == type)
			{
				questionType = item;
				return true;
			}
		}

		questionType = null;
		return false;
	}
	
	private void InitTemplates()
	{
		foreach (var template in _questionTemplates)
		{
			template.InitTemplate();
		}
	}

	public void CloseViewer()
	{
		_canvasGroup.alpha = 0;

		if (_currentQuestionViewer == null)
			return;

		_currentQuestionViewer.ClearTemplate();
		_currentQuestionViewer.gameObject.SetActive(false);
	}

	private void FillTemplate(Question question)
	{
		switch (question.TypeReadOnly)
		{
			case TypesOfQuestions.Type.text:
				var currentQuestionText = question as QuestionText;
				var currentQuestionTemplateText = _currentQuestionViewer as QuestionViewerText;

				currentQuestionTemplateText.FillTemplate(currentQuestionText.Question, currentQuestionText.Answer);

				_currentQuestionViewer = currentQuestionTemplateText;
				break;

			case TypesOfQuestions.Type.image:
				var currentQuestionImage = question as QuestionImage;
				var currentQuestionTemplateImage = _currentQuestionViewer as QuestionViewerImage;

				currentQuestionTemplateImage.FillTemplate(currentQuestionImage.Question,
					currentQuestionImage.Answer, currentQuestionImage.Image);

				_currentQuestionViewer = currentQuestionTemplateImage;
				break;

			case TypesOfQuestions.Type.audio:
				var currentQuestionAudio = question as QuestionAudio;
				var currentQuestionTemplateAudio = _currentQuestionViewer as QuestionViewerAudio;

				currentQuestionTemplateAudio.FillTemplate(currentQuestionAudio.Question,
					currentQuestionAudio.Audio, currentQuestionAudio.NormalizedPauseTime);

				_currentQuestionViewer = currentQuestionTemplateAudio;
				break;

			case TypesOfQuestions.Type.video:
				var currentQuestionVideo = question as QuestionVideo;
				var currentQuestionTemplateVideo = _currentQuestionViewer as QuestionViewerVideo;

				currentQuestionTemplateVideo.FillTemplate(currentQuestionVideo.Question, currentQuestionVideo.Video,
					currentQuestionVideo.NormalizedPauseTime);

				_currentQuestionViewer = currentQuestionTemplateVideo;
				break;

			case TypesOfQuestions.Type.textWithOptions:
				var currentQuestionTextWithOptions = question as QuestionTextWithOptions;
				var currentQuestionTemplateTextWithOptions = _currentQuestionViewer as QuestionViewerTextWithOptions;

				currentQuestionTemplateTextWithOptions.FillTemplate(currentQuestionTextWithOptions.Question,
					currentQuestionTextWithOptions.Options, currentQuestionTextWithOptions.RightOption);

				_currentQuestionViewer = currentQuestionTemplateTextWithOptions;
				break;

			case TypesOfQuestions.Type.imageWithOptions:
				var currentQuestionImageWithOptions = question as QuestionImageWithOptions;
				var currentQuestionTemplateImageWithOptions = _currentQuestionViewer as QuestionViewerImageWithOptions;

				currentQuestionTemplateImageWithOptions.FillTemplate(currentQuestionImageWithOptions.Question,
					currentQuestionImageWithOptions.Image, currentQuestionImageWithOptions.Options,
					currentQuestionImageWithOptions.RightOption);

				_currentQuestionViewer = currentQuestionTemplateImageWithOptions;
				break;

			case TypesOfQuestions.Type.audioWithOptions:
				var currentQuestionAudioWithOptions = question as QuestionAudioWithOptions;
				var currentQuestionTemplateAudioWithOptions = _currentQuestionViewer as QuestionViewerAudioWithOptions;

				currentQuestionTemplateAudioWithOptions.FillTemplate(currentQuestionAudioWithOptions.Question,
					currentQuestionAudioWithOptions.Audio, currentQuestionAudioWithOptions.GetNormalizedPauseMark(),
					currentQuestionAudioWithOptions.Options, currentQuestionAudioWithOptions.RightOption);

				_currentQuestionViewer = currentQuestionTemplateAudioWithOptions;
				break;

			case TypesOfQuestions.Type.videoWithOptions:
				var currentQuestionVideoWithOptions = question as QuestionVideoWithOptions;
				var currentQuestionTemplateVideoWithOptions = _currentQuestionViewer as QuestionViewerVideoWithOptions;

				currentQuestionTemplateVideoWithOptions.FillTemplate(currentQuestionVideoWithOptions.Question,
					currentQuestionVideoWithOptions.Video, currentQuestionVideoWithOptions.NormalizedPauseTime,
					currentQuestionVideoWithOptions.Options, currentQuestionVideoWithOptions.RightOption);

				_currentQuestionViewer = currentQuestionTemplateVideoWithOptions;
				break;

			case TypesOfQuestions.Type.imageHidden:
				break;
			case TypesOfQuestions.Type.karaoke:
				break;

			default:
				Debug.Log("Не могу понять тип вопроса.");
				break;
		}
	}
}
