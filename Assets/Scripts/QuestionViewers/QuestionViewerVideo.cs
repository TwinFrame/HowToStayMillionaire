using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class QuestionViewerVideo : QuestionViewerTemplate, IQuestionViewerWithPlayer
{
	[Header("Question")]
	[SerializeField] private TMP_Text _question;
	[SerializeField] private AdvancedVideoPlayer _player;
	[SerializeField] private RawImage _rawImage;

	private float _normalizedPauseTime;

	private RectTransform _questionRectTransform;
	private Vector3 _questionStartPosition;
	private Color _questionStartColor;
	private Color _questionTransparentColor;

	private RectTransform _rawImageRectTransform;
	private Vector3 _rawImageStartPosition;
	private Color _rawImageStartColor;
	private Color _rawImageTransparentColor;

	private Coroutine _enterQuestionJob;
	private Coroutine _exitQuestionJob;

	private Coroutine _fadeInQuestion;
	private Coroutine _fadeOutQuestion;

	private Coroutine _fadeInRawImage;
	private Coroutine _fadeOutRawImage;

	private float _questionCurrentTime;
	private float _questionCurrentTimeNormalize;
	private float _rawImageCurrentTime;
	private float _rawImageCurrentTimeNormalize;

	//public AdvancedVideoPlayer VideoReadOnly => _player;

	public void FillTemplate(string question, VideoClip video, float normalizedPauseTime)
	{
		_question.text = question;

		_normalizedPauseTime = normalizedPauseTime;
		_normalizedPauseTime = Mathf.Clamp(_normalizedPauseTime, 0, 1);

		_player.LoadContent(video);
	}

	public override void InitTemplate()
	{
		//Меняем руками для каждого шаблона
		Type = TypesOfQuestions.Type.video;
		//IsHaveVideo = true;

		_questionRectTransform = _question.gameObject.GetComponent<RectTransform>();
		_questionStartPosition = _questionRectTransform.anchoredPosition3D;
		_questionStartColor = _question.color;
		_questionTransparentColor = new Vector4(_questionStartColor.r, _questionStartColor.g, _questionStartColor.b, 0);

		_rawImageRectTransform = _rawImage.gameObject.GetComponent<RectTransform>();
		_rawImageStartPosition = _rawImageRectTransform.anchoredPosition3D;
		_rawImageStartColor = _rawImage.color;
		_rawImageTransparentColor = new Vector4(_rawImageStartColor.r, _rawImageStartColor.g, _rawImageStartColor.b, 0);

		InitWaitForSeconds();
	}

	public override void PreparationTemplate()
	{
		_player.ReleaseRenderTexture();

		_player.PreparePlayer();
		//Тут надо дожидаться когда закончится Prepared
		_player.SetupAudio();
	}

	public override void Enter(Question question)
	{
		//_question.color = _questionTransparentColor;
		//_rawImage.color = _rawImageTransparentColor;

		if (_enterQuestionJob != null)
			StopCoroutine(_enterQuestionJob);
		_enterQuestionJob = StartCoroutine(EnterQuestionJob(question));
	}

	public override void Exit(QuestionViewer questionViewer)
	{
		/*
		if (_exitQuestionJob != null)
			StopCoroutine(_exitQuestionJob);
		_exitQuestionJob = StartCoroutine(ExitQuestionJob(questionViewer));
		*/

		questionViewer.CloseViewer();
	}

	public override void ClearTemplate()
	{
		_question.text = string.Empty;
		_normalizedPauseTime = 0;
		_player.ClearPlayer();
		//IsStartCountdown = false; //нужно для каждого Viewer
	}

	public IAdvancedPlayer GetPlayer()
	{
		return _player;
	}

	public void Play()
	{
		_player.Play();
	}

	public void PlayFull()
	{
		_player.PlayFull();
	}

	public void PlayUntilPauseMark()
	{
		_player.PlayUntilPauseMark(_normalizedPauseTime);
	}
	
	public void PlayAfterPauseMark()
	{
		_player.PlayAfterPauseMark(_normalizedPauseTime);
	}

	public void Pause()
	{
		_player.Pause();
	}

	public void SetLoop(bool isOn)
	{
		_player.SetIsLoop(isOn);
	}

	public bool GetIsOnLoop()
	{
		return _player.IsLooping;
	}

	public double GetCurrentTime()
	{
		return _player.NormalizeTime;
	}

	//Этот код копипастится в зависимости от наличия полей

	private IEnumerator EnterQuestionJob(Question question)
	{
		//_player.ReleaseRenderTexture();

		//_player.PreparePlayer();

		yield return new WaitUntil(() => _player.IsPrepared);
		//yield return _player.IsPrepared;

		//_player.SetupAudio();
		/*
		yield return WaitBetweenViewers;

		if (_fadeInQuestion != null)
			StopCoroutine(_fadeInQuestion);
		_fadeInQuestion = StartCoroutine(FadeInQuestion());

		yield return WaitBetweenElements;

		if (_fadeInRawImage != null)
			StopCoroutine(_fadeInRawImage);
		_fadeInRawImage = StartCoroutine(FadeInRawImage());

		yield return _fadeInRawImage;
		*/
		_player.PlayUntilPauseMark(_normalizedPauseTime);

		yield return new WaitUntil(() => question.IsAskedReadOnly);

		question.OnAnswerShowed();
	}

	private IEnumerator ExitQuestionJob(QuestionViewer questionViewer)
	{
		if (_fadeOutRawImage != null)
			StopCoroutine(_fadeOutRawImage);
		_fadeOutRawImage = StartCoroutine(FadeOutRawImage());

		yield return WaitBetweenElements;

		if (_fadeOutQuestion != null)
			StopCoroutine(_fadeOutQuestion);
		_fadeOutQuestion = StartCoroutine(FadeOutQuestion());

		yield return _fadeOutQuestion;

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


	private IEnumerator FadeInRawImage()
	{
		_rawImageCurrentTime = 0;

		_rawImageRectTransform.anchoredPosition = _rawImageStartPosition + _properties.OffsetPosition;

		while (_rawImageCurrentTime <= _properties.FadeInOutUIElements)
		{
			_rawImageCurrentTime += Time.deltaTime;

			_rawImageCurrentTimeNormalize = _rawImageCurrentTime / _properties.FadeInOutUIElements;

			_rawImageRectTransform.anchoredPosition = Vector2.Lerp(_rawImageStartPosition + _properties.OffsetPosition,
				_rawImageStartPosition, _properties.FadeIn.Evaluate(_rawImageCurrentTimeNormalize));

			_rawImage.color = Vector4.Lerp(_rawImageTransparentColor, _rawImageStartColor, _properties.FadeIn.Evaluate(_rawImageCurrentTimeNormalize));

			yield return null;
		}

		_rawImage.color = _rawImageStartColor;
		_rawImageRectTransform.anchoredPosition = _rawImageStartPosition;
	}

	private IEnumerator FadeOutRawImage()
	{
		_rawImageCurrentTime = 0;

		_rawImageRectTransform.anchoredPosition = _rawImageStartPosition;

		while (_rawImageCurrentTime <= _properties.FadeInOutUIElements)
		{
			_rawImageCurrentTime += Time.deltaTime;

			_rawImageCurrentTimeNormalize = _rawImageCurrentTime / _properties.FadeInOutUIElements;

			_rawImageRectTransform.anchoredPosition = Vector2.Lerp(_rawImageStartPosition,
				_rawImageStartPosition - _properties.OffsetPosition, _properties.FadeOut.Evaluate(_rawImageCurrentTimeNormalize));

			_rawImage.color = Vector4.Lerp(_rawImageStartColor, _rawImageTransparentColor, _properties.FadeOut.Evaluate(_rawImageCurrentTimeNormalize));

			yield return null;
		}

		_rawImage.color = _rawImageTransparentColor;
		_rawImageRectTransform.anchoredPosition = _rawImageStartPosition - _properties.OffsetPosition;
	}
}
