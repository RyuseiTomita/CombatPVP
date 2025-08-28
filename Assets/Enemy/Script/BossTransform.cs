using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTransform : MonoBehaviour
{

	[SerializeField] BossScript parent;
	
    // Start is called before the first frame update
    void Start()
    {
		parent.GetComponent<BossScript>();
	}

	public void Transform()
	{
		parent.Transform();
	}

	public void TransformComplete()
	{
		parent.TransformComplete();
	}
}
