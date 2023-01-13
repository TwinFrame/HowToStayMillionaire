using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuSliderWithCount : BaseMenuElement
{
	[SerializeField] private Slider _slider;
	[SerializeField] private TMP_Text _description;
	[SerializeField] private TMP_Text _textValue;

	private DoubleClickDetector _sliderDoubleClick;

	public Slider Slider => _slider;

	public UnityAction OnSliderDoubleClickedEvent;
	public UnityAction<float> OnChangedSliderEvent;

	private void Awake()
	{
		if (_slider.TryGetComponent<DoubleClickDetector>(out DoubleClickDetector DoubleClickDetector))
			_sliderDoubleClick = DoubleClickDetector;
	}

	private void OnEnable()
	{
		_slider.onValueChanged.AddListener(OnChangedSlider);

		if (_sliderDoubleClick != null)
			_sliderDoubleClick.OnDoubleClickDetectedEvent += OnSliderDoubleClicked;
	}

	private void OnDisable()
	{
		_slider.onValueChanged.RemoveListener(OnChangedSlider);

		if (_sliderDoubleClick != null)
			_sliderDoubleClick.OnDoubleClickDetectedEvent -= OnSliderDoubleClicked;
	}

	public void DisplayValue(string text)
	{
		_textValue.text = text;
	}

	public void SetInteractable(bool isInteractable, Color textColor)
	{
		_slider.interactable = isInteractable;
		_textValue.color = textColor;
		_description.color = textColor;
	}

	public void SetSliderValue(float normalizeValue)
	{
		normalizeValue = Mathf.Clamp(normalizeValue, 0, 1);

		_slider.value = normalizeValue;
	}

	private void OnChangedSlider(float normalizeValue)
	{
		OnChangedSliderEvent?.Invoke(normalizeValue);
	}

	private void OnSliderDoubleClicked()
	{
		OnSliderDoubleClickedEvent?.Invoke();
	}
}
