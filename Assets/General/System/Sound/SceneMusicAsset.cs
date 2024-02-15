using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneMusicAsset", menuName = "Sound Assets/Scene Music")]
public class SceneMusicAsset : ScriptableObject
{
	public AudioClip clip;
	public bool replayOnSameScene;
	public string sceneName;
	public float individualVolume = 1f;
}
