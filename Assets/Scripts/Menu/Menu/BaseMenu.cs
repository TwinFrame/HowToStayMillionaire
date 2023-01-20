using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ShowTextForTime))]

public abstract class BaseMenu : MonoBehaviour
{
	[Header("Tab Buttons")]
	[SerializeField] protected List<TabButton> TabButtons;
	[Header("Tabs")]
	[SerializeField] protected List<BaseTab> Tabs;
	[Header("Header")]
	[SerializeField] protected Button ConnectButtonIndicator;
	[SerializeField] protected Button ThemeButton;
	[Space]
	[SerializeField] private Button _quitButton;
	[SerializeField] private ConfirmQuitWindow _quitWindows;
	[Header("Log Menu")]
	[SerializeField] protected LogMenu LogMenu;

	protected ShowTextForTime ShowTextForTime;
	protected PlayerTab CurrentPlayerTab;
	protected TypesOfTab OpenedTypesTab;

	private InputGame _inputs;
	private bool _areHotkeysBlocked = false;

	public bool AreHotkeysBlocked => _areHotkeysBlocked;

	private void Awake()
	{
		ShowTextForTime = GetComponent<ShowTextForTime>();
	}

	protected void OnEnable()
	{
		_quitButton.onClick.AddListener(OpenQuitWindows);
		_quitWindows.ClosedWindowEvent += OnCloseQuitWindow;
		_quitWindows.QuitEvent += QuitApplication;

		ThemeButton.onClick.AddListener(ChangeThemeMenu);
		ConnectButtonIndicator.onClick.AddListener(OnClickConnectFromHeader);

		LogMenu.ClearedLogEvent += ClearLog;

		_inputs = new InputGame();
		_inputs.Enable();
		_inputs.Game.NextTitle.performed += ctx => OnNextTitleButton();
		_inputs.Game.Pause.performed += ctx => OnMainTitleButton();
		_inputs.Game.TeamsTitle.performed += ctx => OnTeamsTitleButton();
		_inputs.Game.MarkDone.performed += ctx => OnRightAnswerButton();
		_inputs.Game.MarkNotDone.performed += ctx => OnWrongAnswerButton();
		_inputs.Game.StartCountdown.performed += ctx => OnStartCountdown();
		_inputs.Game.Option1.performed += ctx => OnOption1Button();
		_inputs.Game.Option2.performed += ctx => OnOption2Button();
		_inputs.Game.Option3.performed += ctx => OnOption3Button();
		_inputs.Game.Option4.performed += ctx => OnOption4Button();
		_inputs.Game.Option5.performed += ctx => OnOption5Button();
		_inputs.Game.Fireworks.performed += ctx => OnFireworksButton();
		_inputs.Game.Flash.performed += ctx => OnFlashButton();
		_inputs.Game.PlayUntilPauseMark.performed += ctx => OnPlayUntilPauseMark();
		_inputs.Game.PlayAfterPauseMark.performed += ctx => OnPlayAfterPauseMark();
		_inputs.Game.PlayFull.performed += ctx => OnPlayFull();
		_inputs.Game.PlayerPause.performed += ctx => OnPlayerPause();
		_inputs.Game.PlayerLoop.performed += ctx => OnHotkeyPlayerLoop();
		_inputs.Game.Fullscreen.performed += ctx => OnSwitchFullscreenHotkey();
	}

