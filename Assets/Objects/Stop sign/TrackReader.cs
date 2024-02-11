using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

public class TrackReader : MonoBehaviour
{
    [SerializeField] private float animationTime;
	[SerializeField] private TileBase solidTile;
	[SerializeField] private TileBase barrierTile;

	private static Tilemap tilemap;

	private static int minX;
    private static int maxX;
	private static float columnDelay;

	private static TrackReader instance;
	private static Transform focusPoint;
	
	private void Awake()
	{
		tilemap = GameObject.FindGameObjectWithTag("Track Tilemap").GetComponent<Tilemap>();
	}

	void Start()
    {
		Debug.Log($"start {gameObject.GetInstanceID()}");
		if (instance == null)
		{
			instance = this;
			focusPoint = transform.GetChild(0);
			focusPoint.gameObject.SetActive(false);
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public static void LoadPoints(int minPoint, int maxPoint)
	{
		minX = minPoint;
		maxX = maxPoint;

		if (tilemap == null)
		{
			tilemap = GameObject.FindGameObjectWithTag("Track Tilemap").GetComponent<Tilemap>();
		}

		columnDelay = instance.animationTime / (Mathf.Abs(maxPoint - minPoint) + 1);

		instance.StopAllCoroutines();
		instance.StartCoroutine(instance.MoveFocusPoint());
	}
	public static void LoadInstaPoints(int minPoint, int maxPoint)
	{
		minX = minPoint;
		maxX = maxPoint;

		tilemap = GameObject.FindGameObjectWithTag("Track Tilemap").GetComponent<Tilemap>();
		int tilemapHeight = tilemap.cellBounds.max.y - tilemap.cellBounds.min.y;
		
		#region Start Byte
		float startByte = 0f;
		int startByteOffset = 0;
		instance.GetStartByte(tilemapHeight, ref startByte, ref startByteOffset);
		#endregion
		#region End Byte
		float endByte = 0f;
		int endByteCutOff = 0;
		instance.GetEndByte(tilemapHeight, ref endByte, ref endByteCutOff);
		#endregion

		// Total read size in bytes
		int totalBytes = (int)(endByte - startByte);

		// Buffer with all data to be written in the level
		byte[] buffer = new byte[totalBytes];

		// Path to the binary file
		string path = GetFilePath("example.tck");

		using (FileStream fileStream = new FileStream(path, FileMode.Open))
		{
			#region File byte positioning
			// Set the position to byte 8 (index 7) in the file
			fileStream.Seek((long)startByte, SeekOrigin.Begin);

			// Read 4 bytes (from byte 8 to byte 11)
			int bytesRead = fileStream.Read(buffer, 0, totalBytes);
			#endregion

			int bitItr = startByteOffset;
			int byteItr = 0;
			byte currentByte = buffer[byteItr];

			for(int x = minX; x <= maxX; x++)
			{
				Debug.Log($"iterating: {x}");
				instance.IterateColumn(x, tilemapHeight, buffer, ref currentByte, ref byteItr, ref bitItr);
			}
		}
	}

	private IEnumerator MoveFocusPoint()
	{
		focusPoint.gameObject.SetActive(true);
		int tilemapHeight = tilemap.cellBounds.max.y - tilemap.cellBounds.min.y;
		#region Start Byte
		float startByte = 0f;
		int startByteOffset = 0;
		GetStartByte(tilemapHeight, ref startByte, ref startByteOffset);
		#endregion

		#region End Byte
		float endByte = 0f;
		int endByteCutOff = 0;
		GetEndByte(tilemapHeight, ref endByte, ref endByteCutOff);
		#endregion

		// Total read size in bytes
		int totalBytes = (int)(endByte - startByte);

		// Buffer with all data to be written in the level
		byte[] buffer = new byte[totalBytes];

		// Path to the binary file
		string path = GetFilePath("example.tck");

		using (FileStream fileStream = new FileStream(path, FileMode.Open))
		{
			#region File byte positioning
			// Set the position to byte 8 (index 7) in the file
			fileStream.Seek((long)startByte, SeekOrigin.Begin);

			// Read 4 bytes (from byte 8 to byte 11)
			int bytesRead = fileStream.Read(buffer, 0, totalBytes);
			#endregion

			int bitItr = startByteOffset;
			int byteItr = 0;
			byte currentByte = buffer[byteItr];

			#region While Loop Setup
			CameraMovement.SetTarget(focusPoint);
			float time = 0f;
			float maxDistance = Mathf.Abs(maxX - minX) + 1f;
			float maxTime = maxDistance * columnDelay;
			int lastIndex = (int)Mathf.Floor(minX) -1;
			#endregion

			while (time < animationTime)
			{
				time += Time.deltaTime;
				float percent = Mathf.Clamp(time / maxTime, 0f, 1f);
				float distance = percent * maxDistance;
				Vector3 focusPos = Vector3.Lerp(
					new(minX, 0f, 0f), new(maxX, 0f, -10f),
					percent
				);
				focusPoint.position = focusPos;

				while((minX + distance) > lastIndex && (minX + (int)Mathf.Floor(distance)) > lastIndex)
				{
					lastIndex++;
					IterateColumn(lastIndex, tilemapHeight, buffer, ref currentByte, ref byteItr, ref bitItr);
				}

				yield return null;
			}
		}
		focusPoint.gameObject.SetActive(false);
		CameraMovement.SetTarget(PlayerCore.rb.transform);
	}

	private void IterateColumn(int x, int tilemapHeight, byte[] buffer, ref byte currentByte, ref int byteItr, ref int bitItr)
	{
		BoundsInt bounds = new(
			new(x, tilemap.cellBounds.min.y, 0),
			new(1, tilemapHeight, 1)
		);
		TileBase[] columnTiles = new TileBase[tilemapHeight];

		string tiles = "";
		for (int i = 0; i < columnTiles.Length; i++)
		{
			#region Column Tiles
			int mask = (int)Mathf.Pow(2, 7 - bitItr);
			tiles += (currentByte & mask) > 0 ? "1" : "0";

			columnTiles[i] = (currentByte & mask) > 0 ? solidTile : null;

			bitItr++;
			if (bitItr == 8)
			{
				byteItr++;
				bitItr = 0;
				if (byteItr < buffer.Length)
				{
					currentByte = buffer[byteItr];
				}
				else
				{
					break;
				}
				Debug.Log($"byte ind: {byteItr}");
			}
			#endregion
		}
		Debug.Log($"column tiles: {tiles}");
		tilemap.SetTilesBlock(bounds, columnTiles);
	}

	private void GetStartByte(int tilemapHeight, ref float startByte, ref int startByteOffset)
	{
		int minDistance = minX - tilemap.cellBounds.min.x;
		if (minDistance < 0)
		{
			Debug.LogError($"minY não pode ser menor que a borda esquerda do tilemap");
		}
		int startBit = minDistance * tilemapHeight + 1;

		startByte = (startBit - 1) / 8f;
		startByteOffset = 0;
		if (startByte != Mathf.Floor(startByte))
		{
			startByte = Mathf.Floor((startBit - 1) / 8f);
			startByteOffset = (startBit - 1) % 8;
		}
		else
		{
			startByte = Mathf.Floor((startBit - 1) / 8f);
		}
	}

	private void GetEndByte(int tilemapHeight, ref float endByte, ref int endByteCutOff)
	{
		int maxDistance = maxX - tilemap.cellBounds.min.x;
		if (maxX > tilemap.cellBounds.max.x)
		{
			Debug.LogError($"maxX não pode ser maior que a borda direita do tilemap");
		}
		int endBit = (maxDistance + 1) * tilemapHeight + 1;

		endByte = (endBit - 1) / 8f;
		endByteCutOff = 7;
		if (endByte != Mathf.Floor(endByte))
		{
			endByte = Mathf.Floor((endBit - 1) / 8f);
			endByteCutOff = (int)((endBit - 1) % 8);
		}
	}

	private static string GetFilePath(string fileName)
	{
		return Application.persistentDataPath + $"/{fileName}";
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if (focusPoint == null)
		{
			return;
		}

		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(focusPoint.position, .8f);
	}
#endif
}
