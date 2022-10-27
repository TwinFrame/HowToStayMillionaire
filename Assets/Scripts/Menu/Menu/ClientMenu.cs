using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Networking.Transport;
using ARKitStream.Internal;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Canvas))]

public class ClientMenu : BaseMenu
{
	[Space]
	[SerializeField] private ClientPreviewMenu _previewMenu;
	[Header("Service")]
	[SerializeField] private GameClient _gameClient;
	//[SerializeField] private DisplayController _clientDisplaySettings;
	//[SerializeField] private MenuThemeChanger _menuThemeChanger;
	[SerializeField] private ClientProperties _properties;

	private byte[] _currentRawTextureBytes;
	private Texture2D _currentTexture2D;

	private void OnEnable()
	{
		base.OnEnable();

		RegisterClientEvents();

		_properties.DisplayController.OnRefreshDisplayInfoEvent += (displayInfo, isFullscreen) => OnRefreshMenuDisplayInfoInMenu(displayInfo, isFullscreen);

		//Client
		_gameClient.Client.ActivateEvent += OnActivateClient;
		_gameClient.Client.ConnectionDroppedEvent += OnDroppedClientConnection;
		_gameClient.Client.ConnectedEvent += ConnectedClient;
		_gameClient.Client.ShutdownEvent += OnShutdownClient;

		//PreviewMenu
		_previewMenu.OnDoubleClickedPreviewEvent += OnDoubleClickedPreview;
		_previewMenu.OnRequestPreviewTextureEvent += (width, height) => OnRequestPreviewTexture(width, height);
		//_previewMenu.OnChangedPreviewQualityEvent += (normalizeValue) => OnChangedPreviewQuality(normalizeValue);
		//_previewMenu.OnChangedPreviewFPSEvent += (value) => OnChangedPreviewFPS(value);

		foreach (var button in TabButtons)
		{
			if (button.TryGetComponent<Button>(out Button buttonWithEvent))
				buttonWithEvent.onClick.AddListener(() => ClickMenuButton(button.TypesMenuReadOnly));
		}

		foreach (var tab in Tabs)
		{
			switch (tab.TypesMenuReadOnly)
			{
				case TypesOfTab.Game:
					GameTab gameTab = tab as GameTab;

					gameTab.NextTitleButtonEvent += OnNextTitleButton;
					gameTab.TeamsTitleButtonEvent += OnTeamsTitleButton;
					gameTab.MainTitleButtonEvent += OnMainTitleButton;
					gameTab.RightAnswerButtonEvent += OnRightAnswerButton;
					gameTab.WrongAnswerButtonEvent += OnWrongAnswerButton;
					gameTab.StartCountdownEvent += OnStartCountdown;
					gameTab.Option1ButtonEvent += OnOption1Button;
					gameTab.Option2ButtonEvent += OnOption2Button;
					gameTab.Option3ButtonEvent += OnOption3Button;
					gameTab.Option4ButtonEvent += OnOption4Button;
					gameTab.Option5ButtonEvent += OnOption5Button;
					gameTab.FireworksEvent += OnFireworksButton;
					gameTab.FlashEvent += OnFlashButton;
					gameTab.ForceEvent += (value, isInside) => OnForceOnPrimitives(value, isInside);
					gameTab.RestartPrimitivesEvent += OnRestartPrimitives;
					gameTab.Force.BlockHotkeyEvent += BlockHotkey;
					gameTab.Force.UnblockHotkeyEvent += UnblockHotkey;
					break;

				case TypesOfTab.Teams:
					TeamsTab teamsTab = tab as TeamsTab;

					teamsTab.ReplaceCurrentTeamEvent += (newNumTeam) => ReplaceCurrentTeam(newNumTeam);
					teamsTab.ChangeTeamNameEvent += (newNumTeam, newNameTeam) => ChangeTeamName(newNumTeam, newNameTeam);
					teamsTab.AddMoneyEvent += (numTeam, newMoney) => AddMoneyToTeam(numTeam, newMoney);
					teamsTab.BlockHotkeyEvent += BlockHotkey;
					teamsTab.UnblockHotkeyEvent += UnblockHotkey;
					break;

				case TypesOfTab.Player:

					if (CurrentPlayerTab == null)
						CurrentPlayerTab = tab as PlayerTab;

					CurrentPlayerTab.PlayUntilPauseMarkEvent += OnPlayUntilPauseMark;
					CurrentPlayerTab.PlayAfterPauseMarkEvent += OnPlayAfterPauseMark;
					CurrentPlayerTab.PlayFullEvent += OnPlayFull;
					CurrentPlayerTab.PauseEvent += OnPlayerPause;
					CurrentPlayerTab.LoopEvent += (isLoop) => OnPlayerLoop(isLoop);
					CurrentPlayerTab.SetPauseMarkEvent += (pauseMark) => OnSetPauseMark(pauseMark);
					CurrentPlayerTab.BlockHotkeyEvent += BlockHotkey;
					CurrentPlayerTab.UnblockHotkeyEvent += UnblockHotkey;
					break;

				case TypesOfTab.PropertiesOnClient:
					PropertiesTabOnClient propertiesTab = tab as PropertiesTabOnClient;

					propertiesTab.OnBlockHotkeyEvent += BlockHotkey;
					propertiesTab.OnUnblockHotkeyEvent += UnblockHotkey;

					//AudioMixer
					propertiesTab.OnChangedAudioChannelEvent += (channel, normalizeValue) => OnChangedAudioChannel(channel, normalizeValue);
					propertiesTab.OnChannelDoubleClickedEvent += (channel) => OnChannelDoubleClicked(channel);

					//ColorsWindow
					propertiesTab.OnChangedPaletteEvent += (numPalette) => OnChangedPalette(numPalette);
					propertiesTab.OnChangedColorInPaletteEvent += (numPalette, gameColor, color) => OnChangedColorInPalette(numPalette,
						gameColor, color);
					propertiesTab.OnRefreshPalettesEvent += OnNeedRefreshPalettes;

					//ClientMenu
					propertiesTab.OnClientButtonEvent += (ip, port) => OnConnectButton(ip, port);

					//GameDisplayWindow
					propertiesTab.OnRefreshGameDisplayInfoEvent += OnRefreshGameDisplayInfo;
					propertiesTab.OnChangedGameDisplayResolutionEvent += (width, height) => OnSetGameDisplayResolution(width, height);
					propertiesTab.OnSwitchedGameDisplayFullscreenEvent += (isFullscreen) => OnSwitchedGameDisplayFullscreen(isFullscreen);

					//MenuDisplayWindow
					propertiesTab.OnRefreshMenuDisplayInfoEvent += OnRefreshMenuDisplayInfo;
					propertiesTab.OnChangedMenuDisplayResolutionEvent += (width, height) => OnSetMenuDisplayResolution(width, height);
					propertiesTab.OnSwitchedMenuDisplayFullscreenEvent += (isFullscreen) => OnSwitchedMenuDisplayFullscreen(isFullscreen);

					//GameTextsWindow
					propertiesTab.OnRefreshGameTextsEvent += OnRefreshGameTexts;
					propertiesTab.OnChangedCurrentMonetaryUnitEvent += (MonetaryUnit) => OnChangedCurrentMonetaryUnit(MonetaryUnit);
					propertiesTab.OnChangedGameTextsEvent += (name, finalText) => OnChangedGameTexts(name, finalText);

					//LoadImageWindow
					propertiesTab.OnRefreshLogoEvent += OnRefreshLogo;
					propertiesTab.OnUserLoadedLogoEvent += (logo, path) => OnUserLoadedLogo(logo, path);
					propertiesTab.OnDeleteLogoEvent += OnDeleteLogo;
					break;

				case TypesOfTab.PropertiesOnServer:
				default:
					break;
			}
		}
	}

