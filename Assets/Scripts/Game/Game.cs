using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ConvertToMoneyFormat))]

public class Game : MonoBehaviour
{
	[SerializeField] private int _initialCapital;
	[Header("Titles")]
	[TextArea]
	[SerializeField] private string _name;
	//[SerializeField] private string _description;
	[Space]
	[TextArea]
	[SerializeField] private string _finalTitleText;
	//[SerializeField] private string _finalDescriptionText;
	[Space]
	[Header("Teams")]
	[SerializeField] private List<Team> _teams;
	[Header("Service")]
	//[SerializeField] private TourTitleViewer _tourTitleViewer;
	//[SerializeField] private FinishTitleViewer _finishTitleViewer;
	//[SerializeField] private QuestionViewer _questionViewer;
	[Space]
	[SerializeField] private MainTitleAnimator _mainTitleAnimator;
	[SerializeField] private TeamsTitleAnimator _teamsTitleAnimator;
	[SerializeField] private TourTitleAnimator _toursTitleAnimator;
	[SerializeField] private QuestionAnimator _questionAnimator;
	//[SerializeField] private CountdownAnimator _countdownAnimator;
	[SerializeField] private FinalTitleAnimator _finalTitleAnimator;
	[Space]
	[SerializeField] private GameMenu _menu;
	[Space]
	[SerializeField] private TintLightsForAnswer _tintLight;
	[SerializeField] private ParticleSystem _fireworks;
	[SerializeField] private PrimitiveObjects _primitiveObjects;
	//[SerializeField] private Transform _objectsBG;
	//[SerializeField] private SideElementsAnimator _sideElements;
	//[Space]
	//[SerializeField] private ConvertToMoneyFormat _convertToMoney;
	[Space]
	[SerializeField] private GameEvents _gameEvents;

	private readonly List<Tour> _tours = new List<Tour>();
	private int _currentNumTour;
	private int _currentNumQuestion;
	private int _currentNumTeam;
	private Transition[] _transitions;
	private Transition _currentTransition;
	private char _currentMonetaryUnit = '₽';

	private QuestionViewerTemplate _questionTemplate;

	private bool _isPlaying;
	private bool _isFinished;
	private bool _isPauseWithTimeStop;
	private bool _isTeamsReady;
	private bool _isTeamsTitleState = false;
	private bool _isPreparationState = false;
	private bool _isAskQuestionState = false;
	private bool _isNeedRemoveMoneyWhenTeamsTitleStart = false;
	//private bool _isGameOver = false;


	private Coroutine _preparationTeamsJob;
	private Coroutine _enterTeamsTitleJob;
	private Coroutine _preparationTeamJob;
	private Coroutine _changeTeamMoneyJob;

	public UnityAction<string, string> OnChangedGameTextEvent;

	public string Name => _name;
	public string FinalTitleText => _finalTitleText;
	public List<Team> Teams => _teams;
	public List<Tour> Tours => _tours;
	public int CurrentNumTour => _currentNumTour;
	public int CurrentNumQuestion => _currentNumQuestion;
	public int CurrentNumTeam => _currentNumTeam;
	public char CurrentMonetaryUnit => _currentMonetaryUnit;
	public bool IsFinished => _isFinished;
	public bool IsPlaying => _isPlaying;

	public GameEvents GameEvents => _gameEvents;
	public bool IsPauseWithTimeStop => _isPauseWithTimeStop;
	//public bool IsPause => _isPause;
	public bool IsTeamsReady => _isTeamsReady;
	public bool IsTeamsTitle => _isTeamsTitleState;


	private void Start()
	{
		CheckSavedGameTexts();
		_mainTitleAnimator.SetName(_name);
		_finalTitleAnimator.SetName(_finalTitleText);

		InitToursAndQuestions();

		_transitions = InitTransitions();
		if (_transitions == null)
			WriteLog("Переходы в игре не найдены.");

		ResetGame(); //добавить для TitleAnimation, как MainTitleAnim
	}

	public void EnterPreparationTeams()
	{
		_isPreparationState = true;

		if (_preparationTeamsJob != null)
			StopCoroutine(_preparationTeamsJob);
		_preparationTeamsJob = StartCoroutine(PreparationTeamsJob());
	}

