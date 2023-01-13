using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProperties : BaseProperties
{
	[Header("Service")]
	[SerializeField] private GameColorChanger _gameColorChanger;
	public GameColorChanger GameColorChanger => _gameColorChanger;


	#region PreviewMenu
	[Space]
	[SerializeField] private RenderTexture _previewRenderTexture;
	public RenderTexture PreviewRenderTexture => _previewRenderTexture;
	#endregion

	#region Animation Properties

	[Header("Animation Properties")]
	[Tooltip("The time for an UI Elements to fade in/out")]
	[SerializeField] private float _fadeInOutUIElements;
	[Space]
	[Tooltip("The time delay between Titles")]
	[SerializeField] private float _delayBetweenTitleAnimations;
	[Tooltip("The time delay between elements inside the Title")]
	[SerializeField] private float _delayBetweenElementAnimations;
	[Header("Teams Title")]
	[Tooltip("The scale of the TeamFields on the TeamTitle. Deselected scale = 1")]
	[SerializeField] private float _scaleSelectedTeamFields;
	[Tooltip("Time to change the scale of the TeamFields")]
	[SerializeField] private float _scalingTime;
	[Tooltip("The Time of deposite to the Team on Preparation Teams Title")]
	[SerializeField] private float _changeTeamMoneyTime;
	[Space]
	[Tooltip("The offset of the element relative to the main Transform. Only the Position works")]
	[SerializeField] private Vector3 _offsetPosition;
	[SerializeField] private Vector3 _offsetRotation;
	[SerializeField] private Vector3 _offsetSize;
	[Space]
	[Tooltip("How much to increase the size of the selected option")]
	[SerializeField] private Vector3 _addSizeOfSelectedOption;
	[Space]
	[Tooltip("The delay time between setting the resolution and displaying information on Display Tab in Menu")]
	[SerializeField] private float _delayBetweenSetResolutionAndDisplay;

	public float FadeInOutUIElements => _fadeInOutUIElements;
	public float DelayBetweenElementAnimations => _delayBetweenElementAnimations;
	public float ScaleSelectedTeamFields => _scaleSelectedTeamFields;
	public float ScalingTime => _scalingTime;
	public float DelayBetweenTitleAnimations => _delayBetweenTitleAnimations;
	public float ChangeTeamMoneyTime => _changeTeamMoneyTime;
	public Vector3 OffsetPosition => _offsetPosition;
	public Vector3 OffsetRotation => _offsetRotation;
	public Vector3 OffsetSize => _offsetSize;
	public Vector3 AddSizeOfSelectedOption => _addSizeOfSelectedOption;
	public float DelayBetweenSetResolutionAndDisplay => _delayBetweenSetResolutionAndDisplay;
	#endregion

	#region Audio Vizualizator
	[Header("Audio Vizualizator")]
	[Tooltip("The height of audio envelope")]
	[SerializeField] private float _heightAudioEnvelope;
	[Tooltip("The number of points in one half of the sound envelope, aka Quality")]
	[Range(30, 230)] //не более 247, иначе не визуализируется аудио огибающая.
	[SerializeField] private int _sampleCount;

	public float HeightAudioEnvelope => _heightAudioEnvelope;
	public int SampleCount => _sampleCount;

	#endregion

	#region Game Space
	[Header("Game Space")]
	[SerializeField] private float _endZoneRadius;

	public float EndZoneRadius => _endZoneRadius;
	#endregion

	#region Monetary Units
	[Header("Monetary Units")]
	[SerializeField] private char[] _monetaryUnits;

	public char[] MonetaryUnits => _monetaryUnits;
	#endregion

	#region Lighting
	[Header("Lighting Answer")]
	[SerializeField] private float _timeOfLightChange;
	[SerializeField] private float _illuminationTime;
	[Header("Burst Of Glowing by Default")]
	[SerializeField] private float _fadeInBurstTimeByDefault;
	[SerializeField] private float _fadeOutBurstTimeByDefault;
	[Header("Burst Of Glowing Answer")]
	[SerializeField] private float _fadeInBurstTime;
	[SerializeField] private float _fadeOutBurstTime;

	public float TimeOfLightChange => _timeOfLightChange;
	public float IlluminationTime => _illuminationTime;
	public float FadeInBurstTimeByDefault => _fadeInBurstTimeByDefault;
	public float FadeOutBurstTimeByDefault => _fadeOutBurstTimeByDefault;
	public float FadeInBurstTime => _fadeInBurstTime;
	public float FadeOutBurstTime => _fadeOutBurstTime;
	#endregion

	#region Colors
	[Header("Glow Colors")]
	[SerializeField] private Color _glowColor;

	public Color GlowColor => _glowColor;
	#endregion

	#region Curves
	[Header("Curves")]
	[SerializeField] private AnimationCurve _fadeIn;
	[SerializeField] private AnimationCurve _fadeOut;
	[SerializeField] private AnimationCurve _easeSoftInOut;
	[SerializeField] private AnimationCurve _easeHardInOut;

	public AnimationCurve FadeIn => _fadeIn;
	public AnimationCurve FadeOut => _fadeOut;
	public AnimationCurve EaseSoftInOut => _easeSoftInOut;
	public AnimationCurve EaseHardInOut => _easeHardInOut;
	#endregion
}
