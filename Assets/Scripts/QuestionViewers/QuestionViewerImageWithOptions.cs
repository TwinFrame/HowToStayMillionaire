using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionViewerImageWithOptions : QuestionViewerTemplateWithOptions
{
	[Header("Question")]
	[SerializeField] private TMP_Text _question;
	[SerializeField] private Image _image;

	//ниже переменные для копипаста
	private RectTransform _imageRectTransform;
	private Vector3 _imageStartPosition;
	private Color _imageStartColor;
	private Color _imageTransparentColor;

	private RectTransform _questionRectTransform;
	private Vector3 _questionStartPosition;
	private Color _questionStartColor;
	private Color _questionTransparentColor;

	private Coroutine _enterQuestionJob;
	private Coroutine _exitQuestionJob;

	private Coroutine _fadeInQuestionJob;
	private Coroutine _fadeOutQuestionJob;

	private Coroutine _fadeInImageJob;
	private Coroutine _fadeOutImageJob;

	private float _questionCurrentTime;
	private float _questionCurrentTimeNormalize;
	private float _imageCurrentTime;
	private float _imageCurrentTimeNormalize;

	public void FillTemplate(string question, Sprite image, List<string> optionText, int rightOption)
	{
		_question.text = question;
		_image.sprite = image;

		FillOptions(optionText, rightOption);
	}

	public override void InitTemplate()
	{
		//Меняем руками для каждого шаблона
		Type = TypesOfQuestions.Type.imageWithOptions;

		_imageRectTransform = _image.gameObject.GetComponent<RectTransform>();
		_imageStartPosition = _imageRectTransform.anchoredPosition3D;
		_imageStartColor = Color.white;
		_imageTransparentColor = new Vector4(_imageStartColor.r, _imageStartColor.g, _imageStartColor.b, 0);

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
		_image.sprite = null;

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

		if (_fadeOutQuestionJob != null)
			StopCoroutine(_fadeOutQuestionJob);
		_fadeOutQuestionJob = StartCoroutine(FadeOutQuestionJob());

		if (ZoomOutOptionCoroutine != null)
			StopCoroutine(ZoomOutOptionCoroutine);
		ZoomOutOptionCoroutine = StartCoroutine(ZoomOutOptionJob());

		yield return WaitBetweenElements;

		if (_fadeOutImageJob != null)
			StopCoroutine(_fadeOutImageJob);
		_fadeOutImageJob = StartCoroutine(FadeOutImage());

		yield return _fadeOutImageJob;

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
	}
}
