using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Events;

public class DoubleClickDetector : MonoBehaviour, IPointerUpHandler
{
    private int _clickCount = 0;

    private float _timeBetweenClicks = 0.35f;
    private float _timeFirstClick;
    private Coroutine _doubleClickJob;

    public UnityAction DoubleClickDetectedEvent;

    public void OnPointerUp(PointerEventData eventData)
    {
        _clickCount++;

        if (_clickCount == 1)
        {
            if (_doubleClickJob != null)
                StopCoroutine(_doubleClickJob);
            _doubleClickJob = StartCoroutine(DoubleClickJob());
        }

        else if (_clickCount == 2 && _doubleClickJob != null)
        {
            StopCoroutine(_doubleClickJob);

            _clickCount = 0;

            DoubleClickDetectedEvent?.Invoke();
        }
        
    }

    private IEnumerator DoubleClickJob()
    {
        _timeFirstClick = Time.time;

        yield return new WaitUntil(() => Time.time - _timeFirstClick >= _timeBetweenClicks);

        _clickCount = 0;
    }
}
