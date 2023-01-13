using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuThemeChanger : MonoBehaviour
{
	[Header("Menu Fonts")]
	[SerializeField] private Material[] _fontMaterials;
	[Header("Service")]
	[SerializeField] private BaseMenu _menu;

	private ThemeMenu[] _themes;
	private int _currentTheme;

	private ConnectState _currentConnectState;

	private List<Button> _buttons = new List<Button>();
	private List<Image> _panels = new List<Image>();
	private List<Image> _panelsWithStroke = new List<Image>();
	private List<Image> _audioMixers = new List<Image>();
	private List<Image> _audioLines = new List<Image>();
	private List<Image> _slidersBG = new List<Image>();
	private List<Image> _slidersFillArea = new List<Image>();
	private List<Image> _slidersHandle = new List<Image>();
	private List<TMP_InputField> _inputFields = new List<TMP_InputField>();
	private List<Toggle> _toggles = new List<Toggle>();
	private List<Image> _checkmarksInToggle = new List<Image>();
	private List<TMP_Dropdown> _dropdowns = new List<TMP_Dropdown>();
	private List<Image> _dropdownsTemplate = new List<Image>();
	private List<Image> _dropdownsArrow = new List<Image>();
	private List<Image> _dropdownsItemBG = new List<Image>();
	private List<Image> _dropdownsItemCheckmark = new List<Image>();
	private List<Button> _quitButtons = new List<Button>();
	private List<Button> _connectButtons = new List<Button>();
	private List<Scrollbar> _scrollbars = new List<Scrollbar>();
	private List<Image> _scrollbarsBG = new List<Image>();
	private List<Image> _scrollbarHandles = new List<Image>();
	private List<Image> _displays = new List<Image>();
	private List<Button> _playUntilMarkButtons = new List<Button>();
	private List<Button> _playAfterMarkButtons = new List<Button>();
	private List<Button> _playFullButtons = new List<Button>();
	private List<Button> _pauseButtons = new List<Button>();
	private ColorBlock _temporaryColorBlock;

	private void Awake()
	{
		_currentTheme = 0;

		_themes = GetComponentsInChildren<ThemeMenu>();

		CollectUIElements();

		SetTheme();
	}

	public void ChangeTheme()
	{
		_currentTheme = GetNumberNextTheme();

		SetTheme();
	}

	public Sprite GetFollowingThemeSprite()
	{
		return _themes[GetNumberNextTheme()].Panel;
	}

	public Sprite GetButtonSprite()
	{
		return _themes[_currentTheme].Button;
	}

	public Color GetTextColor()
	{
		return _themes[_currentTheme].TextColor;
	}

	public Color GetDisabledColor()
	{
		return _themes[_currentTheme].DisabledButtonColor;
	}

	public Sprite GetSelectedSprite()
	{
		return _themes[_currentTheme].SelectedButton;
	}

	public Sprite GetRedSprite()
	{
		return _themes[_currentTheme].RedButton;
	}

	public Sprite GetYellowSprite()
	{
		return _themes[_currentTheme].YellowButton;
	}

	public Sprite GetGreenSprite()
	{
		return _themes[_currentTheme].GreenButton;
	}

	private void SetTheme()
	{
		ChangeFontsColor();
		ChangePanels();
		ChangePanelsWithStroke();
		ChangeAudioMixers();
		ChangeSliders();
		ChangeButtonsSprite();
		ChangeToggles();
		ChangeDropdowns();
		ChangeInputFields();
		ChangeQuitButtonsSprite();
		ChangeConnectButtonsSprite();
		ChangeScrollbars();
		ChangeDisplays();
		ChangePlayerButtons();
	}

	private int GetNumberNextTheme()
	{
		if (_currentTheme + 1 < _themes.Length)
			return _currentTheme + 1;
		else
			return 0;
	}

	private void CollectUIElements()
	{
		_buttons.Clear();
		_panels.Clear();
		_panelsWithStroke.Clear();
		_audioMixers.Clear();
		_slidersBG.Clear();
		_slidersFillArea.Clear();
		_slidersHandle.Clear();
		_audioLines.Clear();
		_inputFields.Clear();
		_toggles.Clear();
		_checkmarksInToggle.Clear();
		_dropdowns.Clear();
		_dropdownsTemplate.Clear();
		_dropdownsArrow.Clear();
		_dropdownsItemBG.Clear();
		_dropdownsItemCheckmark.Clear();
		_quitButtons.Clear();
		_connectButtons.Clear();
		_scrollbars.Clear();
		_scrollbarsBG.Clear();
		_scrollbarHandles.Clear();
		_displays.Clear();
		_playUntilMarkButtons.Clear();
		_playAfterMarkButtons.Clear();
		_playFullButtons.Clear();
		_pauseButtons.Clear();

		if (_menu.TryGetComponent<Image>(out Image imageMenu))
			_panels.Add(imageMenu);

		foreach (RectTransform child in _menu.GetComponentsInChildren<RectTransform>(true))
		{
			if (child.TryGetComponent<MenuButton>(out MenuButton menuButton) &&
				menuButton.TryGetComponent<Button>(out Button button))
			{
				_buttons.Add(button);
			}
			
			else if (child.TryGetComponent<MenuPanel>(out MenuPanel menuPanel) &&
				menuPanel.TryGetComponent<Image>(out Image imagePanel))
			{
				_panels.Add(imagePanel);
			}

			else if (child.TryGetComponent<MenuPanelWithStroke>(out MenuPanelWithStroke menuPanelwithStroke) &&
				menuPanelwithStroke.TryGetComponent<Image>(out Image imagePanelWithStroke))
			{
				_panelsWithStroke.Add(imagePanelWithStroke);
			}

			else if (child.TryGetComponent<MenuAudioMixer>(out MenuAudioMixer menuAudioMixer) &&
				menuAudioMixer.TryGetComponent<Image>(out Image imageAudioMixer))
			{
				_audioMixers.Add(imageAudioMixer);

				foreach (RectTransform childAudioMixer in menuAudioMixer.GetComponentsInChildren<RectTransform>(true))
				{
					if (childAudioMixer.TryGetComponent<MenuAudioLine>(out MenuAudioLine menuAudioLine) &&
						menuAudioLine.TryGetComponent<Image>(out Image imageAudioLine))
						_audioLines.Add(imageAudioLine);
				}
			}

			else if (child.TryGetComponent<Slider>(out Slider slider))
			{
				foreach (RectTransform childSlider in slider.GetComponentsInChildren<RectTransform>(true))
				{
					if (childSlider.TryGetComponent<MenuSliderBackground>(out MenuSliderBackground sliderBG))
						_slidersBG.Add(sliderBG.GetComponent<Image>());
				}

				if(slider.fillRect.TryGetComponent<Image>(out Image sliderAreaFill))
					_slidersFillArea.Add(sliderAreaFill);

				if (slider.handleRect.TryGetComponent<Image>(out Image sliderHandle))
					_slidersHandle.Add(sliderHandle);
			}

			else if (child.TryGetComponent<MenuInputField>(out MenuInputField menuInputField) &&
				menuInputField.TryGetComponent<TMP_InputField>(out TMP_InputField inputField))
			{
				_inputFields.Add(inputField);
			}

			else if (child.TryGetComponent<MenuToggle>(out MenuToggle menuToggle) &&
				menuToggle.TryGetComponent<Toggle>(out Toggle toggle))
			{
				_toggles.Add(toggle);

				if (toggle.graphic.TryGetComponent<Image>(out Image image))
					_checkmarksInToggle.Add(image);
			}

			else if (child.TryGetComponent<MenuDropdown>(out MenuDropdown menuDropdown) &&
				menuDropdown.TryGetComponent<TMP_Dropdown>(out TMP_Dropdown dropdown))
			{
				_dropdowns.Add(dropdown);

				if (dropdown.template.TryGetComponent<Image>(out Image image))
					_dropdownsTemplate.Add(image);

				foreach (RectTransform dropdownChild in dropdown.GetComponentsInChildren<RectTransform>(true))
				{
					if (dropdownChild.TryGetComponent<MenuDropdownArrow>(out MenuDropdownArrow menuDropdownArrow) &&
						menuDropdownArrow.TryGetComponent<Image>(out Image imageArrow))
					{
						_dropdownsArrow.Add(imageArrow);
					}
					else if (dropdownChild.TryGetComponent<Toggle>(out Toggle itemToggle))
					{
						if (itemToggle.targetGraphic.TryGetComponent<Image>(out Image itemImageBG))
							_dropdownsItemBG.Add(itemImageBG);

						if (itemToggle.graphic.TryGetComponent<Image>(out Image itemImageCheckmark))
							_dropdownsItemCheckmark.Add(itemImageCheckmark);
					}
				}
			}

			else if (child.TryGetComponent<MenuQuitButton>(out MenuQuitButton menuQuitButton) &&
				menuQuitButton.TryGetComponent<Button>(out Button quitButton))
			{
				_quitButtons.Add(quitButton);
			}

			else if (child.TryGetComponent<MenuConnectButton>(out MenuConnectButton menuConnectButton) &&
				menuConnectButton.TryGetComponent<Button>(out Button connectButton))
			{
				_connectButtons.Add(connectButton);
			}

			else if (child.TryGetComponent<Scrollbar>(out Scrollbar scrollbar))
			{
				_scrollbars.Add(scrollbar);

				if (scrollbar.TryGetComponent<Image>(out Image scrollbarBG))
					_scrollbarsBG.Add(scrollbarBG);

				if (scrollbar.targetGraphic.TryGetComponent<Image>(out Image scrollbarHandle))
					_scrollbarHandles.Add(scrollbarHandle);
			}

			else if (child.TryGetComponent<MenuDisplay>(out MenuDisplay menuDisplay) &&
				menuDisplay.TryGetComponent<Image>(out Image imageDisplay))
			{
				_displays.Add(imageDisplay);
			}

			else if (child.TryGetComponent<PlayerTab>(out PlayerTab playerTab))
			{
				_playUntilMarkButtons.Add(playerTab.PlayUntilMarkButton);
				_playAfterMarkButtons.Add(playerTab.PlayAfterMarkButton);
				_playFullButtons.Add(playerTab.PlayFullButton);
				_pauseButtons.Add(playerTab.PauseButton);
			}

			else if (child.TryGetComponent<MenuWindow>(out MenuWindow menuWindow) &&
				menuWindow.TryGetComponent<Image>(out Image imageWindow))
			{
				_panelsWithStroke.Add(imageWindow);
			}
		}

	}

	#region Set Theme
	private void ChangeFontsColor()
	{
		foreach (Material font in _fontMaterials)
			font.SetColor("_FaceColor", _themes[_currentTheme].TextColor);
	}

	private void ChangePanels()
	{
		foreach (Image panel in _panels)
			panel.sprite = _themes[_currentTheme].Panel;
	}

	private void ChangePanelsWithStroke()
	{
		foreach (Image panel in _panelsWithStroke)
			panel.sprite = _themes[_currentTheme].Button;
	}

	private void ChangeAudioMixers()
	{
		foreach (Image audioMixer in _audioMixers)
			audioMixer.sprite = _themes[_currentTheme].Button;

		foreach (Image line in _audioLines)
			line.sprite = _themes[_currentTheme].AudioLine;
	}

	private void ChangeSliders()
	{
		foreach (Image sliderBG in _slidersBG)
			sliderBG.sprite = _themes[_currentTheme].SliderBG;

		foreach (Image sliderFillAres in _slidersFillArea)
			sliderFillAres.sprite = _themes[_currentTheme].SliderFillArea;

		foreach (Image sliderHandle in _slidersHandle)
			sliderHandle.sprite = _themes[_currentTheme].SliderHandle;
	}

	private void ChangeButtonsSprite()
	{
		foreach (Button button in _buttons)
		{
			button.image.sprite = _themes[_currentTheme].Button;

			_temporaryColorBlock = button.colors;
			_temporaryColorBlock.disabledColor = _themes[_currentTheme].DisabledButtonColor;
			button.colors = _temporaryColorBlock;
		}
	}

	private void ChangeToggles()
	{
		foreach (Toggle toggle in _toggles)
		{
			toggle.image.sprite = _themes[_currentTheme].Button;

			_temporaryColorBlock = toggle.colors;
			_temporaryColorBlock.disabledColor = _themes[_currentTheme].DisabledButtonColor;
			toggle.colors = _temporaryColorBlock;
		}

		foreach (Image checkmark in _checkmarksInToggle)
			checkmark.sprite = _themes[_currentTheme].ChekmarkInToggle;
	}

	private void ChangeDropdowns()
	{
		foreach (TMP_Dropdown dropdown in _dropdowns)
		{
			dropdown.image.sprite = _themes[_currentTheme].Button;

			_temporaryColorBlock = dropdown.colors;
			_temporaryColorBlock.disabledColor = _themes[_currentTheme].DisabledButtonColor;
			dropdown.colors = _temporaryColorBlock;
		}

		foreach (Image template in _dropdownsTemplate)
			template.sprite = _themes[_currentTheme].Panel;

		//dropdown.arrow
		foreach (Image arrow in _dropdownsArrow)
			arrow.sprite = _themes[_currentTheme].Arrow;

		//dropdown.item
		foreach (Image itemBG in _dropdownsItemBG)
			itemBG.sprite = _themes[_currentTheme].Panel;

		foreach (Image checkmark in _dropdownsItemCheckmark)
			checkmark.sprite = _themes[_currentTheme].Checkmark;
	}

	private void ChangeInputFields()
	{
		foreach (TMP_InputField inputField in _inputFields)
		{
			inputField.image.sprite = _themes[_currentTheme].InputField;

			inputField.selectionColor = _themes[_currentTheme].InputFieldSelectionColor;

			inputField.caretWidth = _themes[_currentTheme].CaretWitdh;

			inputField.customCaretColor = true;
			inputField.caretColor = _themes[_currentTheme].InputFieldCaretColor;

			_temporaryColorBlock = inputField.colors;
			_temporaryColorBlock.disabledColor = _themes[_currentTheme].DisabledButtonColor;
			inputField.colors = _temporaryColorBlock;
		}
	}

	private void ChangeQuitButtonsSprite()
	{
		foreach (Button quitButton in _quitButtons)
		{
			quitButton.image.sprite = _themes[_currentTheme].QuitButton;

			_temporaryColorBlock = quitButton.colors;
			_temporaryColorBlock.disabledColor = _themes[_currentTheme].DisabledButtonColor;
			quitButton.colors = _temporaryColorBlock;
		}
	}

	private void ChangeConnectButtonsSprite()
	{
		_currentConnectState = _menu.GetConnectState();

		foreach (Button connectButton in _connectButtons)
		{
			switch (_currentConnectState)
			{
				default:
				case ConnectState.Off:
					connectButton.image.sprite = _themes[_currentTheme].RedButton;
					break;
				case ConnectState.Wait:
					connectButton.image.sprite = _themes[_currentTheme].YellowButton;
					break;
				case ConnectState.On:
					connectButton.image.sprite = _themes[_currentTheme].GreenButton;
					break;
			}

			_temporaryColorBlock = connectButton.colors;
			_temporaryColorBlock.disabledColor = _themes[_currentTheme].DisabledButtonColor;
			connectButton.colors = _temporaryColorBlock;
		}
	}

	private void ChangeScrollbars()
	{
		foreach (Scrollbar scrollbar in _scrollbars)
		{
			_temporaryColorBlock = scrollbar.colors;
			_temporaryColorBlock.disabledColor = _themes[_currentTheme].DisabledButtonColor;
			scrollbar.colors = _temporaryColorBlock;
		}

		foreach (Image bg in _scrollbarsBG)
			bg.sprite = _themes[_currentTheme].Button;

		foreach (Image handle in _scrollbarHandles)
			handle.sprite = _themes[_currentTheme].SelectedButton;
	}

	private void ChangeDisplays()
	{
		foreach (Image display in _displays)
			display.sprite = _themes[_currentTheme].InputField;
	}

	private void ChangePlayerButtons()
	{
		foreach (Button playUntilMarkButton in _playUntilMarkButtons)
		{
			playUntilMarkButton.image.sprite = _themes[_currentTheme].PlayUntilMarkButton;

			_temporaryColorBlock = playUntilMarkButton.colors;
			_temporaryColorBlock.disabledColor = _themes[_currentTheme].DisabledButtonColor;
			playUntilMarkButton.colors = _temporaryColorBlock;
		}

		foreach (Button playAfterMarkButton in _playAfterMarkButtons)
		{
			playAfterMarkButton.image.sprite = _themes[_currentTheme].PlayAfterMarkButton;

			_temporaryColorBlock = playAfterMarkButton.colors;
			_temporaryColorBlock.disabledColor = _themes[_currentTheme].DisabledButtonColor;
			playAfterMarkButton.colors = _temporaryColorBlock;
		}

		foreach (Button playFullButton in _playFullButtons)
		{
			playFullButton.image.sprite = _themes[_currentTheme].PlayFullButton;

			_temporaryColorBlock = playFullButton.colors;
			_temporaryColorBlock.disabledColor = _themes[_currentTheme].DisabledButtonColor;
			playFullButton.colors = _temporaryColorBlock;
		}

		foreach (Button pauseButton in _pauseButtons)
		{
			pauseButton.image.sprite = _themes[_currentTheme].PauseButton;

			_temporaryColorBlock = pauseButton.colors;
			_temporaryColorBlock.disabledColor = _themes[_currentTheme].DisabledButtonColor;
			pauseButton.colors = _temporaryColorBlock;
		}
	}
	#endregion
}
