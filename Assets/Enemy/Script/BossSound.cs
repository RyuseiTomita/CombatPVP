using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSound : MonoBehaviour
{
	[Serializable]

	public class SoundData
	{
		public Type type;
		public AudioClip sound;
	}

	public enum Type
	{
		ModelChange,
	}

	[SerializeField] List<SoundData> m_sounds;

	public void Play2D(Type type)
	{
		SoundEffect.Play2D(
			m_sounds[(int)type].sound);
	}
}
