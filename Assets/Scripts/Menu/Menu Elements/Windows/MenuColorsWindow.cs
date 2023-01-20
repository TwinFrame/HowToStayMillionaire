using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuColorsWindow : MenuWindow
{
	[SerializeField] private Button _quitButton;
	[Header("Colors")]
	[SerializeField] private Button _mainColorButton;
	[SerializeField] private Button _add1ColorButton;
	[SerializeField] private Button _slaveColorButton;
	[SerializeField] private Button _add2ColorButton;
	[SerializeField] private Button _textColorButton;
	[SerializeField] private Button _selectedColorButton;
	[SerializeField] private Button _rightColorButton;
	[SerializeField] private Button _wrongColorButton;
	[Space]
	[SerializeField] private TMP_Dropdown _palettesDropdown;
	[Space]
	[SerializeField] private TMP_Text _textField;
	[Space]
	[SerializeField] private MenuColorPickerWindow _colorPickerWindow;
	[Header("Service")]
	[SerializeField] private ShowTextForTime _showTextForTime;

	private TypesOfGameColor _currentTypeOfColor;
	private List<ColorPalette> _currentPalettes = new List<ColorPalette>();
	private List<string> _currentPaletteNames = new List<string>();
	private bool _isUserAllowedChange = false;

	public UnityAction ClosedWindowEvent;
	public UnityAction<int> ChangedPaletteEvent;
	public UnityAction<int, TypesOfGameColor, Color> ChangedColorInPaletteEvent;
	public UnityAction BlockedHotkeyEvent;
	public UnityAction UnblockedHotkeyEvent;


	private void OnEnable()
	{
		_quitButton.onClick.AddListener(OnCloseWindow);

		_mainColorButton.onClick.AddListener(OnClickChangeMainColor);
		_slaveColorButton.onClick.AddListener(OnClickChangeSlaveColor);
		_add1ColorButton.onClick.AddListener(OnClickChangeAdd1Color);
		_add2ColorButton.onClick.AddListener(OnClickChangeAdd2Color);
		_textColorButton.onClick.AddListener(OnClickChangeTextColor);
		_selectedColorButton.onClick.AddListener(OnClickChangeSelectedColor);
		_rightColorButton.onClick.AddListener(OnClickChangeRightColor);
		_wrongColorButton.onClick.AddListener(OnClickChangeWrongColor);

		_palettesDropdown.onValueChanged.AddListener(OnChangedPalette);

		_colorPickerWindow.ChangedColorEvent += (color) => OnChangedColor(color);
		_colorPickerWindow.ClosedWindowEvent += CloseColorWindow;
	}

	private void OnDisable()
	{
		_quitButton.onClick.AddListener(OnCloseWindow);

		_mainColorButton.onClick.RemoveListener(OnClickChangeMainColor);
		_slaveColorButton.onClick.RemoveListener(OnClickChangeSlaveColor);
		_add1ColorButton.onClick.RemoveListener(OnClickChangeAdd1Color);
		_add2ColorButton.onClick.RemoveListener(OnClickChangeAdd2Color);
		_textColorButton.onClick.RemoveListener(OnClickChangeTextColor);
		_selectedColorButton.onClick.RemoveListener(OnClickChangeSelectedColor);
		_rightColorButton.onClick.RemoveListener(OnClickChangeRightColor);
		_wrongColorButton.onClick.RemoveListener(OnClickChangeWrongColor);

		_palettesDropdown.onValueChanged.RemoveListener(OnChangedPalette);

		_colorPickerWindow.ChangedColorEvent -= (color) => OnChangedColor(color);
		_colorPickerWindow.ClosedWindowEvent -= CloseColorWindow;
	}

	private void OnChangedPalette(int numPalette)
	{
		ChangedPaletteEvent?.Invoke(numPalette);
	}

	public void SetColorsFromPalette(List<ColorPalette> colorPalettes, int numPalette)
	{
		List<string> namesPalette = new List<string>();

		foreach (var colorPalette in colorPalettes)
		{
			namesPalette.Add(colorPalette.Name);
		}

		_palettesDropdown.ClearOptions();
		_palettesDropdown.AddOptions(namesPalette);

		_palettesDropdown.SetValueWithoutNotify(numPalette);

		SetColorPickers(colorPalettes[numPalette]);
	}

	private void SetColorPickers(ColorPalette colorPalette)
	{
		_isUserAllowedChange = colorPalette.IsUserAllowedChange;

		_mainColorButton.image.color = colorPalette.MainColor;
		_slaveColorButton.image.color = colorPalette.SlaveColor;
		_add1ColorButton.image.color = colorPalette.Add1Color;
		_add2ColorButton.image.color = colorPalette.Add2Color;
		_textColorButton.image.color = colorPalette.TextColor;
		_selectedColorButton.image.color = colorPalette.SelectedColor;
		_rightColorButton.image.color = colorPalette.RightColor;
		_wrongColorButton.image.color = colorPalette.WrongColor;
	}

	public void RefreshPalettesInMenu(List<ColorPalette> palettes, int currentNumPalette)
	{
		_currentPalettes = palettes;

		_currentPaletteNames.Clear();
		foreach (var palette in _currentPalettes)
			_currentPaletteNames.Add(palette.Name);

		_palettesDropdown.ClearOptions();
		_palettesDropdown.AddOptions(_currentPaletteNames);

		currentNumPalette = Mathf.Clamp(currentNumPalette, 0, palettes.Count - 1);

		_palettesDropdown.SetValueWithoutNotify(currentNumPalette);
	}

	public void OnClickChangeMainColor()
	{
		if (TryOpenColorWindow(TypesOfGameColor.Main, out Color color))
			OpenColorWindow(color);
	}

	public void OnClickChangeSlaveColor()
	{
		if (TryOpenColorWindow(TypesOfGameColor.Slave, out Color color))
			OpenColorWindow(color);
	}

	public void OnClickChangeAdd1Color()
	{
		if (TryOpenColorWindow(TypesOfGameColor.Additional1, out Color color))
			OpenColorWindow(color);
	}

	public void OnClickChangeAdd2Color()
	{
		if (TryOpenColorWindow(TypesOfGameColor.Additional2, out Color color))
			OpenColorWindow(color);
	}

	public void OnClickChangeTextColor()
	{
		if (TryOpenColorWindow(TypesOfGameColor.Text, out Color color))
			OpenColorWindow(color);
	}

	public void OnClickChangeSelectedColor()
	{
		if (TryOpenColorWindow(TypesOfGameColor.Selected, out Color color))
			OpenColorWindow(color);
	}

	public void OnClickChangeRightColor()
	{
		if (TryOpenColorWindow(TypesOfGameColor.Right, out Color color))
			OpenColorWindow(color);
	}

	public void OnClickChangeWrongColor()
	{
		if (TryOpenColorWindow(TypesOfGameColor.Wrong, out Color color))
			OpenColorWindow(color);
	}

	private void OnCloseWindow()
	{
		_showTextForTime.ClearText(_textField);

		ClosedWindowEvent?.Invoke();
	}

	private void OnChangedColor(Color color)
	{
		switch (_currentTypeOfColor)
		{
			case TypesOfGameColor.Main:
				_mainColorButton.image.color = color;
				ChangedColorInPaletteEvent?.Invoke(_palettesDropdown.value, TypesOfGameColor.Main, color);
				break;

			case TypesOfGameColor.Slave:
				_slaveColorButton.image.color = color;
				ChangedColorInPaletteEvent?.Invoke(_palettesDropdown.value, TypesOfGameColor.Slave, color);
				break;

			case TypesOfGameColor.Additional1:
				_add1ColorButton.image.color = color;
				ChangedColorInPaletteEvent?.Invoke(_palettesDropdown.value, TypesOfGameColor.Additional1, color);
				break;

			case TypesOfGameColor.Additional2:
				_add2ColorButton.image.color = color;
				ChangedColorInPaletteEvent?.Invoke(_palettesDropdown.value, TypesOfGameColor.Additional2, color);
				break;

			case TypesOfGameColor.Text:
				_textColorButton.image.color = color;
				ChangedColorInPaletteEvent?.Invoke(_palettesDropdown.value, TypesOfGameColor.Text, color);
				break;

			case TypesOfGameColor.Selected:
				_selectedColorButton.image.color = color;
				ChangedColorInPaletteEvent?.Invoke(_palettesDropdown.value, TypesOfGameColor.Selected, color);
				break;

			case TypesOfGameColor.Right:
				_rightColorButton.image.color = color;
				ChangedColorInPaletteEvent?.Invoke(_palettesDropdown.value, TypesOfGameColor.Right, color);
				break;

			case TypesOfGameColor.Wrong:
				_wrongColorButton.image.color = color;
				ChangedColorInPaletteEvent?.Invoke(_palettesDropdown.value, TypesOfGameColor.Wrong, color);
				break;

			default:
				Debug.Log("Unknown GameColor.");
				break;
		}

		CloseColorWindow();
	}

	private bool TryOpenColorWindow(TypesOfGameColor TypeOfColor, out Color color)
	{
		bool currentBool;

		if (_isUserAllowedChange)
		{
			_currentTypeOfColor = TypeOfColor;

			switch (TypeOfColor)
			{
				case TypesOfGameColor.Main:
					color = _mainColorButton.image.color;
					break;
				case TypesOfGameColor.Slave:
					color = _slaveColorButton.image.color;
					break;
				case TypesOfGameColor.Additional1:
					color = _add1ColorButton.image.color;
					break;
				case TypesOfGameColor.Additional2:
					color = _add2ColorButton.image.color;
					break;
				case TypesOfGameColor.Text:
					color = _textColorButton.image.color;
					break;
				case TypesOfGameColor.Selected:
					color = _selectedColorButton.image.color;
					break;
				case TypesOfGameColor.Right:
					color = _rightColorButton.image.color;
					break;
				case TypesOfGameColor.Wrong:
					color = _wrongColorButton.image.color;
					break;
				default:
					color = new Color(0, 0, 0, 0);
					break;
			}

			currentBool = true;
		}
		else
		{
			color = new Color(0, 0, 0, 0);
			currentBool = false;

			_showTextForTime.ShowText(_textField, $"Только пользовательские палитры (User) могут быть изменены. " +
				$"{_palettesDropdown.options[_palettesDropdown.value].text} не пользовательская палитра.");
		}

		return currentBool;
	}

	private void OpenColorWindow(Color color)
	{
		_colorPickerWindow.gameObject.SetActive(true);

		_colorPickerWindow.SetChannelSliders(color);

		BlockedHotkeyEvent?.Invoke();
	}

	private void CloseColorWindow()
	{
		_colorPickerWindow.SetChannelSliders(Color.white);

		_colorPickerWindow.gameObject.SetActive(false);

		UnblockedHotkeyEvent?.Invoke();
	}
}
