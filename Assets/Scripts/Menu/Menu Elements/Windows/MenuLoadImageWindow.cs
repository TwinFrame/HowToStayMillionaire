using SimpleFileBrowser;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MenuLoadImageWindow : MenuWindow
{
	[SerializeField] private Button _quitButton;
	[SerializeField] private TMP_Text _displayPath;
	[SerializeField] private RawImage _logoImage;
	[SerializeField] private Button _loadButton;
	[SerializeField] private Button _deleteButton;
	[Space]
	[SerializeField] private ConfirmImageDeletingWindow _confirmWindow;

	private string _path;
	private Coroutine _showLoadDialogCoroutine;
	private Coroutine _openExplorerCoroutine;
	private Texture2D _logo;

	private DoubleClickDetector _doubleClickDetector;

	public UnityAction ClosedLoadImageWindowEvent;

	public UnityAction<Texture2D, string> UserLoadedLogoEvent;
	public UnityAction DeletedLogoEvent;

	private void Awake()
	{
		_logoImage.TryGetComponent<DoubleClickDetector>(out _doubleClickDetector);
	}

	private void OnEnable()
	{
		_quitButton.onClick.AddListener(OnCloseLoadImageWindow);
		_loadButton.onClick.AddListener(OpenExplorerWindow);
		_deleteButton.onClick.AddListener(OpenCofirmDeleteLogoWindow);

		if (_doubleClickDetector != null)
			_doubleClickDetector.DoubleClickDetectedEvent += OpenCofirmDeleteLogoWindow;

		_confirmWindow.ClosedWindowEvent += CloseConfirmDeleteLogoWindow;
		_confirmWindow.DeletedLogoEvent += DeleteLogo;
	}

	private void OnDisable()
	{
		_quitButton.onClick.RemoveListener(OnCloseLoadImageWindow);
		_loadButton.onClick.RemoveListener(OpenExplorerWindow);
		_deleteButton.onClick.RemoveListener(OpenCofirmDeleteLogoWindow);

		if (_doubleClickDetector != null)
			_doubleClickDetector.DoubleClickDetectedEvent -= OpenCofirmDeleteLogoWindow;

		_confirmWindow.ClosedWindowEvent -= CloseConfirmDeleteLogoWindow;
		_confirmWindow.DeletedLogoEvent -= DeleteLogo;
	}

	public void ChangeLogoInfo(Texture2D logo, string path)
	{
		_logoImage.texture = logo;

		_displayPath.text = path;
	}

	public bool TryGetTextureFromLocalPath(string path, out Texture2D texture)
	{
		if (Path.HasExtension(path))
		{
			byte[] bytes = File.ReadAllBytes(path);

			texture = new Texture2D(1, 1);

			if (ImageConversion.LoadImage(texture, bytes, false))
			{
				return true;
			}
			else
			{
				texture = null;
				return false;
			}
		}
		else
		{
			texture = null;
			return false;
		}
	}

	private void OpenCofirmDeleteLogoWindow()
	{
		_confirmWindow.SetImage(_logoImage.texture);

		_confirmWindow.gameObject.SetActive(true);
	}

	private void CloseConfirmDeleteLogoWindow()
	{
		_confirmWindow.SetImage(null);

		_confirmWindow.gameObject.SetActive(false);
	}

	private void DeleteLogo()
	{
		CloseConfirmDeleteLogoWindow();

		DeletedLogoEvent?.Invoke();
	}

	private void OnCloseLoadImageWindow()
	{
		ClosedLoadImageWindowEvent?.Invoke();
	}

	private void OpenExplorerWindow()
	{
		if (_openExplorerCoroutine != null)
			StopCoroutine(_openExplorerCoroutine);
		_openExplorerCoroutine = StartCoroutine(OpenExplorerCoroutine());
	}

	private IEnumerator OpenExplorerCoroutine()
	{
		
		FileBrowser.SetFilters(true, new FileBrowser.Filter("Image", ".png", ".tga", ".jpg", ".jpeg"));
		FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

		if (_showLoadDialogCoroutine != null)
			StopCoroutine(_showLoadDialogCoroutine);
		_showLoadDialogCoroutine = StartCoroutine(ShowLoadDialogCoroutine());

		yield return _showLoadDialogCoroutine;

		if(TryGetTextureFromLocalPath(_path, out Texture2D texture))
			UserLoadedLogoEvent?.Invoke(texture, _path);
	}

	private IEnumerator ShowLoadDialogCoroutine()
	{
		yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, true, null, null, "Import Logo", "Import");

		if (FileBrowser.Success)
		{
			_path = FileBrowser.Result[0];

			// Read the bytes of the first file via FileBrowserHelpers
			// Contrary to File.ReadAllBytes, this function works on Android 10+, as well
			//byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

			//_texture = new Texture2D(1, 1);

			//ImageConversion.LoadImage(_texture, bytes, false);
			


			// Or, copy the first file to persistentDataPath
			//_path = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));

			//FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
		}
	}
}
