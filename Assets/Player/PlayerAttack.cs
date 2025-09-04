using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	[SerializeField] 
	List<int> m_damageList;

	[SerializeField]
	Collider m_collider;

	private int m_damageIndex;

	public void OnAttackHit(int damage)
	{
		m_collider.enabled = true;
		m_damageIndex = damage;
	}

	public void OnAttackEnd()
	{
		m_collider.enabled = false;
		m_damageIndex = 0;
	}

	private void OnTriggerEnter(Collider other)
	{
		Health health;

		if(other.TryGetComponent(out  health))
		{
			Debug.Log(m_damageIndex);
			health.HitAttack(m_damageList[m_damageIndex]);
		}
	}


	// Update is called once per frame
	void FixedUpdate()
    {
        
    }
}
