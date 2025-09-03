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

			m_sounds.Play2D(BossSound.Type.ModelChange);
			m_isStop = true;
			modelTime = 15;

			// もし現在アイスモデルならアイスエフェクトを生成
			if (m_modelIndex == (int)ModelType.IceModel)
			{
				m_model[(int)ModelType.IceModel].GetComponent<IceBoss>().IceEffectChange(true);
			}
			// ファイヤーモデルならファイヤーエフェクトを生成
			else
			{
				m_model[(int)ModelType.FireModel].GetComponent<FireBoss>().FireEffectChange(true);
			}

		}
	}

	// 攻撃技
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

	// 属性チェンジ
	public void Transform()
	{
		// アイス状態だったら
		if(m_modelIndex == (int)ModelType.IceModel)
		{
			// 見た目を変える
			m_model[(int)ModelType.IceModel].SetActive(false);
			m_model[(int)ModelType.FireModel].SetActive(true);

			// ファイヤーモデルを取り込む
			m_animator = m_model[(int)ModelType.FireModel].GetComponent<Animator>();

			// サウンドを鳴らす
			m_model[(int)ModelType.FireModel].GetComponent<FireBoss>().ModelChangeSound();

			// ファイヤーモードに切り替える
			m_modelIndex = (int)ModelType.FireModel;
		}
		// ファイヤー状態だったら
		else
		{
			// 見た目を変える
			m_model[(int)ModelType.IceModel].SetActive(true);
			m_model[(int)ModelType.FireModel].SetActive(false);
			
			// アイスモデルを取り込む
			m_animator = m_model[(int)ModelType.IceModel].GetComponent<Animator>();

			// サウンドを鳴らす
			m_model[(int)ModelType.IceModel].GetComponent<IceBoss>().ModelChangeSound();

			// アイスモードに切り替える
			m_modelIndex = (int)ModelType.IceModel;
		}
		
	}

	// 完全に切り替わったら
	public void TransformComplete()
	{
		// 動けるようにする
		m_isStop = false;

		// エフェクトを消す
		m_model[(int)ModelType.IceModel].GetComponent<IceBoss>().IceEffectChange(false);
	}

	public void EnemyMove()
	{
		// 動けるようにする
		m_isStop = false;
	}
}
