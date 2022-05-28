using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionTextWithOptions : QuestionWithOptions
{
	[TextArea]
	[SerializeField] private string _question;

	public string Question => _question;

	public override void InitQuestion()
	{
		IsAnswerShow = false;
		IsAsked = false;
		Type = TypesOfQuestions.Type.textWithOptions;
	}
}
