using UnityEngine;
using System.Collections;

public class EggBehavior : MonoBehaviour {
	
	private float mSpeed = 100f;

	void Start()
	{
	}

	// Update is called once per frame
	void Update () {
		transform.position += (mSpeed * Time.smoothDeltaTime) * transform.up;
	}
	
	public void SetForwardDirection(Vector3 f)
	{
		transform.up = f;
	}
}
