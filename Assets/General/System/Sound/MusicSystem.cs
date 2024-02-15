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

    [SerializeField] private List<SceneMusicAsset> musicList;
    public static MusicSystem instance;
    public static AudioSource source;

    /// <summary>
    /// The name of the last scene
    /// </summary>
    public static string lastScene = "";

    private static SceneMusicAsset activeAsset = null;

    void Awake()
    {
        if (instance == null)
        {
			SceneManager.sceneLoaded += SceneEnter;
            source = GetComponent<AudioSource>();
            source.loop = true;
			instance = this;
            lastScene = SceneManager.GetActiveScene().name;

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
        source.volume = 
            SoundVolumeController.generalVolume * SoundVolumeController.musicVolume *
            (activeAsset == null? 1f: activeAsset.individualVolume);
    }

    public void SceneEnter(Scene scene, LoadSceneMode mode)
    {
		if (!source.isPlaying)
		{
			SceneMusicAsset musicAsset = GetMusicFromSceneName(scene.name);
			if (musicAsset.clip != null)
			{
                source.clip = musicAsset.clip;
				source.Play();
				activeAsset = musicAsset;
			}
			else
			{
				Debug.LogError($"Couldn't get clip from scene name: {scene.name}");
			}
		}
        else
        {
			SceneMusicAsset musicAsset = GetMusicFromSceneName(scene.name);
			if (musicAsset.sceneName == lastScene)
			{
                if (musicAsset.replayOnSameScene)
                {
					source.clip = musicAsset.clip;
					source.Play();
					activeAsset = musicAsset;
				}
			}
			else
			{
				source.Stop();
				source.clip = musicAsset.clip;
				source.Play();
				activeAsset = musicAsset;
			}
		}

        lastScene = scene.name;
    }

    public SceneMusicAsset GetMusicFromSceneName(string sceneName)
    {
        foreach(SceneMusicAsset music in musicList)
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
