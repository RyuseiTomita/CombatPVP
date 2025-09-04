using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static IceBoss;

public class FireBoss : MonoBehaviour
{

	// FireBreath
	int m_fireBreathIndex;
	int m_fireBreathBomb;
	float m_fireBreathCoolTime;
	bool m_isFireBreath = false;
	GameObject[] m_fireBreathAttack = new GameObject[10];

	bool m_isFireShot;
	float m_fireShotTime;

	// 攻撃エフェクト
	[SerializeField]
	GameObject[] m_fireEffects;

	// 攻撃予測範囲エフェクト
	[SerializeField]
	GameObject[] m_predictionRangeEffect;

	[SerializeField]
	BossScript m_bossScript;

	Transform m_player;
	FireSound m_fireSound;

	public enum FireEffectType
	{
		FireChangeEffect,
		FireChangeAttackEffect,
		FireBreathAttackEffect,
		FireShotAttackEffect,
	}

	public enum PredictionRange
	{
		FireChangeRange,
		FireBreathRange,
		FireShotRange,
	}

	void Awake()
	{
		m_fireSound = GetComponent<FireSound>();
		m_player = GameObject.FindWithTag("Player").transform;
	}

	private void Start()
	{
		m_fireBreathIndex = 0;
		m_fireBreathCoolTime = 1f;

		m_fireShotTime = 1f;
	}

	public void FireEffectChange(bool fire)
	{
		if(fire)
		{
			m_fireEffects[(int)FireEffectType.FireChangeEffect].SetActive(true);
			m_predictionRangeEffect[(int)PredictionRange.FireChangeRange].SetActive(true);
		}
		else
		{
			m_fireEffects[(int)FireEffectType.FireChangeEffect].SetActive(false);
			m_predictionRangeEffect[(int)PredictionRange.FireChangeRange].SetActive(false);
		}
	}

	public void ModelChange()
	{
		m_fireSound.Play2D(FireSound.FireType.FireChangeSound);
		m_fireEffects[(int)FireEffectType.FireChangeAttackEffect].SetActive(true);

		StartCoroutine(FireChangeAttackEnd());
	}

	IEnumerator FireChangeAttackEnd()
	{
		yield return new WaitForSeconds(2);
		m_fireEffects[(int)FireEffectType.FireChangeAttackEffect].SetActive(false);
	}

	public void FireBreathSound()
	{
		m_fireSound.Play2D(FireSound.FireType.FireBreathAnimationSound);
	}

	public void FireBreathEffect()
	{
		Debug.Log("af");

		if(m_fireBreathIndex <= 4)
		{
			// 現在攻撃中か
			m_isFireBreath = true;

			// カウントダウン
			m_fireBreathCoolTime -= Time.deltaTime;

			if (m_fireBreathCoolTime <= 0)
			{
				if (m_fireBreathIndex == 0)
				{
					m_fireBreathBomb = 0;
				}

				// プレイヤーの位置に攻撃予測範囲を生成
				GameObject m_fireEffectIndex= Instantiate(
					m_predictionRangeEffect[(int)PredictionRange.FireBreathRange],
					m_player.transform.position, Quaternion.Euler(90, 0, 0));

				m_fireBreathAttack[m_fireBreathIndex] = m_fireEffectIndex;

				// カウントを上げる
				m_fireBreathIndex++;

				// クールタイムを上げる
				m_fireBreathCoolTime = 1f;

				// サウンドを鳴らす
				m_fireSound.Play2D(FireSound.FireType.FireBreathRangeSound);

				StartCoroutine(FireBreathAttack());
			}

			return;
		}

		m_fireBreathIndex = 0;
		m_fireBreathCoolTime = 1;
		m_isFireBreath = false;
		Debug.Log("完了");

	}

	IEnumerator FireBreathAttack()
	{
		yield return new WaitForSeconds(2);
		Instantiate(m_fireEffects[(int)FireEffectType.FireBreathAttackEffect],
			m_fireBreathAttack[m_fireBreathBomb].transform.position + Vector3.down,
			Quaternion.identity);

		// サウンドを鳴らす
		m_fireSound.Play2D(FireSound.FireType.FireBreathAttackSound);

		m_fireBreathBomb++;
	}

	public void FireShotEffect()
	{
		m_predictionRangeEffect[(int)PredictionRange.FireShotRange].SetActive(true);

	}

	public void FireEffectEnd()
	{
		
	}

	public void FireShotAnimationSound()
	{
		m_fireSound.Play2D(FireSound.FireType.FireShotChargeSound);
		StartCoroutine(FireShotAttack());
	}

	IEnumerator FireShotAttack()
	{
		yield return new WaitForSeconds(1.5f);
		m_fireEffects[(int)FireEffectType.FireShotAttackEffect].SetActive(true);
		m_predictionRangeEffect[(int)PredictionRange.FireShotRange].SetActive(false);
		m_isFireShot = true;
		m_fireSound.Play2D(FireSound.FireType.FireShotAttackSound);
	}

	// Update is called once per frame
	void FixedUpdate()
    {
        if(m_isFireBreath)
		{
			FireBreathEffect();
		}

		if(m_isFireShot)
		{
			m_fireShotTime -= Time.deltaTime;
			
			if(m_fireShotTime <= 0)
			{
				m_fireEffects[(int)FireEffectType.FireShotAttackEffect].SetActive(false);
				m_isFireShot = false;
				m_fireShotTime = 1f;
				m_bossScript.EnemyMove();
			}
		}
    }
}
