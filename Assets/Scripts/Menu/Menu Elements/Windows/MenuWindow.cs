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
        //���� ���� ����� �� ���� �� ������� ������ ������
        if (transform.position.x > Screen.width)
        {
            //���������� ��� � ������� ������
            transform.position = new Vector3(Screen.width - _pixeSafeZone, transform.position.y);
        }
        //���� ���� ����� �� ���� �� ������� ������ �����
        else if (transform.position.x <= 0)
        {
            //���������� ��� � ������� ������
            transform.position = new Vector3(_pixeSafeZone, transform.position.y);
        }

        //���� ���� ����� �� ������ �� ������� ������ ������
        if (transform.position.y > Screen.height)
        {
            //���������� ��� � ������� ������
            transform.position = new Vector3(transform.position.x, Screen.height - _pixeSafeZone);
        }
        //���� ���� ����� �� ������ �� ������� ������ �����
        else if (transform.position.y <= 0)
        {
            //���������� ��� � ������� ������
            transform.position = new Vector3(transform.position.x, _pixeSafeZone);
        }
    }
    */
}