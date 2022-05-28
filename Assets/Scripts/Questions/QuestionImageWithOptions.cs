using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionImageWithOptions : QuestionWithOptions
{
	[TextArea]
	[SerializeField] private string _question;
	[SerializeField] private Sprite _image;

	public string Question => _question;
	public Sprite Image => _image;

	public override void InitQuestion()
	{
		IsAnswerShow = false;
		IsAsked = false;
		Type = TypesOfQuestions.Type.imageWithOptions;
	}
}
