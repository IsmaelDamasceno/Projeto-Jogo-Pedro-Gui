using UnityEngine;


public class SoundAsset : ScriptableObject
{
	public float individualVolume = 1f;
	public bool distanceDependent = true;

	public virtual void Play(AudioSource PlaySource)
	{
		Debug.LogError("Calling SoundAsset base");
	}
}
