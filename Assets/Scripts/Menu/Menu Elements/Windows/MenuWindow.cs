using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]

public class MenuWindow : BaseMenuElement //, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /*
	private Vector3 _mouseStartingPosition;
	private bool _isDragging = true;

    private int _pixeSafeZone = 20;

    public void OnBeginDrag(PointerEventData eventData)
	{
		_mouseStartingPosition = Input.mousePosition;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (_isDragging)
		{
			transform.position -= _mouseStartingPosition - Input.mousePosition;
			_mouseStartingPosition = Input.mousePosition;
		}
	}

	void IEndDragHandler.OnEndDrag(PointerEventData eventData)
	{
		CheckOutScreen();
	}

    private void CheckOutScreen()
    {
        //если окно вышло по иску за пределы экрана вправо
        if (transform.position.x > Screen.width)
        {
            //возвращаем его в пределы экрана
            transform.position = new Vector3(Screen.width - _pixeSafeZone, transform.position.y);
        }
        //если окно вышло по иску за пределы экрана влево
        else if (transform.position.x <= 0)
        {
            //возвращаем его в пределы экрана
            transform.position = new Vector3(_pixeSafeZone, transform.position.y);
        }

        //если окно вышло по игреку за пределы экрана вправо
        if (transform.position.y > Screen.height)
        {
            //возвращаем его в пределы экрана
            transform.position = new Vector3(transform.position.x, Screen.height - _pixeSafeZone);
        }
        //если окно вышло по игреку за пределы экрана влево
        else if (transform.position.y <= 0)
        {
            //возвращаем его в пределы экрана
            transform.position = new Vector3(transform.position.x, _pixeSafeZone);
        }
    }
    */
}