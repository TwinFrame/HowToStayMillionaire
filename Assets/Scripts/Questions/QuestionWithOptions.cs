using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestionWithOptions : Question
{
	[SerializeField] protected List<string> _options;
	[SerializeField] protected int _rightOption; //{ set { value = Mathf.Clamp(value, 0, _options.Count); } }

	public int RightOption => _rightOption;

	public List<string> Options => _options;
}
