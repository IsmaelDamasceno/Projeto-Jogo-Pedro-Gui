#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Windows;
using File = System.IO.File;

public enum StripColor : byte
{
	Blue,
	Red,
	Yellow,
	Green,
	Purple
}

public struct GraphPos
{

	public GraphPos(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	public float x;
	public float y;
}
public struct Strip
{
	public Strip(float x, string label, byte color)
	{
		this.x = x;
		this.label = label;
		this.color = color;
	}

	public float x;
	public string label;
	public byte color;
}

public class Speedmeter : MonoBehaviour
{

	public static Speedmeter instance;

	private List<GraphPos> posList = new();
	private List<Strip> stripList = new();

	private bool recording = false;

	private Rigidbody2D playerRb;

	private float time = 0f;

	private int inputLastFrame = 0;

	private void Start()
	{
		if (instance == null)
		{
			instance = this;

			playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
		}
		else
		{
			Debug.LogError($"Instância de componente SpeedMeter já existante, deletando {gameObject.name}");
			Destroy(gameObject);
		}
	}

	public void StartRecording()
	{
		recording = true;
	}
	public void StopRecording()
	{
		string fileName = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/file.grp";

		using (var stream = File.Open(fileName, FileMode.Create))
		{
			using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
			{
				writer.Write(posList.Count);
				foreach (GraphPos pos in posList)
				{
					writer.Write(pos.x);
					writer.Write(pos.y);
				}

				writer.Write(stripList.Count);
				foreach(Strip strip in stripList)
				{
					writer.Write(strip.color);

					byte[] label = Encoding.ASCII.GetBytes(strip.label);
					writer.Write(label.Length);
					writer.Write(label);
				}
			}
		}

		posList = new();
		stripList = new();
		time = 0f;
		recording = false;
	}

	private void Update()
	{
		if (recording)
		{
			// Positions
			time += Time.deltaTime;
			posList.Add(new GraphPos(time, playerRb.velocity.x));

			// Strips
			int input = InputController.moveAxis.GetValRaw();
			if (input != inputLastFrame)
			{
				if (input == -1)
				{
					stripList.Add(new Strip(time, "Move A", (byte)StripColor.Blue));
				}
				else if (input == 0)
				{
					stripList.Add(new Strip(time, "Stop Moving", (byte)StripColor.Red));
				}
				else if (input == 1)
				{
					stripList.Add(new Strip(time, "Move D", (byte)StripColor.Blue));
				}
			}
			inputLastFrame = input;
		}
	}
}

#endif
