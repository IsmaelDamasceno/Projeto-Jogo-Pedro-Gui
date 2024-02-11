using System;
using System.IO;
using System.Text;
using UnityEngine;

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

    public static void Save(byte checkpointIndex)
    {
		byte[] name = Encoding.ASCII.GetBytes(activePlayerName);
		byte health = (byte)HealthSystem.health;
		byte nameLength = (byte)name.Length;
		// float time = TimerController.time;

		string saveFile = $"save# {activePlayerName}";
		string filePath = Path.Combine(Application.persistentDataPath, saveFile);

		using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
			{
				// binaryWriter.Write(time);
				binaryWriter.Write(health);
				binaryWriter.Write(nameLength);
				binaryWriter.Write(name);
			}
		}
	}
}
