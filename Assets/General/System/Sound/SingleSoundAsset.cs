using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SingleSound", menuName = "Sound Assets/Single Sound")]
public class SingleSoundAsset : SoundAsset
{
    public AudioClip clip;

    private Coroutine coroutine;
    public MonoBehaviour caller;

	public void Setup(AudioSource source)
	{
		this.source = source;
	}

	public override void Play()
    {
        if (source == null)
        {
            Debug.LogError("Audio source not properly setup for sound asset");
        }

		source.volume = individualVolume * SoundVolumeController.generalVolume * SoundVolumeController.effectsVolume;
		source.PlayOneShot(clip);
    }

    public void PlayRepeat()
    {
        if (coroutine != null)
        {
            return;
        }
        coroutine = caller.StartCoroutine(PlayRepeatCoroutine());
    }
    public void Stop()
    {
        if (coroutine == null)
        {
            return;
        }
        caller.StopCoroutine(coroutine);
        coroutine = null;
        source.Stop();
	}

    IEnumerator PlayRepeatCoroutine()
    {
        while(true)
        {
			source.volume = individualVolume * SoundVolumeController.generalVolume * SoundVolumeController.effectsVolume;
			source.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }
    }
}
