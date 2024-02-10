using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TrackReader : MonoBehaviour
{
    [SerializeField] private float columnPlaceDelay;
	[SerializeField] private TileBase solidTile;
	
    private Tilemap tilemap;

    private int minX;
    private int maxX;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();

		minX = (int)Mathf.Floor(
            Utils.SearchObjectWithComponent<Transform>(transform, "Min X").position.x);
		maxX = (int)Mathf.Floor(
			Utils.SearchObjectWithComponent<Transform>(transform, "Max X").position.x);

		StartCoroutine(PlaceColumns());
	}

	 private IEnumerator PlaceColumns()
	 {
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

		Debug.Log($"tilemap height: {tilemapHeight}");
		Debug.Log($"minx: {minX}, maxX: {maxX}");
	
		// Total read size in bytes
		int totalBytes = (int)(endByte - startByte);

		// Buffer with all tada to be written in the level
		byte[] buffer = new byte[totalBytes];

		// Path to the binary file
		string path = GetFilePath("example.tck");

		// Open the binary file
		using (FileStream fileStream = new FileStream(path, FileMode.Open))
		{
			#region File byte positioning
			// Set the position to byte 8 (index 7) in the file
			fileStream.Seek((long)startByte, SeekOrigin.Begin);

			// Read 4 bytes (from byte 8 to byte 11)
			int bytesRead = fileStream.Read(buffer, 0, totalBytes);
			#endregion

			Debug.Log($"Start Logging");
			foreach(byte itrByte in buffer)
			{
				Debug.Log($"{Convert.ToString(itrByte, 2)} ({itrByte})");
			}

			int bitItr = startByteOffset;
			int byteItr = 0;
			byte currentByte = buffer[byteItr];
			Debug.Log($"Starting loop, initial bit offset: {bitItr}");
			for (int x = minX; x <= maxX; x++)
			{
				IterateColumn(x, tilemapHeight, buffer, ref currentByte, ref byteItr, ref bitItr);

				yield return new WaitForSeconds(columnPlaceDelay);
			}
		}
	}

	private void IterateColumn(int x, int tilemapHeight, byte[] buffer, ref byte currentByte, ref int byteItr, ref int bitItr)
	{
		Debug.Log($"Writing to column {x}");

		BoundsInt bounds = new(
			new(x, tilemap.cellBounds.min.y, 0),
			new(1, tilemapHeight, 1)
		);
		TileBase[] columnTiles = new TileBase[tilemapHeight];

		Debug.Log("starting buffer iteration for column");
		for (int i = 0; i < columnTiles.Length; i++)
		{
			int mask = (int)Mathf.Pow(2, 7 - bitItr);

			columnTiles[i] = (currentByte & mask) > 0 ? solidTile : null;
			Debug.Log($"id: {i}, tile: {(currentByte & mask) > 0}, byte: {Convert.ToString(currentByte, 2)}");

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
					Debug.LogWarning($"cannot get next, byte: {byteItr}, size: {buffer.Length}");
					break;
				}
			}
		}
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
			Debug.Log("Not exact integer start bit position");
			startByte = Mathf.Floor((startBit - 1) / 8f);
			startByteOffset = (startBit - 1) % 8;
			Debug.Log($"start byte: {startByte}, offset: {startByteOffset}");
		}
		else
		{
			Debug.Log("Exact start bit position");
			startByte = Mathf.Floor((startBit - 1) / 8f);
			Debug.Log($"start byte: {startByte}");
		}
	}

	private void GetEndByte(int tilemapHeight, ref float endByte, ref int endByteCutOff)
	{
		int maxDistance = maxX - tilemap.cellBounds.min.x;
		if (maxDistance > tilemap.cellBounds.max.x)
		{
			Debug.LogError($"maxX não pode ser maior que a borda direita do tilemap");
		}
		int endBit = (maxDistance + 1) * tilemapHeight + 1;

		endByte = (endBit - 1) / 8f;
		endByteCutOff = 7;
		if (endByte != Mathf.Floor(endByte))
		{
			Debug.Log("Not exact integer end bit position");
			endByte = Mathf.Floor((endByte - 1) / 8f);
			endByteCutOff = (int)((endByte - 1) % 8);
			Debug.Log($"end byte byte: {endByte}, cut off: {endByteCutOff}");
		}
	}

	private string GetFilePath(string fileName)
	{
		return Application.persistentDataPath + $"/{fileName}";
	}
}
