using UnityEngine;
using UnityEngine.UI;

public class SelectorScreen : MonoBehaviour
{
    [SerializeField] private GameMenu _menu;
    [SerializeField] private Camera _menuCamera;
    private RaycastHit hit;
    private Ray ray;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
#if UNITY_EDITOR
            var mousePosInScreenCoords = Input.mousePosition;
#else
            // Convert the global mouse position into a relative position for the current display
            var mousePosInScreenCoords = Display.RelativeMouseAt(Input.mousePosition);
#endif
            _menu.WriteLog("Mouse Button Down");
            // z is the display Id
            if (mousePosInScreenCoords.z == _menuCamera.targetDisplay)
            {
                _menu.WriteLog("Button down in Menu Display");

                ray = _menuCamera.ScreenPointToRay(mousePosInScreenCoords);

                if (Physics.Raycast(ray, out hit))
                {
                    _menu.WriteLog("Physics.Raycast");
                    hit.collider.gameObject.SetActive(false);
                }
            }
        }
    }
}

