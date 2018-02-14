using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PowerUpData : ScriptableObject
{

	public string PowerType;
	public int PowerLevel;
	public AudioClip ThisAudioClip;
	public ParticleSystem VFX;

}
