using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSound : MonoBehaviour
{
	[Serializable]

	public class FireSoundData
	{
		public FireType fireType;
		public AudioClip fireSound;
	}
   
	public enum FireType
	{
		FireChange,
	}

	[SerializeField] List<FireSoundData> m_fireSounds;

	public void Play2D(FireType fireType)
	{
		SoundEffect.Play2D(
			m_fireSounds[(int)fireType].fireSound);
	}
}
