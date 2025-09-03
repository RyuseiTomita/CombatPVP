using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBoss : MonoBehaviour
{
	Animator m_animator;

	[SerializeField]
	GameObject[] m_iceEffects;

	[SerializeField]
	GameObject[] m_predictionRangeEffect;

	IceSound m_iceSound;

	public enum IceEffectType
	{
		IceChange,
		FrostStormAttackEffect,
	}

	public enum IceBossAttack
	{
		FrostStorm,
		FrostStormAttack,
	}

	// 予測範囲攻撃のエフェクト
	public enum PredictionRange
	{
		FrostStormRange,
	}

	// Start is called before the first frame update
	void Start()
	{
		m_animator = GetComponent<Animator>();
		m_iceSound = GetComponent<IceSound>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{

	}

	// エフェクトの切り替え
	public void IceEffectChange(bool ice)
	{
		if (!ice)
		{
			m_iceEffects[(int)IceEffectType.IceChange].SetActive(false);
		}
		else
		{
			m_iceEffects[(int)IceEffectType.IceChange].SetActive(true);
		}
	}

	public void ModelChangeSound()
	{
		m_iceSound.Play2D(IceSound.IceType.IceChangeSound);
	}


	public  void FrostStormAttack()
	{
		m_iceEffects[(int)IceEffectType.FrostStormAttackEffect].SetActive(true);
		m_predictionRangeEffect[(int)PredictionRange.FrostStormRange].SetActive(false);

		m_iceSound.Play2D(IceSound.IceType.FrostStormAttackSound);
	}

	public void FrostStormSound()
	{
		m_iceSound.Play2D(IceSound.IceType.FrostStormSound);
	}


	public void FrostStormEffect()
	{
		m_predictionRangeEffect[(int)PredictionRange.FrostStormRange].SetActive(true);
	}



}
