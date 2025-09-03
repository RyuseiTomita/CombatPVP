using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IceBoss;

public class FireBoss : MonoBehaviour
{

	// 攻撃エフェクト
	[SerializeField]
	GameObject[] m_fireEffects;

	// 攻撃予測範囲エフェクト
	[SerializeField]
	GameObject[] m_predictionRangeEffect;

	FireSound m_fireSound;

	public enum FireEffectType
	{
		fireChange,
		fireChangeAttack,
	}

	public enum PredictionRange
	{
		FireChangeRange,
	}

	void Awake()
	{
		m_fireSound = GetComponent<FireSound>();
	}

	public void FireEffectChange(bool fire)
	{
		if (!fire) m_fireEffects[(int)FireEffectType.fireChange].SetActive(false);

		else m_fireEffects[(int)FireEffectType.fireChange].SetActive(true);
	}
	
	public void ModelChange()
	{
		m_fireSound.Play2D(FireSound.FireType.FireChangeSound);
		m_fireEffects[(int)FireEffectType.fireChangeAttack].SetActive(true);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
