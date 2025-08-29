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

	public void FireEffectChange(bool fire)
	{
		if (!fire) m_fireEffects[(int)FireEffectType.fireChange].SetActive(false);

		else m_fireEffects[(int)FireEffectType.fireChange].SetActive(true);
	}
	
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
