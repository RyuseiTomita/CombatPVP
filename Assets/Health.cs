using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

	[SerializeField]
	Health m_target;

	[SerializeField]
	int m_health;

	[SerializeField]
	Slider m_slider;

    void Start()
    {
		m_slider.value = m_health;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		//m_slider.value = m_health;
	}

	public void HitAttack(int damage)
	{
		m_health -= damage; 
	}
}
