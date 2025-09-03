using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

public class BossScript : MonoBehaviour
{
	float modelTime = 100f;
	float m_attackTime = 5f;

	float m_spped = 2;
	bool m_isStop;
	Transform m_player;
	Animator m_animator;

	int m_modelIndex;
	int m_attackPattern;

	[SerializeField]
	GameObject[] m_model;

	enum ModelType
	{
		IceModel,
		FireModel
	}

	enum IceAttackPattern
	{
		FrostStorm,
		FrostBurst,
	}

	enum FireAttackPattern
	{

	}


	private BossSound m_sounds;

	// Start is called before the first frame update
	void Start()
	{
		m_model[(int)ModelType.IceModel].SetActive(true);
		m_model[(int)ModelType.FireModel].SetActive(false);

		m_player = GameObject.FindWithTag("Player").transform;

		m_animator = m_model[(int)ModelType.IceModel].GetComponent<Animator>();

		m_sounds = GetComponent<BossSound>();
		m_modelIndex = (int)ModelType.IceModel;
		m_isStop = true;
		m_modelIndex = 0;
		m_attackPattern = 0;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		if (m_isStop) return;

		ModelTime();

		switch (m_attackPattern)
		{
			case (int)IceAttackPattern.FrostStorm:
				FrostStorm();
				break;

			case (int)IceAttackPattern.FrostBurst:
				FrostBurst();
				break;
		}

		// �v���C���[������
        Vector3 forward = m_player.position - transform.position;
		forward.Scale(new Vector3(1, 0, 1));

		transform.rotation = Quaternion.Lerp(
			transform.rotation,
			Quaternion.LookRotation(forward.normalized.normalized),
			0.2f);

		// �v���C���[�Ɍ����Ĉړ�
		bool isMove = false;
		if((m_player.position - transform.position).magnitude > 3)
		{
			transform.position += transform.forward * m_spped * Time.deltaTime;
			isMove = true;
		}

		m_animator.SetBool("Move", isMove);
    }

	// �����؂�ւ�����
	private void ModelTime()
	{
		modelTime -= Time.deltaTime;

		if (modelTime <= 0)
		{
			m_animator.SetTrigger("Transform");

			m_sounds.Play2D(BossSound.Type.ModelChange);
			m_isStop = true;
			modelTime = 15;

			// �������݃A�C�X���f���Ȃ�A�C�X�G�t�F�N�g�𐶐�
			if (m_modelIndex == (int)ModelType.IceModel)
			{
				m_model[(int)ModelType.IceModel].GetComponent<IceBoss>().IceEffectChange(true);
			}
			// �t�@�C���[���f���Ȃ�t�@�C���[�G�t�F�N�g�𐶐�
			else
			{
				m_model[(int)ModelType.FireModel].GetComponent<FireBoss>().FireEffectChange(true);
			}

		}
	}

	// �U���Z
	private void FrostStorm()
	{
		m_attackTime -= Time.deltaTime;

		if (m_attackTime <= 0)
		{
			m_isStop = true;
			m_model[(int)ModelType.IceModel].GetComponent<IceBoss>().FrostStormEffect();
			m_animator.SetTrigger("FrostStorm");

			StartCoroutine(FrostStormAttack());
			m_attackTime = 15f;
		}
	}

	IEnumerator FrostStormAttack()
	{
		yield return new WaitForSeconds(3);
		m_model[(int)ModelType.IceModel].GetComponent<IceBoss>().FrostStormAttack();
	}

	private void FrostBurst()
	{
		m_attackTime -= Time.deltaTime;

		if (m_attackTime <= 0)
		{
			m_isStop = true;
			m_animator.SetTrigger("FrostBurst");
		}
	}

	// �����`�F���W
	public void Transform()
	{
		// �A�C�X��Ԃ�������
		if(m_modelIndex == (int)ModelType.IceModel)
		{
			// �����ڂ�ς���
			m_model[(int)ModelType.IceModel].SetActive(false);
			m_model[(int)ModelType.FireModel].SetActive(true);

			// �t�@�C���[���f������荞��
			m_animator = m_model[(int)ModelType.FireModel].GetComponent<Animator>();

			// �T�E���h��炷
			m_model[(int)ModelType.FireModel].GetComponent<FireBoss>().ModelChangeSound();

			// �t�@�C���[���[�h�ɐ؂�ւ���
			m_modelIndex = (int)ModelType.FireModel;
		}
		// �t�@�C���[��Ԃ�������
		else
		{
			// �����ڂ�ς���
			m_model[(int)ModelType.IceModel].SetActive(true);
			m_model[(int)ModelType.FireModel].SetActive(false);
			
			// �A�C�X���f������荞��
			m_animator = m_model[(int)ModelType.IceModel].GetComponent<Animator>();

			// �T�E���h��炷
			m_model[(int)ModelType.IceModel].GetComponent<IceBoss>().ModelChangeSound();

			// �A�C�X���[�h�ɐ؂�ւ���
			m_modelIndex = (int)ModelType.IceModel;
		}
		
	}

	// ���S�ɐ؂�ւ������
	public void TransformComplete()
	{
		// ������悤�ɂ���
		m_isStop = false;

		// �G�t�F�N�g������
		m_model[(int)ModelType.IceModel].GetComponent<IceBoss>().IceEffectChange(false);
	}

	public void EnemyMove()
	{
		// ������悤�ɂ���
		m_isStop = false;
	}
}
