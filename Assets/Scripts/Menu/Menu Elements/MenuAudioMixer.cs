using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class MenuAudioMixer : BaseMenuElement
{
	[Header("Volume Sliders")]
	[SerializeField] private Slider _masterSlider;
	[SerializeField] private Slider _fxSlider;
	[SerializeField] private Slider _countdownSlider;
	[SerializeField] private Slider _questionSlider;
	[SerializeField] private Slider _musicSlider;

	private DoubleClickDetector _masterDoubleClick;
	private DoubleClickDetector _fxDoubleClick;
	private DoubleClickDetector _countdownDoubleClick;
	private DoubleClickDetector _questionDoubleClick;
	private DoubleClickDetector _musicDoubleClick;

	public UnityAction<TypesOfAudioChannel, float> OnChangedAudioChannelEvent;
	public UnityAction<TypesOfAudioChannel> OnChannelDoubleClickedEvent;

	private void Awake()
	{
		_masterSlider.TryGetComponent<DoubleClickDetector>(out _masterDoubleClick);

		if (_fxSlider.TryGetComponent<DoubleClickDetector>(out DoubleClickDetector fxDoubleClickDetector))
			_fxDoubleClick = fxDoubleClickDetector;

		if (_countdownSlider.TryGetComponent<DoubleClickDetector>(out DoubleClickDetector countdownDoubleClickDetector))
			_countdownDoubleClick = countdownDoubleClickDetector;

		if (_questionSlider.TryGetComponent<DoubleClickDetector>(out DoubleClickDetector questionDoubleClickDetector))
			_questionDoubleClick = questionDoubleClickDetector;

		if (_musicSlider.TryGetComponent<DoubleClickDetector>(out DoubleClickDetector musicDoubleClickDetector))
			_musicDoubleClick = musicDoubleClickDetector;
	}

	private void OnEnable()
	{
		if (_masterDoubleClick != null)
			_masterDoubleClick.OnDoubleClickDetectedEvent += OnMasterDoubleClick;
		_masterSlider.onValueChanged.AddListener(OnChangedMasterChannel);

		if (_fxDoubleClick != null)
			_fxDoubleClick.OnDoubleClickDetectedEvent += OnFxDoubleClick;
		_fxSlider.onValueChanged.AddListener(OnChangedFxChannel);

		if (_countdownDoubleClick != null)
			_countdownDoubleClick.OnDoubleClickDetectedEvent += OnCountdownDoubleClick;
		_countdownSlider.onValueChanged.AddListener(OnChangedCountdownChannel);

		if (_questionDoubleClick != null)
			_questionDoubleClick.OnDoubleClickDetectedEvent += OnQuestionDoubleClick;
		_questionSlider.onValueChanged.AddListener(OnChangedQuestionChannel);

		if (_musicDoubleClick != null)
			_musicDoubleClick.OnDoubleClickDetectedEvent += OnMusicDoubleClick;
		_musicSlider.onValueChanged.AddListener(OnChangedMusicChannel);

	}

	private void OnDisable()
	{
		if (_masterDoubleClick != null)
			_masterDoubleClick.OnDoubleClickDetectedEvent -= OnMasterDoubleClick;
		_masterSlider.onValueChanged.RemoveListener(OnChangedMasterChannel);

		if (_fxDoubleClick != null)
			_fxDoubleClick.OnDoubleClickDetectedEvent -= OnFxDoubleClick;
		_fxSlider.onValueChanged.RemoveListener(OnChangedFxChannel);

		if (_countdownDoubleClick != null)
			_countdownDoubleClick.OnDoubleClickDetectedEvent -= OnCountdownDoubleClick;
		_countdownSlider.onValueChanged.RemoveListener(OnChangedCountdownChannel);

		if (_questionDoubleClick != null)
			_questionDoubleClick.OnDoubleClickDetectedEvent -= OnQuestionDoubleClick;
		_questionSlider.onValueChanged.RemoveListener(OnChangedQuestionChannel);

		if (_musicDoubleClick != null)
			_musicDoubleClick.OnDoubleClickDetectedEvent -= OnMusicDoubleClick;
		_musicSlider.onValueChanged.RemoveListener(OnChangedMusicChannel);

	}

	public void SetChannelWithoutNotify(TypesOfAudioChannel channel, float normalizeValue)
	{
		switch (channel)
		{
			case TypesOfAudioChannel.Master:
				SetMasterChannelWithoutNotify(normalizeValue);
				break;
			case TypesOfAudioChannel.Fx:
				SetFxChannelWithoutNotify(normalizeValue);
				break;
			case TypesOfAudioChannel.Countown:
				SetCountdownChannelWithoutNotify(normalizeValue);
				break;
			case TypesOfAudioChannel.Question:
				SetQuestionChannelWithoutNotify(normalizeValue);
				break;
			case TypesOfAudioChannel.Music:
				SetMusicChannelWithoutNotify(normalizeValue);
				break;
			default:
				break;
		}
	}

	private void SetMasterChannelWithoutNotify(float normalizedVolume)
	{
		_masterSlider.SetValueWithoutNotify(normalizedVolume);
	}

	private void SetFxChannelWithoutNotify(float normalizedVolume)
	{
		_fxSlider.SetValueWithoutNotify(normalizedVolume);
	}

	private void SetCountdownChannelWithoutNotify(float normalizedVolume)
	{
		_countdownSlider.SetValueWithoutNotify(normalizedVolume);
	}

	private void SetQuestionChannelWithoutNotify(float normalizedVolume)
	{
		_questionSlider.SetValueWithoutNotify(normalizedVolume);
	}

	private void SetMusicChannelWithoutNotify(float normalizedVolume)
	{
		_musicSlider.SetValueWithoutNotify(normalizedVolume);
	}

	private void OnChangedMasterChannel(float normalizeValue)
	{
		OnChangedAudioChannelEvent?.Invoke(TypesOfAudioChannel.Master, normalizeValue);
	}

	private void OnChangedFxChannel(float normalizeValue)
	{
		OnChangedAudioChannelEvent?.Invoke(TypesOfAudioChannel.Fx, normalizeValue);
	}

	private void OnChangedCountdownChannel(float normalizeValue)
	{
		OnChangedAudioChannelEvent?.Invoke(TypesOfAudioChannel.Countown, normalizeValue);
	}

	private void OnChangedQuestionChannel(float normalizeValue)
	{
		OnChangedAudioChannelEvent?.Invoke(TypesOfAudioChannel.Question, normalizeValue);
	}

	private void OnChangedMusicChannel(float normalizeValue)
	{
		OnChangedAudioChannelEvent?.Invoke(TypesOfAudioChannel.Music, normalizeValue);
	}

	private void OnMasterDoubleClick()
	{
		OnChannelDoubleClickedEvent?.Invoke(TypesOfAudioChannel.Master);
	}

	private void OnFxDoubleClick()
	{
		OnChannelDoubleClickedEvent?.Invoke(TypesOfAudioChannel.Fx);
	}

	private void OnCountdownDoubleClick()
	{
		OnChannelDoubleClickedEvent?.Invoke(TypesOfAudioChannel.Countown);
	}


	private void OnQuestionDoubleClick()
	{
		OnChannelDoubleClickedEvent?.Invoke(TypesOfAudioChannel.Question);
	}

	private void OnMusicDoubleClick()
	{
		OnChannelDoubleClickedEvent?.Invoke(TypesOfAudioChannel.Music);
	}
}