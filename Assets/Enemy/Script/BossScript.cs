using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{

	float m_spped = 2;
	Transform m_player;
	Animator m_animator;

	// Start is called before the first frame update
	void Start()
	{
		m_player = GameObject.FindWithTag("Player").transform;
		m_animator = GetComponent<Animator>();	
	}

    // Update is called once per frame
    void Update()
    {

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
}
