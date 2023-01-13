using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//[RequireComponent(typeof(RectTransform))]

public class QuestionViewerText : QuestionViewerTemplate
{
	[Header("Question")]
	[SerializeField] private TMP_Text _question;
	[SerializeField] private TMP_Text _answer;

	private RectTransform _questionRectTransform;
	private Vector3 _questionStartPosition;
	private Color _questionStartColor;
	private Color _questionTransparentColor;

	private RectTransform _answerRectTransform;
	private Vector3 _answerStartPosition;
	private Color _answerStartColor;
	private Color _answerTransparentColor;

	private Coroutine _enterQuestionJob;
	private Coroutine _exitQuestionJob;
	private Coroutine _fadeInQuestion;
	private Coroutine _fadeOutQuestion;
	private Coroutine _fadeInAnswer;
	private Coroutine _fadeOutAnswer;
	private float _questionCurrentTime;
	private float _questionCurrentTimeNormalize;
	private float _answerCurrentTime;
	private float _answerCurrentTimeNormalize;

	public void FillTemplate(string question, string answer)
	{
		_question.text = question;
		_answer.text = answer;
	}

	public override void InitTemplate()
	{
		//Меняем руками для каждого шаблона
		Type = TypesOfQuestions.Type.text;

		_questionRectTransform = _question.gameObject.GetComponent<RectTransform>();
		_questionStartPosition = _questionRectTransform.anchoredPosition3D;
		_questionStartColor = _question.color;
		_questionTransparentColor = new Vector4(_questionStartColor.r, _questionStartColor.g, _questionStartColor.b, 0);

		_answerRectTransform = _answer.gameObject.GetComponent<RectTransform>();
		_answerStartPosition = _answerRectTransform.anchoredPosition3D;
		_answerStartColor = _answer.color;
		_answerTransparentColor = new Vector4(_answerStartColor.r, _answerStartColor.g, _answerStartColor.b, 0);

		InitWaitForSeconds();
	}

	public override void PreparationTemplate()
	{
		_answer.color = _answerTransparentColor;
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
	}

	public override void ClearTemplate()
	{
		_question.text = string.Empty;
		_answer.text = string.Empty;

		_questionRectTransform.anchoredPosition3D = _questionStartPosition;
		_question.color = _questionStartColor;

		_answerRectTransform.anchoredPosition3D = _answerStartPosition;
		_answer.color = _answerStartColor;
	}

	//Этот код копипастится в зависимости от наличия полей
	private IEnumerator EnterQuestionJob(Question question)
	{
		yield return new WaitUntil(() => question.IsAskedReadOnly);

		if (_fadeOutQuestion != null)
			StopCoroutine(_fadeOutQuestion);
		_fadeOutQuestion = StartCoroutine(FadeOutQuestion());

		yield return WaitBetweenViewers;

		if (_fadeInAnswer != null)
			StopCoroutine(_fadeInAnswer);
		_fadeInAnswer = StartCoroutine(FadeInAnswer());

		question.OnAnswerShowed();
	}

	private IEnumerator ExitQuestionJob(QuestionViewer questionViewer)
	{
		yield return null;

		questionViewer.CloseViewer();
	}

	private IEnumerator FadeInQuestion()
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

	private IEnumerator FadeOutQuestion()
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

		_question.color = _questionTransparentColor;
		_questionRectTransform.anchoredPosition = _questionStartPosition - _properties.OffsetPosition;
	}

	private IEnumerator FadeInAnswer()
	{
		_answerCurrentTime = 0;

		_answerRectTransform.anchoredPosition = _answerStartPosition + _properties.OffsetPosition;

		while (_answerCurrentTime <= _properties.FadeInOutUIElements)
		{
			_answerCurrentTime += Time.deltaTime;

			_answerCurrentTimeNormalize = _answerCurrentTime / _properties.FadeInOutUIElements;

			_answerRectTransform.anchoredPosition = Vector2.Lerp(_answerStartPosition + _properties.OffsetPosition,
				_answerStartPosition, _properties.FadeIn.Evaluate(_answerCurrentTimeNormalize));

			_answer.color = Vector4.Lerp(_answerTransparentColor, _answerStartColor, _properties.FadeIn.Evaluate(_answerCurrentTimeNormalize));

			yield return null;
		}

		_answer.color = _answerStartColor;
		_answerRectTransform.anchoredPosition = _answerStartPosition;
	}

	private IEnumerator FadeOutAnswer()
	{
		_answerCurrentTime = 0;

		_answerRectTransform.anchoredPosition = _answerStartPosition;

		while (_answerCurrentTime <= _properties.FadeInOutUIElements)
		{
			_answerCurrentTime += Time.deltaTime;

			_answerCurrentTimeNormalize = _answerCurrentTime / _properties.FadeInOutUIElements;

			_answerRectTransform.anchoredPosition = Vector2.Lerp(_answerStartPosition,
				_answerStartPosition - _properties.OffsetPosition, _properties.FadeOut.Evaluate(_answerCurrentTimeNormalize));

			_answer.color = Vector4.Lerp(_questionStartColor, _questionTransparentColor, _properties.FadeOut.Evaluate(_answerCurrentTimeNormalize));

			yield return null;
		}

		_answer.color = _questionTransparentColor;
		_answerRectTransform.anchoredPosition = _answerStartPosition - _properties.OffsetPosition;
	}
}
