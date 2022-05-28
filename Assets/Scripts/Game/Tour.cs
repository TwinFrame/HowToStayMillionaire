using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tour : MonoBehaviour
{
	[Header("Необходимо кратное кол-во вопросов для команд.")]
	[Space]
	[SerializeField] private string _name = "Тур";
	[SerializeField] private int _questionCost;
	[SerializeField] private float _timeToQuestion;

	private List<Question> _questions = new List<Question>();

	public string Name => _name;
	public int QuestionCost => _questionCost;
	public float TimeToQuestion => _timeToQuestion;
	public List<Question> Questions => _questions;



	public void InitQuestions()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).TryGetComponent<Question>(out Question question))
			{
				question.InitQuestion();

				_questions.Add(question);
			}
		}
	}
}
