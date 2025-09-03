using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSound : MonoBehaviour
{
	[Serializable]

	public class IceSoundData
	{
		public IceType iceType;
		public AudioClip iceSound;
	}

	public enum IceType
	{
		IceChangeSound,
		FrostStormSound,
		FrostStormAttackSound,
	}

	[SerializeField] List<IceSoundData> m_iceSounds;

	public void Play2D(IceType iceType)
	{
		SoundEffect.Play2D(
			m_iceSounds[(int)iceType].iceSound);
	}
}
