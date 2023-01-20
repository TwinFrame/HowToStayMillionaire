using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameTab : BaseTab
{
	[Space]
	[Header("Titles")]
	[SerializeField] private Button _nextTitleButton;
	[SerializeField] private Button _mainTitleButton;
	[SerializeField] private Button _teamsTitleButton;
	[Header("Answer")]
	[SerializeField] private Button _rightAnswerButton;
	[SerializeField] private Button _wrongAnswerButton;
	[Header("Countdown")]
	[SerializeField] private Button _countdownButton;
	[Header("Options")]
	[Tooltip("It is important to place the buttons in order")]
	[SerializeField] private Button _options1Button;
	[SerializeField] private Button _options2Button;
	[SerializeField] private Button _options3Button;
	[SerializeField] private Button _options4Button;
	[SerializeField] private Button _options5Button;
	[Header("Primitive Objects")]
	[Tooltip("The component with UI elements for influencing primitive objects")]
	[SerializeField] private ForceInfluenceOnPrimitivesObjects _force;
	[Header("Fx")]
	[SerializeField] private Button _flash;
	[SerializeField] private Button _fireworks;


	public ForceInfluenceOnPrimitivesObjects Force => _force;

	public UnityAction NextTitleButtonEvent;
	public UnityAction TeamsTitleButtonEvent;
	public UnityAction MainTitleButtonEvent;
	public UnityAction RightAnswerButtonEvent;
	public UnityAction WrongAnswerButtonEvent;
	public UnityAction StartCountdownEvent;
	public UnityAction Option1ButtonEvent;
	public UnityAction Option2ButtonEvent;
	public UnityAction Option3ButtonEvent;
	public UnityAction Option4ButtonEvent;
	public UnityAction Option5ButtonEvent;
	public UnityAction<float, bool> ForcedEvent;
	public UnityAction RestartedPrimitivesEvent;
	public UnityAction FlashedEvent;
	public UnityAction FireworkedEvent;

	private void OnEnable()
	{
		_nextTitleButton.onClick.AddListener(OnNextTitleButton);
		_teamsTitleButton.onClick.AddListener(OnTeamsTitleButton);
		_mainTitleButton.onClick.AddListener(OnMaintitleButton);

		_rightAnswerButton.onClick.AddListener(OnRightAnswerButton);
		_wrongAnswerButton.onClick.AddListener(OnWrongAnswerButton);

		_countdownButton.onClick.AddListener(OnStartCountdown);

		_options1Button.onClick.AddListener(OnOption1Button);
		_options2Button.onClick.AddListener(OnOption2Button);
		_options3Button.onClick.AddListener(OnOption3Button);
		_options4Button.onClick.AddListener(OnOption4Button);
		_options5Button.onClick.AddListener(OnOption5Button);

		_force.ForcedEvent += (value, isInside) => OnForce(value, isInside);
		_force.RestartedEvent += OnRestartPrimitives;

		_flash.onClick.AddListener(OnFlashButton);
		_fireworks.onClick.AddListener(OnFireworksButton);
	}

	private void OnDisable()
	{
		_nextTitleButton.onClick.RemoveListener(OnNextTitleButton);
		_teamsTitleButton.onClick.RemoveListener(OnTeamsTitleButton);
		_mainTitleButton.onClick.RemoveListener(OnMaintitleButton);

		_rightAnswerButton.onClick.RemoveListener(OnRightAnswerButton);
		_wrongAnswerButton.onClick.RemoveListener(OnWrongAnswerButton);

		_countdownButton.onClick.RemoveListener(OnStartCountdown);

		_options1Button.onClick.RemoveListener(OnOption1Button);
		_options2Button.onClick.RemoveListener(OnOption2Button);
		_options3Button.onClick.RemoveListener(OnOption3Button);
		_options4Button.onClick.RemoveListener(OnOption4Button);
		_options5Button.onClick.RemoveListener(OnOption5Button);

		_force.ForcedEvent -= (value, isInside) => OnForce(value, isInside);
		_force.RestartedEvent -= OnRestartPrimitives;

		_flash.onClick.RemoveListener(OnFlashButton);
		_fireworks.onClick.RemoveListener(OnFireworksButton);
	}

	public override void RefreshTab()
	{
	}

	public override bool[] GetInteractables()
	{
		bool[] currentInteractables = new bool[]
			{
				_nextTitleButton.interactable,
				_countdownButton.interactable,
				_rightAnswerButton.interactable,
				_wrongAnswerButton.interactable,
				_options1Button.interactable,
				_options2Button.interactable,
				_options3Button.interactable,
				_options4Button.interactable,
				_options5Button.interactable
			};

		return currentInteractables;
	}


	public override void SetTabInteractables(bool[] isInteractables)
	{
		_nextTitleButton.interactable = isInteractables[0];
		_countdownButton.interactable = isInteractables[1];
		_rightAnswerButton.interactable = isInteractables[2];
		_wrongAnswerButton.interactable = isInteractables[3];
		_options1Button.interactable = isInteractables[4];
		_options2Button.interactable = isInteractables[5];
		_options3Button.interactable = isInteractables[6];
		_options4Button.interactable = isInteractables[7];
		_options5Button.interactable = isInteractables[8];
	}

	private void OnNextTitleButton()
	{
		NextTitleButtonEvent?.Invoke();
	}

	private void OnMaintitleButton()
	{
		MainTitleButtonEvent?.Invoke();
	}

	private void OnTeamsTitleButton()
	{
		TeamsTitleButtonEvent?.Invoke();
	}

	private void OnRightAnswerButton()
	{
		RightAnswerButtonEvent?.Invoke();
	}
	private void OnWrongAnswerButton()
	{
		WrongAnswerButtonEvent?.Invoke();
	}

	private void OnStartCountdown()
	{
		StartCountdownEvent?.Invoke();
	}

	private void OnOption1Button()
	{
		Option1ButtonEvent?.Invoke();
	}

	private void OnOption2Button()
	{
		Option2ButtonEvent?.Invoke();
	}

	private void OnOption3Button()
	{
		Option3ButtonEvent?.Invoke();
	}

	private void OnOption4Button()
	{
		Option4ButtonEvent?.Invoke();
	}

	private void OnOption5Button()
	{
		Option5ButtonEvent?.Invoke();
	}

	private void OnForce(float value, bool isInside)
	{
		ForcedEvent?.Invoke(value, isInside);
	}

	private void OnRestartPrimitives()
	{
		RestartedPrimitivesEvent?.Invoke();
	}

	private void OnFlashButton()
	{
		FlashedEvent?.Invoke();
	}

	private void OnFireworksButton()
	{
		FireworkedEvent?.Invoke();
	}

	public void InteractableNextButton(bool isInteractable)
	{
		if (isInteractable)
			_nextTitleButton.interactable = true;
		else
			_nextTitleButton.interactable = false;
	}

	public bool GetIsInteractibleNextButton()
	{
		return _nextTitleButton.IsInteractable();
	}

	public void InteractableCountdownButton(bool isInteractable)
	{
		if (isInteractable)
			_countdownButton.interactable = true;
		else
			_countdownButton.interactable = false;
	}

	public void InteractableAnswerButtons(bool isInteractable)
	{
		if (isInteractable)
		{
			_rightAnswerButton.interactable = true;
			_wrongAnswerButton.interactable = true;
		}
		else
		{
			_rightAnswerButton.interactable = false;
			_wrongAnswerButton.interactable = false;
		}
	}

	public bool GetIsInteractibleAnswerButtons()
	{
		return _rightAnswerButton.IsInteractable();
	}

	public void EnableOptionsButtons()
	{
		_options1Button.interactable = true;
		_options2Button.interactable = true;
		_options3Button.interactable = true;
		_options4Button.interactable = true;
		_options5Button.interactable = true;
	}

	public void DisableOptionsButtons()
	{
		_options1Button.interactable = false;
		_options2Button.interactable = false;
		_options3Button.interactable = false;
		_options4Button.interactable = false;
		_options5Button.interactable = false;
	}

	public void DisableUnusedOptionsButtons(int optionsCount)
	{
		if (optionsCount >= 5)
			return;

		if (optionsCount <= 4)
			_options5Button.interactable = false;

		if (optionsCount <= 3)
			_options4Button.interactable = false;

		if (optionsCount <= 2)
			_options3Button.interactable = false;

		if (optionsCount <= 1)
			_options2Button.interactable = false;
	}
}
