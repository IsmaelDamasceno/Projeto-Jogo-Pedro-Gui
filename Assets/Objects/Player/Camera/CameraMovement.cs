using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using Random = UnityEngine.Random;

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
	private static CameraMovement instance;

	/// <summary>
	/// Dimensões da câmera
	/// </summary>
	public static float camWidth;
	public static float camHeight;

	private static float shakeScale;
	private static float shakeStart;
	private static float shakeStop;
	private static float shakePercent;
	private static bool shaking;

	private static Vector2 offset;

	void Start()
	{
		shakeScale = 0f;
		shaking = false;

		GameObject player = GameObject.FindGameObjectWithTag("Player");
		playerTrs = player.transform;

		camBounds =
			GameObject.FindGameObjectWithTag("Cam Bounds").GetComponent<BoxCollider2D>().bounds;

		Camera cam = Camera.main;
		camHeight = 2f * cam.orthographicSize;
		camWidth = camHeight * cam.aspect;

		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Debug.LogError($"Duas instâncias de CameraMovement encontradas, deletando {gameObject.name}");
			Destroy(gameObject);
		}
	}

	private void Update()
	{
		#region Shake
		if (shaking)
		{
			shakePercent = (Time.unscaledTime - shakeStart) / (shakeStop - shakeStart);
			if (shakePercent >= 1f)
			{
				shaking = false;
				offset = Vector2.zero;
				return;
			}

			offset = shakePercent * shakeScale * Random.insideUnitCircle;

			Vector3 destiny = playerTrs.position;

			destiny.x = Mathf.Clamp(
				destiny.x, camBounds.min.x + camWidth * 0.5f, camBounds.max.x - camWidth * 0.5f);
			destiny.y = Mathf.Clamp(
				destiny.y, camBounds.min.y + camHeight * 0.5f, camBounds.max.y - camHeight * 0.5f);

			destiny += (Vector3)offset;

			destiny = Vector3.Lerp(transform.position, destiny, lerpT);
			destiny.z = zDepth;

			transform.position = destiny;
		}
		#endregion
	}

	public static void ShakeIt(float scale, float time)
	{
		shaking = true;
		shakeScale = scale;
		shakeStart = Time.unscaledTime;
		shakeStop = shakeStart + time;
	}

	void FixedUpdate()
	{
		Vector3 destiny = playerTrs.position;

		destiny += (Vector3)offset;

		destiny.x = Mathf.Clamp(
			destiny.x, camBounds.min.x + camWidth * 0.5f, camBounds.max.x - camWidth * 0.5f);
		destiny.y = Mathf.Clamp(
			destiny.y, camBounds.min.y + camHeight * 0.5f, camBounds.max.y - camHeight * 0.5f);

		destiny = Vector3.Lerp(transform.position, destiny, lerpT);
		destiny.z = zDepth;

		transform.position = destiny;
	}

}
