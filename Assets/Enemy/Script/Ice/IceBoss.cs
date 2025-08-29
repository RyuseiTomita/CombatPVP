using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBoss : MonoBehaviour
{

	[SerializeField]
	GameObject[] m_iceEffects;

	public enum IceEffectType
	{
		IceChange
	}

	public enum IceBossAttack
	{

	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void IceEffectChange(bool ice)
	{
		if(!ice) m_iceEffects[(int)IceEffectType.IceChange].SetActive(false);

		else m_iceEffects[(int)IceEffectType.IceChange].SetActive(true);
	}
    
}
