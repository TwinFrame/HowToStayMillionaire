using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PropertiesTabOnServer : BaseTab
{
	[Header("Properties Buttons")]
	[SerializeField] private Button _displayButton;
	[SerializeField] private Button _textsButton;
	[SerializeField] private Button _colorsButton;
	[SerializeField] private Button _loadImageButton;
	[Header("Windows")]
	[SerializeField] private MenuDisplayWindow _gameDisplayWindow;
	[SerializeField] private MenuGameTextsWindows _gameTextsWindows;
	[SerializeField] private MenuColorsWindow _colorsWindow;
	[SerializeField] private MenuLoadImageWindow _loadImageWindow;
	[Header("Panels")]
	[SerializeField] private MenuAudioMixer _audioMixerUI;
	[SerializeField] private ServerConnectMenu _serverConnectMenu;
	[SerializeField] private Button _serverButton;

	public UnityAction<TypesOfAudioChannel, float> OnChangedAudioChannelEvent;
	public UnityAction<TypesOfAudioChannel> OnChannelDoubleClickedEvent;

	public UnityAction<int> OnChangedPaletteEvent;
	public UnityAction<int, TypesOfGameColor, Color> OnChangedColorInPaletteEvent;

	public UnityAction OnRefreshGameDisplayInfoEvent;
	public UnityAction<int, int> OnChangedGameDisplayResolutionEvent;
	public UnityAction<bool> OnSwitchedGameDisplayFullscreenEvent;

	public UnityAction OnRefreshGameTextsEvent;
	public UnityAction<char> OnChangedCurrentMonetaryUnitEvent;
	public UnityAction<string, string> OnChangedGameTextsEvent;

	public UnityAction<Texture2D, string> OnUserLoadedLogoEvent;
	public UnityAction OnDeleteLogoEvent;

	public UnityAction<string> OnServerButtonEvent;

	public UnityAction OnBlockHotkeyEvent;
	public UnityAction OnUnblockHotkeyEvent;

	private void OnEnable()
	{
		_displayButton.onClick.AddListener(OnOpenGameDisplayWindow);
		_gameDisplayWindow.OnCloseWindowEvent += OnCloseGameDisplayWindow;
		_gameDisplayWindow.OnRefreshDisplayInfoEvent += OnRefreshGameDisplayInfo;
		_gameDisplayWindow.OnChangedResolutionEvent += OnChangedResolution;
		_gameDisplayWindow.OnSwitchedFullscreenEvent += (isFullscreen) => OnSwitchedFullscreen(isFullscreen);

		_textsButton.onClick.AddListener(OnOpenTextsWindow);
		_gameTextsWindows.OnCloseWindowEvent += OnCloseTextsWindow;
		_gameTextsWindows.OnChangeCurrentSymbolEvent += (symbol) => OnChangeCurrentSymbol(symbol);
		_gameTextsWindows.OnChangedGameTextsEvent += (name, finalText) => OnChangedGameTexts(name, finalText);

		_loadImageButton.onClick.AddListener(OnOpenLoadImageWindow);
		_loadImageWindow.OnCloseLoadImageWindowEvent += OnCloseLoadImageWindow;
		_loadImageWindow.OnUserLoadedLogoEvent += (logo, path) => OnUserLoadedLogo(logo, path);
		_loadImageWindow.OnDeleteLogoEvent += OnDeleteLogo;

		_colorsButton.onClick.AddListener(OnOpenColorsWindow);
		_colorsWindow.OnCloseWindowEvent += OnCloseColorsWindow;
		_colorsWindow.OnChangedPaletteEvent += (numPalette) => OnChangedPalette(numPalette);
		_colorsWindow.OnChangedColorInPaletteEvent += (numPalette, gameColor, color) => OnChangedColorInPalette(numPalette, gameColor, color);
		_colorsWindow.OnBlockHotkeyEvent += BlockHotkey;
		_colorsWindow.OnUnblockHotkeyEvent += UnblockHotkey;

		_audioMixerUI.OnChangedAudioChannelEvent += (channel, normalizeValue) => OnChangedAudioChannel(channel, normalizeValue);
		_audioMixerUI.OnChannelDoubleClickedEvent += (channel) => OnChannelDoubleClicked(channel);

		_serverButton.onClick.AddListener(OnClickServerButton);
		_serverConnectMenu.OnBlockHotkeyEvent += BlockHotkey;
		_serverConnectMenu.OnUnblockHotkeyEvent += UnblockHotkey;
	}

	private void OnDisable()
	{
		_displayButton.onClick.RemoveListener(OnOpenGameDisplayWindow);
		_gameDisplayWindow.OnCloseWindowEvent -= OnCloseGameDisplayWindow;
		_gameDisplayWindow.OnRefreshDisplayInfoEvent -= OnRefreshGameDisplayInfo;
		_gameDisplayWindow.OnChangedResolutionEvent -= OnChangedResolution;
		_gameDisplayWindow.OnSwitchedFullscreenEvent -= (isFullscreen) => OnSwitchedFullscreen(isFullscreen);

		_textsButton.onClick.RemoveListener(OnOpenTextsWindow);
		_gameTextsWindows.OnCloseWindowEvent -= OnCloseTextsWindow;
		_gameTextsWindows.OnChangeCurrentSymbolEvent -= (symbol) => OnChangeCurrentSymbol(symbol);
		_gameTextsWindows.OnChangedGameTextsEvent -= (name, finalText) => OnChangedGameTexts(name, finalText);

		_loadImageButton.onClick.RemoveListener(OnOpenLoadImageWindow);
		_loadImageWindow.OnCloseLoadImageWindowEvent -= OnCloseLoadImageWindow;
		_loadImageWindow.OnUserLoadedLogoEvent -= (logo, path) => OnUserLoadedLogo(logo, path);
		_loadImageWindow.OnDeleteLogoEvent -= OnDeleteLogo;

		_colorsButton.onClick.RemoveListener(OnOpenColorsWindow);
		_colorsWindow.OnCloseWindowEvent -= OnCloseColorsWindow;
		_colorsWindow.OnChangedPaletteEvent -= (numPalette) => OnChangedPalette(numPalette);
		_colorsWindow.OnChangedColorInPaletteEvent -= (numPalette, gameColor, color) => OnChangedColorInPalette(numPalette, gameColor, color);
		_colorsWindow.OnBlockHotkeyEvent -= BlockHotkey;
		_colorsWindow.OnUnblockHotkeyEvent -= UnblockHotkey;

		_audioMixerUI.OnChangedAudioChannelEvent -= (channel, normalizeValue) => OnChangedAudioChannel(channel, normalizeValue);
		_audioMixerUI.OnChannelDoubleClickedEvent -= (channel) => OnChannelDoubleClicked(channel);

		_serverButton.onClick.RemoveListener(OnClickServerButton);
		_serverConnectMenu.OnBlockHotkeyEvent -= BlockHotkey;
		_serverConnectMenu.OnUnblockHotkeyEvent -= UnblockHotkey;
	}

	public override void RefreshTab()
	{
		OnCloseGameDisplayWindow();
		OnCloseTextsWindow();
		OnCloseLoadImageWindow();
		OnCloseColorsWindow();

		//_startCountdownButton.interactable = false;
		/*
		_sliderMaser;
		_sliderFx;
		_sliderCountdown;
		_symbolDropdown;
		*/
	}

	public override bool[] GetInteractables()
	{
		return new bool[0];
	}

	public override void SetTabInteractables(bool[] isInteractables)
	{
		throw new System.NotImplementedException();
	}

	private void BlockHotkey()
	{
		OnBlockHotkeyEvent?.Invoke();
	}

	private void UnblockHotkey()
	{
		OnUnblockHotkeyEvent?.Invoke();
	}

	private void OnChangeCurrentSymbol(char symbol)
	{
		OnChangedCurrentMonetaryUnitEvent?.Invoke(symbol);
	}

	#region ServerMenu

	public void OnClickConnectFromHeader()
	{
		_serverConnectMenu.EnterHintData();

		OnClickServerButton();
	}

	public void SetSpriteOnConnectButton(Sprite sprite)
	{
		_serverConnectMenu.SetSpriteOnConnectButton(sprite);
	}

	private void OnClickServerButton()
	{
		OnServerButtonEvent?.Invoke(_serverConnectMenu.GetCurrentPort());
	}

	#endregion

	#region GameDisplayWindow

	private void OnOpenGameDisplayWindow()
	{
		BlockHotkey();

		OnRefreshGameDisplayInfo();

		_gameDisplayWindow.gameObject.SetActive(true);
	}

	private void OnCloseGameDisplayWindow()
	{
		UnblockHotkey();

		_gameDisplayWindow.gameObject.SetActive(false);
	}

	private void OnRefreshGameDisplayInfo()
	{
		OnRefreshGameDisplayInfoEvent?.Invoke();
	}

	public void RefreshGameDisplayInfoInMenu(string displayInfo, bool isFullscreen)
	{
		_gameDisplayWindow.SetDisplayInfo(displayInfo, isFullscreen);
	}

	private void OnChangedResolution(int width, int height)
	{
		OnChangedGameDisplayResolutionEvent?.Invoke(width, height);
	}

	private void OnSwitchedFullscreen(bool isFullscreen)
	{
		OnSwitchedGameDisplayFullscreenEvent?.Invoke(isFullscreen);
	}

	#endregion

	#region GameTextsWindows

	private void OnOpenTextsWindow()
	{
		BlockHotkey();

		OnRefreshGameTexts();

		_gameTextsWindows.gameObject.SetActive(true);
	}

	private void OnCloseTextsWindow()
	{
		UnblockHotkey();

		_gameTextsWindows.gameObject.SetActive(false);
	}

	private void OnRefreshGameTexts()
	{
		OnRefreshGameTextsEvent?.Invoke();
	}

	private void OnChangedGameTexts(string name, string finalText)
	{
		OnChangedGameTextsEvent?.Invoke(name, finalText);

		OnCloseTextsWindow();
	}

	public void SetGameTexts(string name, string finalText, char[] monetaryUnits)
	{
		_gameTextsWindows.SetGameTexts(name, finalText, monetaryUnits);
	}

	#endregion

	#region LoadImageWindow

	public void ChangeLogoInfo(Texture2D logo, string path)
	{
		_loadImageWindow.ChangeLogoInfo(logo, path);
	}

	public bool TryGetTextureFromLocalPath(string path, out Texture2D texture)
	{
		if (_loadImageWindow.TryGetTextureFromLocalPath(path, out Texture2D localTexture))
		{
			texture = localTexture;
			return true;
		}
		else
		{
			texture = null;
			return false;
		}
	}

	private void OnDeleteLogo()
	{
		OnDeleteLogoEvent?.Invoke();

		//OnCloseLoadImageWindow();
	}

	private void OnUserLoadedLogo(Texture2D logo, string path)
	{
		OnUserLoadedLogoEvent?.Invoke(logo, path);

		//OnCloseLoadImageWindow();
	}

	private void OnOpenLoadImageWindow()
	{
		BlockHotkey();

		_loadImageWindow.gameObject.SetActive(true);
	}

	private void OnCloseLoadImageWindow()
	{
		UnblockHotkey();

		_loadImageWindow.gameObject.SetActive(false);
	}

	#endregion

	#region ColorsWindow

	private void OnOpenColorsWindow()
	{
		BlockHotkey();

		_colorsWindow.gameObject.SetActive(true);
	}

	private void OnCloseColorsWindow()
	{
		UnblockHotkey();

		_colorsWindow.gameObject.SetActive(false);
	}

	//_colors.RefreshPalettes остался только на клиентн и по логике должен автоматом остаться без линка
	public void RefreshPalettes(List<ColorPalette> palettes, List<string> paletteNames, int currentNumPalette)
	{
		_colorsWindow.RefreshPalettesInMenu(palettes, currentNumPalette);
	}

	public void SetColorsFromPalette(List<ColorPalette> colorPalettes, int numPalette)
	{
		_colorsWindow.SetColorsFromPalette(colorPalettes, numPalette);
	}

	private void OnChangedPalette(int numPalette)
	{
		OnChangedPaletteEvent?.Invoke(numPalette);
	}

	private void OnChangedColorInPalette(int numPalette, TypesOfGameColor gameColor, Color color)
	{
		OnChangedColorInPaletteEvent?.Invoke(numPalette, gameColor, color);
	}

	#endregion

	#region Audio Sliders
	private void OnChangedAudioChannel(TypesOfAudioChannel channel, float normalizeValue)
	{
		OnChangedAudioChannelEvent?.Invoke(channel, normalizeValue);
	}

	private void OnChannelDoubleClicked(TypesOfAudioChannel channel)
	{
		OnChannelDoubleClickedEvent?.Invoke(channel);
	}

	public void UpdateVolumeChannel(float normalizeMasterVolume, float normalizeFxVolume,
		float normalizeCountdownVolume, float normalizeQuestionVolume, float normalizeMusicVolume)
	{
		SetChannel(TypesOfAudioChannel.Master, normalizeMasterVolume);
		SetChannel(TypesOfAudioChannel.Fx, normalizeFxVolume);
		SetChannel(TypesOfAudioChannel.Countown, normalizeCountdownVolume);
		SetChannel(TypesOfAudioChannel.Question, normalizeQuestionVolume);
		SetChannel(TypesOfAudioChannel.Music, normalizeMusicVolume);
	}

	public void SetChannel(TypesOfAudioChannel channel, float normalizedVolume)
	{
		_audioMixerUI.SetChannelWithoutNotify(channel, normalizedVolume);
	}
	#endregion
}
