using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClientPreviewMenu : BasePreviewMenu
{
	[SerializeField] private Texture2D _defaultTexture;
	//[SerializeField] private RenderTexture _previewTexture;
	[SerializeField] private RawImage _previewImage;
	[Space]
	[SerializeField] private MenuSliderWithCount _qualitySlider;
	[Range(0, 1)]
	[SerializeField] private float _defaultQuality;
	[Space]
	[SerializeField] private MenuSliderWithCount _fpsSlider;
	[Range(0, 50)]
	[SerializeField] private int _defaultFPS;
	//[Header("Service")]
	//[SerializeField] private TextureConverter _textureConverter;

	private int _minFPSValue = 0;
	private int _maxFPSValue = 50;
	private int _currentFPS;
	public int _currentWidth;
	public int _currentHeight;
	private int _currentValue;
	private float _currentNormalizeValue;

	private bool _isOn = false;
	private bool _isZeroFPS = false;

	private WaitForSeconds _waitFramesPerSeconds;
	private Coroutine _startSendingPreviewTextureJob;

	public RawImage PreviewImage => _previewImage;
	public bool IsOn => _isOn;

	public UnityAction<float> OnChangedPreviewQualityEvent;
	public UnityAction<int> OnChangedPreviewFPSEvent;
	public UnityAction OnDoubleClickedPreviewEvent;
	public UnityAction<int, int> OnRequestPreviewTextureEvent;

	private DoubleClickDetector _previewDoubleClick;

	private void Awake()
	{
		_previewImage.TryGetComponent<DoubleClickDetector>(out _previewDoubleClick);
	}

	private void OnEnable()
	{
		_qualitySlider.OnChangedSliderEvent += (normalizeValue) => OnChangedPreviewQuality(normalizeValue);
		_qualitySlider.OnSliderDoubleClickedEvent += OnQualityDoubleClick;

		_fpsSlider.OnChangedSliderEvent += (normalizeValue) => OnChangedPreviewFPS(normalizeValue);
		_fpsSlider.OnSliderDoubleClickedEvent += OnFPSDoubleClick;

		if (_previewDoubleClick != null)
			_previewDoubleClick.OnDoubleClickDetectedEvent += OnDoubleClickedPreview;
	}

	private void OnDisable()
	{
		_qualitySlider.OnChangedSliderEvent -= (normalizeValue) => OnChangedPreviewQuality(normalizeValue);
		_qualitySlider.OnSliderDoubleClickedEvent -= OnQualityDoubleClick;


		_fpsSlider.OnChangedSliderEvent -= (normalizeValue) => OnChangedPreviewFPS(normalizeValue);
		_fpsSlider.OnSliderDoubleClickedEvent -= OnFPSDoubleClick;

		if (_previewDoubleClick != null)
			_previewDoubleClick.OnDoubleClickDetectedEvent -= OnDoubleClickedPreview;
	}

	private void Start()
	{
		_qualitySlider.SetSliderValue(_defaultQuality);

		_fpsSlider.SetSliderValue(RangeMapperToNormalize(_defaultFPS, _minFPSValue, _maxFPSValue));
	}

	public void SwitchOn(Color textColor)
	{
		_isOn = true;

		_previewImage.texture = null;

		_qualitySlider.SetInteractable(true, textColor);
		_fpsSlider.SetInteractable(true, textColor);

		StartSendingPreviewTexture();
	}

	public void SwitchOff(Color textColor)
	{
		_isOn = false;

		if (_startSendingPreviewTextureJob != null)
			StopCoroutine(_startSendingPreviewTextureJob);

		_previewImage.texture = _defaultTexture;

		_qualitySlider.SetInteractable(false, textColor);
		_fpsSlider.SetInteractable(false, textColor);
	}

	private void StartSendingPreviewTexture()
	{
		if (_startSendingPreviewTextureJob != null)
			StopCoroutine(_startSendingPreviewTextureJob);
		_startSendingPreviewTextureJob = StartCoroutine(StartSendingRequestPreviewTextureJob());
	}

	private void OnDoubleClickedPreview()
	{
		OnDoubleClickedPreviewEvent?.Invoke();
	}

	private void OnChangedPreviewQuality(float normalizeValue)
	{
		_qualitySlider.DisplayValue(string.Format("{0:0.0}", normalizeValue));

		OnChangedPreviewQualityEvent?.Invoke(normalizeValue);
	}

	private void OnQualityDoubleClick()
	{
		_qualitySlider.SetSliderValue(_defaultQuality);
	}

	private void OnChangedPreviewFPS(float normalizeValue)
	{
		var value = RangeMapperFromNormalize(normalizeValue, _minFPSValue, _maxFPSValue);

		if (value <= 0)  // && _startSendingPreviewTextureJob != null)
		{
			if (_startSendingPreviewTextureJob != null)
				StopCoroutine(_startSendingPreviewTextureJob);

			_isZeroFPS = true;

			//Debug.Log("FPS: 0; Stop.");
		}
		else if(_isOn && _isZeroFPS)
		{
			//Debug.Log("FPS: !=0; Start.");

			_isZeroFPS = false;

			if (_startSendingPreviewTextureJob != null)
				StopCoroutine(_startSendingPreviewTextureJob);
			_startSendingPreviewTextureJob = StartCoroutine(StartSendingRequestPreviewTextureJob());
		}

		_fpsSlider.DisplayValue(value.ToString());

		OnChangedPreviewFPSEvent?.Invoke(value);
	}

	private void OnFPSDoubleClick()
	{
		_fpsSlider.SetSliderValue(RangeMapperToNormalize(_defaultFPS, _minFPSValue, _maxFPSValue));
	}

	private IEnumerator StartSendingRequestPreviewTextureJob()
	{
		while (!_isZeroFPS)
		{
			_currentWidth = Convert.ToInt32(_previewImage.rectTransform.rect.width * _qualitySlider.Slider.value);
			_currentHeight = Convert.ToInt32(_previewImage.rectTransform.rect.height * _qualitySlider.Slider.value);

			OnRequestPreviewTextureEvent?.Invoke(_currentWidth, _currentHeight);

			_currentFPS = RangeMapperFromNormalize(_fpsSlider.Slider.value, _minFPSValue, _maxFPSValue);

			_waitFramesPerSeconds = new WaitForSeconds(1 / _currentFPS);

			yield return _waitFramesPerSeconds;
		}
	}

	private int RangeMapperFromNormalize(float normalizeValue, int minValue, int maxValue)
	{
		normalizeValue = Mathf.Clamp(normalizeValue, 0, 1);

		_currentValue = Convert.ToInt32(minValue + normalizeValue * (maxValue - minValue));
		//_currentNormalizeValue = _minNormalizeValue + ((valueIndB - _minValue) * (_maxNormalizeValue - _minNormalizeValue)) / (_maxValue - _minValue);

		return _currentValue;
	}

	private float RangeMapperToNormalize(int value, int minValue, int maxValue)
	{
		value = Mathf.Clamp(value, minValue, maxValue);

		_currentNormalizeValue = (value - minValue) * (1 / (float)(maxValue - minValue));

		return _currentNormalizeValue;
	}
}
