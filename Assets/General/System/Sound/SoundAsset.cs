using UnityEngine;


public class SoundAsset : ScriptableObject
{
	public AudioSource source;
	public float individualVolume = 1f;

	public virtual void Play()
	{
		Debug.LogError("Calling SoundAsset base");
	}
}