	private void OnDisable()
	{
		base.OnDisable();

		UnregisterEvents();

		_properties.DisplayController.OnRefreshDisplayInfoEvent -= (displayInfo, isFullscreen) => OnRefreshMenuDisplayInfoInMenu(displayInfo, isFullscreen);

		//Client
		_gameClient.Client.ActivateEvent -= OnActivateClient;
		_gameClient.Client.ConnectionDroppedEvent -= OnDroppedClientConnection;
		_gameClient.Client.ConnectedEvent -= ConnectedClient;
		_gameClient.Client.ShutdownEvent -= OnShutdownClient;

		//PreviewMenu
		_previewMenu.OnDoubleClickedPreviewEvent -= OnDoubleClickedPreview;
		_previewMenu.OnRequestPreviewTextureEvent -= (width, height) => OnRequestPreviewTexture(width, height);
		//_previewMenu.OnChangedPreviewQualityEvent -= (normalizeValue) => OnChangedPreviewQuality(normalizeValue);
		//_previewMenu.OnChangedPreviewFPSEvent -= (value) => OnChangedPreviewFPS(value);

		foreach (var button in TabButtons)
		{
			if (button.TryGetComponent<Button>(out Button buttonWithEvent))
				buttonWithEvent.onClick.RemoveListener(() => ClickMenuButton(button.TypesMenuReadOnly));
		}

		foreach (var tab in Tabs)
		{
			switch (tab.TypesMenuReadOnly)
			{
				case TypesOfTab.Game:
					GameTab gameTab = tab as GameTab;

					gameTab.NextTitleButtonEvent -= OnNextTitleButton;
					gameTab.TeamsTitleButtonEvent -= OnTeamsTitleButton;
					gameTab.MainTitleButtonEvent -= OnMainTitleButton;
					gameTab.RightAnswerButtonEvent -= OnRightAnswerButton;
					gameTab.WrongAnswerButtonEvent -= OnWrongAnswerButton;
					gameTab.StartCountdownEvent -= OnStartCountdown;
					gameTab.Option1ButtonEvent -= OnOption1Button;
					gameTab.Option2ButtonEvent -= OnOption2Button;
					gameTab.Option3ButtonEvent -= OnOption3Button;
					gameTab.Option4ButtonEvent -= OnOption4Button;
					gameTab.Option5ButtonEvent -= OnOption5Button;
					gameTab.FireworksEvent -= OnFireworksButton;
					gameTab.FlashEvent -= OnFlashButton;
					gameTab.ForceEvent -= (value, isInside) => OnForceOnPrimitives(value, isInside);
					gameTab.RestartPrimitivesEvent -= OnRestartPrimitives;
					gameTab.Force.BlockHotkeyEvent -= BlockHotkey;
					gameTab.Force.UnblockHotkeyEvent -= UnblockHotkey;
					break;

				case TypesOfTab.Teams:
					TeamsTab teamsTab = tab as TeamsTab;

					teamsTab.ReplaceCurrentTeamEvent -= (newNumTeam) => ReplaceCurrentTeam(newNumTeam);
					teamsTab.ChangeTeamNameEvent -= (newNumTeam, newNameTeam) => ChangeTeamName(newNumTeam, newNameTeam);
					teamsTab.AddMoneyEvent -= (numTeam, newMoney) => AddMoneyToTeam(numTeam, newMoney);
					teamsTab.BlockHotkeyEvent -= BlockHotkey;
					teamsTab.UnblockHotkeyEvent -= UnblockHotkey;
					break;

				case TypesOfTab.Player:

					if (CurrentPlayerTab == null)
						CurrentPlayerTab = tab as PlayerTab;

					CurrentPlayerTab.PlayUntilPauseMarkEvent -= OnPlayUntilPauseMark;
					CurrentPlayerTab.PlayAfterPauseMarkEvent -= OnPlayAfterPauseMark;
					CurrentPlayerTab.PlayFullEvent -= OnPlayFull;
					CurrentPlayerTab.PauseEvent -= OnPlayerPause;
					CurrentPlayerTab.LoopEvent -= (isLoop) => OnPlayerLoop(isLoop);
					CurrentPlayerTab.SetPauseMarkEvent -= (pauseMark) => OnSetPauseMark(pauseMark);
					CurrentPlayerTab.BlockHotkeyEvent -= BlockHotkey;
					CurrentPlayerTab.UnblockHotkeyEvent -= UnblockHotkey;

					CurrentPlayerTab = null;
					break;

				case TypesOfTab.PropertiesOnClient:
					PropertiesTabOnClient propertiesTab = tab as PropertiesTabOnClient;

					propertiesTab.OnBlockHotkeyEvent -= BlockHotkey;
					propertiesTab.OnUnblockHotkeyEvent -= UnblockHotkey;

					//AudioMixer
					propertiesTab.OnChangedAudioChannelEvent -= (channel, normalizeValue) => OnChangedAudioChannel(channel, normalizeValue);
					propertiesTab.OnChannelDoubleClickedEvent -= (channel) => OnChannelDoubleClicked(channel);

					//ColorsWindow
					propertiesTab.OnChangedPaletteEvent -= (numPalette) => OnChangedPalette(numPalette);
					propertiesTab.OnChangedColorInPaletteEvent -= (numPalette, gameColor, color) => OnChangedColorInPalette(numPalette,
						gameColor, color);
					propertiesTab.OnRefreshPalettesEvent -= OnNeedRefreshPalettes;

					//ClientMenu
					propertiesTab.OnClientButtonEvent -= (ip, port) => OnConnectButton(ip, port);

					//GameDisplayWindow
					propertiesTab.OnRefreshGameDisplayInfoEvent -= OnRefreshGameDisplayInfo;
					propertiesTab.OnChangedGameDisplayResolutionEvent -= (width, height) => OnSetGameDisplayResolution(width, height);
					propertiesTab.OnSwitchedGameDisplayFullscreenEvent -= (isFullscreen) => OnSwitchedGameDisplayFullscreen(isFullscreen);

					//MenuDisplayWindow
					propertiesTab.OnRefreshMenuDisplayInfoEvent -= OnRefreshMenuDisplayInfo;
					propertiesTab.OnChangedMenuDisplayResolutionEvent -= (width, height) => OnSetMenuDisplayResolution(width, height);
					propertiesTab.OnSwitchedMenuDisplayFullscreenEvent -= (isFullscreen) => OnSwitchedMenuDisplayFullscreen(isFullscreen);

					//GameTextsWindow
					propertiesTab.OnRefreshGameTextsEvent -= OnRefreshGameTexts;
					propertiesTab.OnChangedCurrentMonetaryUnitEvent -= (MonetaryUnit) => OnChangedCurrentMonetaryUnit(MonetaryUnit);
					propertiesTab.OnChangedGameTextsEvent -= (name, finalText) => OnChangedGameTexts(name, finalText);

					//LoadImageWindow
					propertiesTab.OnRefreshLogoEvent -= OnRefreshLogo;
					propertiesTab.OnUserLoadedLogoEvent -= (logo, path) => OnUserLoadedLogo(logo, path);
					propertiesTab.OnDeleteLogoEvent -= OnDeleteLogo;
					break;

				case TypesOfTab.PropertiesOnServer:
				default:
					break;
			}
		}
	}

