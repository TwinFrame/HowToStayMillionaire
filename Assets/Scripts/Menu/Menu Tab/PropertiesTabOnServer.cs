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

	public UnityAction<TypesOfAudioChannel, float> ChangedAudioChannelEvent;
	public UnityAction<TypesOfAudioChannel> ChannelDoubleClickedEvent;

	public UnityAction<int> ChangedPaletteEvent;
	public UnityAction<int, TypesOfGameColor, Color> ChangedColorInPaletteEvent;

	public UnityAction RefreshedGameDisplayInfoEvent;
	public UnityAction<int, int> ChangedGameDisplayResolutionEvent;
	public UnityAction<bool> SwitchedGameDisplayFullscreenEvent;

	public UnityAction RefreshedGameTextsEvent;
	public UnityAction<char> ChangedCurrentMonetaryUnitEvent;
	public UnityAction<string, string> ChangedGameTextsEvent;

	public UnityAction<Texture2D, string> UserLoadedLogoEvent;
	public UnityAction DeletedLogoEvent;

	public UnityAction<string> ServerButtonEvent;

	public UnityAction BlockedHotkeyEvent;
	public UnityAction UnblockedHotkeyEvent;

	private void OnEnable()
	{
		_displayButton.onClick.AddListener(OnOpenGameDisplayWindow);
		_gameDisplayWindow.ClosedWindowEvent += OnCloseGameDisplayWindow;
		_gameDisplayWindow.RefreshedDisplayInfoEvent += OnRefreshGameDisplayInfo;
		_gameDisplayWindow.ChangedResolutionEvent += OnChangedResolution;
		_gameDisplayWindow.SwitchedFullscreenEvent += (isFullscreen) => OnSwitchedFullscreen(isFullscreen);

		_textsButton.onClick.AddListener(OnOpenTextsWindow);
		_gameTextsWindows.ClosedWindowEvent += OnCloseTextsWindow;
		_gameTextsWindows.ChangedCurrentSymbolEvent += (symbol) => OnChangeCurrentSymbol(symbol);
		_gameTextsWindows.ChangedGameTextsEvent += (name, finalText) => OnChangedGameTexts(name, finalText);
		_gameTextsWindows.BlockedHotkeyEvent += BlockHotkey;
		_gameTextsWindows.UnblockedHotkeyEvent += UnblockHotkey;

		_loadImageButton.onClick.AddListener(OnOpenLoadImageWindow);
		_loadImageWindow.ClosedLoadImageWindowEvent += OnCloseLoadImageWindow;
		_loadImageWindow.UserLoadedLogoEvent += (logo, path) => OnUserLoadedLogo(logo, path);
		_loadImageWindow.DeletedLogoEvent += OnDeleteLogo;

		_colorsButton.onClick.AddListener(OnOpenColorsWindow);
		_colorsWindow.ClosedWindowEvent += OnCloseColorsWindow;
		_colorsWindow.ChangedPaletteEvent += (numPalette) => OnChangedPalette(numPalette);
		_colorsWindow.ChangedColorInPaletteEvent += (numPalette, gameColor, color) => OnChangedColorInPalette(numPalette, gameColor, color);
		_colorsWindow.BlockedHotkeyEvent += BlockHotkey;
		_colorsWindow.UnblockedHotkeyEvent += UnblockHotkey;

		_audioMixerUI.ChangedAudioChannelEvent += (channel, normalizeValue) => OnChangedAudioChannel(channel, normalizeValue);
		_audioMixerUI.ChannelDoubleClickedEvent += (channel) => OnChannelDoubleClicked(channel);

		_serverButton.onClick.AddListener(OnClickServerButton);
		_serverConnectMenu.BlockedHotkeyEvent += BlockHotkey;
		_serverConnectMenu.UnblockedHotkeyEvent += UnblockHotkey;
	}

	private void OnDisable()
	{
		_displayButton.onClick.RemoveListener(OnOpenGameDisplayWindow);
		_gameDisplayWindow.ClosedWindowEvent -= OnCloseGameDisplayWindow;
		_gameDisplayWindow.RefreshedDisplayInfoEvent -= OnRefreshGameDisplayInfo;
		_gameDisplayWindow.ChangedResolutionEvent -= OnChangedResolution;
		_gameDisplayWindow.SwitchedFullscreenEvent -= (isFullscreen) => OnSwitchedFullscreen(isFullscreen);

		_textsButton.onClick.RemoveListener(OnOpenTextsWindow);
		_gameTextsWindows.ClosedWindowEvent -= OnCloseTextsWindow;
		_gameTextsWindows.ChangedCurrentSymbolEvent -= (symbol) => OnChangeCurrentSymbol(symbol);
		_gameTextsWindows.ChangedGameTextsEvent -= (name, finalText) => OnChangedGameTexts(name, finalText);
		_gameTextsWindows.BlockedHotkeyEvent -= BlockHotkey;
		_gameTextsWindows.UnblockedHotkeyEvent -= UnblockHotkey;

		_loadImageButton.onClick.RemoveListener(OnOpenLoadImageWindow);
		_loadImageWindow.ClosedLoadImageWindowEvent -= OnCloseLoadImageWindow;
		_loadImageWindow.UserLoadedLogoEvent -= (logo, path) => OnUserLoadedLogo(logo, path);
		_loadImageWindow.DeletedLogoEvent -= OnDeleteLogo;

		_colorsButton.onClick.RemoveListener(OnOpenColorsWindow);
		_colorsWindow.ClosedWindowEvent -= OnCloseColorsWindow;
		_colorsWindow.ChangedPaletteEvent -= (numPalette) => OnChangedPalette(numPalette);
		_colorsWindow.ChangedColorInPaletteEvent -= (numPalette, gameColor, color) => OnChangedColorInPalette(numPalette, gameColor, color);
		_colorsWindow.BlockedHotkeyEvent -= BlockHotkey;
		_colorsWindow.UnblockedHotkeyEvent -= UnblockHotkey;

		_audioMixerUI.ChangedAudioChannelEvent -= (channel, normalizeValue) => OnChangedAudioChannel(channel, normalizeValue);
		_audioMixerUI.ChannelDoubleClickedEvent -= (channel) => OnChannelDoubleClicked(channel);

		_serverButton.onClick.RemoveListener(OnClickServerButton);
		_serverConnectMenu.BlockedHotkeyEvent -= BlockHotkey;
		_serverConnectMenu.UnblockedHotkeyEvent -= UnblockHotkey;
	}

	public override void RefreshTab()
	{
		OnCloseGameDisplayWindow();
		OnCloseTextsWindow();
		OnCloseLoadImageWindow();
		OnCloseColorsWindow();
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
		BlockedHotkeyEvent?.Invoke();
	}

	private void UnblockHotkey()
	{
		UnblockedHotkeyEvent?.Invoke();
	}

	private void OnChangeCurrentSymbol(char symbol)
	{
		ChangedCurrentMonetaryUnitEvent?.Invoke(symbol);
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
		ServerButtonEvent?.Invoke(_serverConnectMenu.GetCurrentPort());
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
		RefreshedGameDisplayInfoEvent?.Invoke();
	}

	public void RefreshGameDisplayInfoInMenu(string displayInfo, bool isFullscreen)
	{
		_gameDisplayWindow.SetDisplayInfo(displayInfo, isFullscreen);
	}

	private void OnChangedResolution(int width, int height)
	{
		ChangedGameDisplayResolutionEvent?.Invoke(width, height);
	}

	private void OnSwitchedFullscreen(bool isFullscreen)
	{
		SwitchedGameDisplayFullscreenEvent?.Invoke(isFullscreen);
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
		RefreshedGameTextsEvent?.Invoke();
	}

	private void OnChangedGameTexts(string name, string finalText)
	{
		ChangedGameTextsEvent?.Invoke(name, finalText);

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
		DeletedLogoEvent?.Invoke();
	}

	private void OnUserLoadedLogo(Texture2D logo, string path)
	{
		UserLoadedLogoEvent?.Invoke(logo, path);
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

	public void SetColorsFromPalette(List<ColorPalette> colorPalettes, int numPalette)
	{
		_colorsWindow.SetColorsFromPalette(colorPalettes, numPalette);
	}

	private void OnChangedPalette(int numPalette)
	{
		ChangedPaletteEvent?.Invoke(numPalette);
	}

	private void OnChangedColorInPalette(int numPalette, TypesOfGameColor gameColor, Color color)
	{
		ChangedColorInPaletteEvent?.Invoke(numPalette, gameColor, color);
	}

	#endregion

	#region Audio Sliders

	private void OnChangedAudioChannel(TypesOfAudioChannel channel, float normalizeValue)
	{
		ChangedAudioChannelEvent?.Invoke(channel, normalizeValue);
	}

	private void OnChannelDoubleClicked(TypesOfAudioChannel channel)
	{
		ChannelDoubleClickedEvent?.Invoke(channel);
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
