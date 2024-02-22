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
	[SerializeField] private AnimationCurve cameraHeightBlendCurve;

	/// <summary>
	/// Struct que repreenta as bordas da fase, as quais a câmera não pode passar
	/// </summary>
	public static Bounds camBounds;

	private static Transform targetTrs;
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

	public static float startHeight;
	public static void SetTarget(Transform newTarget)
	{
		if (newTarget == null)
		{
			targetTrs = Camera.main.transform;
		}
		else
		{
			targetTrs = newTarget;
		}
	}

	void Start()
	{
		shakeScale = 0f;
		shaking = false;

		camBounds =
			GameObject.FindGameObjectWithTag("Cam Bounds").GetComponent<BoxCollider2D>().bounds;

		SetTarget(PlayerCore.rb.transform);

		Camera cam = Camera.main;
		camHeight = 2f * cam.orthographicSize;
		camWidth = camHeight * cam.aspect;
		startHeight = camHeight;

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
		}
		#endregion

		Vector3 destiny = targetTrs.position;

		destiny.x = Mathf.Clamp(
			destiny.x, camBounds.min.x + camWidth * 0.5f, camBounds.max.x - camWidth * 0.5f);
		destiny.y = Mathf.Clamp(
			destiny.y, camBounds.min.y + camHeight * 0.5f, camBounds.max.y - camHeight * 0.5f);
		
		destiny += (Vector3)offset;

		destiny = Vector3.Lerp(transform.position, destiny, lerpT);
		destiny.z = zDepth;

		transform.position = destiny;
	}

	public static void ShakeIt(float scale, float time)
	{
		shaking = true;
		shakeScale = scale;
		shakeStart = Time.unscaledTime;
		shakeStop = shakeStart + time;
	}

	public static void SetCameraHeight(float newHeight, float time)
	{
		instance.StartCoroutine(instance.CameraHeightCoroutine(newHeight, time));
	}

	IEnumerator CameraHeightCoroutine(float newHeight, float animationTime)
	{
		Camera cam = Camera.main;
		float currentTime = 0f;
		float initialHeight = camHeight;
		while(currentTime < animationTime)
		{
			currentTime += Time.deltaTime;
			currentTime = Mathf.Clamp(currentTime, 0f, animationTime);

			float percent = currentTime / animationTime;
			float point = cameraHeightBlendCurve.Evaluate(percent);
			camHeight = Mathf.Lerp(initialHeight, newHeight, point);
			cam.orthographicSize = camHeight / 2f;

			yield return null;
		}
	}
}
