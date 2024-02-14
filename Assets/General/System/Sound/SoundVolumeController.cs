using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum VolumeType
{
    General,
    Music,
    Effects
}

public class SoundVolumeController: MonoBehaviour
{
    public static float generalVolume = 1f;
    public static float musicVolume = 1f;
    public static float effectsVolume = 1f;

    public static SoundVolumeController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
		}
        else
        {
            Debug.LogError($"duplicated SoundVolumeController found, deleting {gameObject.name}");
            Destroy(gameObject);
        }
    }

    public static void SetVolume(VolumeType type, float value) { 
        switch(type)
        {
            case VolumeType.General:
                {
                    SetGeneralVolume(value);
				}
				break;
			case VolumeType.Music:
				{
					SetMusicVolume(value);
				}
				break;
			case VolumeType.Effects:
				{
					SetEffectsVolume(value);
				}
				break;
		}
    }

	public static void SetGeneralVolume(float value)
    {
        generalVolume = value;
	}
    public static void SetMusicVolume(float value)
    {
		musicVolume = value;
	}
	public static void SetEffectsVolume(float value)
	{
		effectsVolume = value;
	}
}
