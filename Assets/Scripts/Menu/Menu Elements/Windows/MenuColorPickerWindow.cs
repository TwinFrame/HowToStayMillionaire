using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuColorPickerWindow : MenuWindow
{
	[SerializeField] private Button _okButton;
	[SerializeField] private Button _quitButton;
	[SerializeField] private Image _colorPreview;
	[Space]
	[SerializeField] private TMP_InputField _inputHexadecimal;
	[Space]
	[SerializeField] private MenuColorChannel _rChannel;
	[SerializeField] private MenuColorChannel _gChannel;
	[SerializeField] private MenuColorChannel _bChannel;
	[SerializeField] private MenuColorChannel _aChannel;

	private Color _currentColor = new Color();

	public UnityAction<Color> ChangedColorEvent;
	public UnityAction ClosedWindowEvent;


	private void OnEnable()
	{
		_colorPreview.color = new Color(_rChannel.Slider.value, _gChannel.Slider.value,
			_bChannel.Slider.value, _aChannel.Slider.value);

		_okButton.onClick.AddListener(SetGameColor);
		_quitButton.onClick.AddListener(CloseWindow);

		_inputHexadecimal.onValueChanged.AddListener(ChangeHexadecimal);

		_rChannel.ChangedChannelEvent += (value) => ChangeRedChannel(value);
		_gChannel.ChangedChannelEvent += (value) => ChangeGreenChannel(value);
		_bChannel.ChangedChannelEvent += (value) => ChangeBlueChannel(value);
		_aChannel.ChangedChannelEvent += (value) => ChangeAlphaChannel(value);
	}

	private void OnDisable()
	{
		_okButton.onClick.RemoveListener(SetGameColor);
		_quitButton.onClick.RemoveListener(CloseWindow);

		_inputHexadecimal.onValueChanged.RemoveListener(ChangeHexadecimal);

		_rChannel.ChangedChannelEvent -= (value) => ChangeRedChannel(value);
		_gChannel.ChangedChannelEvent -= (value) => ChangeGreenChannel(value);
		_bChannel.ChangedChannelEvent -= (value) => ChangeBlueChannel(value);
		_aChannel.ChangedChannelEvent -= (value) => ChangeAlphaChannel(value);
	}

	public void SetChannelSliders(Color color)
	{
		_rChannel.OnChangeSlider(color.r);
		_gChannel.OnChangeSlider(color.g);
		_bChannel.OnChangeSlider(color.b);
		_aChannel.OnChangeSlider(color.a);
	}

	private void SetChannelSlidersWithoutNotify(Color color)
	{
		_rChannel.OnChangeChannelWithoutNotify(color.r);
		_gChannel.OnChangeChannelWithoutNotify(color.g);
		_bChannel.OnChangeChannelWithoutNotify(color.b);
		_aChannel.OnChangeChannelWithoutNotify(color.a);
	}

	private void SetGameColor()
	{
		ChangedColorEvent?.Invoke(_colorPreview.color);
	}

	private void CloseWindow()
	{
		ClosedWindowEvent?.Invoke();
	}

	private void ChangeHexadecimal(string hexadecimal)
	{
		if (ColorUtility.TryParseHtmlString("#" + hexadecimal, out Color color))
		{
			SetChannelSlidersWithoutNotify(color);

			_colorPreview.color = color;
		}
	}

	private void ChangeRedChannel(float value)
	{
		_colorPreview.color = ModifyColor(_colorPreview.color, ColorChannel.r, value);

		_inputHexadecimal.SetTextWithoutNotify(ColorUtility.ToHtmlStringRGBA(_colorPreview.color));
	}

	private void ChangeGreenChannel(float value)
	{
		_colorPreview.color = ModifyColor(_colorPreview.color, ColorChannel.g, value);

		_inputHexadecimal.SetTextWithoutNotify(ColorUtility.ToHtmlStringRGBA(_colorPreview.color));
	}

	private void ChangeBlueChannel(float value)
	{
		_colorPreview.color = ModifyColor(_colorPreview.color, ColorChannel.b, value);

		_inputHexadecimal.SetTextWithoutNotify(ColorUtility.ToHtmlStringRGBA(_colorPreview.color));
	}

	private void ChangeAlphaChannel(float value)
	{
		_colorPreview.color = ModifyColor(_colorPreview.color, ColorChannel.a, value);

		_inputHexadecimal.SetTextWithoutNotify(ColorUtility.ToHtmlStringRGBA(_colorPreview.color));
	}

	private Color ModifyColor(Color color, ColorChannel colorChannel, float value)
	{
		switch (colorChannel)
		{
			case ColorChannel.r:
				_currentColor = new Color(value, color.g, color.b, color.a);
				break;
			case ColorChannel.g:
				_currentColor = new Color(color.r, value, color.b, color.a);
				break;
			case ColorChannel.b:
				_currentColor = new Color(color.r, color.g, value, color.a);
				break;
			case ColorChannel.a:
				_currentColor = new Color(color.r, color.g, color.b, value);
				break;
			default:
				break;
		}

		return _currentColor;
	}
}
