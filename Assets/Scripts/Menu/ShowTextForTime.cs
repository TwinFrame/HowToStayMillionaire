using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowTextForTime : MonoBehaviour
{
	[Range(2, 10)]
	[SerializeField] private float _ShowTime = 5f;

	private WaitForSeconds _waitLogDisplayTime;
	private Coroutine _logDisplayTimeJob;

	private void Awake()
	{
		_waitLogDisplayTime = new WaitForSeconds(_ShowTime);
	}

	public void ShowText(TMP_Text textField, string text)
	{
		if (_logDisplayTimeJob != null)
			StopCoroutine(_logDisplayTimeJob);
		_logDisplayTimeJob = StartCoroutine(LogDisplayTimeJob(textField, text));
	}

	public void ClearText(TMP_Text textField)
	{
		if (_logDisplayTimeJob != null)
			StopCoroutine(_logDisplayTimeJob);

		textField.text = string.Empty;
	}

	private IEnumerator LogDisplayTimeJob(TMP_Text textField, string text)
	{
		textField.text = text;

		yield return _waitLogDisplayTime;

		textField.text = string.Empty;
	}
}
