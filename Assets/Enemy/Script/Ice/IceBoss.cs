using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBoss : MonoBehaviour
{
	Animator m_animator;

	float m_frostStormAttackTime = 5;
	bool m_dorwEffectfrostStorm;
	bool m_dorwEffectChange;

	// 攻撃エフェクト
	[SerializeField]
	GameObject[] m_iceEffects;

	// 攻撃予測範囲エフェクト
	[SerializeField]
	GameObject[] m_predictionRangeEffect;

	// サウンド
	IceSound m_iceSound;

	// エフェクト
	public enum IceEffectType
	{
		IceChange,
		IceChangeAttack,
		FrostStormAttackEffect,
		FrostBurstAttackEffect,
	}

	// 予測範囲攻撃のエフェクト
	public enum PredictionRange
	{
		IceChangeRange,
		FrostStormRange,
		FrostBurstRange,
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
		if (m_dorwEffectfrostStorm)
		{
			m_frostStormAttackTime -= Time.deltaTime;

			if(m_frostStormAttackTime <= 0)
			{
				m_iceEffects[(int)IceEffectType.FrostStormAttackEffect].SetActive(false);
				m_dorwEffectfrostStorm = false;
				m_frostStormAttackTime = 5f;
			}
		}
		

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
			m_predictionRangeEffect[(int)PredictionRange.IceChangeRange].SetActive(true);
		}
	}

	IEnumerator ChangeAttackEffect()
	{
		yield return new WaitForSeconds(2);
		m_iceEffects[(int)IceEffectType.IceChangeAttack].SetActive(true);
	}
		

	public void ModelChange()
	{
		m_iceSound.Play2D(IceSound.IceType.IceChangeSound);
		m_predictionRangeEffect[(int)PredictionRange.IceChangeRange].SetActive(false);
		m_iceEffects[(int)IceEffectType.IceChangeAttack].SetActive(true);
		StartCoroutine(ChangeAttackEffect());
	}


	public void FrostStormAttack()
	{
		m_iceEffects[(int)IceEffectType.FrostStormAttackEffect].SetActive(true);
		m_predictionRangeEffect[(int)PredictionRange.FrostStormRange].SetActive(false);
		m_iceSound.Play2D(IceSound.IceType.FrostStormAttackSound);

		m_dorwEffectfrostStorm = true;
	}

	public void FrostStormSound()
	{
		m_iceSound.Play2D(IceSound.IceType.FrostStormSound);
	}


	public void FrostStormEffect()
	{
		m_predictionRangeEffect[(int)PredictionRange.FrostStormRange].SetActive(true);
	}


	public void FrostBurstEffect(bool burst)
	{
		m_predictionRangeEffect[(int)PredictionRange.FrostBurstRange].SetActive(burst);

		if (!burst) return;
		m_iceSound.Play2D(IceSound.IceType.FrostBurstChargeSound);
	}

	public void FrostBurstAttack()
	{
		m_iceEffects[(int)IceEffectType.FrostBurstAttackEffect].SetActive(true);
		m_iceSound.Play2D(IceSound.IceType.FrostBurstAttackSound);
	}

	public void FrostBurstAttackEnd()
	{
		m_iceEffects[(int)IceEffectType.FrostBurstAttackEffect].SetActive(false);
		Debug.Log("AAA");
	}



}
