using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LogToTextFile : MonoBehaviour
{
	private void Start()
	{
		Directory.CreateDirectory(Application.streamingAssetsPath + "/Log_File/");
	}
}
