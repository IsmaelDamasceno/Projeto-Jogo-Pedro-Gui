using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeController : MonoBehaviour
{

    private static VolumeController instance;
    private Volume volume;

    void Start()
    {
        if (instance == null)
        {
            instance = this;

            volume = GetComponent<Volume>();

			DontDestroyOnLoad(gameObject);
        }   
        else
        {
            Debug.LogError($"Script duplicado, deletando objeto: {gameObject.name}");
            Destroy(gameObject);
        }
    }

    public static void SetProfile(string profileName)
    {
        VolumeProfile newSelectedProfile = Resources.Load<VolumeProfile>($"Volumes/{profileName}");
        if (!newSelectedProfile)
        {
            Debug.LogError($"Profile \'{profileName}\" não encontrado");
            return;
        }
        instance.volume.profile = newSelectedProfile;
    }
}
