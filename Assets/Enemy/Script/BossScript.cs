using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossScript : MonoBehaviour
{
	float modelTime = 5f;

	float m_spped = 2;
	bool m_transform;
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

	// Start is called before the first frame update
	void Start()
	{
		m_model[(int)ModelType.IceModel].SetActive(true);
		m_model[(int)ModelType.FireModel].SetActive(false);
		m_player = GameObject.FindWithTag("Player").transform;
		m_animator = m_model[(int)ModelType.IceModel].GetComponent<Animator>();
		m_modelIndex = (int)ModelType.IceModel;
		m_transform = true;
		m_modelIndex = 0;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		modelTime -= Time.deltaTime;

		if(modelTime <= 0)
		{
			m_animator.SetTrigger("Transform");
			m_transform = true;
			modelTime = 15;
		}

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

	public void Transform()
	{
		if(m_modelIndex == (int)ModelType.IceModel)
		{
			m_model[(int)ModelType.IceModel].SetActive(false);
			m_model[(int)ModelType.FireModel].SetActive(true);

			m_animator = m_model[(int)ModelType.FireModel].GetComponent<Animator>();
			m_modelIndex = (int)ModelType.FireModel;
		}
		else
		{
			m_model[(int)ModelType.IceModel].SetActive(true);
			m_model[(int)ModelType.FireModel].SetActive(false);
			m_animator = m_model[(int)ModelType.IceModel].GetComponent<Animator>();
			m_modelIndex = (int)ModelType.IceModel;
		}
		
	}

	public void TransformComplete()
	{
		m_transform = false;
		Debug.Log("入った");
	}
}
