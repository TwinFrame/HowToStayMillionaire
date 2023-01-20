using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmQuitWindow : MenuWindow
{
	[SerializeField] private Button _quitButton;
	[SerializeField] private Button _yesButton;
	[SerializeField] private Button _noButton;

	public UnityAction ClosedWindowEvent;
	public UnityAction QuitEvent;

	private void OnEnable()
	{
		_quitButton.onClick.AddListener(CloseWindow);
		_yesButton.onClick.AddListener(QuitApplication);
		_noButton.onClick.AddListener(CloseWindow);
	}

	private void OnDisable()
	{
		_quitButton.onClick.RemoveListener(CloseWindow);
		_yesButton.onClick.RemoveListener(QuitApplication);
		_noButton.onClick.RemoveListener(CloseWindow);
	}

	private void CloseWindow()
	{
		ClosedWindowEvent?.Invoke();
	}

	private void QuitApplication()
	{
		QuitEvent?.Invoke();
	}
}
