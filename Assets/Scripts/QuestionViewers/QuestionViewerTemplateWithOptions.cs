using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public abstract class QuestionViewerTemplateWithOptions : QuestionViewerTemplate
{
	[SerializeField] protected TMP_Text OptionTemplate;
	[SerializeField] protected RectTransform OptionsFolder;

	protected int RightOption = 0;

	protected Vector3 ScrollOptionsStartPosition;
	protected CanvasGroup ScrollOptionsCanvasGroup;

	protected List<TMP_Text> Options = new List<TMP_Text>();
	private TMP_Text _currentOption;

	protected List<RectTransform> OptionsRectTransform = new List<RectTransform>();
	protected List<Vector3> OptionsStartScale = new List<Vector3>();

	protected float OptionsCurrentTime;
	protected float OptionsCurrentTimeNormalize;
	protected bool IsChoosedOption;
	protected int CurrentChoosedOption;
	protected float ZoomInOptionCurrentTime;
	protected float ZoomOutOptionCurrentTime;
	protected float ZoomOptionCurrentTimeNormalize;

	protected Coroutine FadeInOptionsCoroutine;
	protected Coroutine FadeOutOptionsCoroutine;
	protected Coroutine ChangeCurrentOptionCoroutine;
	protected Coroutine ZoomInOptionCoroutine;
	protected Coroutine ZoomOutOptionCoroutine;

	public void FillOptions(List<string> optionText, int rightOption)
	{
		RightOption = rightOption;

		CreateOptions(optionText.Count);

		for (int i = 0; i < Options.Count; i++)
		{
			Options[i].text = optionText[i];
		}

		InitOptionsProperties();
	}

	public void ChangeCurrentOption(int newOptionNumber)
	{
		if (ChangeCurrentOptionCoroutine != null)
			StopCoroutine(ChangeCurrentOptionCoroutine);
		ChangeCurrentOptionCoroutine = StartCoroutine(ChangeCurrentOptionJob(newOptionNumber));
	}

	public int GetOptionsCount()
	{
		return Options.Count;
	}

	public int GetCurrentChoosedOption()
	{
		return CurrentChoosedOption;
	}

	public void SetChooseOption(int numberOption)
	{
		CurrentChoosedOption = numberOption;
		IsChoosedOption = true;
	}

	public bool TryChoosedOption(int numberOption)
	{
		if (numberOption > 0 && numberOption <= Options.Count)
			return true;

		return false;
	}

	protected void ResetOptions()
	{
		IsChoosedOption = false;
		CurrentChoosedOption = 0;

		foreach (var text in Options)
		{
			text.text = string.Empty;
		}
	}

	protected void DeleteOptions()
	{
		for (int i = 0; i < OptionsFolder.childCount; i++)
		{
			Destroy(OptionsFolder.GetChild(i).gameObject);
		}
	}

	protected void InitOptionsProperties()
	{
		foreach (var option in Options)
			option.color = _properties.GameColorChanger.GetTextColor();

		OptionsRectTransform.Clear();
		foreach (var option in Options)
			OptionsRectTransform.Add(option.gameObject.GetComponent<RectTransform>());

		OptionsStartScale.Clear();
		foreach (var transform in OptionsRectTransform)
			OptionsStartScale.Add(transform.localScale);
	}

	private IEnumerator ChangeCurrentOptionJob(int newOptionNumber)
	{
		if (ZoomOutOptionCoroutine != null)
			StopCoroutine(ZoomOutOptionCoroutine);
		ZoomOutOptionCoroutine = StartCoroutine(ZoomOutOptionJob());

		yield return ZoomOutOptionCoroutine;

		CurrentChoosedOption = newOptionNumber;

		if (ZoomInOptionCoroutine != null)
			StopCoroutine(ZoomInOptionCoroutine);
		ZoomInOptionCoroutine = StartCoroutine(ZoomInOptionJob());
	}

	private void CreateOptions(int count)
	{
		Options.Clear();

		for (int i = 0; i < count; i++)
		{
			_currentOption = Instantiate(OptionTemplate, OptionsFolder);

			Options.Add(_currentOption);
		}
	}

	//Этот код копипастится в зависимости от наличия полей

	protected IEnumerator ZoomInOptionJob()
	{
		ZoomInOptionCurrentTime = 0;

		OptionsRectTransform[CurrentChoosedOption - 1].localScale = OptionsStartScale[CurrentChoosedOption - 1];

		while (ZoomInOptionCurrentTime <= _properties.FadeInOutUIElements)
		{
			ZoomInOptionCurrentTime += Time.deltaTime;

			ZoomOptionCurrentTimeNormalize = ZoomInOptionCurrentTime / _properties.FadeInOutUIElements;

			OptionsRectTransform[CurrentChoosedOption - 1].localScale = Vector3.Lerp(OptionsStartScale[CurrentChoosedOption - 1],
				OptionsStartScale[CurrentChoosedOption - 1] + _properties.AddSizeOfSelectedOption, _properties.FadeIn.Evaluate(ZoomOptionCurrentTimeNormalize));

			Options[CurrentChoosedOption - 1].color = Vector4.Lerp(_properties.GameColorChanger.GetTextColor(),
				_properties.GameColorChanger.GetSelectedColor(), _properties.FadeIn.Evaluate(ZoomOptionCurrentTimeNormalize));

			yield return null;
		}
	}

	protected IEnumerator ZoomOutOptionJob()
	{
		ZoomOutOptionCurrentTime = 0;

		OptionsRectTransform[CurrentChoosedOption - 1].localScale = OptionsStartScale[CurrentChoosedOption - 1] + _properties.AddSizeOfSelectedOption;

		while (ZoomOutOptionCurrentTime <= _properties.FadeInOutUIElements)
		{
			ZoomOutOptionCurrentTime += Time.deltaTime;

			ZoomOptionCurrentTimeNormalize = ZoomOutOptionCurrentTime / _properties.FadeInOutUIElements;

			OptionsRectTransform[CurrentChoosedOption - 1].localScale = Vector3.Lerp(OptionsStartScale[CurrentChoosedOption - 1] + _properties.AddSizeOfSelectedOption,
				OptionsStartScale[CurrentChoosedOption - 1], _properties.FadeOut.Evaluate(ZoomOptionCurrentTimeNormalize));

			Options[CurrentChoosedOption - 1].color = Vector4.Lerp(_properties.GameColorChanger.GetSelectedColor(),
				_properties.GameColorChanger.GetTextColor(), _properties.FadeOut.Evaluate(ZoomOptionCurrentTimeNormalize));

			yield return null;
		}
	}

	protected void SetSameFontSizeInOptions()
	{
		if (Options.Count == 0 || Options == null)
			return;

		List<float> autoFontSize = new List<float>();

		foreach (var option in Options)
		{
			option.enableAutoSizing = true;

			string text = option.text;

			autoFontSize.Add(option.fontSize);
		}

		float minSize = autoFontSize.Min();

		foreach (var option in Options)
		{
			option.enableAutoSizing = false;

			option.fontSize = minSize;
		}
	}
}
