using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerTab : BaseTab
{
	[Space]
	[SerializeField] private Button _playUntilMarkButton;
	[SerializeField] private Button _playAfterMarkButton;
	[SerializeField] private Button _playFullButton;
	[SerializeField] private Button _pauseButton;
	[SerializeField] private Toggle _loopToggle;
	[SerializeField] private TMP_Text _timeDisplay;
	[SerializeField] private Button _setPauseMarkButton;
	[SerializeField] private TMP_InputField _currentPauseMark;

	public Button PlayUntilMarkButton => _playUntilMarkButton;
	public Button PlayAfterMarkButton => _playAfterMarkButton;
	public Button PlayFullButton => _playFullButton;
	public Button PauseButton => _pauseButton;

	public UnityAction PlayUntilPauseMarkEvent;
	public UnityAction PlayAfterPauseMarkEvent;
	public UnityAction PlayFullEvent;
	public UnityAction PauseEvent;
	public UnityAction<bool> LoopEvent;
	public UnityAction<string> SetPauseMarkEvent;
	public UnityAction BlockHotkeyEvent;
	public UnityAction UnblockHotkeyEvent;


	private void OnEnable()
	{
		_playUntilMarkButton.onClick.AddListener(PlayUntilPauseMark);
		_playAfterMarkButton.onClick.AddListener(PlayAfterPauseMark);
		_playFullButton.onClick.AddListener(PlayFull);
		_pauseButton.onClick.AddListener(PlayerPause);
		_loopToggle.onValueChanged.AddListener((isLoop) => PlayerLoop(isLoop));
		_setPauseMarkButton.onClick.AddListener(SetPauseMark);

		_currentPauseMark.onSelect.AddListener(OnBlockHotkey);
		_currentPauseMark.onDeselect.AddListener(OnUnblockHotkey);
	}

	private void OnDisable()
	{
		_playUntilMarkButton.onClick.RemoveListener(PlayUntilPauseMark);
		_playAfterMarkButton.onClick.RemoveListener(PlayAfterPauseMark);
		_playFullButton.onClick.RemoveListener(PlayFull);
		_pauseButton.onClick.RemoveListener(PlayerPause);
		_loopToggle.onValueChanged.RemoveListener((isLoop) => PlayerLoop(isLoop));
		_setPauseMarkButton.onClick.RemoveListener(SetPauseMark);

		_currentPauseMark.onSelect.RemoveListener(OnBlockHotkey);
		_currentPauseMark.onDeselect.RemoveListener(OnUnblockHotkey);
	}

	public override void RefreshTab()
	{
		_timeDisplay.text = string.Empty;
		_currentPauseMark.text = string.Empty;
	}

	public override bool[] GetInteractables()
	{
		return new bool[1] { _playUntilMarkButton.interactable };
	}

	public override void SetTabInteractables(bool[] isInteractables)
	{
		_playUntilMarkButton.interactable = isInteractables[0];
		_playAfterMarkButton.interactable = isInteractables[0];
		_playFullButton.interactable = isInteractables[0];
		_pauseButton.interactable = isInteractables[0];
		_loopToggle.interactable = isInteractables[0];
		_setPauseMarkButton.interactable = isInteractables[0];
		_currentPauseMark.interactable = isInteractables[0];

		_currentPauseMark.text = string.Empty;
		_timeDisplay.text = string.Empty;
	}

	public void EnablePlayerButtons()
	{
		_playUntilMarkButton.interactable = true;
		_playAfterMarkButton.interactable = true;
		_playFullButton.interactable = true;
		_pauseButton.interactable = true;
		_loopToggle.interactable = true;
		_setPauseMarkButton.interactable = true;
		_currentPauseMark.interactable = true;
		_currentPauseMark.text = string.Empty;
		_timeDisplay.text = string.Empty;

		_loopToggle.isOn = false;
	}

	public void DisablePlayerButtons()
	{
		_loopToggle.isOn = false;

		_playUntilMarkButton.interactable = false;
		_playAfterMarkButton.interactable = false;
		_playFullButton.interactable = false;
		_pauseButton.interactable = false;
		_loopToggle.interactable = false;
		_setPauseMarkButton.interactable = false;
		_currentPauseMark.interactable = false;
		_currentPauseMark.text = string.Empty;
		_timeDisplay.text = string.Empty;
	}

	public void WriteOnTimeDisplay(string time)
	{
		_timeDisplay.text = time;
	}

	public void WriteOnPauseMarkField(string pauseMark)
	{
		_currentPauseMark.text = pauseMark;
	}

	private void PlayUntilPauseMark()
	{
		PlayUntilPauseMarkEvent?.Invoke();
	}

	private void PlayAfterPauseMark()
	{
		PlayAfterPauseMarkEvent?.Invoke();
	}

	private void PlayFull()
	{
		PlayFullEvent?.Invoke();
	}

	private void PlayerPause()
	{
		PauseEvent?.Invoke();
	}

	public void SetPlayerLoopWithoutNotify(bool isLoop)
	{
		_loopToggle.SetIsOnWithoutNotify(isLoop);
	}

	private void PlayerLoop(bool isLoop)
	{
		LoopEvent?.Invoke(isLoop);
	}

	private void SetPauseMark()
	{
		SetPauseMarkEvent?.Invoke(_currentPauseMark.text);
	}

	private void OnBlockHotkey(string text)
	{
		BlockHotkeyEvent?.Invoke();
	}

	private void OnUnblockHotkey(string text)
	{
		UnblockHotkeyEvent?.Invoke();
	}
}
