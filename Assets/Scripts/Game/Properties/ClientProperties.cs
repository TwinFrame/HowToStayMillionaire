using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientProperties : BaseProperties
{
	[Space]
	[SerializeField] private Texture2D _transparentTexture;
	[Space]
	[SerializeField] private Material _previewMaterial;

	public Texture2D TransparentTexture => _transparentTexture;

	public Material PreviewMaterial => _previewMaterial;
}
