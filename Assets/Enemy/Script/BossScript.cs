using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

public class BossScript : MonoBehaviour
{
	float modelTime = 35f;
	float m_attackTime = 5f;

	float m_spped = 2;
	bool m_transform;
	bool m_onAttack;
	Transform m_player;
	Animator m_animator;

	int m_modelIndex;

	[SerializeField]
	GameObject[] m_model;

	enum ModelType
	{
		IceModel,
		FireModel
	}

	private BossSound m_sounds;
	private FireSound m_fireSound;
	private IceSound m_iceSound;
	private IceBoss m_iceBoss;
	private FireBoss m_fireBoss;

	// Start is called before the first frame update
	void Start()
	{
		m_model[(int)ModelType.IceModel].SetActive(true);
		m_model[(int)ModelType.FireModel].SetActive(false);

		m_player = GameObject.FindWithTag("Player").transform;

		m_animator = m_model[(int)ModelType.IceModel].GetComponent<Animator>();

		m_sounds = GetComponent<BossSound>();
		m_fireSound = GetComponent<FireSound>();
		m_iceSound = GetComponent<IceSound>();
		m_iceBoss = GetComponent<IceBoss>();
		m_fireBoss = GetComponent<FireBoss>();
		m_modelIndex = (int)ModelType.IceModel;
		m_transform = true;
		m_modelIndex = 0;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		ModelTime();
		BossAttackTime();
		if (m_transform) return;

		// プレイヤーを向く
        Vector3 forward = m_player.position - transform.position;
		forward.Scale(new Vector3(1, 0, 1));

		transform.rotation = Quaternion.Lerp(
			transform.rotation,
			Quaternion.LookRotation(forward.normalized.normalized),
			0.2f);

		// プレイヤーに向けて移動
		bool isMove = false;
		if((m_player.position - transform.position).magnitude > 3)
		{
			transform.position += transform.forward * m_spped * Time.deltaTime;
			isMove = true;
		}

		m_animator.SetBool("Move", isMove);
    }

	// 属性切り替え時間
	private void ModelTime()
	{
		modelTime -= Time.deltaTime;

		if (modelTime <= 0)
		{
			m_animator.SetTrigger("Transform");
			m_sounds.Play2D(BossSound.Type.ModeChange);
			m_transform = true;
			modelTime = 15;

			if (m_modelIndex == (int)ModelType.IceModel)
			{
				m_iceBoss.IceEffectChange(true);
			}
			else
			{
				m_fireBoss.FireEffectChange(true);
			}

		}
	}

	// 攻撃技
	private void BossAttackTime()
	{
		bool isAttack = false;
		m_attackTime -= Time.deltaTime;

		if (m_attackTime <= 0　&& !isAttack)
		{
			isAttack = true;
			m_transform = true;
			m_model[(int)ModelType.IceModel].GetComponent<IceBoss>().FrostStormAttack();
		}
	}

	// 属性チェンジ
	public void Transform()
	{
		// アイス状態だったら
		if(m_modelIndex == (int)ModelType.IceModel)
		{
			m_model[(int)ModelType.IceModel].SetActive(false);
			m_model[(int)ModelType.FireModel].SetActive(true);
			m_fireSound.Play2D(FireSound.FireType.FireChange);
			m_animator = m_model[(int)ModelType.FireModel].GetComponent<Animator>();
			m_modelIndex = (int)ModelType.FireModel;
		}
		// ファイヤー状態だったら
		else
		{
			m_model[(int)ModelType.IceModel].SetActive(true);
			m_model[(int)ModelType.FireModel].SetActive(false);
			m_iceSound.Play2D(IceSound.IceType.IceChange);
			m_animator = m_model[(int)ModelType.IceModel].GetComponent<Animator>();
			m_modelIndex = (int)ModelType.IceModel;
		}
		
	}

	// 完全に切り替わったら
	public void TransformComplete()
	{
		m_transform = false;
		m_model[(int)ModelType.IceModel].GetComponent<IceBoss>().IceEffectChange(false);
		m_fireBoss.FireEffectChange(false);
		Debug.Log("入った");
	}
}
