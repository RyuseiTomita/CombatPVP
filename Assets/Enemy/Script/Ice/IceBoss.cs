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

	public enum IceEffectType
	{
		IceChange,
	}

	public enum IceBossAttack
	{
		FrostStorm,
		FrostStormAttack,
	}

	// �\���͈͍U���̃G�t�F�N�g
	public enum PredictionRange
	{
		FrostStormRange,
	}

	// Start is called before the first frame update
	void Start()
	{
		m_animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{

	}

	// �G�t�F�N�g�̐؂�ւ�
	public void IceEffectChange(bool ice)
	{
		if(!ice) m_iceEffects[(int)IceEffectType.IceChange].SetActive(false);

		else m_iceEffects[(int)IceEffectType.IceChange].SetActive(true);
	}

	public  void FrostStormAttack()
	{
		m_animator.SetTrigger("FrostStorm");

	}

	public void FrostStormEffect()
	{
		m_predictionRangeEffect[(int)PredictionRange.FrostStormRange].SetActive(true);
	}



}
