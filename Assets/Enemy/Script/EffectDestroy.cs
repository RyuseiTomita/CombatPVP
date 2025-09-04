using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroy : MonoBehaviour
{
	[SerializeField]
	float m_effectDestroyTime;

    // Start is called before the first frame update
    void Start()
    {

	}

    // Update is called once per frame
    void FixedUpdate()
    {
		m_effectDestroyTime -= Time.deltaTime;

		if (m_effectDestroyTime <= 0)
		{
			Destroy(gameObject);
		}
    }
}
