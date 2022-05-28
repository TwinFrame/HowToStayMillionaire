using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmImageDeletingWindow : MenuWindow
{
	[SerializeField] private Button _quitButton;
	[SerializeField] private RawImage _image;
	[SerializeField] private Button _yesButton;
	[SerializeField] private Button _noButton;

	public UnityAction OnCloseWindowEvent;
	public UnityAction OnDeleteLogoEvent;

	private void OnEnable()
	{
		_quitButton.onClick.AddListener(CloseWindow);
		_yesButton.onClick.AddListener(DeleteLogo);
		_noButton.onClick.AddListener(CloseWindow);
	}

	private void OnDisable()
	{
		_quitButton.onClick.RemoveListener(CloseWindow);
		_yesButton.onClick.RemoveListener(DeleteLogo);
		_noButton.onClick.RemoveListener(CloseWindow);
	}

	public void SetImage(Texture image)
	{
		_image.texture = image;
	}

	private void CloseWindow()
	{
		OnCloseWindowEvent?.Invoke();
	}

	private void DeleteLogo()
	{
		OnDeleteLogoEvent?.Invoke();
	}
}