	private void Start()
	{
		DeselectAllTabButtons();
		SelectTabButton(TypesOfTab.PropertiesOnClient);

		CloseAllTabs();
		OpenTab(TypesOfTab.PropertiesOnClient);

		RefreshAllTabs();

		_gameClient.Client.Shutdown();
		_previewMenu.SwitchOff(_properties.ThemeMenuChanger.GetDisabledColor());

		//SetGameTabInteractables(false);
		//SetTeamsTabInteractables(false);
	}

	public override ConnectState GetConnectState()
	{
		if (_gameClient.Client.IsActive && _gameClient.Client.Connection.IsCreated)
			return ConnectState.On;

		else if (_gameClient.Client.IsActive && !_gameClient.Client.Connection.IsCreated)
			return ConnectState.Wait;

		else
			return ConnectState.Off;
	}

	#region TabsUpdate

	private void UpdateTabsWhenConnected()
	{
		if (_gameClient.Client.IsActive && _gameClient.Client.Connection.IsCreated)
		{
			UpdateGameTab();
			UpdateTeamsTab();
			UpdatePlayerTab();
		}
	}

	private void UpdateGameTab()
	{
		TrySendToServer(new NetTabRefresh(TypesOfTab.Game));
	}

	private void UpdateTeamsTab()
	{
		TrySendToServer(new NetTabRefresh(TypesOfTab.Teams));
	}

	private void UpdatePlayerTab()
	{
		TrySendToServer(new NetTabRefresh(TypesOfTab.Player));
	}


	private void SetTabInteractables(TypesOfTab type, bool[] isInteractables)
	{
		foreach (var tab in Tabs)
		{
			if (tab.TypesMenuReadOnly == type)
				tab.SetTabInteractables(isInteractables);
		}
	}
	#endregion

	#region Tabs Buttons

	protected override void SelectTabButton(TypesOfTab typesMenu)
	{
		var button = TabButtons.Find(t => t.TypesMenuReadOnly == typesMenu);

		if (button != null)
			button.SelectButton(_properties.ThemeMenuChanger.GetSelectedSprite());
	}

	protected override void DeselectAllTabButtons()
	{
		foreach (var button in TabButtons)
			button.SelectButton(_properties.ThemeMenuChanger.GetButtonSprite());
	}

	protected override void OpenTab(TypesOfTab typesMenu)
	{
		OpenedTypesTab = typesMenu;

		BaseTab tab = Tabs.Find(t => t.TypesMenuReadOnly == typesMenu);

		if (tab != null)
		{
			tab.gameObject.SetActive(true);

			if (tab.TypesMenuReadOnly == TypesOfTab.PropertiesOnClient)
			{
				if (_gameClient.Client.IsActive && _gameClient.Client.Connection.IsCreated)
				{
					_gameClient.Client.SendToServer(new NetUpdateAllAudioChannels());
					_gameClient.Client.SendToServer(new NetRefreshPalettes());
				}
			}
		}
	}

	#endregion

	#region Header

