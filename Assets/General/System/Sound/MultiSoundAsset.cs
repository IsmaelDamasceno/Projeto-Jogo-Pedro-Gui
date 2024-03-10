using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiSound", menuName = "Sound Assets/Multi Sound")]
public class MultiSoundAsset : SoundAsset
{
    public List<AudioClip> clips;

    private int currentIndex = 0;
    private bool repeating = true;
    private Vector2 delay = Vector2.zero;
    private MonoBehaviour caller;

    public bool random = true;

    private float delayScale = 1f;
    private Coroutine soundCoroutine;

    public void SetDelay(float delay)
    {
        this.delay = new(delay, delay);
    }
    public void SetDelay(Vector2 delay)
    {
        this.delay = delay;
    }
    public void SetRepeating(bool repeating)
    {
        this.repeating = repeating;
    }
    public void SetDelayScale(float scale)
    {
        if (scale == delayScale)
        {
            return;
        }

        delayScale = scale;
        Stop();
    }

	public void Setup(MonoBehaviour caller)
	{
		this.caller = caller;
	}

	public override void Play(AudioSource playSource)
    {
		if (soundCoroutine == null)
        {
			soundCoroutine = caller.StartCoroutine(StartPlaying(playSource));
		}
    }
    public void Stop()
    {
        if (soundCoroutine == null)
        {
            return;
        }

		caller.StopCoroutine(soundCoroutine);
        soundCoroutine = null;
	}

    IEnumerator StartPlaying(AudioSource playSource)
    {
        bool playOnce = !repeating;
        bool played = false;
        repeating = true;
        while(repeating)
        {
            if (playOnce && played)
            {
                repeating = false;
                continue;
            }

            AudioClip clip;
            if (random)
            {
				clip = clips[Random.Range(0, clips.Count)];
            }
            else
            {
				clip = clips[currentIndex];
                currentIndex++;
                currentIndex %= clips.Count;
            }
			playSource.volume = individualVolume * SoundVolumeController.generalVolume * SoundVolumeController.effectsVolume;
			playSource.PlayOneShot(clip);

			played = true;
            yield return new WaitForSeconds(Random.Range(delay.x, delay.y) * delayScale);
        }
        soundCoroutine = null;
    }
}