	public void ExitPreparationTeams()
	{
		_isPreparationState = false;

		ExitTeamsTitle();
	}

	public void EnterMainTitle()
	{
		_mainTitleAnimator.Enter(out Coroutine enterJob);

		_primitiveObjects.EnterPrimitives();
	}

	public void ExitMainTitle()
	{
		if (_mainTitleAnimator.IsEntering)
			_mainTitleAnimator.Exit();

		_primitiveObjects.ExitPrimitives();
	}

	public void EnterQuestion()
	{
		_isAskQuestionState = true;

		if (_questionAnimator.TryActivationViewer(_tours[_currentNumTour].Questions[_currentNumQuestion].TypeReadOnly, out _questionTemplate))
		{
			_questionAnimator.SetActivityOnPlate(true);

			_questionAnimator.SetAndFillQuestionTemplate(_tours[_currentNumTour].Questions[_currentNumQuestion], _questionTemplate);

			//_questionViewer.Enter(_tours[_currentNumTour].Questions[_currentNumQuestion]);

			_questionAnimator.Enter(out Coroutine enterQuestionJob);

			//_countdownAnimator.Enter();
		}
		else
		{
			Debug.Log("Не нашлось подходящего шаблона.");
		}
	}

	public void ExitQuestion()
	{
		_isAskQuestionState = false;

		if (_questionAnimator.IsEntering)
		{
			//_questionViewer.Exit();
			_questionAnimator.Exit();
		}

			//_countdownAnimator.Exit();
	}

	public void EnterTourTitle()
	{
		_menu.RefreshTeamsDropdown(_teams);

		//_tourTitleViewer.Enter(_tours[_currentNumTour].Name, _teams[_currentNumTeam].Name);

		_toursTitleAnimator.ActualizeTexts(_tours[_currentNumTour].Name, _teams[_currentNumTeam].Name);
		_toursTitleAnimator.Enter(out Coroutine enterToursJob); //для всех Enter- в этот компонент добавить private _enterToursJob 

		_primitiveObjects.EnterPrimitives();

		_gameEvents.OnTourTitleEnter?.Invoke();
	}

	public void ExitTourTitle()
	{
		/*
		if (_tourTitleViewer.IsEntering)
			_tourTitleViewer.Exit();
		*/

		if (_toursTitleAnimator.IsEntering)
			_toursTitleAnimator.Exit();

		_primitiveObjects.ExitPrimitives();

		_gameEvents.OnTourTitleExit?.Invoke();
	}

	public void EnterTeamsTitle()
	{
		_menu.RefreshTeamsDropdown(_teams);

		_isTeamsTitleState = true;

		if (_enterTeamsTitleJob != null)
			StopCoroutine(_enterTeamsTitleJob);
		_enterTeamsTitleJob = StartCoroutine(EnterTeamsTitleJob());
	}

	public void ExitTeamsTitle()
	{
		_isTeamsTitleState = false;

		if (_teamsTitleAnimator.IsEntering)
		_teamsTitleAnimator.Exit();
	}

	public void EnterFinalTitle()
	{
		//_isGameOver = true;

		_finalTitleAnimator.Enter(out Coroutine enterFinishTitleJob);

		_primitiveObjects.EnterPrimitives();
		//_finishTitleViewer.Enter(_finalTitleText, _finalDescriptionText);

		//_gameEvent.OnFinalTitleEnter?.Invoke();
	}

	public void ExitFinalTitle()
	{
		if (_finalTitleAnimator.IsEntering)
			_finalTitleAnimator.Exit();

		_primitiveObjects.ExitPrimitives();

		/*
		if (_finishTitleViewer.IsEntering)
			_finishTitleViewer.Exit();

		_gameEvent.OnFinalTitleExit?.Invoke();
		*/
	}

