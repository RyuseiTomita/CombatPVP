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
		// 歩くのを止める
		m_isStop = true;

		// 変身アニメーション
		m_animator.SetTrigger("Transform");

		// サウンドを鳴らす
		m_sounds.Play2D(BossSound.Type.ModelChange);

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

	// 攻撃技(氷)
	private void FrostStorm()
	{
		// 歩くのを止める
		m_isStop = true;

		// エフェクトを生成
		m_model[(int)ModelType.IceModel].GetComponent<IceBoss>().FrostStormEffect();

		// アニメーションをする
		m_animator.SetTrigger("FrostStorm");

		// 次の攻撃に切り替える
		m_attackPattern = (int)IceAttackPattern.FrostBurst;

		
		StartCoroutine(FrostStormAttack());
	}

	IEnumerator FrostStormAttack()
	{
		// 3秒後に発生
		yield return new WaitForSeconds(3);

		// エリアを生成
		m_model[(int)ModelType.IceModel].GetComponent<IceBoss>().FrostStormAttack();
	}

	//攻撃技(氷)
	private void FrostBurst()
	{
		// 歩くのを止める
		m_isStop = true;

		// エフェクトを生成
		m_model[(int)ModelType.IceModel].GetComponent<IceBoss>().FrostBurstEffect(true);

		// アニメーションをする 
		m_animator.SetTrigger("FrostBurstCharge");

		// 次の攻撃に切り替える
		m_attackPattern = (int)IceAttackPattern.FrostStorm;
		
		
		StartCoroutine(FrostBurstAttack());
	}

	IEnumerator FrostBurstAttack()
	{
		yield return new WaitForSeconds(3);

		m_model[(int)ModelType.IceModel].GetComponent<IceBoss>().FrostBurstEffect(false);
		m_animator.SetTrigger("FrostBurstAttack");
	}

	// 攻撃技(火)
	private void FireBreath()
	{
		m_animator.SetTrigger("");
	}

	// 攻撃技(火)
	private void FireShot()
	{

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

			// サウンドと攻撃範囲を生成
			m_model[(int)ModelType.FireModel].GetComponent<FireBoss>().ModelChange();

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

			// サウンドと攻撃範囲を生成
			m_model[(int)ModelType.IceModel].GetComponent<IceBoss>().ModelChange();

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

		// 次の変身時間まで
		m_modelTime = 15f;

		// 攻撃するまで
		m_attackTime = 5f;
	}

	public void EnemyMove()
	{
		// 動けるようにする
		m_isStop = false;

		// 攻撃するまで
		m_attackTime = 5f;
	}
}
