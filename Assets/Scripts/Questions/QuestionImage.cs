using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionImage : Question
{
	[TextArea]
	[SerializeField] private string _question;
	[TextArea]
	[SerializeField] private string _answer;
	[SerializeField] private Sprite _image;

	public string Question => _question;
	public string Answer => _answer;
	public Sprite Image => _image;

	public override void InitQuestion()
	{
		IsAnswerShow = false;
		IsAsked = false;
		Type = TypesOfQuestions.Type.image;
	}
}