	public bool TryGetNextQuestion(out bool isNeedChangeTour)
	{
		if (_currentNumQuestion + 1 > _tours[_currentNumTour].Questions.Count - 1)
		{
			if (_currentNumTour + 1 > _tours.Count - 1)
			{
				isNeedChangeTour = false;
				return false;
			}
			else
			{
				isNeedChangeTour = true;
				return true;
			}
		}

		isNeedChangeTour = false;
		return true;
	}
	/*
	public void SetPauseWithTimeStop()
	{
		if (_isPauseWithTimeStop)
			_isPauseWithTimeStop = false;
		else
			_isPauseWithTimeStop = true;

		if (_isPauseWithTimeStop)
		{
			Time.timeScale = 0f;
			_menu.SetPauseOnMainTitleButton(true);

			WriteLog(LogMessages.TurnOnPauseWithStopTime);
		}
		else
		{
			//здесь стоит останавливать еще и плеера, ставить на паузу.
			Time.timeScale = 1f;
			_menu.SetPauseOnMainTitleButton(false);
			WriteLog(LogMessages.TurnOffPauseWithStopTime);
		}
	}
	*/
	private void MarkCurrentQuestion(bool isRight)
	{
		if (IsQuestionWithOption(out QuestionWithOptions questionWithOptions))
		{
			if (TryGetCurrentChoosedOption(out int currentChoosedOption))
			{
				if (currentChoosedOption == questionWithOptions.RightOption)
					CurrentQuestionRightAnswer();
				else
					CurrentQuestionWrongAnswer();
			}
			else
				WriteLog("Не выбран вариант в вопросе.");
		}
		else
		{
			if (isRight)
				CurrentQuestionRightAnswer();
			else
				CurrentQuestionWrongAnswer();
		}
	}

	public void CurrentQuestionRightAnswer()
	{
		_tours[_currentNumTour].Questions[_currentNumQuestion].OnRightAnswerAndAsked();

		_gameEvents.OnRightAnswer?.Invoke();

		_isNeedRemoveMoneyWhenTeamsTitleStart = false;
	}

	public void CurrentQuestionWrongAnswer()
	{
		_tours[_currentNumTour].Questions[_currentNumQuestion].OnWrongAnswerAndAsked();

		_gameEvents.OnWrongAnswer?.Invoke();

		_isNeedRemoveMoneyWhenTeamsTitleStart = true;
	}

	public void OnStartCountdown()
	{
		if (_isAskQuestionState)
			_questionAnimator.StartCountdown(_tours[_currentNumTour].TimeToQuestion);
		else
			WriteLog("Обратный отсчёт запскается только на слайде с вопросом.");
	}

	public bool TryGetIsAnsweredCurrentQuestion(out bool isRightAnswer)
	{
		if (_tours[_currentNumTour].Questions[_currentNumQuestion].IsAskedReadOnly &&
		_tours[_currentNumTour].Questions[_currentNumQuestion].IsAnswerShowReadOnly)
		{
			isRightAnswer = _tours[_currentNumTour].Questions[_currentNumQuestion].IsRightAnswerReadOnly;
			return true;
		}

		isRightAnswer = false;
		return false;
	}

	public bool TryGetIsAnsweredCurrentQuestion()
	{
		if (_tours[_currentNumTour].Questions[_currentNumQuestion].IsAskedReadOnly)
			return true;

		WriteLog("Не выбран ответ правильно/нет (Y/N)");
		return false;
	}

	public bool IsQuestionWithOption(out QuestionWithOptions questionWithOptions)
	{
		if (Tours[CurrentNumTour].Questions[CurrentNumQuestion] is QuestionWithOptions)
		{
			questionWithOptions = Tours[CurrentNumTour].Questions[CurrentNumQuestion] as QuestionWithOptions;
			return true;
		}
		else
		{
			questionWithOptions = null;
			return false;
		}
	}

	public bool IsCurrentQuestionWithPlayer()
	{
		if (Tours[CurrentNumTour].Questions[CurrentNumQuestion] is IQuestionThatNeedsPlayer)
			return true;
		else
			return false;
	}

	public bool TryGetCurrentChoosedOption(out int currentChoosedOption)
	{
		if (_questionAnimator.GetCurrentQuestionViewer() is QuestionViewerTemplateWithOptions &&
			Tours[CurrentNumTour].Questions[CurrentNumQuestion] is QuestionWithOptions)
		{
			var currentQuestionWithOptions = Tours[CurrentNumTour].Questions[CurrentNumQuestion] as QuestionWithOptions;

			currentChoosedOption = _questionAnimator.GetCurrentChoosedOption();

			if (currentChoosedOption > 0 && currentChoosedOption <=
				currentQuestionWithOptions.Options.Count)
			{
				return true;
			}

			currentChoosedOption = 0;
			return false;
		}

		WriteLog("Тип вопроса без вариантов ответа.");
		currentChoosedOption = 0;
		return false;
	}

