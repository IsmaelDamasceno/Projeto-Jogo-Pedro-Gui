using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneMusic
{
    public AudioClip clip;
    public bool replayOnSameScene;
    public string sceneName;


	public SceneMusic(string clipName, string sceneName, bool replayOnSameScene)
    {
        clip = Resources.Load<AudioClip>($"Music/{clipName}");
        this.replayOnSameScene = replayOnSameScene;
        this.sceneName = sceneName;
    }
}


public class MusicSystem : MonoBehaviour
{

    public static List<SceneMusic> musicList;
    public static MusicSystem instance;
    public static AudioSource source;

    public static string lastScene = "";

    void Awake()
    {
        if (instance == null)
        {
			SceneManager.sceneLoaded += SceneEnter;
            source = GetComponent<AudioSource>();
            source.loop = true;
			instance = this;
            lastScene = SceneManager.GetActiveScene().name;

            musicList = new List<SceneMusic> {
                new SceneMusic("Menu", "Main Menu", false),
                new SceneMusic("Level", "Level", false)
            };

            DontDestroyOnLoad(gameObject);
		}
        else
        {
            Debug.LogError($"Duplicated MusicSystem, deleting {gameObject.name}");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        source.volume = SoundVolumeController.generalVolume * SoundVolumeController.musicVolume;
    }

    public void SceneEnter(Scene scene, LoadSceneMode mode)
    {
		if (!source.isPlaying)
		{
			AudioClip clip = GetMusicFromSceneName(scene.name).clip;
			if (clip != null)
			{
				source.clip = clip;
				source.Play();
			}
			else
			{
				Debug.LogError($"Couldn't get clip from scene name: {scene.name}");
			}
		}
        else
        {
			SceneMusic activeSceneMusic = GetMusicFromSceneName(scene.name);
			if (scene.name == lastScene)
			{
                if (activeSceneMusic.replayOnSameScene)
                {
                    source.Stop();
					source.clip = activeSceneMusic.clip;
					source.Play();
				}
			}
			else
			{
				source.Stop();
				source.clip = activeSceneMusic.clip;
				source.Play();
			}
		}

        lastScene = scene.name;
    }

    public SceneMusic GetMusicFromSceneName(string sceneName)
    {
        foreach(SceneMusic music in musicList)
        {
            if (music.sceneName == sceneName)
            {
                return music;
            }
        }
        return null;
    }

    private void OnDestroy()
    {
		SceneManager.sceneLoaded -= SceneEnter;
	}
}
