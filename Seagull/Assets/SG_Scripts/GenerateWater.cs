using UnityEngine;
using System.Collections;

public class GenerateWater : MonoBehaviour {

	[SerializeField]
	GameObject water;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 10; i ++)
		{
			for (int j = 0; j < 10; j++)
			{
				GameObject o  = (GameObject)GameObject.Instantiate(water, 
					transform.position + Vector3.left * 100 * i + Vector3.forward * 100* j, 
					transform.rotation);
				o.transform.parent = this.transform;
				
			}
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
