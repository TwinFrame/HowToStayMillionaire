using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromCameraToBytes : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    //[SerializeField] private RenderTe _texture;

    private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            StartCoroutine(RenderTexturesScreenCapture(Application.persistentDataPath + "/CaptureScreenshot1" + Time.time.ToString() + ".png"));
        }
    }

    private IEnumerator RenderTexturesScreenCapture(string filename)
    {
        yield return _waitForEndOfFrame;
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        Texture2D screenShot = new Texture2D(256, 256, TextureFormat.RGB24, false);
        // Traverse all cameras
        //foreach (Camera camera in Camera.allCameras)
        //{
        //    camera.targetTexture = rt;
        //    camera.Render();
        //    camera.targetTexture = null;
        //}
        //Camera.main.targetTexture = rt;
        //Camera.main.Render();
        //Camera.main.targetTexture = null;
        Camera.allCameras[1].targetTexture = rt;
        Camera.allCameras[1].Render();
        Camera.allCameras[1].targetTexture = null;

        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(Screen.width / 4, Screen.height / 4, Screen.width / 4 * 3, Screen.height / 4 * 3), 0, 0);
        //Camera.main.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        yield return 0;
        byte[] bytes = screenShot.EncodeToPNG();
        screenShot.LoadImage(bytes);

        //File.WriteAllBytes(filename, bytes);
    }
}