	protected void OnDisable()
	{
		_quitButton.onClick.RemoveListener(OpenQuitWindows);
		_quitWindows.ClosedWindowEvent -= OnCloseQuitWindow;
		_quitWindows.QuitEvent -= QuitApplication;

		ThemeButton.onClick.RemoveListener(ChangeThemeMenu);
		ConnectButtonIndicator.onClick.RemoveListener(OnClickConnectFromHeader);

		LogMenu.ClearedLogEvent -= ClearLog;

		_inputs.Game.NextTitle.performed -= ctx => OnNextTitleButton();
		_inputs.Game.Pause.performed -= ctx => OnMainTitleButton();
		_inputs.Game.TeamsTitle.performed -= ctx => OnTeamsTitleButton();
		_inputs.Game.MarkDone.performed -= ctx => OnRightAnswerButton();
		_inputs.Game.MarkNotDone.performed -= ctx => OnWrongAnswerButton();
		_inputs.Game.StartCountdown.performed -= ctx => OnStartCountdown();
		_inputs.Game.Option1.performed -= ctx => OnOption1Button();
		_inputs.Game.Option2.performed -= ctx => OnOption2Button();
		_inputs.Game.Option3.performed -= ctx => OnOption3Button();
		_inputs.Game.Option4.performed -= ctx => OnOption4Button();
		_inputs.Game.Option5.performed -= ctx => OnOption5Button();
		_inputs.Game.Fireworks.performed -= ctx => OnFireworksButton();
		_inputs.Game.Flash.performed -= ctx => OnFlashButton();
		_inputs.Game.PlayUntilPauseMark.performed -= ctx => OnPlayUntilPauseMark();
		_inputs.Game.PlayAfterPauseMark.performed -= ctx => OnPlayAfterPauseMark();
		_inputs.Game.PlayFull.performed -= ctx => OnPlayFull();
		_inputs.Game.PlayerPause.performed -= ctx => OnPlayerPause();
		_inputs.Game.PlayerLoop.performed -= ctx => OnHotkeyPlayerLoop();
		_inputs.Game.Fullscreen.performed -= ctx => OnSwitchFullscreenHotkey();
	}

	protected void Start()
	{
		DeselectAllTabButtons();
		SelectTabButton(TypesOfTab.Game);

		CloseAllTabs();
		OpenTab(TypesOfTab.Game);

		RefreshAllTabs();
	}

	public abstract ConnectState GetConnectState();

	protected virtual void BlockHotkey()
	{
		WriteLog("Горячие клавиши заблокированы.");
		_areHotkeysBlocked = true;
	}

	protected virtual void UnblockHotkey()
	{
		WriteLog("Горячие клавиши разблокированы.");
		_areHotkeysBlocked = false;
	}

	#region Tabs Buttons

	protected abstract void DeselectAllTabButtons();

	protected abstract void SelectTabButton(TypesOfTab typesMenu);

	protected abstract void OpenTab(TypesOfTab typesMenu);

	protected void ClickMenuButton(TypesOfTab typesMenu)
	{
		DeselectAllTabButtons();
		SelectTabButton(typesMenu);

		CloseAllTabs();
		OpenTab(typesMenu);
	}

	protected void CloseAllTabs()
	{
		foreach (var tab in Tabs)
			tab.gameObject.SetActive(false);
	}

	protected void RefreshAllTabs()
	{
		foreach (var tab in Tabs)
			tab.RefreshTab();
	}

	#endregion

	#region Header

	protected abstract void ChangeThemeMenu();

	protected abstract void OnClickConnectFromHeader();

	private void OpenQuitWindows()
	{
		_quitWindows.gameObject.SetActive(true);
	}

	private void OnCloseQuitWindow()
	{
		_quitWindows.gameObject.SetActive(false);
	}

	private void QuitApplication()
	{
		Application.Quit();
	}
	#endregion

	#region GameTab

	protected abstract void OnNextTitleButton();
	protected abstract void OnMainTitleButton();
	protected abstract void OnTeamsTitleButton();
	protected abstract void OnRightAnswerButton();
	protected abstract void OnWrongAnswerButton();
	protected abstract void OnStartCountdown();
	protected abstract void OnOption1Button();
	protected abstract void OnOption2Button();
	protected abstract void OnOption3Button();
	protected abstract void OnOption4Button();
	protected abstract void OnOption5Button();
	protected abstract void OnFireworksButton();
	protected abstract void OnFlashButton();
	protected abstract void OnPlayUntilPauseMark();
	protected abstract void OnPlayAfterPauseMark();
	protected abstract void OnPlayFull();
	protected abstract void OnPlayerPause();
	protected abstract void OnHotkeyPlayerLoop();
	protected abstract void OnSwitchFullscreenHotkey();

	#endregion

	#region Log Menu

	public abstract void WriteLog(string text);

	private void ClearLog()
	{
		ShowTextForTime.ClearText(LogMenu.LogInputField);
	}

	#endregion
}
