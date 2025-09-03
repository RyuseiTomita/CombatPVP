using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBoss : MonoBehaviour
{
	[SerializeField]
	GameObject[] m_fireEffects;

	public enum FireEffectType
	{
		fireChange
	}

	FireSound m_fireSound;

	void Awake()
	{
		m_fireSound = GetComponent<FireSound>();
	}

	public void FireEffectChange(bool fire)
	{
		if (!fire) m_fireEffects[(int)FireEffectType.fireChange].SetActive(false);

		else m_fireEffects[(int)FireEffectType.fireChange].SetActive(true);
	}
	
	public void ModelChangeSound()
	{
		m_fireSound.Play2D(FireSound.FireType.FireChangeSound);
		Debug.Log("a");
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
