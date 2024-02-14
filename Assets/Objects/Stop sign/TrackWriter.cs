using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TrackWriter : MonoBehaviour
{
	private Tilemap tilemap;
	private static bool trackLoaded = false;
	void Awake()
	{
		tilemap = GetComponent<Tilemap>();

		string path = GetFilePath("example.tck");

		if (!trackLoaded)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				using (BinaryWriter writer = new BinaryWriter(fileStream))
				{
					byte allocationByte = 0;
					byte byteItr = 0;

					int columnHeight = tilemap.cellBounds.max.y - tilemap.cellBounds.min.y;
					for (int x = tilemap.cellBounds.min.x; x < tilemap.cellBounds.max.x; x++)
					{
						ReadColumn(x, columnHeight, writer, ref allocationByte, ref byteItr);
					}
				}
			}
		}
		TileBase[] airTiles = new TileBase[tilemap.size.x * tilemap.size.y];
		BoundsInt totalBounds = new(
			new(tilemap.cellBounds.min.x, tilemap.cellBounds.min.y, 0),
			new(tilemap.size.x, tilemap.size.y, 1)
		);
		tilemap.SetTilesBlock(totalBounds, airTiles);

		if (trackLoaded && CheckpointSave.activeCheckpoint > 0)
		{
			CheckpointManager.TrackInstaPlacement();
		}
		trackLoaded = true;
	}

	private void ReadColumn(int column, int columnHeight, BinaryWriter writer, ref byte allocationByte, ref byte byteItr)
	{
		// Bounds of the column to be read
		
		BoundsInt bounds = new(
			new(column, tilemap.cellBounds.min.y, 0),
			new(1, columnHeight, 1)
		);

		// Debug.Log($"reading column: bounds: {bounds}, byte: {allocationByte}, byteItr: {byteItr}");

		// All the tiles inside the current iteration column
		TileBase[] tiles = tilemap.GetTilesBlock(bounds);
		foreach (TileBase tile in tiles)
		{
			// Compounds tile data into single bit, and group everything in a byte, and then write that byte to the file (0 = air, 1 = track)
			// Debug.Log($"tile: {tile}");
			if (tile != null)
			{
				byte sum = (byte)Mathf.Pow(2, 7 - byteItr);
				allocationByte += sum;
			}
			byteItr++;
			if (byteItr == 8)
			{
				// Debug.Log($"byte ready: {allocationByte}");
				writer.Write(allocationByte);
				allocationByte = 0;
				byteItr = 0;
			}
		}
	}

	private string GetFilePath(string fileName)
	{
		return Application.persistentDataPath + $"/{fileName}";
	}
}
