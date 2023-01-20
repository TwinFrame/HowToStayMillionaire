using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DisplayController : MonoBehaviour
{
	public UnityAction<string, bool> RefreshedDisplayInfoEvent;

	private float _delayRefreshDispalyInfo = 0.5f;
	private Coroutine _delayRefreshDisplayInfoJob;
	private float _currentDisplayInfoWaitingTime;

	public void ChangeResolution(int width, int height)
	{
		Screen.SetResolution(width, height, false);

		if (_delayRefreshDisplayInfoJob != null)
			StopCoroutine(_delayRefreshDisplayInfoJob);
		_delayRefreshDisplayInfoJob = StartCoroutine(DelayRefreshDisplayInfoJob());
	}

	public void SetFullscreen(bool isFullscreen)
	{
		Screen.fullScreen = isFullscreen;

		if (_delayRefreshDisplayInfoJob != null)
			StopCoroutine(_delayRefreshDisplayInfoJob);
		_delayRefreshDisplayInfoJob = StartCoroutine(DelayRefreshDisplayInfoJob());
	}

	public string GetDisplayInfo()
	{
		return $"Monitor: {Screen.currentResolution};\nDisplay: {Display.main.renderingWidth} x" +
			$"{Display.main.renderingHeight}";
	}

	public bool GetIsFullscreen()
	{
		return Screen.fullScreen;
	}

	private IEnumerator DelayRefreshDisplayInfoJob()
	{
		_currentDisplayInfoWaitingTime = 0;

		while (_currentDisplayInfoWaitingTime < _delayRefreshDispalyInfo)
		{
			_currentDisplayInfoWaitingTime += Time.deltaTime;
			yield return null;
		}

		RefreshedDisplayInfoEvent?.Invoke(GetDisplayInfo(), GetIsFullscreen());
	}
}