	public void MarkCurrentTeamIsReady()
	{
		if (!_teams[_currentNumTeam].IsConfirmNameWhenPreparing)
			_teams[_currentNumTeam].ConfirmNameWhenPreparing();
		else
			_teams[_currentNumTeam].OnIsReady();
	}

	public void TryChangeGameText(string name, string finalText)
	{
		if (name != _name || finalText != _finalTitleText)
			ChangeGameText(name, finalText);
	}

	private void ChangeGameText(string name, string finalText)
	{
		_name = name;
		_mainTitleAnimator.SetName(_name);
		PlayerPrefs.SetString("GameName", _name);

		_finalTitleText = finalText;
		_finalTitleAnimator.SetName(_finalTitleText);
		PlayerPrefs.SetString("FinalText", _finalTitleText);

		//OnChangedGameTextEvent?.Invoke(_name, _finalTitleText);
	}

	private void CheckSavedGameTexts()
	{
		if (PlayerPrefs.HasKey("GameName"))
			_name = PlayerPrefs.GetString("GameName");
		else
			PlayerPrefs.SetString("GameName", _name);

		if (PlayerPrefs.HasKey("FinalText"))
			_finalTitleText = PlayerPrefs.GetString("FinalText");
		else
			PlayerPrefs.SetString("FinalText", _finalTitleText);

		if (PlayerPrefs.HasKey("CurrentMonetaryInit"))
			_currentMonetaryUnit = PlayerPrefs.GetString("CurrentMonetaryInit")[0];
		else
			PlayerPrefs.SetString("CurrentMonetaryInit", _currentMonetaryUnit.ToString());
	}

	public string GetName()
	{
		return _name;
	}

	public string GetFinalText()
	{
		return _finalTitleText;
	}

	public void NextNumTour()
	{
		_currentNumTour++;
	}

	public void NextNumQuestion()
	{
		_currentNumQuestion++;
	}

	public void ResetNumQuestion()
	{
		_currentNumQuestion = 0;
	}

	public void ChooseTeamWinner()
	{
		var maxBank = _teams.Max(bank => bank.Bank);

		foreach (var team in _teams)
		{
			if (team.Bank == maxBank)
				team.OnIsWinner();
		}
	}

	public void CurrentTeamIsAsked()
	{
		_teams[_currentNumTeam].OnIsAsked();
	}

	public bool IsAllTeamsIsAsked()
	{
		foreach (var team in _teams)
		{
			if (!team.IsAsked)
				return false;
		}

		return true;
	}

	public void GetNextTeam()
	{
		SetCurrentNumTeam(_teams.Find(t => !t.IsAsked));
	}

	public void ResetTeamsIsAsked()
	{
		foreach (var team in _teams)
		{
			team.ResetIsAsked();
		}
	}

	public void ReplaceCurrentTeam(int num)
	{
		if (_teams[num].IsAsked)
		{
			WriteLog("Команда " + _teams[num].Name + " уже играла");
			return;
		}

		if (num == _currentNumTeam)
		{
			WriteLog("Команда " + _teams[num].Name + " уже выбрана");
			return;
		}

		if (num <= _teams.Count - 1)
		{
			_currentNumTeam = num;

			_toursTitleAnimator.ChangeCurrentTeam(_teams[_currentNumTeam].Name);
			//_tourTitleViewer.ChangeDescription(_teams[_currentNumTeam].Name);
		}
		else
			Debug.Log("Не нашел такой команды в игре.");
	}

	public void AddMoneyToTeamFromMenu(int numTeam, int money)
	{
		_teamsTitleAnimator.ChangeTeamMoney(numTeam, money, true, out _changeTeamMoneyJob);

		//_teams[numTeam].AddingMoney(money);
	}


