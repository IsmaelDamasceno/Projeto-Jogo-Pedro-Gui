using System;
using System.IO;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class PlayerScore
{
    public string name;
    public float time;
    public int health;
}

public class CheckpointSave : MonoBehaviour
{
	public static string activePlayerName;
    public static byte activeCheckpoint = 0;
    public static float activeCheckpointtime;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

	public static void Restart()
	{
		activePlayerName = "";
		activeCheckpoint = 0;
		activeCheckpointtime = 0f;
		TimerController.time = 0f;
		ClockCollision.clockColected = false;
		HealthSystem.hasDiedOnce = false;
		TrackWriter.trackLoaded = false;

		CheckpointManager.curIndPoint = 0;
		CheckpointManager.minPoint = 0;
		CheckpointManager.maxPoint = 1;
	}

    public static void Save()
    {
		byte[] name = Encoding.ASCII.GetBytes(activePlayerName);
		byte health = (byte)HealthSystem.health;
		float time = TimerController.time;

		string folderPath = Path.Combine(Application.persistentDataPath, "Saves");
		if (!Directory.Exists(folderPath))
		{
			Directory.CreateDirectory(folderPath);
		}

		string saveFile = $"save #{activePlayerName} ${Random.Range(0, 50000)}.sav";
		string filePath = Path.Combine(folderPath, saveFile);

		using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
			{
				binaryWriter.Write(time);
				binaryWriter.Write(health);
			}
		}
	}
}
