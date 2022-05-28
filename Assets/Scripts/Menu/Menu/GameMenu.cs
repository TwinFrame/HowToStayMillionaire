using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Networking.Transport;
using Unity.Collections;
using ARKitStream.Internal;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Canvas))]

public class GameMenu : BaseMenu
{
	[Header("ScreenShot")]
	[SerializeField] private Button _screenShotButton;
	[Header("Service")]
	[SerializeField] private Game _game;
	[SerializeField] private GameServer _gameServer;
	[SerializeField] private GameAudioMixer _audioMixer;
	[SerializeField] private PrimitiveObjects _primitiveObjects;
	[SerializeField] private GameProperties _properties;

	private Coroutine _screenShotJob;
	private readonly WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

	private Coroutine _updateTimeDisplayJob;

	private IAdvancedPlayer _currentPlayer;
	private float _currentPauseMark;

	private byte[] _currentPreviewTexture;
	private NativeArray<byte> _currentNativeArray = new NativeArray<byte>();
	private int _currentWidth;
	private int _currentHeight;
	private readonly List<string> _teamNames = new List<string>();

	//Multi logic
	private int _clientCount = -1;

	private void OnEnable()
	{
		_screenShotButton.onClick.AddListener(ScreenShot);

		base.OnEnable();

		RegisterServerEvents();

		_properties.DisplayController.OnRefreshDisplayInfoEvent += (displayInfo, isFullscreen) => OnRefreshGameDisplayInfoInMenu(displayInfo, isFullscreen);

		_properties.GameColorChanger.OnChangedLogoEvent += (logo, path) => OnChangedLogo(logo, path);
		_properties.GameColorChanger.OnSetColorsFromPalette += (palettes, currentNumPalette) => SetColorsFromPalette(palettes, currentNumPalette);

		_audioMixer.OnChangedChannelEvent += (channel, normalizeValue) => OnSetAudioChannelInMenu(channel, normalizeValue);

		//Server
		_gameServer.Server.NoClientConnectionEvent += NoClientConnection;
		_gameServer.Server.HaveGotClientConnectionEvent += HaveGotClientConnection;
		_gameServer.Server.ShutdownEvent += ShutdownServer;

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

				case TypesOfTab.PropertiesOnServer:
					PropertiesTabOnServer propertiesTab = tab as PropertiesTabOnServer;

					propertiesTab.OnBlockHotkeyEvent += BlockHotkey;
					propertiesTab.OnUnblockHotkeyEvent += UnblockHotkey;

					//AudioMixer
					propertiesTab.OnChangedAudioChannelEvent += (channel, normalizeValue) => OnChangedAudioChannel(channel, normalizeValue);
					propertiesTab.OnChannelDoubleClickedEvent += (channel) => OnChannelDoubleClicked(channel);

					//ColorsWindow
					propertiesTab.OnChangedPaletteEvent += (numPalette) => OnChangedPalette(numPalette);
					propertiesTab.OnChangedColorInPaletteEvent += (numPalette, gameColor, color) => OnChangedColorInPalette(numPalette,
						gameColor, color);

					//ServerMenu
					propertiesTab.OnServerButtonEvent += (port) => OnConnectButton(port);

					//GameDisplayWindow
					propertiesTab.OnRefreshGameDisplayInfoEvent += OnRefreshGameDisplayInfo;
					propertiesTab.OnChangedGameDisplayResolutionEvent += (width, height) => OnSetGameDisplayResolution(width, height);
					propertiesTab.OnSwitchedGameDisplayFullscreenEvent += (isFullscreen) => OnSwitchedGameDisplayFullscreen(isFullscreen);

					//GameTextsWindow
					propertiesTab.OnRefreshGameTextsEvent += OnRefreshGameTexts;
					propertiesTab.OnChangedCurrentMonetaryUnitEvent += (monetaryUnit) => OnChangedCurentMonetaryUnit(monetaryUnit);
					propertiesTab.OnChangedGameTextsEvent += (name, finalText) => OnChangedGameTexts(name, finalText);

					//LoadImageWindow
					propertiesTab.OnUserLoadedLogoEvent += (logo, path) => OnUserLoadedLogo(logo, path);
					propertiesTab.OnDeleteLogoEvent += OnDeleteLogo;
					break;

				case TypesOfTab.PropertiesOnClient:
				default:
					break;
			}
		}
	}

	private void OnDisable()
	{
		_screenShotButton.onClick.RemoveListener(ScreenShot);

		base.OnDisable();

		UnregisterEvents();

		_properties.DisplayController.OnRefreshDisplayInfoEvent -= (displayInfo, isFullscreen) => OnRefreshGameDisplayInfoInMenu(displayInfo, isFullscreen);

		_properties.GameColorChanger.OnChangedLogoEvent -= (logo, path) => OnChangedLogo(logo, path);
		_properties.GameColorChanger.OnSetColorsFromPalette -= (currentNumPalette, numPalette) => SetColorsFromPalette(currentNumPalette, numPalette);

		_audioMixer.OnChangedChannelEvent -= (channel, normalizeValue) => OnSetAudioChannelInMenu(channel, normalizeValue);

		//Server
		_gameServer.Server.NoClientConnectionEvent -= NoClientConnection;
		_gameServer.Server.HaveGotClientConnectionEvent -= HaveGotClientConnection;
		_gameServer.Server.ShutdownEvent -= ShutdownServer;

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
					teamsTab.ChangeTeamNameEvent -= (numTeam, newNameTeam) => ChangeTeamName(numTeam, newNameTeam);
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

				case TypesOfTab.PropertiesOnServer:
					PropertiesTabOnServer propertiesTab = tab as PropertiesTabOnServer;

					propertiesTab.OnBlockHotkeyEvent -= BlockHotkey;
					propertiesTab.OnUnblockHotkeyEvent -= UnblockHotkey;

					//AudioMixer
					propertiesTab.OnChangedAudioChannelEvent -= (channel, normalizeValue) => OnChangedAudioChannel(channel, normalizeValue);
					propertiesTab.OnChannelDoubleClickedEvent -= (channel) => OnChannelDoubleClicked(channel);

					//ColorsWindow
					propertiesTab.OnChangedPaletteEvent -= (numPalette) => OnChangedPalette(numPalette);
					propertiesTab.OnChangedColorInPaletteEvent -= (numPalette, gameColor, color) => OnChangedColorInPalette(numPalette,
						gameColor, color);

					//ServerMenu
					propertiesTab.OnServerButtonEvent -= (port) => OnConnectButton(port);

					//GameDisplayWindow
					propertiesTab.OnRefreshGameDisplayInfoEvent -= OnRefreshGameDisplayInfo;
					propertiesTab.OnChangedGameDisplayResolutionEvent -= (width, height) => OnSetGameDisplayResolution(width, height);
					propertiesTab.OnSwitchedGameDisplayFullscreenEvent -= (isFullscreen) => OnSwitchedGameDisplayFullscreen(isFullscreen);

					//GameTextsWindow
					propertiesTab.OnRefreshGameTextsEvent -= OnRefreshGameTexts;
					propertiesTab.OnChangedCurrentMonetaryUnitEvent -= (monetaryUnit) => OnChangedCurentMonetaryUnit(monetaryUnit);
					propertiesTab.OnChangedGameTextsEvent -= (name, finalText) => OnChangedGameTexts(name, finalText);

					//LoadImageWindow
					propertiesTab.OnUserLoadedLogoEvent -= (logo, path) => OnUserLoadedLogo(logo, path);
					propertiesTab.OnDeleteLogoEvent -= OnDeleteLogo;
					break;

				case TypesOfTab.PropertiesOnClient:
				default:
					break;
			}
		}
	}

	private void Start()
	{
		base.Start();

		RefreshTeamsDropdown(_game.Teams);

		_gameServer.Server.Shutdown();

		ResetAudioMixer();

		if (!_game.CheckEqualQuestionsByTeam())
			WriteLog("Кол-во вопросов в туре(-ах) не кратно кол-ву команд.");
	}

	public override ConnectState GetConnectState()
	{
		if (_gameServer.Server.IsActive && _gameServer.Server.CheckingClientConnections())
			return ConnectState.On;

		else if (_gameServer.Server.IsActive && !_gameServer.Server.CheckingClientConnections())
			return ConnectState.Wait;

		else
			return ConnectState.Off;
	}

	public void ResetMenu()
	{
		InteractableNextButton(true);
		//Server and Client нужно тоже добавить
		InteractableCountdownButton(false);
		InteractableAnswerButtons(false);
		InteractableReplaceCurrentTeams(false);
		InteractableAddMoneyToTeam(false);
		InteractableChangeName(false);
		DisableOptionsButtons();
		DisablePlayerButtons();
	}
	#region TabsUpdate

	public bool TryGetTabInteractebles(TypesOfTab type, out bool[] currentInteractables)
	{
		foreach (var tab in Tabs)
		{
			if (tab.TypesMenuReadOnly == type)
			{
				currentInteractables = tab.GetInteractables();

				if (currentInteractables.Length > 0)
					return true;
				else
					return false;
			}
		}

		currentInteractables = null;
		return false;
	}

	private void UpdateClientTab(TypesOfTab type)
	{
		if (_gameServer.Server.IsActive && _gameServer.Server.CheckingClientConnections())
		{
			if (TryGetTabInteractebles(type, out bool[] isInteractables))
				TrySendBroadcast(new NetTabInteractables(type, isInteractables));
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

			if (tab.TypesMenuReadOnly == TypesOfTab.PropertiesOnServer)
			{
				PropertiesTabOnServer propertiesTab = tab as PropertiesTabOnServer;

				propertiesTab.UpdateVolumeChannel(_audioMixer.NormalizeMasterVolume, _audioMixer.NormalizeFxVolume,
					_audioMixer.NormalizeCountdownVolume, _audioMixer.NormalizeQuestionVolume, _audioMixer.NormalizeMusicVolume);
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
		PropertiesTabOnServer tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnServer) as PropertiesTabOnServer;
		if (tab != null)
			tab.OnClickConnectFromHeader();
	}

	#endregion

	#region LogMenu

	public override void WriteLog(string text)
	{
		ShowTextForTime.ShowText(LogMenu.LogInputField, text);

		Debug.Log(text);

		TrySendBroadcast(new NetWriteLog(text));
	}

	#endregion

	#region GameTab

	protected override void OnNextTitleButton()
	{
		if (AreHotkeysBlocked)
			return;

		_game.OnNextButton();
	}

	protected override void OnMainTitleButton()
	{
		if (AreHotkeysBlocked)
			return;

		_game.OnMainTitleButton();
	}

	protected override void OnTeamsTitleButton()
	{
		if (AreHotkeysBlocked)
			return;

		_game.OnTeamsTitleButton();
	}

	protected override void OnRightAnswerButton()
	{
		if (AreHotkeysBlocked)
			return;

		_game.OnAnswerButton(true);
	}

	protected override void OnWrongAnswerButton()
	{
		if (AreHotkeysBlocked)
			return;

		_game.OnAnswerButton(false);
	}

	protected override void OnStartCountdown()
	{
		if (AreHotkeysBlocked)
			return;

		_game.OnStartCountdown();
	}

	protected override void OnOption1Button()
	{
		if (AreHotkeysBlocked)
			return;

		_game.OnOptionButton(1);
	}

	protected override void OnOption2Button()
	{
		if (AreHotkeysBlocked)
			return;

		_game.OnOptionButton(2);
	}

	protected override void OnOption3Button()
	{
		if (AreHotkeysBlocked)
			return;

		_game.OnOptionButton(3);
	}

	protected override void OnOption4Button()
	{
		if (AreHotkeysBlocked)
			return;

		_game.OnOptionButton(4);
	}

	protected override void OnOption5Button()
	{
		if (AreHotkeysBlocked)
			return;

		_game.OnOptionButton(5);
	}

	protected override void OnFireworksButton()
	{
		if (AreHotkeysBlocked)
			return;

		_game.OnFireworks();
	}

	protected override void OnFlashButton()
	{
		if (AreHotkeysBlocked)
			return;

		_game.OnFlashGlow();
	}

	private void OnForceOnPrimitives(float value, bool isInside)
	{
		_primitiveObjects.AddForce(value, isInside);
	}

	private void OnRestartPrimitives()
	{
		_primitiveObjects.Restart();
	}
	/*
	public void SetPauseOnMainTitleButton(bool isPause)
	{
		GameTab tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfMenu.Game) as GameTab;

		if (tab != null)
		{
			if (isPause)
			{
				//tab.SetPauseOnMainTitleButton(_properties.PauseButtonColor);
				tab.SetSpriteOnMainTitleButton(_properties.ThemeMenuChanger.GetRedSprite());

				TrySendBroadcast(new NetMainTitleStateButton(TypesOfButtonState.Pause));
			}
			else
			{
				tab.SetSpriteOnMainTitleButton(_properties.ThemeMenuChanger.GetButtonSprite());

				TrySendBroadcast(new NetMainTitleStateButton(TypesOfButtonState.Default));
			}
		}
	}
	*/
	public void InteractableNextButton(bool isInteractable)
	{
		GameTab tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.Game) as GameTab;

		if (tab != null)
			tab.InteractableNextButton(isInteractable);

		UpdateClientTab(TypesOfTab.Game);
	}

	public void InteractableAnswerButtons(bool isInteractable)
	{
		GameTab tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.Game) as GameTab;

		if (tab != null)
			tab.InteractableAnswerButtons(isInteractable);

		UpdateClientTab(TypesOfTab.Game);
	}

	public void InteractableCountdownButton(bool isInteractable)
	{
		GameTab tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.Game) as GameTab;

		if (tab != null)
			tab.InteractableCountdownButton(isInteractable);

		UpdateClientTab(TypesOfTab.Game);
	}

	public void EnableOptionsButtons(int optionsCount)
	{
		GameTab tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.Game) as GameTab;

		if (tab != null)
		{
			tab.EnableOptionsButtons();
			tab.DisableUnusedOptionsButtons(optionsCount);
		}

		UpdateClientTab(TypesOfTab.Game);
	}

	public void DisableOptionsButtons()
	{
		GameTab tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.Game) as GameTab;

		if (tab != null)
			tab.DisableOptionsButtons();

		UpdateClientTab(TypesOfTab.Game);
	}
	#endregion

	#region TeamsTab
	private void ReplaceCurrentTeam(int newNumTeam)
	{
		_game.ReplaceCurrentTeam(newNumTeam);

		RefreshTeamsDropdown(_game.Teams);
	}

	private void ChangeTeamName(int numTeam, string newName)
	{
		_game.ChangeTeamName(numTeam, newName);

		RefreshTeamsDropdown(_game.Teams);
	}

	private void AddMoneyToTeam(int numTeam, string money)
	{
		if (int.TryParse(money.Replace(" ", string.Empty), out var intMoney))
		{
			_game.AddMoneyToTeamFromMenu(numTeam, intMoney);

			RefreshTeamsDropdown(_game.Teams);
		}
		else
		{
			RefreshTeamsDropdown(_game.Teams);

			WriteLog("It didn`t work out. Enter a number.");
		}
	}

	public void InteractableReplaceCurrentTeams(bool isInteractable)
	{
		TeamsTab tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.Teams) as TeamsTab;

		if (tab != null)
			tab.InteractableReplaceCurrentTeam(isInteractable);

		UpdateClientTab(TypesOfTab.Teams);
		//TrySendBroadcast(new NetInteractableReplaceCurrentTeam(isInteractable));
	}

	public void InteractableAddMoneyToTeam(bool isInteractable)
	{
		TeamsTab tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.Teams) as TeamsTab;

		if (tab != null)
			tab.InteractableAddMoney(isInteractable);

		UpdateClientTab(TypesOfTab.Teams);
	}

	public void InteractableChangeName(bool isInteractable)
	{
		TeamsTab tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.Teams) as TeamsTab;

		if (tab != null)
			tab.InteractableChangeName(isInteractable);

		UpdateClientTab(TypesOfTab.Teams);
	}

	public void RefreshTeamsDropdown(List<Team> teams)
	{

		_teamNames.Clear();

		for (int i = 0; i < teams.Count; i++)
		{
			_teamNames.Add($"{i + 1}. {teams[i].Name}");
		}

		foreach (var tab in Tabs)
		{
			if (tab.TypesMenuReadOnly == TypesOfTab.Teams)
			{
				TeamsTab TeamsTab = tab as TeamsTab;

				TeamsTab.RefreshTeamsDropdown(_teamNames);
			}
		}

		NetRefreshTeamsDropdown netRefreshTeamsDropdown = new NetRefreshTeamsDropdown(_teamNames);
		TrySendBroadcast(netRefreshTeamsDropdown);
	}

	#endregion

	#region PlayerTab

	protected override void OnPlayUntilPauseMark()
	{
		if (_currentPlayer == null)
			return;

		if (AreHotkeysBlocked)
			return;

		_currentPlayer.PlayUntilPauseMark(_currentPauseMark);
	}

	protected override void OnPlayAfterPauseMark()
	{
		if (_currentPlayer == null)
			return;

		if (AreHotkeysBlocked)
			return;

		_currentPlayer.PlayAfterPauseMark(_currentPauseMark);
	}

	protected override void OnPlayFull()
	{
		if (_currentPlayer == null)
			return;

		if (AreHotkeysBlocked)
			return;

		_currentPlayer.PlayFull();
	}

	protected override void OnPlayerPause()
	{
		if (_currentPlayer == null)
			return;

		if (AreHotkeysBlocked)
			return;

		_currentPlayer.Pause();
	}

	protected override void OnHotkeyPlayerLoop()
	{
		if (_currentPlayer == null)
			return;

		if (AreHotkeysBlocked)
			return;

		if (_currentPlayer.GetIsLoop())
			OnPlayerLoop(false);
		else
			OnPlayerLoop(true);
	}

	private void OnPlayerLoop(bool isLoop)
	{
		if (_currentPlayer == null)
			return;

		_currentPlayer.SetIsLoop(isLoop);

		PlayerTab tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.Player) as PlayerTab;
		if (tab != null)
			tab.SetPlayerLoopWithoutNotify(isLoop);

		NetPlayerLoop netPlayerLoop = new NetPlayerLoop(isLoop);
		TrySendBroadcast(netPlayerLoop);
	}

	private void WriteOnPauseMarkField(string pauseMark)
	{
		if (CurrentPlayerTab == null)
			return;
		CurrentPlayerTab.WriteOnPauseMarkField(pauseMark);

		TrySendBroadcast(new NetDisplayPauseMark(pauseMark));
	}

	private void WriteOnTimeDisplay(string time)
	{
		if (CurrentPlayerTab == null)
			return;

		CurrentPlayerTab.WriteOnTimeDisplay(time);

		TrySendBroadcast(new NetDisplayPlayerPlayback(time));
	}

	private void OnSetPauseMark(string pauseMark)
	{
		//pauseMark = PreparationNormalizedPauseMark(pauseMark);

		if (TryGetConvertStringToFloat(pauseMark, out float pauseMarkFloat))
		{
			if (IsNormalizedPauseMark(pauseMarkFloat))
			{
				_game.SetPauseMark(pauseMarkFloat);

				_currentPauseMark = pauseMarkFloat;

				WriteOnPauseMarkField(pauseMarkFloat.ToString());
			}
			else
			{
				Debug.Log("The value must be between 0 and 1. Example: 0,55. Your: " + pauseMarkFloat);
				WriteLog("The value must be between 0 and 1. Example: 0,55. Your: " + pauseMarkFloat);

				WriteOnPauseMarkField(_currentPauseMark.ToString());
			}
		}
		else
		{
			Debug.Log("It is possible to enter only integers 0-9 and one \",\" or \".\"");
			WriteLog("It is possible to enter only integers 0-9 and one \",\" or \".\"");

			WriteOnPauseMarkField(_currentPauseMark.ToString());
		}
	}

	public void EnablePlayerButtons(IAdvancedPlayer player, float currentPauseMark)
	{
		_currentPlayer = player;
		_currentPauseMark = currentPauseMark;

		if (CurrentPlayerTab == null)
			return;

		_currentPlayer.SetIsLoop(false);
		CurrentPlayerTab.EnablePlayerButtons();
		WriteOnPauseMarkField(_currentPauseMark.ToString());
		WriteOnTimeDisplay("0");

		UpdateClientTab(TypesOfTab.Player);

		if (_updateTimeDisplayJob != null)
			StopCoroutine(_updateTimeDisplayJob);
		_updateTimeDisplayJob = StartCoroutine(UpdateTimeDisplayJob());
	}

	public void DisablePlayerButtons()
	{
		//CurrentPlayerTab.DisablePlayerButtons();

		PlayerTab tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.Player) as PlayerTab;

		if (tab != null)
			tab.DisablePlayerButtons();

		_currentPlayer = null;
		_currentPauseMark = 1;

		UpdateClientTab(TypesOfTab.Player);

		if (_updateTimeDisplayJob != null)
			StopCoroutine(_updateTimeDisplayJob);
	}

	private bool IsNormalizedPauseMark(float pauseMark)
	{
		if (pauseMark >= 0 && pauseMark <= 1)
			return true;
		else
			return false;
	}

	private IEnumerator UpdateTimeDisplayJob()
	{
		while (true)
		{
			if (_currentPlayer.GetIsPlaying())
				WriteOnTimeDisplay(string.Format("{0:0.000}", _currentPlayer.GetCurrentTime()));

			yield return null;
		}
	}

	#endregion

	#region PropertiesTabOnServer

	#region GameDisplayWindow

	private void OnRefreshGameDisplayInfo()
	{
		OnRefreshGameDisplayInfoInMenu(_properties.DisplayController.GetDisplayInfo(), _properties.DisplayController.GetIsFullscreen());
	}

	private void OnSetGameDisplayResolution(int width, int height)
	{
		_properties.DisplayController.ChangeResolution(width, height);
	}

	private void OnSwitchedGameDisplayFullscreen(bool isFullscreen)
	{
		_properties.DisplayController.SetFullscreen(isFullscreen);
	}

	private void OnRefreshGameDisplayInfoInMenu(string displayInfo, bool isFullscreen)
	{
		PropertiesTabOnServer tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnServer) as PropertiesTabOnServer;
		if (tab != null)
			tab.RefreshGameDisplayInfoInMenu(displayInfo, isFullscreen);

		NetGameDisplayInfo netDisplayInfo = new NetGameDisplayInfo(displayInfo, isFullscreen);
		TrySendBroadcast(netDisplayInfo);
	}

	protected override void OnSwitchFullscreenHotkey()
	{
		OnSwitchedGameDisplayFullscreen(!_properties.DisplayController.GetIsFullscreen());
	}

	#endregion

	#region GameTextsWindows

	private void OnRefreshGameTexts()
	{
		OnRefreshGameTextsInMenus(_game.GetName(), _game.GetFinalText(), _properties.MonetaryUnits);
	}

	private void OnRefreshGameTextsInMenus(string name, string finalText, char[] monetaryUnits)
	{
		PropertiesTabOnServer tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnServer) as PropertiesTabOnServer;
		if (tab != null)
			tab.SetGameTexts(name, finalText, monetaryUnits);

		NetGameTextsWindow netGameTextsWindow = new NetGameTextsWindow(name, finalText, monetaryUnits);
		TrySendBroadcast(netGameTextsWindow);
	}

	private void OnChangedGameTexts(string name, string finalText)
	{
		_game.TryChangeGameText(name, finalText);
	}

	private void OnChangedCurentMonetaryUnit(char MonetaryUnit)
	{
		_game.OnChangedCurentMonetaryUnit(MonetaryUnit);
	}

	#endregion

	#region LoadImageWindows

	private void OnChangedLogo(Texture2D logo, string path)
	{

		PropertiesTabOnServer tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnServer) as PropertiesTabOnServer;
		if (tab != null)
			tab.ChangeLogoInfo(logo, path);

		TrySendBroadcast(new NetUserLoadedLogo(path));
	}

	private void OnUserLoadedLogo(Texture2D logo, string path)
	{
		_properties.GameColorChanger.UserLoadedLogo(logo, path);
	}

	private void OnDeleteLogo()
	{
		_properties.GameColorChanger.DeleteLogo();
	}

	#endregion

	#region ColorWindows

	private void SetColorsFromPalette(List<ColorPalette> palettes, int currentNumPalette)
	{
		//в Debug Mode обратил внимание, что несколько раз попадаю сюда подряд, когда выбираю новую палитру в меню.
		PropertiesTabOnServer tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnServer) as PropertiesTabOnServer;
		if (tab != null)
			tab.SetColorsFromPalette(palettes, currentNumPalette);

		TrySendBroadcast(new NetColorPalettes(palettes, currentNumPalette));
	}

	private void OnChangedPalette(int numPalette)
	{
		_properties.GameColorChanger.ChangePalette(numPalette);
	}

	private void OnChangedColorInPalette(int numPalette, TypesOfGameColor gameColor, Color color)
	{
		_properties.GameColorChanger.ChangeColorInPalette(numPalette, gameColor, color);
	}


	private void RefreshPalettesOnClient()
	{
		List<ColorPalette> palettes = _properties.GameColorChanger.GetPalettes(out int currentNumPalette);

		TrySendBroadcast(new NetColorPalettes(palettes, currentNumPalette));
	}

	#endregion

	#region AudioMixerMenu

	private void OnChangedAudioChannel(TypesOfAudioChannel channel, float normalizeValue)
	{
		_audioMixer.SetChannel(channel, normalizeValue);
	}

	private void OnSetAudioChannelInMenu(TypesOfAudioChannel channel, float normalizeValue)
	{
		PropertiesTabOnServer tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnServer) as PropertiesTabOnServer;
		if (tab != null)
			tab.SetChannel(channel, normalizeValue);

		NetSetAudioChannel netSetAudioChannel = new NetSetAudioChannel(channel, normalizeValue);
		TrySendBroadcast(netSetAudioChannel);
	}

	private void OnChannelDoubleClicked(TypesOfAudioChannel channel)
	{
		OnChangedAudioChannel(channel, _audioMixer.GetNormalizeValue(0));
	}

	private void ResetAudioMixer()
	{
		OnChangedAudioChannel(TypesOfAudioChannel.Master, _audioMixer.GetNormalizeValue(0));
		OnChangedAudioChannel(TypesOfAudioChannel.Fx, _audioMixer.GetNormalizeValue(0));
		OnChangedAudioChannel(TypesOfAudioChannel.Countown, _audioMixer.GetNormalizeValue(0));
		OnChangedAudioChannel(TypesOfAudioChannel.Question, _audioMixer.GetNormalizeValue(0));
		OnChangedAudioChannel(TypesOfAudioChannel.Music, _audioMixer.GetNormalizeValue(0));
	}

	#endregion

	#region ServerMenu

	private void OnConnectButton(string port)
	{
		if (_gameServer.Server.IsActive)
		{
			_gameServer.Server.Shutdown();
		}
		else
		{
			if (TryConvertStringToUshort(port, out ushort serverPort))
				_gameServer.Server.Init(serverPort);
			else
				WriteLog("No good port");
		}
	}

	private void NoClientConnection()
	{
		PropertiesTabOnServer tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnServer) as PropertiesTabOnServer;
		if (tab != null)
			tab.SetSpriteOnConnectButton(_properties.ThemeMenuChanger.GetYellowSprite());

		ConnectButtonIndicator.image.sprite = _properties.ThemeMenuChanger.GetYellowSprite();
	}

	private void HaveGotClientConnection()
	{
		PropertiesTabOnServer tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnServer) as PropertiesTabOnServer;
		if (tab != null)
			tab.SetSpriteOnConnectButton(_properties.ThemeMenuChanger.GetGreenSprite());

		ConnectButtonIndicator.image.sprite = _properties.ThemeMenuChanger.GetGreenSprite();
	}

	private void ShutdownServer()
	{
		PropertiesTabOnServer tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnServer) as PropertiesTabOnServer;
		if (tab != null)
			tab.SetSpriteOnConnectButton(_properties.ThemeMenuChanger.GetRedSprite());

		ConnectButtonIndicator.image.sprite = _properties.ThemeMenuChanger.GetRedSprite();
	}

	#endregion

	#endregion

	#region Server

	private void RegisterServerEvents()
	{
		NetUtility.S_WELCOME += OnWelcomeServer;
		NetUtility.S_TAB_REFRESH += OnTabRefreshServer;

		//PreviewMenu
		NetUtility.S_REQUEST_PREVIEW_TEXTURE += OnRequestPreviewTexture;

		//GameTab
		NetUtility.S_NEXT_TITLE += OnNextTitleButtonServer;
		//NetUtility.S_INTERACTABLE_NEXT_BUTTON += OnIntaractibleNextButtonServer;
		NetUtility.S_MAIN_TITLE += OnMainTitleButtonServer;
		NetUtility.S_TEAM_TITLE += OnTeamsTitleButtonServer;
		NetUtility.S_RIGHT_ANSWER += OnRightAnswerButtonServer;
		NetUtility.S_WRONG_ANSWER += OnWrongAnswerButtonServer;
		NetUtility.S_FLASH_LIGHT += OnFlashButtonServer;
		NetUtility.S_FIREWORKS += OnFireworksButtonServer;
		NetUtility.S_FORCE_ON_PRIMITIVES += OnForceOnPrimitivesServer;
		NetUtility.S_RESTART_PRIMITIVES += OnRestartPrimitivesServer;
		NetUtility.S_START_COUNTDOWN += OnStartCountdownButtonServer;
		NetUtility.S_OPTION += OnOptionButtonServer;

		//TeamsTab
		NetUtility.S_REPLACE_CURRENT_TEAM += OnReplaceCurrentTeamServer;
		NetUtility.S_CHANGE_TEAM_NAME += OnChangeTeamNameServer;
		NetUtility.S_ADD_MONEY_TO_TEAM += OnAddMoneyToTeamServer;

		//PlayerTab
		NetUtility.S_PLAY_UNTIL_MARK += OnPlayUntilMarkServer;
		NetUtility.S_PLAY_AFTER_MARK += OnPlayAfterMarkServer;
		NetUtility.S_PLAY_FULL += OnPlayFullServer;
		NetUtility.S_PLAYER_PAUSE += OnPlayerPauseServer;
		NetUtility.S_PLAYER_LOOP += OnPlayerLoopServer;
		NetUtility.S_SET_PAUSE_MARK += OnSetPauseMarkServer;

		//PropertiesTab
		NetUtility.S_GAME_DISPLAY_INFO += OnGameDisplayInfoServer;
		NetUtility.S_SET_GAME_RESOLUTION += OnSetResolutionServer;
		NetUtility.S_GAME_FULLSCREEN += OnFullscreenServer;

		NetUtility.S_REFRESH_PALETTES += OnNeedRefreshPalettesServer;
		NetUtility.S_CHANGE_PALETTE += OnChangePaletteOnServer;
		NetUtility.S_CHANGE_COLOR_IN_PALETTE += OnChangeColorInPaletteOnServer;

		NetUtility.S_REFRESH_LOGO += OnRefreshLogoServer;
		NetUtility.S_USER_LOADED_LOGO += OnUserLoadedLogoServer;
		NetUtility.S_USER_DELETED_LOGO += OnUserDeletedLogoServer;

		NetUtility.S_GAME_TEXTS += OnRefreshGameTextServer;
		NetUtility.S_CHANGED_GAME_TEXTS += OnChangedGameTextServer;

		NetUtility.S_MONETARY_UNIT += OnChangedMonetaryUnitServer;

		NetUtility.S_SET_AUDIO_CHANNEL += OnSetAudioChannelServer;
		NetUtility.S_UPDATE_ALL_AUDIO_CHANNELS += OnUpdateVolumeChannelsServer;
		NetUtility.S_AUDIO_CHANNEL_DOUBLE_CLICKED += OnAudioChannelDoubleClickedServer;
	}

	private void UnregisterEvents()
	{
		NetUtility.S_WELCOME -= OnWelcomeServer;
		NetUtility.S_TAB_REFRESH -= OnTabRefreshServer;

		//PreviewMenu
		NetUtility.S_REQUEST_PREVIEW_TEXTURE -= OnRequestPreviewTexture;

		//GameTab
		NetUtility.S_NEXT_TITLE -= OnNextTitleButtonServer;
		//NetUtility.S_INTERACTABLE_NEXT_BUTTON -= OnIntaractibleNextButtonServer;
		NetUtility.S_MAIN_TITLE -= OnMainTitleButtonServer;
		NetUtility.S_TEAM_TITLE -= OnTeamsTitleButtonServer;
		NetUtility.S_RIGHT_ANSWER -= OnRightAnswerButtonServer;
		NetUtility.S_WRONG_ANSWER -= OnWrongAnswerButtonServer;
		NetUtility.S_FLASH_LIGHT -= OnFlashButtonServer;
		NetUtility.S_FORCE_ON_PRIMITIVES -= OnForceOnPrimitivesServer;
		NetUtility.S_RESTART_PRIMITIVES -= OnRestartPrimitivesServer;
		NetUtility.S_FIREWORKS -= OnFireworksButtonServer;
		NetUtility.S_START_COUNTDOWN -= OnStartCountdownButtonServer;
		NetUtility.S_OPTION -= OnOptionButtonServer;

		//TeamsTab
		NetUtility.S_REPLACE_CURRENT_TEAM -= OnReplaceCurrentTeamServer;
		NetUtility.S_CHANGE_TEAM_NAME -= OnChangeTeamNameServer;
		NetUtility.S_ADD_MONEY_TO_TEAM -= OnAddMoneyToTeamServer;

		//PlayerTab
		NetUtility.S_PLAY_UNTIL_MARK -= OnPlayUntilMarkServer;
		NetUtility.S_PLAY_AFTER_MARK -= OnPlayAfterMarkServer;
		NetUtility.S_PLAY_FULL -= OnPlayFullServer;
		NetUtility.S_PLAYER_PAUSE -= OnPlayerPauseServer;
		NetUtility.S_PLAYER_LOOP -= OnPlayerLoopServer;
		NetUtility.S_SET_PAUSE_MARK -= OnSetPauseMarkServer;

		//PropertiesTab
		NetUtility.S_GAME_DISPLAY_INFO -= OnGameDisplayInfoServer;
		NetUtility.S_SET_GAME_RESOLUTION -= OnSetResolutionServer;
		NetUtility.S_GAME_FULLSCREEN -= OnFullscreenServer;

		NetUtility.S_REFRESH_PALETTES -= OnNeedRefreshPalettesServer;
		NetUtility.S_CHANGE_PALETTE -= OnChangePaletteOnServer;
		NetUtility.S_CHANGE_COLOR_IN_PALETTE -= OnChangeColorInPaletteOnServer;

		NetUtility.S_REFRESH_LOGO -= OnRefreshLogoServer;
		NetUtility.S_USER_LOADED_LOGO -= OnUserLoadedLogoServer;
		NetUtility.S_USER_DELETED_LOGO -= OnUserDeletedLogoServer;

		NetUtility.S_GAME_TEXTS -= OnRefreshGameTextServer;
		NetUtility.S_CHANGED_GAME_TEXTS -= OnChangedGameTextServer;

		NetUtility.S_MONETARY_UNIT -= OnChangedMonetaryUnitServer;

		NetUtility.S_SET_AUDIO_CHANNEL -= OnSetAudioChannelServer;
		NetUtility.S_UPDATE_ALL_AUDIO_CHANNELS -= OnUpdateVolumeChannelsServer;
		NetUtility.S_AUDIO_CHANNEL_DOUBLE_CLICKED -= OnAudioChannelDoubleClickedServer;
	}

	private void TrySendBroadcast(NetMessage msg)
	{
		if (_gameServer.Server.IsActive && _gameServer.Server.CheckingClientConnections())
			_gameServer.Server.Broadcast(msg);
	}

	private void OnWelcomeServer(NetMessage msg, NetworkConnection networkConnection)
	{
		// Client has connected, assign a team and return the message back to him
		NetWelcome netWelcome = msg as NetWelcome;

		// Assign a team
		netWelcome.AssignedClient = ++_clientCount;

		// Return back to the client
		Server.Instance.SendToClient(networkConnection, netWelcome);
	}

	#region PreviewMenu

	private void OnRequestPreviewTexture(NetMessage msg, NetworkConnection networkConnection)
	{
		NetRequestPreviewTexture netRequestPreviewTexture = msg as NetRequestPreviewTexture;

		_currentWidth = netRequestPreviewTexture.Width;
		_currentHeight = netRequestPreviewTexture.Height;

		_currentPreviewTexture = _properties.TextureConverter.GetRawTextureDataFromRenderTexture(_properties.PreviewRenderTexture,
			_currentWidth, _currentHeight);


		_currentNativeArray = NativeArrayExtension.FromRawBytes<byte>(_currentPreviewTexture, Allocator.Temp);

		_gameServer.Server.SendToClient(networkConnection, new NetSendPreviewTexture(_currentNativeArray, _currentWidth, _currentHeight));
		//TrySendBroadcast(new NetSendPreviewTexture(_currentPreviewTexture, _currentWidth, _currentHeight));
		//_gameServer.Server.SendToClient(networkConnection, new NetSendPreviewTexture(_currentPreviewTexture, _currentWidth, _currentHeight));
	}

	#endregion

	#region GameTab

	private void OnNextTitleButtonServer(NetMessage msg, NetworkConnection networkConnection)
	{
		OnNextTitleButton();
	}

	private void OnMainTitleButtonServer(NetMessage msg, NetworkConnection networkConnection)
	{
		OnMainTitleButton();
	}

	private void OnTeamsTitleButtonServer(NetMessage msg, NetworkConnection networkConnection)
	{
		OnTeamsTitleButton();
	}

	private void OnRightAnswerButtonServer(NetMessage msg, NetworkConnection networkConnection)
	{
		OnRightAnswerButton();
	}

	private void OnWrongAnswerButtonServer(NetMessage msg, NetworkConnection networkConnection)
	{
		OnWrongAnswerButton();
	}

	private void OnFlashButtonServer(NetMessage msg, NetworkConnection networkConnection)
	{
		OnFlashButton();
	}

	private void OnFireworksButtonServer(NetMessage msg, NetworkConnection networkConnection)
	{
		OnFireworksButton();
	}

	private void OnForceOnPrimitivesServer(NetMessage msg, NetworkConnection networkConnection)
	{
		NetForceOnPrimitives netForceOnPrimitives = msg as NetForceOnPrimitives;

		OnForceOnPrimitives(netForceOnPrimitives.Value, netForceOnPrimitives.IsInside);
	}

	private void OnRestartPrimitivesServer(NetMessage msg, NetworkConnection networkConnection)
	{
		OnRestartPrimitives();
	}

	private void OnStartCountdownButtonServer(NetMessage msg, NetworkConnection networkConnection)
	{
		OnStartCountdown();
	}

	private void OnOptionButtonServer(NetMessage msg, NetworkConnection networkConnection)
	{
		NetOption netOption = msg as NetOption;

		switch (netOption.Option)
		{
			case 1:
				OnOption1Button();
				break;
			case 2:
				OnOption2Button();
				break;
			case 3:
				OnOption3Button();
				break;
			case 4:
				OnOption4Button();
				break;
			case 5:
				OnOption5Button();
				break;
			default:
				WriteLog($"Option {netOption.Option} not found");
				break;
		}
	}

	private void OnTabRefreshServer(NetMessage msg, NetworkConnection networkConnection)
	{
		NetTabRefresh netTabRefresh = msg as NetTabRefresh;

		switch (netTabRefresh.Type)
		{
			case TypesOfTab.Game:
				UpdateClientTab(TypesOfTab.Game);
				break;
			case TypesOfTab.Teams:
				UpdateClientTab(TypesOfTab.Teams);
				break;
			case TypesOfTab.Player:
				UpdateClientTab(TypesOfTab.Player);
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
	private void OnIntaractibleNextButtonServer(NetMessage msg, NetworkConnection networkConnection)
	{
		WriteLog("Server: OnIntaractibleNextButtonServer");
	}
	*/

	#endregion

	#region TeamsTab

	private void OnReplaceCurrentTeamServer(NetMessage msg, NetworkConnection networkConnection)
	{
		NetReplaceCurrentTeam netReplaceCurrentTeam = msg as NetReplaceCurrentTeam;

		if (netReplaceCurrentTeam.NewNumTeam < _game.Teams.Count)
			ReplaceCurrentTeam(netReplaceCurrentTeam.NewNumTeam);
		else
			WriteLog($"The number {netReplaceCurrentTeam.NewNumTeam + 1} more than present Teams");
	}

	private void OnChangeTeamNameServer(NetMessage msg, NetworkConnection networkConnection)
	{
		NetChangeTeamName netChangeTeamName = msg as NetChangeTeamName;

		if (netChangeTeamName.NewNumTeam < _game.Teams.Count)
			ChangeTeamName(netChangeTeamName.NewNumTeam, netChangeTeamName.NewNameTeam);
		else
			WriteLog($"The number {netChangeTeamName.NewNumTeam + 1} more than present Teams");
	}

	private void OnAddMoneyToTeamServer(NetMessage msg, NetworkConnection networkConnection)
	{
		NetAddMoneyToTeam addMoneyToTeam = msg as NetAddMoneyToTeam;

		if (addMoneyToTeam.NumTeam < _game.Teams.Count)
			AddMoneyToTeam(addMoneyToTeam.NumTeam, addMoneyToTeam.Money);
		else
			WriteLog($"The number {addMoneyToTeam.NumTeam + 1} more than present Teams");
	}

	#endregion

	#region PlayerTab

	private void OnPlayUntilMarkServer(NetMessage msg, NetworkConnection networkConnection)
	{
		OnPlayUntilPauseMark();
	}

	private void OnPlayAfterMarkServer(NetMessage msg, NetworkConnection networkConnection)
	{
		OnPlayAfterPauseMark();
	}

	private void OnPlayFullServer(NetMessage msg, NetworkConnection networkConnection)
	{
		OnPlayFull();
	}

	private void OnPlayerPauseServer(NetMessage msg, NetworkConnection networkConnection)
	{
		OnPlayerPause();
	}

	private void OnPlayerLoopServer(NetMessage msg, NetworkConnection networkConnection)
	{
		NetPlayerLoop netPlayerLoop = msg as NetPlayerLoop;

		OnHotkeyPlayerLoop();
	}

	private void OnSetPauseMarkServer(NetMessage msg, NetworkConnection networkConnection)
	{
		NetSetPauseMark netSetPauseMark = msg as NetSetPauseMark;

		OnSetPauseMark(netSetPauseMark.PauseMark);
	}

	#endregion

	#region Properties

	private void OnChangedMonetaryUnitServer(NetMessage msg, NetworkConnection networkConnection)
	{
		NetMonetaryUnit netMonetaryUnit = msg as NetMonetaryUnit;

		OnChangedCurentMonetaryUnit(netMonetaryUnit.MonetaryUnit);
	}

	private void OnSetAudioChannelServer(NetMessage msg, NetworkConnection networkConnection)
	{
		NetSetAudioChannel netSetNormalizeVolume = msg as NetSetAudioChannel;

		OnChangedAudioChannel(netSetNormalizeVolume.Channel, netSetNormalizeVolume.NormalizeVolume);
	}

	private void OnUpdateVolumeChannelsServer(NetMessage msg, NetworkConnection networkConnection)
	{
		NetUpdateAllAudioChannels netUpdateVolumeChannels = new NetUpdateAllAudioChannels(_audioMixer.NormalizeMasterVolume,
			_audioMixer.NormalizeFxVolume, _audioMixer.NormalizeCountdownVolume,
			_audioMixer.NormalizeQuestionVolume, _audioMixer.NormalizeMusicVolume);

		TrySendBroadcast(netUpdateVolumeChannels);
	}

	private void OnAudioChannelDoubleClickedServer(NetMessage msg, NetworkConnection networkConnection)
	{
		NetDoubleClickAudioChannel netDoubleClickAudioChannel = msg as NetDoubleClickAudioChannel;

		OnChannelDoubleClicked(netDoubleClickAudioChannel.Channel);
	}

	private void OnSetResolutionServer(NetMessage msg, NetworkConnection networkConnection)
	{
		NetSetGameDisplayResolution netSetResolution = msg as NetSetGameDisplayResolution;

		OnSetGameDisplayResolution(netSetResolution.Width, netSetResolution.Height);
	}

	private void OnFullscreenServer(NetMessage msg, NetworkConnection networkConnection)
	{
		NetGameDisplayFullscreen netFullscreen = msg as NetGameDisplayFullscreen;

		OnSwitchedGameDisplayFullscreen(netFullscreen.IsFullscreen);
	}

	private void OnGameDisplayInfoServer(NetMessage msg, NetworkConnection networkConnection)
	{
		OnRefreshGameDisplayInfo();
	}

	private void OnChangePaletteOnServer(NetMessage msg, NetworkConnection networkConnection)
	{
		NetChangePalette netChangePalette = msg as NetChangePalette;

		OnChangedPalette(netChangePalette.NumPalette);
	}

	private void OnChangeColorInPaletteOnServer(NetMessage msg, NetworkConnection networkConnection)
	{
		NetChangeColorInPalette netChangeColorInPalette = msg as NetChangeColorInPalette;

		OnChangedColorInPalette(netChangeColorInPalette.NumPalette, netChangeColorInPalette.GameTransferColor,
			netChangeColorInPalette.TransferColor);
	}
	
	private void OnNeedRefreshPalettesServer(NetMessage msg, NetworkConnection networkConnection)
	{
		RefreshPalettesOnClient();
	}
	
	private void OnRefreshGameTextServer(NetMessage msg, NetworkConnection networkConnection)
	{
		TrySendBroadcast(new NetGameTextsWindow(_game.Name, _game.FinalTitleText, _properties.MonetaryUnits));
	}

	private void OnChangedGameTextServer(NetMessage msg, NetworkConnection networkConnection)
	{
		NetChangedGameTexts netChangedGameTexts = msg as NetChangedGameTexts;

		OnChangedGameTexts(netChangedGameTexts.Name, netChangedGameTexts.FinalText);
	}

	private void OnRefreshLogoServer(NetMessage msg, NetworkConnection networkConnection)
	{
		_properties.GameColorChanger.TryGetLogo(out string path);

		TrySendBroadcast(new NetUserLoadedLogo(path));

	}

	private void OnUserLoadedLogoServer(NetMessage msg, NetworkConnection networkConnection)
	{
		NetUserLoadedLogo netUserLoadedLogo = msg as NetUserLoadedLogo;

		PropertiesTabOnServer tab = Tabs.Find(t => t.TypesMenuReadOnly == TypesOfTab.PropertiesOnServer) as PropertiesTabOnServer;
		if (tab != null)
		{
			if (tab.TryGetTextureFromLocalPath(netUserLoadedLogo.Path, out Texture2D texture))
				OnUserLoadedLogo(texture, netUserLoadedLogo.Path);
		}
	}


	private void OnUserDeletedLogoServer(NetMessage msg, NetworkConnection networkConnection)
	{
		OnDeleteLogo();
	}

	#endregion

	#endregion

	private IEnumerator ScreenShotJob()
	{
		yield return _waitForEndOfFrame;

		Texture2D screenshot = new Texture2D(455, 256, TextureFormat.RGB24, false);

		screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

		//byte[] bytes = screenыhot.EncodeToJPG();

		//File.WriteAllBytes(Application.dataPath + "/Textures/Screenshot/ScreenshotTexture.jpg", bytes);

		//_gameServer.Server.Broadcast(new NetScreenshot(bytes));
		_gameServer.Server.Broadcast(new NetScreenshot(screenshot, "Нажал на Shot"));

		if (_screenShotJob != null)
			StopCoroutine(_screenShotJob);
	}

	private void ScreenShot()
	{
		Texture2D screenshot = new Texture2D(455, 256, TextureFormat.RGB24, false);

		screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

		//TryToSendBroadcast();
		if (_gameServer.Server.IsActive && _gameServer.Server.CheckingClientConnections())
		{
			if (_screenShotJob != null)
				StopCoroutine(_screenShotJob);
			_screenShotJob = StartCoroutine(ScreenShotJob());
		}
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

	private bool TryGetConvertStringToFloat(string pauseMark, out float pauseMarkFloat)
	{
		if (float.TryParse(pauseMark, out pauseMarkFloat))
			return true;
		else
			return false;
	}
}