	public void ChangeTeamName(int numTeam, string newName)
	{
		_teams[numTeam].ChangeTeamName(newName);

		_teamsTitleAnimator.ChangeTeamName(numTeam, newName);
	}

	public void OnNextButton()
	{
		_currentTransition = FindActiveTransition();

		if (_currentTransition != null)
			_currentTransition.OnNextButton();
	}

	public void OnTeamsTitleButton()
	{
		_currentTransition = FindActiveTransition();

		if (_currentTransition != null)
			_currentTransition.OnTeamsTitleButton();
	}

	public void OnMainTitleButton()
	{
		_currentTransition = FindActiveTransition();

		if (_currentTransition != null)
		{
			if (_currentTransition is AskQuestionTransition)
				WriteLog("Сначала отыграйте текущий вопрос.");
			else if (_currentTransition is PreparationTeamsTransition)
				WriteLog("Сначала подготовьте все команды.");
			else if (_currentTransition is MainTitleTransition)
				WriteLog("Вы уже на главной заставке.");
			else
				_currentTransition.OnMainTitleButton();
		}
	}

	public void OnAnswerButton(bool isRight)
	{
		if (_isPreparationState)
			MarkCurrentTeamIsReady();
		else if (_isAskQuestionState)
			MarkCurrentQuestion(isRight);
		else
			WriteLog("На этом титре нет функционала для кнопки.");
	}

	public void OnOptionButton(int numOption)
	{
		numOption = Mathf.Clamp(numOption, 1, 5);

		if (_isAskQuestionState)
		{
			if (TrySetChoosedOption(numOption))
			{
				if (TryGetCurrentChoosedOption(out int currentChoosedOption))
				{
					if (currentChoosedOption == numOption)
					{
						WriteLog("Этот вариант уже выбран.");
						return;
					}

					_questionAnimator.ChangeCurrentOption(numOption);
				}
				else
					_questionAnimator.SetChooseOption(numOption);
			}
			else
				WriteLog("Такого варианта ответа нет или вопрос без вариантов ответа.");
		}
		else
			WriteLog("Сейчас не слайд с вопросом.");
	}

	public void OnChangedCurentMonetaryUnit(char MonetaryUnit)
	{
		_currentMonetaryUnit = MonetaryUnit;

		PlayerPrefs.SetString("CurrentMonetaryInit", _currentMonetaryUnit.ToString());
	}

	public void SetPauseMark(float normalizedPauseMark)
	{
		if (IsCurrentQuestionWithPlayer())
		{
			var question = _tours[CurrentNumTour].Questions[CurrentNumQuestion] as IQuestionThatNeedsPlayer;

			question.SetNormalizedPauseMark(normalizedPauseMark);

			//_gameEvent.SetNormalizedPauseMark(normalizedPauseMark);
		}
		else
			WriteLog("Вопрос без аудио или видео контента.");
	}

	private Transition[] InitTransitions()
	{
		Transition[] transitions = gameObject.GetComponents<Transition>();

		if (transitions != null)
			return transitions;
		else
			return null;
	}

	private Transition FindActiveTransition()
	{
		foreach (var transition in _transitions)
		{
			if (transition.isActiveAndEnabled)
				return transition;
		}

		return null;
	}

	public bool CheckEqualQuestionsByTeam()
	{
		foreach (var tour in _tours)
		{
			var multiple = tour.Questions.Count % _teams.Count;
			if (multiple != 0)
				return false;
		}

		return true;
	}

	public bool TrySetChoosedOption(int numberOption)
	{
		if (_questionAnimator.TryChoosedOption(numberOption))
			return true;

		return false;
	}

	public void OnIsPlaying()
	{
		_isPlaying = true;
		_isFinished = false;
	}

	public void OnIsFinished()
	{
		_isFinished = true;
		_isPlaying = false;
	}

	public void OnFireworks()
	{
		_fireworks.Play(true);
	}

	public void OnFlashGlow()
	{
		if(_gameEvents.OnFlash != null)
			_gameEvents.OnFlash?.Invoke();
	}

	public void WriteLog(string text)
	{
		_menu.WriteLog(text);
	}
	private void SetCurrentNumTeam(Team team)
	{
		for (int i = 0; i < _teams.Count; i++)
		{
			if (_teams[i] == team)
			{
				_currentNumTeam = i;

				return;
			}
		}

		Debug.Log("Не нашел такой команды в игре.");
	}

