using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

public class BossScript : MonoBehaviour
{
	float m_modelTime = 15f;
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
		FireBreath,
		FireShot,
	}


	 BossSound m_sounds;

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
		m_attackPattern = (int)IceAttackPattern.FrostStorm;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		if (m_isStop) return;

		m_modelTime -= Time.deltaTime;

		if (m_modelTime <= 0)
		{
			ModelTime();

			return;
		}


		m_attackTime -= Time.deltaTime;

		if (m_attackTime <= 0)
		{
			if (m_modelIndex == (int)ModelType.IceModel)
			{
				switch (m_attackPattern)
				{
					case (int)IceAttackPattern.FrostStorm:
						FrostStorm();
						break;

					case (int)IceAttackPattern.FrostBurst:
						FrostBurst();
						break;
				}
			}
			else
			{
				switch(m_attackPattern)
				{
					case (int)FireAttackPattern.FireBreath:
						FireBreath();
						break;

					case (int)FireAttackPattern.FireShot:
						FireShot();
						break;
				}
			}
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
		// �����̂��~�߂�
		m_isStop = true;

		// �ϐg�A�j���[�V����
		m_animator.SetTrigger("Transform");

		// �T�E���h��炷
		m_sounds.Play2D(BossSound.Type.ModelChange);

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

	// �U���Z(�X)
	private void FrostStorm()
	{
		// �����̂��~�߂�
		m_isStop = true;

		// �G�t�F�N�g�𐶐�
		m_model[(int)ModelType.IceModel].GetComponent<IceBoss>().FrostStormEffect();

		// �A�j���[�V����������
		m_animator.SetTrigger("FrostStorm");

		// ���̍U���ɐ؂�ւ���
		m_attackPattern = (int)IceAttackPattern.FrostBurst;

		
		StartCoroutine(FrostStormAttack());
	}

	IEnumerator FrostStormAttack()
	{
		// 3�b��ɔ���
		yield return new WaitForSeconds(3);

		// �G���A�𐶐�
		m_model[(int)ModelType.IceModel].GetComponent<IceBoss>().FrostStormAttack();
	}

	//�U���Z(�X)
	private void FrostBurst()
	{
		// �����̂��~�߂�
		m_isStop = true;

		// �G�t�F�N�g�𐶐�
		m_model[(int)ModelType.IceModel].GetComponent<IceBoss>().FrostBurstEffect(true);

		// �A�j���[�V���������� 
		m_animator.SetTrigger("FrostBurstCharge");

		// ���̍U���ɐ؂�ւ���
		m_attackPattern = (int)IceAttackPattern.FrostStorm;
		
		
		StartCoroutine(FrostBurstAttack());
	}

	IEnumerator FrostBurstAttack()
	{
		yield return new WaitForSeconds(3);

		m_model[(int)ModelType.IceModel].GetComponent<IceBoss>().FrostBurstEffect(false);
		m_animator.SetTrigger("FrostBurstAttack");
	}

	// �U���Z(��)
	private void FireBreath()
	{
		m_animator.SetTrigger("");
	}

	// �U���Z(��)
	private void FireShot()
	{

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

			// �T�E���h�ƍU���͈͂𐶐�
			m_model[(int)ModelType.FireModel].GetComponent<FireBoss>().ModelChange();

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

			// �T�E���h�ƍU���͈͂𐶐�
			m_model[(int)ModelType.IceModel].GetComponent<IceBoss>().ModelChange();

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

		// ���̕ϐg���Ԃ܂�
		m_modelTime = 15f;

		// �U������܂�
		m_attackTime = 5f;
	}

	public void EnemyMove()
	{
		// ������悤�ɂ���
		m_isStop = false;

		// �U������܂�
		m_attackTime = 5f;
	}
}
