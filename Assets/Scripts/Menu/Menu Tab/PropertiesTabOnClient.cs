using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PropertiesTabOnClient : BaseTab
{
	[Header("Properties Buttons")]
	[SerializeField] private Button _menuDisplayButton;
	[SerializeField] private Button _gameDisplayButton;
	[SerializeField] private Button _textsButton;
	[SerializeField] private Button _colorsButton;
	[SerializeField] private Button _loadImageButton;
	[Header("Windows")]
	[SerializeField] private MenuDisplayWindow _gameDisplayWindow;
	[SerializeField] private MenuDisplayWindow _menuDisplayWindow;
	[SerializeField] private MenuGameTextsWindows _gameTextsWindows;
	[SerializeField] private MenuColorsWindow _colorsWindow;
	[SerializeField] private MenuLoadImageWindow _loadImageWindow;
	[Header("Panels")]
	[SerializeField] private MenuAudioMixer _audioMixerUI;
	[SerializeField] private ClientConnectMenu _clientConnectMenu;
	[SerializeField] private Button _clientButton;

	public UnityAction<TypesOfAudioChannel, float> ChangedAudioChannelEvent;
	public UnityAction<TypesOfAudioChannel> ChannelDoubleClickedEvent;

	public UnityAction RefreshedGameDisplayInfoEvent;
	public UnityAction<int, int> ChangedGameDisplayResolutionEvent;
	public UnityAction<bool> SwitchedGameDisplayFullscreenEvent;

	public UnityAction RefreshedMenuDisplayInfoEvent;
	public UnityAction<int, int> ChangedMenuDisplayResolutionEvent;
	public UnityAction<bool> SwitchedMenuDisplayFullscreenEvent;

	public UnityAction RefreshedGameTextsEvent;
	public UnityAction<char> ChangedCurrentMonetaryUnitEvent;
	public UnityAction<string, string> ChangedGameTextsEvent;

	public UnityAction RefreshedPalettesEvent;
	public UnityAction<int> ChangedPaletteEvent;
	public UnityAction<int, TypesOfGameColor, Color> ChangedColorInPaletteEvent;

	public UnityAction RefreshedLogoEvent;
	public UnityAction<Texture2D, string> UserLoadedLogoEvent;
	public UnityAction DeletedLogoEvent;

	public UnityAction<string, string> ClientButtonEvent;

	public UnityAction BlockedHotkeyEvent;
	public UnityAction UnblockedHotkeyEvent;

	private void OnEnable()
	{
		_gameDisplayButton.onClick.AddListener(OnOpenGameDisplayWindow);
		_gameDisplayWindow.ClosedWindowEvent += OnCloseGameDisplayWindow;
		_gameDisplayWindow.RefreshedDisplayInfoEvent += OnRefreshGameDisplayInfo;
		_gameDisplayWindow.ChangedResolutionEvent += OnChangedGameDisplayResolution;
		_gameDisplayWindow.SwitchedFullscreenEvent += (isFullscreen) => OnSwitchedGameDisplayFullscreen(isFullscreen);

		_menuDisplayButton.onClick.AddListener(OnOpenMenuDisplayWindow);
		_menuDisplayWindow.ClosedWindowEvent += OnCloseMenuDisplayWindow;
		_menuDisplayWindow.RefreshedDisplayInfoEvent += OnRefreshMenuDisplayInfo;
		_menuDisplayWindow.ChangedResolutionEvent += OnChangedResolutionOnMenuDisplay;
		_menuDisplayWindow.SwitchedFullscreenEvent += (isFullscreen) => OnSwitchedFullscreenOnMenuDisplay(isFullscreen);

		_textsButton.onClick.AddListener(OnOpenTextsWindow);
		_gameTextsWindows.ClosedWindowEvent += OnCloseTextsWindow;
		_gameTextsWindows.ChangedCurrentSymbolEvent += (symbol) => OnChangeCurrentSymbol(symbol);
		_gameTextsWindows.ChangedGameTextsEvent += (name, finalText) => OnChangedGameTexts(name, finalText);
		_gameTextsWindows.BlockedHotkeyEvent += BlockHotkey;
		_gameTextsWindows.UnblockedHotkeyEvent += UnblockHotkey;

		_colorsButton.onClick.AddListener(OnOpenColorsWindow);
		_colorsWindow.ClosedWindowEvent += OnCloseColorsWindow;
		_colorsWindow.ChangedPaletteEvent += (numPalette) => OnChangedPalette(numPalette);
		_colorsWindow.ChangedColorInPaletteEvent += (numPalette, gameColor, color) => OnChangedColorInPalette(numPalette, gameColor, color);
		_colorsWindow.BlockedHotkeyEvent += BlockHotkey;
		_colorsWindow.UnblockedHotkeyEvent += UnblockHotkey;

		_loadImageButton.onClick.AddListener(OnOpenLoadImageWindow);
		_loadImageWindow.ClosedLoadImageWindowEvent += OnCloseLoadImageWindow;
		_loadImageWindow.UserLoadedLogoEvent += (logo, path) => OnUserLoadedLogo(logo, path);
		_loadImageWindow.DeletedLogoEvent += OnDeleteLogo;

		_audioMixerUI.ChangedAudioChannelEvent += (channel, normalizeValue) => OnChangedAudioChannel(channel, normalizeValue);
		_audioMixerUI.ChannelDoubleClickedEvent += (channel) => OnChannelDoubleClicked(channel);

		_clientButton.onClick.AddListener(OnClickClientButton);
		_clientConnectMenu.BlockedHotkeyEvent += BlockHotkey;
		_clientConnectMenu.UnblockedHotkeyEvent += UnblockHotkey;
	}

	private void OnDisable()
	{
		_gameDisplayButton.onClick.RemoveListener(OnOpenGameDisplayWindow);
		_gameDisplayWindow.ClosedWindowEvent -= OnCloseGameDisplayWindow;
		_gameDisplayWindow.RefreshedDisplayInfoEvent -= OnRefreshGameDisplayInfo;
		_gameDisplayWindow.ChangedResolutionEvent -= OnChangedGameDisplayResolution;
		_gameDisplayWindow.SwitchedFullscreenEvent -= (isFullscreen) => OnSwitchedGameDisplayFullscreen(isFullscreen);

		_menuDisplayButton.onClick.RemoveListener(OnOpenMenuDisplayWindow);
		_menuDisplayWindow.ClosedWindowEvent -= OnCloseMenuDisplayWindow;
		_menuDisplayWindow.RefreshedDisplayInfoEvent -= OnRefreshMenuDisplayInfo;
		_menuDisplayWindow.ChangedResolutionEvent -= OnChangedResolutionOnMenuDisplay;
		_menuDisplayWindow.SwitchedFullscreenEvent -= (isFullscreen) => OnSwitchedFullscreenOnMenuDisplay(isFullscreen);

		_textsButton.onClick.RemoveListener(OnOpenTextsWindow);
		_gameTextsWindows.ClosedWindowEvent -= OnCloseTextsWindow;
		_gameTextsWindows.ChangedCurrentSymbolEvent -= (symbol) => OnChangeCurrentSymbol(symbol);
		_gameTextsWindows.ChangedGameTextsEvent -= (name, finalText) => OnChangedGameTexts(name, finalText);
		_gameTextsWindows.BlockedHotkeyEvent -= BlockHotkey;
		_gameTextsWindows.UnblockedHotkeyEvent -= UnblockHotkey;

		_colorsButton.onClick.RemoveListener(OnOpenColorsWindow);
		_colorsWindow.ClosedWindowEvent -= OnCloseColorsWindow;
		_colorsWindow.ChangedPaletteEvent -= (numPalette) => OnChangedPalette(numPalette);
		_colorsWindow.ChangedColorInPaletteEvent -= (numPalette, gameColor, color) => OnChangedColorInPalette(numPalette, gameColor, color);
		_colorsWindow.BlockedHotkeyEvent -= BlockHotkey;
		_colorsWindow.UnblockedHotkeyEvent -= UnblockHotkey;

		_loadImageButton.onClick.RemoveListener(OnOpenLoadImageWindow);
		_loadImageWindow.ClosedLoadImageWindowEvent -= OnCloseLoadImageWindow;
		_loadImageWindow.UserLoadedLogoEvent -= (logo, path) => OnUserLoadedLogo(logo, path);
		_loadImageWindow.DeletedLogoEvent -= OnDeleteLogo;

		_audioMixerUI.ChangedAudioChannelEvent -= (channel, normalizeValue) => OnChangedAudioChannel(channel, normalizeValue);
		_audioMixerUI.ChannelDoubleClickedEvent -= (channel) => OnChannelDoubleClicked(channel);

		_clientButton.onClick.RemoveListener(OnClickClientButton);
		_clientConnectMenu.BlockedHotkeyEvent -= BlockHotkey;
		_clientConnectMenu.UnblockedHotkeyEvent -= UnblockHotkey;
	}

	public override void RefreshTab()
	{
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
		_clientConnectMenu.EnterHintData();

		OnClickClientButton();
	}

	public void SetSpriteOnConnectButton(Sprite sprite)
	{
		_clientConnectMenu.SetSpriteOnConnectButton(sprite);
	}

	private void OnClickClientButton()
	{
		ClientButtonEvent?.Invoke(_clientConnectMenu.GetCurrentIP(), _clientConnectMenu.GetCurrentPort());
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

	private void OnChangedGameDisplayResolution(int width, int height)
	{
		ChangedGameDisplayResolutionEvent?.Invoke(width, height);
	}

	private void OnSwitchedGameDisplayFullscreen(bool isFullscreen)
	{
		SwitchedGameDisplayFullscreenEvent?.Invoke(isFullscreen);
	}

	#endregion

	#region MenuDisplayWindow

	private void OnOpenMenuDisplayWindow()
	{
		BlockHotkey();

		OnRefreshMenuDisplayInfo();

		_menuDisplayWindow.gameObject.SetActive(true);
	}

	private void OnCloseMenuDisplayWindow()
	{
		UnblockHotkey();

		_menuDisplayWindow.gameObject.SetActive(false);
	}

	private void OnRefreshMenuDisplayInfo()
	{
		RefreshedMenuDisplayInfoEvent?.Invoke();
	}

	public void RefreshMenuDisplayInfoInMenu(string displayInfo, bool isFullscreen)
	{
		_menuDisplayWindow.SetDisplayInfo(displayInfo, isFullscreen);
	}

	private void OnChangedResolutionOnMenuDisplay(int width, int height)
	{
		ChangedMenuDisplayResolutionEvent?.Invoke(width, height);
	}

	private void OnSwitchedFullscreenOnMenuDisplay(bool isFullscreen)
	{
		SwitchedMenuDisplayFullscreenEvent?.Invoke(isFullscreen);
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

		OnRefreshLoadImage();

		_loadImageWindow.gameObject.SetActive(true);
	}

	private void OnCloseLoadImageWindow()
	{
		UnblockHotkey();

		_loadImageWindow.gameObject.SetActive(false);
	}
	private void OnRefreshLoadImage()
	{
		RefreshedLogoEvent?.Invoke();
	}

	#endregion

	#region ColorsWindow

	public void SetColorsFromPalette(List<ColorPalette> colorPalettes, int numPalette)
	{
		_colorsWindow.SetColorsFromPalette(colorPalettes, numPalette);
	}

	private void OnOpenColorsWindow()
	{
		BlockHotkey();

		OnRefreshPalettes();

		_colorsWindow.gameObject.SetActive(true);
	}

	private void OnCloseColorsWindow()
	{
		UnblockHotkey();

		_colorsWindow.gameObject.SetActive(false);
	}

	private void OnChangedPalette(int numPalette)
	{
		ChangedPaletteEvent?.Invoke(numPalette);
	}

	private void OnChangedColorInPalette(int numPalette, TypesOfGameColor gameColor, Color color)
	{
		ChangedColorInPaletteEvent?.Invoke(numPalette, gameColor, color);
	}

	private void OnRefreshPalettes()
	{
		RefreshedPalettesEvent?.Invoke();
	}

	#endregion

	#region Audio Sliders

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

	private void OnChangedAudioChannel(TypesOfAudioChannel channel, float normalizeValue)
	{
		ChangedAudioChannelEvent?.Invoke(channel, normalizeValue);
	}

	private void OnChannelDoubleClicked(TypesOfAudioChannel channel)
	{
		ChannelDoubleClickedEvent?.Invoke(channel);
	}
	#endregion
}