	public void ChangeTeamMoney(int numTeam, int money, bool isAdded)
	{
		if (isAdded)
			_teams[numTeam].AddingMoney(money);
		else
			_teams[numTeam].RemovingMoney(money);
	}

	private void ResetGame()
	{
		if (_questionAnimator.MainPlate.isActiveAndEnabled != true)
			_questionAnimator.SetActivityOnPlate(true);

		_questionAnimator.ResetViewers();
		//_mainTitleViewer.ResetViewer();
		//_finishTitleViewer.ResetViewer();
		//_tourTitleViewer.ResetViewer();
		//_preparationTeamsViewer.ResetViewer();

		//mainTitleAnimator.Reset(); нужно реализовать еще у этого класса

		_menu.ResetMenu(); //явно тут что-то пропустил, не все меню ресетю

		ResetTeams();
		ResetAllQuestions();

		_currentNumTeam = 0;
		_currentNumTour = 0;
		_currentNumQuestion = 0;
		_isPlaying = false;
		_isFinished = false;
		_isPauseWithTimeStop = false;
		_isTeamsReady = false;
		_isTeamsTitleState = false;
		_isAskQuestionState = false;
		_isPreparationState = false;
		_isNeedRemoveMoneyWhenTeamsTitleStart = false;
		//_isGameOver = false;
	}

	private void ResetAllQuestions()
	{
		foreach (var tour in _tours)
		{
			foreach (var question in tour.Questions)
			{
				question.ResetQuestion();
			}
		}
	}

	private void ResetTeams()
	{
		foreach (var team in _teams)
		{
			team.ResetTeam();
		}
	}

	private void InitToursAndQuestions()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).TryGetComponent<Tour>(out Tour tour))
			{
				tour.InitQuestions();

				_tours.Add(tour);
			}
		}
	}

	private bool TryCheckingReadinessOfAllTeams()
	{
		foreach (var team in _teams)
		{
			if (!team.IsReady)
				return false;
		}

		return true;
	}

	private IEnumerator PreparationTeamsJob()
	{
		_gameEvents.OnPreparationTitleEnter?.Invoke();

		_teamsTitleAnimator.Enter(out Coroutine enterJob);

		yield return enterJob;

		while (!_isTeamsReady)
		{
			for (int i = 0; i < _teams.Count; i++)
			{
				_currentNumTeam = i;

				//_menu.RefreshTeamsDropdown(_teams, _currentNumTeam);
				_menu.RefreshTeamsDropdown(_teams);

				_teamsTitleAnimator.PreparationTeam(i, _initialCapital, out _preparationTeamJob);

				yield return _preparationTeamJob;
			}

			_currentNumTeam = 0;

			if (TryCheckingReadinessOfAllTeams())
			{
				_isTeamsReady = true;

				_currentTransition = FindActiveTransition();

				if (_currentTransition != null)
					_currentTransition.OnNextButton();
			}
			else
				WriteLog("После подготовки остались не готовые команды.");
		}

		_gameEvents.OnPreparationTitleExit?.Invoke();
	}

	private IEnumerator EnterTeamsTitleJob()
	{
		_teamsTitleAnimator.Enter(out Coroutine enterJob);

		yield return enterJob;

		if (_isNeedRemoveMoneyWhenTeamsTitleStart)
		{
			_isNeedRemoveMoneyWhenTeamsTitleStart = false;

			_teamsTitleAnimator.ChangeTeamMoney(_currentNumTeam, -_tours[_currentNumTour].QuestionCost, true, out _changeTeamMoneyJob);

			yield return _changeTeamMoneyJob;
		}
		/*
		else if (_isFinished)
		{
			for (int i = 0; i < _teams.Count; i++)
			{
				if (_teams[i].IsWinner)
				{
					_teamsTitleAnimator.SelectWinner(i);
				}
			}
		}
		*/
	}
	/*
	private IEnumerator ChangeTeamNameJob(int numTeam, string newName)
	{
		_teamsTitleAnimator.ChangeTeamName(numTeam, newName);
	}
	*/
}
