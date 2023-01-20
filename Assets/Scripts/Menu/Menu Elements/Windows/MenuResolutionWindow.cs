using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuResolutionWindow : MenuWindow
{
	[SerializeField] TMP_InputField _inputField;
	[SerializeField] Button _setButton;
	[SerializeField] TMP_Text _displayText;
	[SerializeField] Button _quit;

	private float aspectRatio = 1.7777f;
	private int _currentHeight = 0;
	private int _currentWidth = 0;

	public UnityAction<int, int> SetUserResolutionEvent;
	public UnityAction QuitWindowEvent;

	private void OnEnable()
	{
		_inputField.onValueChanged.AddListener(ChangeValue);
		_setButton.onClick.AddListener(OnSetCustomResolution);
		_quit.onClick.AddListener(OnQuitCustomResolutionMenu);
	}

	private void OnDisable()
	{
		_inputField.onValueChanged.RemoveListener(ChangeValue);
		_setButton.onClick.RemoveListener(OnSetCustomResolution);
		_quit.onClick.RemoveListener(OnQuitCustomResolutionMenu);
	}

	public void DisableSetButton()
	{
		_setButton.interactable = false;
	}

	private void OnSetCustomResolution()
	{
		if (_currentWidth < 300 || _currentHeight < 170)
		{
			WriteInDescription($"The width should not be < 300 and height < 170. Your resolution: {_currentWidth}x{_currentHeight}");
		}
		else
		{
			SetUserResolutionEvent?.Invoke(_currentWidth, _currentHeight);

			_displayText.text = string.Empty;
			_inputField.text = string.Empty;
			_setButton.interactable = false;
		}
	}

	private void ChangeValue(string text)
	{
		if (TryGetIntFromString(text, out _currentWidth))
		{
			if (_currentWidth < 300)
			{
				WriteInDescription($"The width must be > 300");

				_setButton.interactable = false;

				return;
			}

			_currentHeight = (int)((float)_currentWidth / aspectRatio);

			WriteInDescription($"Your resolution (16x9): {_currentWidth}x{_currentHeight}");

			_setButton.interactable = true;
		}
		else
		{
			WriteInDescription("It is possible to enter only integers 0-9.");

			_setButton.interactable = false;
		}
	}

	private void WriteInDescription(string text)
	{
		_displayText.text = string.Empty;

		_displayText.text = text;
	}

	private bool TryGetIntFromString(string text, out int value)
	{
		if (Int32.TryParse(text, out value))
			return true;
		else
			return false;
	}

	private void OnQuitCustomResolutionMenu()
	{
		_inputField.text = string.Empty;
		_displayText.text = string.Empty;
		_setButton.interactable = false;

		QuitWindowEvent?.Invoke();
	}
}
