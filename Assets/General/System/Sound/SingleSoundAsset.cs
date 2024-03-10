using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SingleSound", menuName = "Sound Assets/Single Sound")]
public class SingleSoundAsset : SoundAsset
{
    public AudioClip clip;

    private Coroutine coroutine;
    public MonoBehaviour caller;

	public override void Play(AudioSource playSource)
    {
        Transform cameraTrs = Camera.main.transform;
        if (distanceDependent && Mathf.Abs(cameraTrs.position.x - playSource.transform.position.x) >= 8f)
        {
            return;
        }

		playSource.volume = individualVolume * SoundVolumeController.generalVolume * SoundVolumeController.effectsVolume;
		playSource.PlayOneShot(clip);
    }

    public void PlayRepeat(AudioSource source)
    {
        if (coroutine != null)
        {
            return;
        }
        coroutine = caller.StartCoroutine(PlayRepeatCoroutine(source));
    }
    public void Stop(AudioSource source)
    {
        if (coroutine == null)
        {
            return;
        }
        caller.StopCoroutine(coroutine);
        coroutine = null;
        source.Stop();
	}

    IEnumerator PlayRepeatCoroutine(AudioSource source)
    {
        while(true)
        {
			source.volume = individualVolume * SoundVolumeController.generalVolume * SoundVolumeController.effectsVolume;
			source.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }
    }
}
