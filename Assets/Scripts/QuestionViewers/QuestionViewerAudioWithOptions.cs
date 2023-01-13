using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class QuestionViewerAudioWithOptions : QuestionViewerTemplateWithOptions, IQuestionViewerWithPlayer
{
	[Header("Question")]
	[SerializeField] private TMP_Text _question;
	[SerializeField] private AdvancedAudioPlayer _player;
	[SerializeField] private AudioVizualizator _vizualizator;


	private RectTransform _questionRectTransform;
	private Vector3 _questionStartPosition;
	private Color _questionStartColor;
	private Color _questionTransparentColor;

	private RectTransform _audioImageRectTransform;
	private Vector3 _audioImageStartPosition;
	private Color _audioImageStartColor;
	private Color _audioImageTransparentColor;

	private List<SpriteShapeRenderer> _shapeRenderers;

	private Coroutine _enterQuestionJob;
	private Coroutine _exitQuestionJob;

	private Coroutine _fadeInQuestion;
	private Coroutine _fadeOutQuestion;

	private Coroutine _fadeInSound;
	private Coroutine _fadeOutSound;

	private float _questionCurrentTime;
	private float _questionCurrentTimeNormalize;
	private float _soundCurrentTime;
	private float _soundCurrentNormalizeTime;

	private float _normalizedPauseTime;

	public void FillTemplate(string question, AudioClip audio, float normalizedPauseTime, List<string> optionText, int rightOption)
	{
		_question.text = question;

		_normalizedPauseTime = normalizedPauseTime;
		_normalizedPauseTime = Mathf.Clamp(_normalizedPauseTime, 0, 1);

		_player.LoadContent(audio);

		FillOptions(optionText, rightOption);
	}

	public override void InitTemplate()
	{
		//Меняем руками для каждого шаблона
		Type = TypesOfQuestions.Type.audioWithOptions;

		_questionRectTransform = _question.gameObject.GetComponent<RectTransform>();
		_questionStartPosition = _questionRectTransform.anchoredPosition3D;
		_questionStartColor = _question.color;
		_questionTransparentColor = new Vector4(_questionStartColor.r, _questionStartColor.g, _questionStartColor.b, 0);

		InitWaitForSeconds();
	}

	public override void PreparationTemplate()
	{
		SetSameFontSizeInOptions();

		_vizualizator.Enter();
	}

	public override void Enter(Question question)
	{
		if (_enterQuestionJob != null)
			StopCoroutine(_enterQuestionJob);
		_enterQuestionJob = StartCoroutine(EnterQuestionJob(question));
	}

	public override void Exit(QuestionViewer questionViewer)
	{
		_vizualizator.Exit();

		questionViewer.CloseViewer();

		DeleteOptions();
	}


	public override void ClearTemplate()
	{
		_question.text = string.Empty;
		_normalizedPauseTime = 0;
		_player.ClearPlayer();

		ResetOptions();
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
		_player.PreparePlayer();

		yield return _player.IsPrepared;

		_player.PlayUntilPauseMark(_normalizedPauseTime);

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
		if (ZoomOutOptionCoroutine != null)
			StopCoroutine(ZoomOutOptionCoroutine);
		ZoomOutOptionCoroutine = StartCoroutine(ZoomOutOptionJob());

		yield return WaitBetweenElements;

		if (_fadeOutQuestion != null)
			StopCoroutine(_fadeOutQuestion);
		_fadeOutQuestion = StartCoroutine(FadeOutQuestion());

		yield return _fadeOutQuestion;

		_vizualizator.Exit();

		questionViewer.CloseViewer();

		DeleteOptions();
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
}
