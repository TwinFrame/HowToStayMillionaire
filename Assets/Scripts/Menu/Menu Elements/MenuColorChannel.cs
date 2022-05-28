using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuColorChannel : BaseMenuElement
{
	[SerializeField] private TMP_InputField _inputField;
	[SerializeField] private Slider _slider;

	private float _minNormalize = 0;
	private float _maxNormalize = 1;
	private float _minChannel = 0;
	private float _maxChannel = 255;

	public Slider Slider => _slider;
	public UnityAction<float> ChangeChannelEvent;

	private void OnEnable()
	{
		_inputField.onValueChanged.AddListener(OnChangeInputField);

		_slider.onValueChanged.AddListener(OnChangeSlider);

		_inputField.text = _slider.value.ToString();
	}

	private void OnDisable()
	{
		_inputField.onValueChanged.RemoveListener(OnChangeInputField);

		_slider.onValueChanged.RemoveListener(OnChangeSlider);
	}

	private void OnChangeInputField(string text)
	{
		if (TryCheckColorValue(text, out int value))
		{
			_slider.value = RemapValue(value, _minChannel, _maxChannel, _minNormalize, _maxNormalize);
		}
	}

	public void OnChangeSlider(float value)
	{
		_inputField.text = Mathf.Round(RemapValue(value, _minNormalize, _maxNormalize, _minChannel, _maxChannel)).ToString();

		ChangeChannelEvent?.Invoke(value);
	}

	public void OnChangeChannelWithoutNotify(float value)
	{
		_slider.SetValueWithoutNotify(value);

		_inputField.SetTextWithoutNotify(Mathf.Round(RemapValue(value, _minNormalize, _maxNormalize, _minChannel, _maxChannel)).ToString());
	}

	private bool TryCheckColorValue(string text, out int valueChannel)
	{
		if (int.TryParse(text.Replace(" ", string.Empty), out int value))
		{
			if (value <= 255 && value >= 0)
			{
				valueChannel = value;
				return true;
			}
			else
			{
				valueChannel = value;
				return false;
			}
		}
		else
		{
			valueChannel = int.MinValue;
			return false;
		}
	}

	private float RemapValue(float value, float minIn, float maxIn, float minOut, float maxOut)
	{
		value = Mathf.Clamp(value, minIn, maxIn);

		float result = minOut + (value - minIn) * (maxOut - minOut) / (maxIn - minIn);

		return result;
	}
}
