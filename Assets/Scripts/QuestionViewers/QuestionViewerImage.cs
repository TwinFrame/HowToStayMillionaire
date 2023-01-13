using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestionViewerImage : QuestionViewerTemplate
{
	[Header("Question")]
	[SerializeField] private TMP_Text _question;
	[SerializeField] private TMP_Text _answer;
	[SerializeField] private Image _image;

	private RectTransform _questionRectTransform;
	private Vector3 _questionStartPosition;
	private Color _questionStartColor;
	private Color _questionTransparentColor;

	private RectTransform _answerRectTransform;
	private Vector3 _answerStartPosition;
	private Color _answerStartColor;
	private Color _answerTransparentColor;

	private RectTransform _imageRectTransform;
	private Vector3 _imageStartPosition;
	private Color _imageStartColor;
	private Color _imageTransparentColor;

	private Coroutine _enterQuestionJob;
	private Coroutine _exitQuestionJob;
	private Coroutine _fadeInQuestion;
	private Coroutine _fadeOutQuestion;
	private Coroutine _fadeInAnswer;
	private Coroutine _fadeOutAnswer;
	private Coroutine _fadeInImage;
	private Coroutine _fadeOutImage;
	private float _questionCurrentTime;
	private float _questionCurrentTimeNormalize;
	private float _answerCurrentTime;
	private float _answerCurrentTimeNormalize;
	private float _imageCurrentTime;
	private float _imageCurrentTimeNormalize;

	public override void InitTemplate()
	{
		//Меняем руками для каждого шаблона
		Type = TypesOfQuestions.Type.image;

		_questionRectTransform = _question.gameObject.GetComponent<RectTransform>();
		_questionStartPosition = _questionRectTransform.anchoredPosition3D;
		_questionStartColor = _question.color;
		_questionTransparentColor = new Vector4(_questionStartColor.r, _questionStartColor.g, _questionStartColor.b, 0);

		_answerRectTransform = _answer.gameObject.GetComponent<RectTransform>();
		_answerStartPosition = _answerRectTransform.anchoredPosition3D;
		_answerStartColor = _answer.color;
		_answerTransparentColor = new Vector4(_answerStartColor.r, _answerStartColor.g, _answerStartColor.b, 0);

		_imageRectTransform = _image.gameObject.GetComponent<RectTransform>();
		_imageStartPosition = _imageRectTransform.anchoredPosition3D;
		_imageStartColor = Color.white;
		_imageTransparentColor = new Vector4(_imageStartColor.r, _imageStartColor.g, _imageStartColor.b, 0);

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
		_image.sprite = null;

		_questionRectTransform.anchoredPosition3D = _questionStartPosition;
		_question.color = _questionStartColor;

		_answerRectTransform.anchoredPosition3D = _answerStartPosition;
		_answer.color = _answerStartColor;

		_imageRectTransform.anchoredPosition3D = _imageStartPosition;
		_image.color = Color.white;
	}

	public void FillTemplate(string question, string answer, Sprite image)
	{
		_question.text = question;
		_answer.text = answer;
		_image.sprite = image;
	}

	//Этот код копипастится в зависимости от наличия полей

	private IEnumerator EnterQuestionJob(Question question)
	{
		yield return new WaitUntil(() => question.IsAskedReadOnly);

		if (_fadeOutImage != null)
			StopCoroutine(_fadeOutImage);
		_fadeOutImage = StartCoroutine(FadeOutImage());

		yield return WaitBetweenElements;

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
		if (_fadeOutAnswer != null)
			StopCoroutine(_fadeOutAnswer);
		_fadeOutAnswer = StartCoroutine(FadeOutAnswer());

		yield return _fadeOutAnswer;

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

	private IEnumerator FadeInImage()
	{
		_imageCurrentTime = 0;

		_imageRectTransform.anchoredPosition = _imageStartPosition + _properties.OffsetPosition;

		while (_imageCurrentTime <= _properties.FadeInOutUIElements)
		{
			_imageCurrentTime += Time.deltaTime;

			_imageCurrentTimeNormalize = _imageCurrentTime / _properties.FadeInOutUIElements;

			_imageRectTransform.anchoredPosition = Vector2.Lerp(_imageStartPosition + _properties.OffsetPosition,
				_imageStartPosition, _properties.FadeIn.Evaluate(_imageCurrentTimeNormalize));

			_image.color = Vector4.Lerp(_imageTransparentColor, _imageStartColor, _properties.FadeIn.Evaluate(_imageCurrentTimeNormalize));

			yield return null;
		}

		_image.color = _imageStartColor;
		_imageRectTransform.anchoredPosition = _imageStartPosition;
	}

	private IEnumerator FadeOutImage()
	{
		_imageCurrentTime = 0;

		_imageRectTransform.anchoredPosition = _imageStartPosition;

		while (_imageCurrentTime <= _properties.FadeInOutUIElements)
		{
			_imageCurrentTime += Time.deltaTime;

			_imageCurrentTimeNormalize = _imageCurrentTime / _properties.FadeInOutUIElements;

			_imageRectTransform.anchoredPosition = Vector2.Lerp(_imageStartPosition,
				_imageStartPosition - _properties.OffsetPosition, _properties.FadeOut.Evaluate(_imageCurrentTimeNormalize));

			_image.color = Vector4.Lerp(_imageStartColor, _imageTransparentColor, _properties.FadeOut.Evaluate(_imageCurrentTimeNormalize));

			yield return null;
		}

		_image.color = _imageTransparentColor;
		_imageRectTransform.anchoredPosition = _imageStartPosition - _properties.OffsetPosition;
	}
}
