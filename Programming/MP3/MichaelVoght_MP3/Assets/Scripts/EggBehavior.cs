using UnityEngine;
using System.Collections;

public class EggBehavior : MonoBehaviour {
	
	private float mSpeed = 100f;
    private GlobalBehavior mGlobalBehavior;

	void Start()
	{
        mGlobalBehavior = GameObject.Find("GameManager").GetComponent<GlobalBehavior>();
    }

	// Update is called once per frame
	void Update () {
        GlobalBehavior.WorldBoundStatus status = mGlobalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
        if (status != GlobalBehavior.WorldBoundStatus.Inside)
            Delete();
        else
            transform.position += (mSpeed * Time.smoothDeltaTime) * transform.up;
	}
	
	public void SetForwardDirection(Vector3 f)
	{
		transform.up = f;
	}

    public void Delete()
    {
        Destroy(gameObject);
    }
}
