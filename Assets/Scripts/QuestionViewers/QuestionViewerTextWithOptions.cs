using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class QuestionViewerTextWithOptions : QuestionViewerTemplateWithOptions
{
	[Header("Question")]
	[SerializeField] private TMP_Text _question;

	private RectTransform _questionRectTransform;
	private Vector3 _questionStartPosition;
	private Color _questionStartColor;
	private Color _questionTransparentColor;

	private Coroutine _enterQuestionJob;
	private Coroutine _exitQuestionJob;

	private Coroutine _fadeInQuestionJob;
	private Coroutine _fadeOutQuestionJob;

	private float _questionCurrentTime;
	private float _questionCurrentTimeNormalize;

	public void FillTemplate(string text, List<string> optionText, int rightOption)
	{
		_question.text = text;

		FillOptions(optionText, rightOption);
	}

	public override void InitTemplate()
	{
		//Меняем руками для каждого шаблона
		Type = TypesOfQuestions.Type.textWithOptions;

		_questionRectTransform = _question.gameObject.GetComponent<RectTransform>();
		_questionStartPosition = _questionRectTransform.anchoredPosition3D;
		_questionStartColor = _question.color;
		_questionTransparentColor = new Vector4(_questionStartColor.r, _questionStartColor.g, _questionStartColor.b, 0);

		ScrollOptionsStartPosition = OptionsFolder.anchoredPosition3D;

		if (OptionsFolder.TryGetComponent<CanvasGroup>(out CanvasGroup canvasGroup))
			ScrollOptionsCanvasGroup = canvasGroup;
		else
			ScrollOptionsCanvasGroup = OptionsFolder.gameObject.AddComponent<CanvasGroup>();

		InitWaitForSeconds();
	}

	public override void PreparationTemplate()
	{
		SetSameFontSizeInOptions();
	}

	public override void Enter(Question question)
	{
		if (_enterQuestionJob != null)
			StopCoroutine(_enterQuestionJob);
		_enterQuestionJob = StartCoroutine(EnterQuestionJob(question));
	}

	public override void Exit(QuestionViewer questionViewer)
	{
		questionViewer.CloseViewer();

		DeleteOptions();
	}

	public override void ClearTemplate()
	{
		_question.text = string.Empty;

		_questionRectTransform.anchoredPosition3D = _questionStartPosition;
		_question.color = _questionStartColor;

		ResetOptions();
	}

	//Этот код копипастится в зависимости от наличия полей
	private IEnumerator EnterQuestionJob(Question question)
	{
		IsChoosedOption = false;

		yield return new WaitUntil(() => IsChoosedOption);

		if (ZoomInOptionCoroutine != null)
			StopCoroutine(ZoomInOptionCoroutine);
		ZoomInOptionCoroutine = StartCoroutine(ZoomInOptionJob());

		yield return new WaitUntil(() => question.IsAskedReadOnly);

		if (question.IsRightAnswerReadOnly)
			Options[CurrentChoosedOption - 1].color = _properties.GameColorChanger.GetRightColor();
		else
		{
			Options[CurrentChoosedOption - 1].color = _properties.GameColorChanger.GetWrongColor();
			Options[RightOption - 1].color = _properties.GameColorChanger.GetRightColor();
		}

		question.OnAnswerShowed();
	}

	private IEnumerator ExitQuestionJob(QuestionViewer questionViewer)
	{
		yield return null;

		questionViewer.CloseViewer();

		DeleteOptions();
	}

	private IEnumerator FadeInQuestionJob()
	{
		_questionCurrentTime = 0;

		_questionRectTransform.anchoredPosition = _questionStartPosition + _properties.OffsetPosition;

		while (_questionCurrentTime <= _properties.FadeInOutUIElements)
		{
			_questionCurrentTime += Time.deltaTime;

			_questionCurrentTimeNormalize = _questionCurrentTime / _properties.FadeInOutUIElements;

			_questionRectTransform.anchoredPosition = Vector2.Lerp(_questionStartPosition + _properties.OffsetPosition,
				_questionStartPosition, _properties.FadeIn.Evaluate(_questionCurrentTimeNormalize));

			_question.color = Vector4.Lerp(_questionTransparentColor, _questionStartColor, _properties.FadeIn.Evaluate(_questionCurrentTimeNormalize));

			yield return null;
		}

		_question.color = _questionStartColor;
		_questionRectTransform.anchoredPosition = _questionStartPosition;
	}

	private IEnumerator FadeOutQuestionJob()
	{
		_questionCurrentTime = 0;

		_questionRectTransform.anchoredPosition = _questionStartPosition;

		while (_questionCurrentTime <= _properties.FadeInOutUIElements)
		{
			_questionCurrentTime += Time.deltaTime;

			_questionCurrentTimeNormalize = _questionCurrentTime / _properties.FadeInOutUIElements;

			_questionRectTransform.anchoredPosition = Vector2.Lerp(_questionStartPosition,
				_questionStartPosition - _properties.OffsetPosition, _properties.FadeOut.Evaluate(_questionCurrentTimeNormalize));

			_question.color = Vector4.Lerp(_questionStartColor, _questionTransparentColor, _properties.FadeOut.Evaluate(_questionCurrentTimeNormalize));

			yield return null;
		}
	}
}
