using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemeMenu : MonoBehaviour
{
	[Header("Button")]
	[SerializeField] private Sprite _button;
	[SerializeField] private Sprite _selectedButton;
	[SerializeField] private Sprite _redButton;
	[SerializeField] private Sprite _yellowButton;
	[SerializeField] private Sprite _greenButton;
	[SerializeField] private Color _disabledButtonColor;
	[Header("Panel")]
	[SerializeField] private Sprite _panel;
	[Header("Quit Button")]
	[SerializeField] private Sprite _quitButton;
	[Header("Player Buttons")]
	[SerializeField] private Sprite _playUntilMarkButton;
	[SerializeField] private Sprite _playAfterMarkButton;
	[SerializeField] private Sprite _playFullButton;
	[SerializeField] private Sprite _pauseButton;
	[Header("InputField")]
	[SerializeField] private Sprite _inputField;
	[SerializeField] private Color _inputFieldSelectionColor;
	[Range(0, 10)]
	[SerializeField] private int _caretWitdh = 7;
	[SerializeField] private Color _inputFieldCaretColor;
	[Header("Toggle")]
	[SerializeField] private Sprite _chekmarkInToggle;
	[Header("Dropdown")]
	[SerializeField] private Sprite _arrow;
	[SerializeField] private Sprite _checkmark;
	[Header("Slider")]
	[SerializeField] private Sprite _sliderBG;
	[SerializeField] private Sprite _sliderFillArea;
	[SerializeField] private Sprite _sliderHandle;
	[Header("AudioMixer")]
	[SerializeField] private Sprite _audioLine;
	[Space]
	//[Header("Menu Background Texture")]
	//[SerializeField] private Texture _backgroundTexture;

	[Header("Fonts")]
	[SerializeField] private Color _textColor;


	public Sprite Button => _button;
	public Sprite SelectedButton => _selectedButton;
	public Sprite RedButton => _redButton;
	public Sprite YellowButton => _yellowButton;
	public Sprite GreenButton => _greenButton;
	public Color DisabledButtonColor => _disabledButtonColor;

	public Sprite Panel => _panel;

	public Sprite QuitButton => _quitButton;

	public Sprite PlayUntilMarkButton => _playUntilMarkButton;
	public Sprite PlayAfterMarkButton => _playAfterMarkButton;
	public Sprite PlayFullButton => _playFullButton;
	public Sprite PauseButton => _pauseButton;


	public Sprite InputField => _inputField;
	public Color InputFieldSelectionColor => _inputFieldSelectionColor;
	public int CaretWitdh => _caretWitdh;
	public Color InputFieldCaretColor => _inputFieldCaretColor;

	public Sprite ChekmarkInToggle => _chekmarkInToggle;

	public Sprite Arrow => _arrow;
	public Sprite Checkmark => _checkmark;

	public Sprite SliderBG => _sliderBG;
	public Sprite SliderFillArea => _sliderFillArea;
	public Sprite SliderHandle => _sliderHandle;

	public Sprite AudioLine => _audioLine;


	//public Texture BackgroundTexture => _backgroundTexture;

	public Color TextColor => _textColor;
}
