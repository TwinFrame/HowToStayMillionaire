using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypesOfQuestions : MonoBehaviour
{
	public enum Type
	{
		text,
		textWithOptions,
		image,
		imageWithOptions,
		imageOptions,
		video,
		videoWithOptions,
		audio,
		audioWithOptions,
		imageHidden,
		karaoke
	}
}
