using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEditor.PlayerSettings;

/// <summary>
/// Controla o movimento e restrições da cãmera
/// </summary>
public class CameraMovement : MonoBehaviour
{
	[SerializeField] private float lerpT;
	[SerializeField] private float zDepth;

	/// <summary>
	/// Struct que repreenta as bordas da fase, as quais a câmera não pode passar
	/// </summary>
	private Bounds camBounds;

	private Transform playerTrs;

	/// <summary>
	/// Dimensões da câmera
	/// </summary>
	public static float camWidth;
	public static float camHeight;

	void Start()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		playerTrs = player.transform;

		camBounds =
			GameObject.FindGameObjectWithTag("Cam Bounds").GetComponent<BoxCollider2D>().bounds;

		Camera cam = Camera.main;
		camHeight = 2f * cam.orthographicSize;
		camWidth = camHeight * cam.aspect;
	}

	private void Update()
	{
		
	}

	void FixedUpdate()
	{
		Vector3 destiny = playerTrs.position;

		destiny.x = Mathf.Clamp(
			destiny.x, camBounds.min.x + camWidth * 0.5f, camBounds.max.x - camWidth * 0.5f);
		destiny.y = Mathf.Clamp(
			destiny.y, camBounds.min.y + camHeight * 0.5f, camBounds.max.y - camHeight * 0.5f);

		destiny = Vector3.Lerp(transform.position, destiny, lerpT);
		destiny.z = zDepth;

		transform.position = destiny;
	}

}
