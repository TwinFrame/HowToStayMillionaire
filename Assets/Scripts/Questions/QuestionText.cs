using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionText : Question
{
	[TextArea]
	[SerializeField] private string _question;
	[TextArea]
	[SerializeField] private string _answer;

	public string Question => _question;
	public string Answer => _answer;

	public override void InitQuestion()
	{
		IsAnswerShow = false;
		IsAsked = false;
		Type = TypesOfQuestions.Type.text;
	}
}