	protected override void ChangeThemeMenu()
	{
		_properties.ThemeMenuChanger.ChangeTheme();

		SelectTabButton(OpenedTypesTab);

		ThemeButton.image.sprite = _properties.ThemeMenuChanger.GetFollowingThemeSprite();
	}

	protected override void OnClickConnectFromHeader()
	{
		PropertiesTabOnClient tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnClient) as PropertiesTabOnClient;
		if (tab != null)
			tab.OnClickConnectFromHeader();
	}

	#endregion

	#region LogMenu

	public override void WriteLog(string text)
	{
		ShowTextForTime.ShowText(LogMenu.LogInputField, text);

		Debug.Log(text);
	}

	#endregion

	#region PreviewMenuOnClient

	private void OnDoubleClickedPreview()
	{
		if (_gameClient.Client.IsActive && _gameClient.Client.Connection.IsCreated)
		{
			if(_previewMenu.IsOn)
				_previewMenu.SwitchOff(_properties.ThemeMenuChanger.GetDisabledColor());
			else
				_previewMenu.SwitchOn(_properties.ThemeMenuChanger.GetTextColor());
		}
		else
		{

			WriteLog("Сначала подключитесь к серверу.");
		}
	}

	private void OnRequestPreviewTexture(int width, int height)
	{
		NetRequestPreviewTexture netRequestPreviewTexture = new NetRequestPreviewTexture(width, height);

		TrySendToServer(netRequestPreviewTexture);
	}

	#endregion

	#region GameTab

	protected override void OnNextTitleButton()
	{
		if (AreHotkeysBlocked)
			return;

		TrySendToServer(new NetNextTitle());
	}

	protected override void OnMainTitleButton()
	{
		if (AreHotkeysBlocked)
			return;

		TrySendToServer(new NetMainTitle());
	}

	protected override void OnTeamsTitleButton()
	{
		if (AreHotkeysBlocked)
			return;

		TrySendToServer(new NetTeamsTitle());
	}

	protected override void OnRightAnswerButton()
	{
		if (AreHotkeysBlocked)
			return;

		TrySendToServer(new NetRightAnswer());
	}

	protected override void OnWrongAnswerButton()
	{
		if (AreHotkeysBlocked)
			return;

		TrySendToServer(new NetWrongAnswer());
	}

	protected override void OnStartCountdown()
	{
		if (AreHotkeysBlocked)
			return;

		TrySendToServer(new NetStartCountdown());
	}

	protected override void OnOption1Button()
	{
		if (AreHotkeysBlocked)
			return;

		TrySendToServer(new NetOption(1));
	}

	protected override void OnOption2Button()
	{
		if (AreHotkeysBlocked)
			return;

		TrySendToServer(new NetOption(2));
	}

	protected override void OnOption3Button()
	{
		if (AreHotkeysBlocked)
			return;

		TrySendToServer(new NetOption(3));
	}

	protected override void OnOption4Button()
	{
		if (AreHotkeysBlocked)
			return;

		TrySendToServer(new NetOption(4));
	}

	protected override void OnOption5Button()
	{
		if (AreHotkeysBlocked)
			return;

		TrySendToServer(new NetOption(5));
	}

	protected override void OnFireworksButton()
	{
		if (AreHotkeysBlocked)
			return;

		TrySendToServer(new NetFireworks());
	}

	protected override void OnFlashButton()
	{
		if (AreHotkeysBlocked)
			return;

		TrySendToServer(new NetFlashLight());
	}

	private void OnForceOnPrimitives(float value, bool isInside)
	{
		TrySendToServer(new NetForceOnPrimitives(value, isInside));
	}

	private void OnRestartPrimitives()
	{
		TrySendToServer(new NetRestartPrimitives());
	}
	/*
	private void SetGameTabInteractables(bool isInteractable)
	{
		bool[] isInteractables = new bool[9];

		for (int i = 0; i < isInteractables.Length; i++)
		{
			isInteractables[i] = isInteractable;
		}

		SetGameTabInteractables(isInteractables);
	}
	*/
	/*
	public void SetSpriteOnMainTitleButton(TypesOfButtonState state)
	{
		GameTab tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfMenu.Game) as GameTab;

		if (tab != null)
		{
			switch (state)
			{
				default:
				case TypesOfButtonState.Default:
					tab.SetSpriteOnMainTitleButton(_properties.ThemeMenuChanger.GetButtonSprite());
					break;

				case TypesOfButtonState.Pause:
					tab.SetSpriteOnMainTitleButton(_properties.ThemeMenuChanger.GetRedSprite());
					break;
			}
		}
	}
	*/
	/*
	public void InteractableNextButton(bool isInteractable)
	{
		GameTab tab = _Tabs.Find(t => t.TypesMenuReadOnly == TypesMenu.Game) as GameTab;

		if (tab != null)
			tab.InteractableNextButton(isInteractable);
	}

	public void InteractableAnswerButtons(bool isInteractable)
	{
		GameTab tab = _Tabs.Find(t => t.TypesMenuReadOnly == TypesMenu.Game) as GameTab;

		if (tab != null)
			tab.InteractableAnswerButtons(isInteractable);
	}
	public void InteractableCountdownButton(bool isInteractable)
	{
		GameTab tab = _Tabs.Find(t => t.TypesMenuReadOnly == TypesMenu.Game) as GameTab;

		if (tab != null)
			tab.InteractableCountdownButton(isInteractable);
	}

	public void EnableOptionsButtons(int optionsCount)
	{
		GameTab tab = _Tabs.Find(t => t.TypesMenuReadOnly == TypesMenu.Game) as GameTab;

		if (tab != null)
		{
			tab.EnableOptionsButtons();
			tab.DisableUnusedOptionsButtons(optionsCount);
		}
	}

	public void DisableOptionsButtons()
	{
		GameTab tab = _Tabs.Find(t => t.TypesMenuReadOnly == TypesMenu.Game) as GameTab;

		if (tab != null)
			tab.DisableOptionsButtons();
	}
	*/

	#endregion

	#region TeamsTab

	private void ReplaceCurrentTeam(int newNumTeam)
	{
		TrySendToServer(new NetReplaceCurrentTeam(newNumTeam));
	}

	private void ChangeTeamName(int numTeam, string newName)
	{
		TrySendToServer(new NetChangeTeamName(numTeam, newName));
	}

	private void AddMoneyToTeam(int numTeam, string money)
	{
		TrySendToServer(new NetAddMoneyToTeam(numTeam, money));
	}

	/*
	public void InteractableReplaceCurrentTeams(bool isInteractable)
	{
		TeamsTab tab = _Tabs.Find(t => t.TypesMenuReadOnly == TypesMenu.Teams) as TeamsTab;

		if (tab != null)
			tab.InteractableReplaceCurrentTeam(isInteractable);
	}

	public void InteractableAddMoneyToTeam(bool isInteractable)
	{
		TeamsTab tab = _Tabs.Find(t => t.TypesMenuReadOnly == TypesMenu.Teams) as TeamsTab;

		if (tab != null)
			tab.InteractableAddMoney(isInteractable);

	}

	public void InteractableChangeName(bool isInteractable)
	{
		TeamsTab tab = _Tabs.Find(t => t.TypesMenuReadOnly == TypesMenu.Teams) as TeamsTab;

		if (tab != null)
			tab.InteractableDisableChangeName(isInteractable);
	}
	#endregion
	*/

	public void InteractableReplaceCurrentTeam(bool isInteractable)
	{
		TeamsTab tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.Teams) as TeamsTab;

		if (tab != null)
			tab.InteractableReplaceCurrentTeam(isInteractable);
	}

	private void RefreshTeamsDropdown(List<string> TeamsName)
	{
		foreach (var tab in Tabs)
		{
			if (tab.TypesMenuReadOnly == TypesOfTab.Teams)
			{
				TeamsTab TeamsTab = tab as TeamsTab;

				TeamsTab.RefreshTeamsDropdown(TeamsName);
			}
		}
	}
	#endregion

	#region PlayerTab

	public void SetPlayerLoopWithoutNotify(bool isLoop)
	{
		PlayerTab tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.Player) as PlayerTab;
		if (tab != null)
			tab.SetPlayerLoopWithoutNotify(isLoop);
	}

	protected override void OnPlayUntilPauseMark()
	{
		if (AreHotkeysBlocked)
			return;

		TrySendToServer(new NetPlayUntilMark());
	}

	protected override void OnPlayAfterPauseMark()
	{
		if (AreHotkeysBlocked)
			return;

		TrySendToServer(new NetPlayAfterMark());
	}

	protected override void OnPlayFull()
	{
		if (AreHotkeysBlocked)
			return;

		TrySendToServer(new NetPlayFull());
	}

	protected override void OnPlayerPause()
	{
		if (AreHotkeysBlocked)
			return;

		TrySendToServer(new NetPlayerPause());
	}

	protected override void OnHotkeyPlayerLoop()
	{
		if (AreHotkeysBlocked)
			return;

		TrySendToServer(new NetPlayerLoop());
	}

	private void OnPlayerLoop(bool isLoop)
	{
		OnHotkeyPlayerLoop();
	}

	private void WriteOnPauseMarkField(string pauseMark)
	{
		PlayerTab tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.Player) as PlayerTab;
		if (tab != null)
			tab.WriteOnPauseMarkField(pauseMark);
	}

	private void WriteOnTimeDisplay(string time)
	{
		if (CurrentPlayerTab == null)
			return;

		CurrentPlayerTab.WriteOnTimeDisplay(time);
	}

	private void OnSetPauseMark(string pauseMark)
	{
		TrySendToServer(new NetSetPauseMark(pauseMark));
	}

	#endregion

	#region PropertiesTabOnClient

	#region GameDisplayWindow

	private void OnRefreshGameDisplayInfo()
	{
		TrySendToServer(new NetGameDisplayInfo("Info", false));
	}

	private void OnSetGameDisplayResolution(int width, int height)
	{
		TrySendToServer(new NetSetGameDisplayResolution(width, height));
	}

	private void OnSwitchedGameDisplayFullscreen(bool isFullscreen)
	{
		TrySendToServer(new NetGameDisplayFullscreen(isFullscreen));
	}

	private void OnRefreshGameDisplayInfoInMenu(string displayInfo, bool isFullscreen)
	{
		PropertiesTabOnClient tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnClient) as PropertiesTabOnClient;
		if (tab != null)
			tab.RefreshGameDisplayInfoInMenu(displayInfo, isFullscreen);
	}

	#endregion

	#region ClientDisplayWindow

	private void OnRefreshMenuDisplayInfo()
	{
		OnRefreshMenuDisplayInfoInMenu(_properties.DisplayController.GetDisplayInfo(), _properties.DisplayController.GetIsFullscreen());
	}

	private void OnSetMenuDisplayResolution(int width, int height)
	{
		_properties.DisplayController.ChangeResolution(width, height);
	}

	private void OnSwitchedMenuDisplayFullscreen(bool isFullscreen)
	{
		_properties.DisplayController.SetFullscreen(isFullscreen);
	}

	private void OnRefreshMenuDisplayInfoInMenu(string displayInfo, bool isFullscreen)
	{
		PropertiesTabOnClient tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnClient) as PropertiesTabOnClient;
		if (tab != null)
			tab.RefreshMenuDisplayInfoInMenu(displayInfo, isFullscreen);
	}

	protected override void OnSwitchFullscreenHotkey()
	{
		OnSwitchedMenuDisplayFullscreen(!_properties.DisplayController.GetIsFullscreen());
	}

	#endregion

	#region GameTextsWindow

	private void OnRefreshGameTexts()
	{
		char[] chars = new char[] { '0' };
		TrySendToServer(new NetGameTextsWindow("Name", "FinalText", chars));
	}

	private void OnRefreshGameTextsInMenus(string name, string finalText, char[] monetaryUnits)
	{
		PropertiesTabOnClient tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnClient) as PropertiesTabOnClient;
		if (tab != null)
			tab.SetGameTexts(name, finalText, monetaryUnits);
	}

	private void OnChangedGameTexts(string name, string finalText)
	{
		TrySendToServer(new NetChangedGameTexts(name, finalText));
	}

	private void OnChangedCurrentMonetaryUnit(char monetaryUnit)
	{
		TrySendToServer(new NetMonetaryUnit(monetaryUnit));
	}

	#endregion

	#region LoadImageWindows

	private void OnRefreshLogo()
	{
		TrySendToServer(new NetRefreshLogo());
	}

	private void OnUserLoadedLogo(Texture2D logo, string path)
	{
		TrySendToServer(new NetUserLoadedLogo(path));
	}

	private void OnDeleteLogo()
	{
		TrySendToServer(new NetUserDeletedLogo());
	}

	private void ChangeLogoInMenus(Texture2D logo, string path)
	{
		PropertiesTabOnClient tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnClient) as PropertiesTabOnClient;
		if (tab != null)
			tab.ChangeLogoInfo(logo, path);
	}

	#endregion

	#region ColorsWindow

	private void RefreshPalettesInMenu(List<ColorPalette> palettes, int currentNumPalette)
	{
		PropertiesTabOnClient tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnClient) as PropertiesTabOnClient;
		if (tab != null)
			tab.SetColorsFromPalette(palettes, currentNumPalette);
	}

	private void OnChangedPalette(int numPalette)
	{
		TrySendToServer(new NetChangePalette(numPalette));
	}

	private void OnChangedColorInPalette(int numPalette, TypesOfGameColor gameColor, Color color)
	{
		TrySendToServer(new NetChangeColorInPalette(numPalette, gameColor, color));
	}

	private void OnNeedRefreshPalettes()
	{
		TrySendToServer(new NetRefreshPalettes());
	}

	#endregion

	#region AudioMixerMenu

	private void OnChangedAudioChannel(TypesOfAudioChannel channel, float normalizeValue)
	{
		TrySendToServer(new NetSetAudioChannel(channel, normalizeValue));
	}

	private void OnSetAudioChannelInMenu(TypesOfAudioChannel channel, float normalizeValue)
	{
		PropertiesTabOnClient tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnClient) as PropertiesTabOnClient;
		if (tab != null)
			tab.SetChannel(channel, normalizeValue);
	}

	private void OnChannelDoubleClicked(TypesOfAudioChannel channel)
	{
		TrySendToServer(new NetDoubleClickAudioChannel(channel));
	}

	private void UpdateVolumeChannelsInClientMenu(float normalizeMaster, float normalizeFx,
		float normalizeCountdown, float normalizeQuestion, float normalizeMusic)
	{
		PropertiesTabOnClient tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnClient) as PropertiesTabOnClient;

		if (tab != null)
		{
			tab.UpdateVolumeChannel(normalizeMaster, normalizeFx, normalizeCountdown,
				normalizeQuestion, normalizeMusic);
		}
	}

	#endregion

	#region ClientMenu

	private void OnConnectButton(string ipAddress, string port)
	{
		if (_gameClient.Client.IsActive)
		{
			_gameClient.Client.Shutdown();
		}
		else
		{
			if (TryConvertStringToUshort(port, out ushort clientPort))
				_gameClient.Client.Init(ipAddress, clientPort);
			else
				WriteLog("Не корректный порт");
		}

		WriteLog("Кнопка подлкючения нажата безуспешно.");
	}

	private void ConnectedClient()
	{
		OnSetClientConnectStatus();

		RefreshAllTabs();

		UpdateTabsWhenConnected();
	}

	private void OnActivateClient()
	{
		PropertiesTabOnClient tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnClient) as PropertiesTabOnClient;
		if (tab != null)
			tab.SetSpriteOnConnectButton(_properties.ThemeMenuChanger.GetYellowSprite());

		ConnectButtonIndicator.image.sprite = _properties.ThemeMenuChanger.GetYellowSprite();

		_previewMenu.SwitchOff(_properties.ThemeMenuChanger.GetDisabledColor());
	}

	private void OnDroppedClientConnection()
	{
		PropertiesTabOnClient tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnClient) as PropertiesTabOnClient;
		if (tab != null)
			tab.SetSpriteOnConnectButton(_properties.ThemeMenuChanger.GetYellowSprite());

		ConnectButtonIndicator.image.sprite = _properties.ThemeMenuChanger.GetYellowSprite();

		_previewMenu.SwitchOff(_properties.ThemeMenuChanger.GetDisabledColor());
	}

	private void OnShutdownClient()
	{
		PropertiesTabOnClient tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnClient) as PropertiesTabOnClient;
		if (tab != null)
			tab.SetSpriteOnConnectButton(_properties.ThemeMenuChanger.GetRedSprite());

		ConnectButtonIndicator.image.sprite = _properties.ThemeMenuChanger.GetRedSprite();

		_previewMenu.SwitchOff(_properties.ThemeMenuChanger.GetDisabledColor());
	}

	private void OnSetClientConnectStatus()
	{
		PropertiesTabOnClient tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnClient) as PropertiesTabOnClient;
		if (tab != null)
			tab.SetSpriteOnConnectButton(_properties.ThemeMenuChanger.GetGreenSprite());

		ConnectButtonIndicator.image.sprite = _properties.ThemeMenuChanger.GetGreenSprite();

		_previewMenu.SwitchOn(_properties.ThemeMenuChanger.GetTextColor());
	}

	#endregion

	#endregion

	#region Client
	private void RegisterClientEvents()
	{
		NetUtility.C_WELCOME += OnWelcomeClient;
		NetUtility.C_SCREENSHOT += OnScreenShotClient;
		NetUtility.C_TAB_INTERACTABLES += OnTabInteractablesClient;

		//PreviewMenu
		NetUtility.C_SEND_PREVIEW_TEXTURE += OnSendPreviewTexture;

		//LogMenu
		NetUtility.C_WRITE_LOG += OnWriteLogClient;

		//GameTab
		//NetUtility.C_MAIN_TITLE_STATE_BUTTON += OnMainTitleStateClient;
		//NetUtility.C_INTERACTABLE_NEXT_BUTTON += OnIntaractibleNextButtonClient;
		//NetUtility.C_INTERACTABLE_ANSWER_BUTTONS += OnIntaractibleAnswerButtonsClient;

		//TeamsTab
		NetUtility.C_INTERACTABLE_REPLACE_CURRENT_TEAM += OnInteractableReplaceCurrentTeamClient;
		NetUtility.C_REFRESH_TEAMS_DROPDOWN += OnRefresfTeamsDropdownClient;

		//PlayerTab
		NetUtility.C_PLAYER_LOOP += OnPlayerLoopClient;
		NetUtility.C_DISPLAY_PLAYER_PLAYBACK += OnDisplayPlayerPlaybackClient;
		NetUtility.C_DISPLAY_PAUSE_MARK += OnDisplayPlayerMarkClient;

		//PropertiesTab
		NetUtility.C_GAME_DISPLAY_INFO += OnGameDisplayInfoClient;
		NetUtility.C_COLOR_PALETTES += OnRefreshPalettesClient;

		NetUtility.C_GAME_TEXTS += OnRefreshGameTextClient;

		NetUtility.C_USER_LOADED_LOGO += OnUserLoadedLogoClient;

		NetUtility.C_SET_AUDIO_CHANNEL += OnSetAudioChannelClient;
		NetUtility.C_UPDATE_ALL_AUDIO_CHANNELS += OnUpdateVolumeChannelsClient;
	}

	private void UnregisterEvents()
	{
		NetUtility.C_WELCOME -= OnWelcomeClient;
		NetUtility.C_SCREENSHOT -= OnScreenShotClient;
		NetUtility.C_TAB_INTERACTABLES -= OnTabInteractablesClient;

		//PreviewMenu
		NetUtility.C_SEND_PREVIEW_TEXTURE -= OnSendPreviewTexture;

		//LogMenu
		NetUtility.C_WRITE_LOG -= OnWriteLogClient;

		//GameTab
		//NetUtility.C_MAIN_TITLE_STATE_BUTTON -= OnMainTitleStateClient;
		//NetUtility.C_INTERACTABLE_NEXT_BUTTON -= OnIntaractibleNextButtonClient;
		//NetUtility.C_INTERACTABLE_ANSWER_BUTTONS -= OnIntaractibleAnswerButtonsClient;

		//TeamsTab
		NetUtility.C_INTERACTABLE_REPLACE_CURRENT_TEAM -= OnInteractableReplaceCurrentTeamClient;
		NetUtility.C_REFRESH_TEAMS_DROPDOWN -= OnRefresfTeamsDropdownClient;

		//PlayerTab
		NetUtility.C_PLAYER_LOOP -= OnPlayerLoopClient;
		NetUtility.C_DISPLAY_PLAYER_PLAYBACK -= OnDisplayPlayerPlaybackClient;
		NetUtility.C_DISPLAY_PAUSE_MARK -= OnDisplayPlayerMarkClient;

		//PropertiesTab
		NetUtility.C_GAME_DISPLAY_INFO -= OnGameDisplayInfoClient;
		NetUtility.C_COLOR_PALETTES -= OnRefreshPalettesClient;

		NetUtility.C_GAME_TEXTS -= OnRefreshGameTextClient;

		NetUtility.C_USER_LOADED_LOGO -= OnUserLoadedLogoClient;

		NetUtility.C_SET_AUDIO_CHANNEL -= OnSetAudioChannelClient;
		NetUtility.C_UPDATE_ALL_AUDIO_CHANNELS -= OnUpdateVolumeChannelsClient;
	}

	private void TrySendToServer(NetMessage msg)
	{
		if (_gameClient.Client.IsActive && _gameClient.Client.Connection.IsCreated)
			_gameClient.Client.SendToServer(msg);
		else
			WriteLog("Подключитесь к серверу.");
	}

	private void OnWelcomeClient(NetMessage msg)
	{
		// Recieve the connection message
		NetWelcome netWelcome = msg as NetWelcome;

		Debug.Log($"My assigned team is {netWelcome.AssignedClient}");
	}

	#region PreviewMenu

	private void OnSendPreviewTexture(NetMessage msg)
	{
		Debug.Log("WOW I Recieved Preview Texture message.");
		NetSendPreviewTexture netSendPreviewTexture = msg as NetSendPreviewTexture;

		_currentRawTextureBytes = new byte[netSendPreviewTexture.OutRawTextureData.Length];
		_currentRawTextureBytes = NativeArrayExtension.ToRawBytes<byte>(netSendPreviewTexture.OutRawTextureData);
		//netSendPreviewTexture.RawTextureData.Dispose();

		_currentTexture2D = new Texture2D(netSendPreviewTexture.Width, netSendPreviewTexture.Height);

		_currentTexture2D = _properties.TextureConverter.GetTexture2DFromBytes(_currentRawTextureBytes,
			_currentTexture2D.width, _currentTexture2D.height);
		//_previewMenu.PreviewImage.texture = _currentTexture2D;
		_properties.PreviewMaterial.SetTexture("_MainTex", _currentTexture2D);
	}

	#endregion

	#region LogMenu

	private void OnWriteLogClient(NetMessage msg)
	{
		NetWriteLog netWriteLog = msg as NetWriteLog;

		WriteLog(netWriteLog.LogText);
	}

	#endregion

	#region GameTab

	private void OnTabInteractablesClient(NetMessage msg)
	{
		NetTabInteractables netTabInteractables = msg as NetTabInteractables;

		switch (netTabInteractables.Type)
		{
			case TypesOfTab.Game:
				SetTabInteractables(TypesOfTab.Game, netTabInteractables.IsInteractables);
				break;
			case TypesOfTab.Teams:
				SetTabInteractables(TypesOfTab.Teams, netTabInteractables.IsInteractables);
				break;
			case TypesOfTab.Player:
				SetTabInteractables(TypesOfTab.Player, netTabInteractables.IsInteractables);
				break;
				/*
			case TypesOfTab.PropertiesOnServer:
				break;
			case TypesOfTab.PropertiesOnClient:
				break;
				*/
			default:
				break;
		}
	}

	/*
	private void OnMainTitleStateClient(NetMessage msg)
	{
		NetMainTitleStateButton netMainTitleSpriteButton = msg as NetMainTitleStateButton;

		SetSpriteOnMainTitleButton(netMainTitleSpriteButton.ButtonState);
	}
	*/
	/*
	private void OnIntaractibleNextButtonClient(NetMessage msg)
	{
		NetInteractableNextButton netInteractableNextButton = msg as NetInteractableNextButton;
		
		InteractableNextButton(netInteractableNextButton.IsOnBool);
	}

	private void OnIntaractibleAnswerButtonsClient(NetMessage msg)
	{
		NetInteractableAnswerButtons netInteractableAnswerButtons = msg as NetInteractableAnswerButtons;

		InteractableAnswerButtons(netInteractableAnswerButtons.IsOnBool);
	}
	*/

	#endregion

	#region TeamsTab

	private void OnInteractableReplaceCurrentTeamClient(NetMessage msg)
	{
		NetInteractableReplaceCurrentTeam netInteractableReplaceCurrentTeam = msg as NetInteractableReplaceCurrentTeam;

		InteractableReplaceCurrentTeam(netInteractableReplaceCurrentTeam.IsInteractable);
	}

	private void OnRefresfTeamsDropdownClient(NetMessage msg)
	{
		NetRefreshTeamsDropdown netRefreshTeamsDropdown = msg as NetRefreshTeamsDropdown;

		RefreshTeamsDropdown(netRefreshTeamsDropdown.TeamsStringFromServer);
	}

	#endregion

	#region PlayerTab

	private void OnPlayerLoopClient(NetMessage msg)
	{
		NetPlayerLoop netPlayerLoop = msg as NetPlayerLoop;

		SetPlayerLoopWithoutNotify(netPlayerLoop.IsOnBool);
	}

	private void OnDisplayPlayerPlaybackClient(NetMessage msg)
	{
		NetDisplayPlayerPlayback netDisplayPlayerPlayback = msg as NetDisplayPlayerPlayback;

		WriteOnTimeDisplay(netDisplayPlayerPlayback.Time);
	}

	private void OnDisplayPlayerMarkClient(NetMessage msg)
	{
		NetDisplayPauseMark netDisplayPlayerMark = msg as NetDisplayPauseMark;

		WriteOnPauseMarkField(netDisplayPlayerMark.PauseMark);
	}

	#endregion

	#region PropertiesTab

	private void OnSetAudioChannelClient(NetMessage msg)
	{
		NetSetAudioChannel netSetAudioChannel = msg as NetSetAudioChannel;

		OnSetAudioChannelInMenu(netSetAudioChannel.Channel, netSetAudioChannel.NormalizeVolume);
	}

	private void OnUpdateVolumeChannelsClient(NetMessage msg)
	{
		NetUpdateAllAudioChannels netUpdateVolumes = msg as NetUpdateAllAudioChannels;

		UpdateVolumeChannelsInClientMenu(netUpdateVolumes.NormalizeMaster, netUpdateVolumes.NormalizeFx,
			netUpdateVolumes.NormalizeCountdown, netUpdateVolumes.NormalizeQuestion, netUpdateVolumes.NormalizeMusic);
	}

	private void OnGameDisplayInfoClient(NetMessage msg)
	{
		NetGameDisplayInfo netGameDisplayInfo = msg as NetGameDisplayInfo;

		OnRefreshGameDisplayInfoInMenu(netGameDisplayInfo.GameDisplayInfo, netGameDisplayInfo.IsFullscreen);
	}

	private void OnRefreshPalettesClient(NetMessage msg)
	{
		NetColorPalettes netRefreshPalettes = msg as NetColorPalettes;

		RefreshPalettesInMenu(netRefreshPalettes.Palettes, netRefreshPalettes.CurrentNumPalette);

		if (netRefreshPalettes.Parent != null)
			Destroy(netRefreshPalettes.Parent);
	}

	private void OnRefreshGameTextClient(NetMessage msg)
	{
		NetGameTextsWindow netGameTextsWindow = msg as NetGameTextsWindow;

		OnRefreshGameTextsInMenus(netGameTextsWindow.NameOfGame, netGameTextsWindow.FinalText, netGameTextsWindow.MonetaryUnits);
	}

	private void OnUserLoadedLogoClient(NetMessage msg)
	{
		NetUserLoadedLogo netUserLoadedLogo = msg as NetUserLoadedLogo;

		PropertiesTabOnClient tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnClient) as PropertiesTabOnClient;
		if (tab != null)
		{
			if (tab.TryGetTextureFromLocalPath(netUserLoadedLogo.Path, out Texture2D texture))
				ChangeLogoInMenus(texture, netUserLoadedLogo.Path);
			else
				ChangeLogoInMenus(_properties.TransparentTexture, netUserLoadedLogo.Path);
		}
	}

	#endregion

	#endregion

	private void OnScreenShotClient(NetMessage msg)
	{
		NetScreenshot netScreenshot = msg as NetScreenshot;

		WriteLog(netScreenshot.Text);
		/*
		NetScreenshot netScreenShot = msg as NetScreenshot;
		Texture2D texture = new Texture2D(455, 256, TextureFormat.RGB24, false);

		texture.LoadImage(netScreenShot.Frame);

		_previewImage.sprite = Sprite.Create(texture, new Rect(0,0, texture.width, texture.height), Vector2.zero);
		//_screenShotTexture2D.LoadImage(netScreenShot.Frame);
		*/
	}

	private bool TryConvertStringToUshort(string port, out ushort portUshort)
	{
		if (ushort.TryParse(port, out ushort portUshrt))
		{
			portUshort = portUshrt;
			return true;
		}
		else
		{
			portUshort = ushort.MinValue;
			return false;
		}
	}
}
