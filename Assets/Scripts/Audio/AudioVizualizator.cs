using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public class AudioVizualizator : MonoBehaviour
{
	[SerializeField] private AdvancedAudioPlayer _audio;
	[Space]
	[SerializeField] private SpriteShapeController _audioShape;
	[SerializeField] private SpriteShapeController _audioMaskShape;
	[SerializeField] private SpriteMask _audioMask;
	[Space]
	[SerializeField] Transform _startPoint;
	[SerializeField] Transform _endPoint;

	[Header("Service")]
	[SerializeField] private GameProperties _properties;

	private float _maxSample;
	private float _normalizeScale;
	private float[] _samples;
	private float[] _averageSamples;

	private Vector3 _soundDistance;
	private Vector3 _stepBeetwenPoints;
	private Vector3[] _upLinePoints;
	private Vector3[] _downLinePoints;
	private Vector3[] _points;

	private Spline _audioShapeSpline;
	private Spline _audioMaskShapeSpline;

	private Vector3 _maskArea;
	private float _reserveMaskArea = 5f;

	private Coroutine _fillInAlreadyPlayedPartJob;

	private float _normalizeTime { get { return (float)_audio.NormalizeTime; } }

	public void Enter()
	{
		if (TryGetPreparationShapeData(_audio))
		{
			VizualizeShapeSound(_points);

			//if (TryGetShapeRenderer(out List<SpriteShapeRenderer> renderers))
			//	ColorizeShapeSound(renderers);
			
			if (_fillInAlreadyPlayedPartJob != null)
				StopCoroutine(_fillInAlreadyPlayedPartJob);
			_fillInAlreadyPlayedPartJob = StartCoroutine(FillInAlreadyPlayedPartJob());
			
		}
	}

	public void Exit()
	{
		if (_fillInAlreadyPlayedPartJob != null)
			StopCoroutine(_fillInAlreadyPlayedPartJob);

		ResetVizualizator();
	}

	public bool TryGetShapeRenderer(out List<SpriteShapeRenderer> renderers)
	{

		if (_audioShape.TryGetComponent<SpriteShapeRenderer>(out SpriteShapeRenderer audioRenderer))
		{
			if (_audioMaskShape.TryGetComponent<SpriteShapeRenderer>(out SpriteShapeRenderer audioMaskRenderer))
			{
				renderers = new List<SpriteShapeRenderer>(2);

				renderers.Add(audioRenderer);
				renderers.Add(audioMaskRenderer);
				return true;
			}

		}

		renderers = null;
		return false;
	}

	private bool TryGetPreparationShapeData(AdvancedAudioPlayer audio)
	{
		if (InitAudioData(audio))
		{
			if (InitShapeData(_averageSamples, _startPoint.localPosition, _endPoint.localPosition))
			{
				if (InitAudioMask())
					return true;

				return false;
			}
			return false;
		}
		return false;
	}


	private bool InitAudioData(AdvancedAudioPlayer audio)
	{
		if (audio.Clip.preloadAudioData)
			audio.Clip.LoadAudioData();

		_samples = new float[audio.Clip.samples * audio.Clip.channels];
		audio.Clip.GetData(_samples, 0);

		AverageSamples(_samples, _properties.SampleCount, out _averageSamples);

		if (_averageSamples == null || _averageSamples.Length == 0)
		{
			Debug.Log("_averageSamples == null or _averageSamples.Length == 0");
			return false;
		}

		return true;
	}

	private bool InitShapeData(float[] samples, Vector3 startPosition, Vector3 endPosition)
	{
		_soundDistance = endPosition - startPosition;

		if (_soundDistance == Vector3.zero)
		{
			Debug.Log("_distanceEnvelope == 0");
			return false;
		}

		_stepBeetwenPoints = _soundDistance / samples.Length;

		_maxSample = samples.Max<float>();

		if (_maxSample == 0)
		{
			Debug.Log("_maxSample == 0");
			return false;
		}

		_normalizeScale = 1 / _maxSample;

		_upLinePoints = new Vector3[samples.Length];
		_downLinePoints = new Vector3[samples.Length];

		for (int i = 0; i < samples.Length; i++)
		{
			_upLinePoints[i] = new Vector3(i * _stepBeetwenPoints.x, samples[i] * _normalizeScale * _properties.HeightAudioEnvelope + i * _stepBeetwenPoints.y, 0);
			_downLinePoints[i] = new Vector3(i * _stepBeetwenPoints.x, -samples[i] * _normalizeScale * _properties.HeightAudioEnvelope + i * _stepBeetwenPoints.y, 0);
		}

		_points = new Vector3[_upLinePoints.Length + _downLinePoints.Length - 1];

		int j = _downLinePoints.Length - 2;

		for (int i = 0; i < _points.Length; i++)
		{
			if (i <= _upLinePoints.Length - 1)
			{
				_points[i] = _upLinePoints[i];
			}
			else
			{
				_points[i] = _downLinePoints[j];
				j--;
			}
		}

		return true;
	}

	private bool InitAudioMask()
	{
		_audioMask.transform.position = _startPoint.transform.localPosition;

		if (_soundDistance == Vector3.zero || _soundDistance == null)
		{
			Debug.Log("_soundDistance = 0 or null");
			return false;
		}

		_maskArea = new Vector3(_soundDistance.x, _properties.HeightAudioEnvelope * 2 + Mathf.Abs(_endPoint.localPosition.y * 2) + _reserveMaskArea, _soundDistance.z);

		_audioMask.transform.localScale = _maskArea;

		return true;
	}

	private void AverageSamples(float[] samples, int sampleCount, out float[] averageSamples)
	{
		int samplesInSlot = samples.Length / sampleCount;

		averageSamples = new float[sampleCount];

		for (int i = 0; i < averageSamples.Length; i++)
		{
			averageSamples[i] = 0;

			if (i == 0 || i == averageSamples.Length - 1)
			{
				averageSamples[i] = 0;
			}
			else
			{
				for (int j = 0; j < samplesInSlot; j++)
				{
					if (((i * samplesInSlot) + j) >= samples.Length)
					{
						Debug.Log("Sample " + ((i * samplesInSlot) + j) + " more than need");
					}
					else
					{
						averageSamples[i] += Mathf.Abs(samples[(i * samplesInSlot) + j]);
					}
				}

				averageSamples[i] /= samplesInSlot;
			}
		}
	}

	private IEnumerator FillInAlreadyPlayedPartJob()
	{
		while (true)
		{
			_audioMask.transform.position = Vector3.Lerp(_startPoint.position, _endPoint.position, _normalizeTime);

			yield return null;
		}
	}

	private void VizualizeShapeSound(Vector3[] points)
	{
		_audioShapeSpline = _audioShape.spline;
		_audioMaskShapeSpline = _audioMaskShape.spline;

		TransferPointsToSpline(_audioShapeSpline, points);
		TransferPointsToSpline(_audioMaskShapeSpline, points);
	}

	private void TransferPointsToSpline(Spline spline, Vector3[] points)
	{
		spline.Clear();

		for (int i = 0; i < points.Length - 1; i++) // если убрать "- 1", возникнет ошибка " Point too close to neighbor".
		{
			spline.InsertPointAt(i, points[i]);
			spline.SetTangentMode(i, ShapeTangentMode.Continuous);
			//spline.SetRightTangent(i, Vector3.down * _tangentLength);
			//spline.SetLeftTangent(i, Vector3.up * _tangentLength);
		}
	}

	private void ResetVizualizator()
	{
		_maskArea = Vector3.zero;

		_soundDistance = Vector3.zero;
		_stepBeetwenPoints = Vector3.zero;
		_maxSample = 0;
		/*
		_upLinePoints = null;
		_downLinePoints = null;
		_points = null;
		_samples = null;
		_averageSamples = null;
		*/
	}

	private double ClampDouble(double value, double min, double max)
	{
		if (value < min) { value = min; return value; }

		if (value > max) { value = max; return value; }

		return value;
	}
}