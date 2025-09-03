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

	// •Ïg
	public void Transform()
	{
		parent.Transform();
	}

	//•ÏgŒã
	public void TransformComplete()
	{
		parent.TransformComplete();
	}

	// “G‚ª“®‚­‚©‚Ç‚¤‚©
	public void EnemyMove()
	{
		parent.EnemyMove();
	}
}
