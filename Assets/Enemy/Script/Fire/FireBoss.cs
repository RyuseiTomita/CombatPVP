using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IceBoss;

public class FireBoss : MonoBehaviour
{

	// FireBreath
	int m_fireBreathIndex;
	int m_fireBreathBomb;
	float m_fireBreathCoolTime;
	bool m_isFireBreath = false;
	GameObject[] m_fireBreathAttack = new GameObject[10];

	// �U���G�t�F�N�g
	[SerializeField]
	GameObject[] m_fireEffects;

	// �U���\���͈̓G�t�F�N�g
	[SerializeField]
	GameObject[] m_predictionRangeEffect;

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
	}

	public void FireEffectChange(bool fire)
	{
		if (!fire) m_fireEffects[(int)FireEffectType.FireChangeEffect].SetActive(false);

		else m_fireEffects[(int)FireEffectType.FireChangeEffect].SetActive(true);
	}
	
	public void ModelChange()
	{
		m_fireSound.Play2D(FireSound.FireType.FireChangeSound);
		m_fireEffects[(int)FireEffectType.FireChangeAttackEffect].SetActive(true);
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
			// ���ݍU������
			m_isFireBreath = true;

			// �J�E���g�_�E��
			m_fireBreathCoolTime -= Time.deltaTime;

			if (m_fireBreathCoolTime <= 0)
			{
				if (m_fireBreathIndex == 0)
				{
					m_fireBreathBomb = 0;
				}

				// �v���C���[�̈ʒu�ɍU���\���͈͂𐶐�
				GameObject m_fireEffectIndex= Instantiate(
					m_predictionRangeEffect[(int)PredictionRange.FireBreathRange],
					m_player.transform.position, Quaternion.Euler(90, 0, 0));

				m_fireBreathAttack[m_fireBreathIndex] = m_fireEffectIndex;

				// �J�E���g���グ��
				m_fireBreathIndex++;

				// �N�[���^�C�����グ��
				m_fireBreathCoolTime = 1f;

				// �T�E���h��炷
				m_fireSound.Play2D(FireSound.FireType.FireBreathRangeSound);

				StartCoroutine(FireBreathAttack());
			}

			return;
		}

		m_fireBreathIndex = 0;
		m_fireBreathCoolTime = 1;
		m_isFireBreath = false;
		Debug.Log("����");

	}

	IEnumerator FireBreathAttack()
	{
		yield return new WaitForSeconds(2);
		Instantiate(m_fireEffects[(int)FireEffectType.FireBreathAttackEffect],
			m_fireBreathAttack[m_fireBreathBomb].transform.position + Vector3.down,
			Quaternion.identity);

		// �T�E���h��炷
		m_fireSound.Play2D(FireSound.FireType.FireBreathAttackSound);

		m_fireBreathBomb++;
	}

	public void FireShotEffect()
	{
		m_predictionRangeEffect[(int)PredictionRange.FireShotRange].SetActive(true);

	}

	public void FireEffectEnd()
	{
		m_predictionRangeEffect[(int)PredictionRange.FireShotRange].SetActive(false);
	}

	public void FireShotAnimationSound()
	{
		m_fireSound.Play2D(FireSound.FireType.FireShotCharge);
	}

	IEnumerator FireShotAttack()
	{
		yield return new WaitForSeconds(2);
		m_fireEffects[(int)FireEffectType.FireShotAttackEffect].SetActive(true);
	}

	// Update is called once per frame
	void FixedUpdate()
    {
        if(m_isFireBreath)
		{
			FireBreathEffect();
		}
    }
}
