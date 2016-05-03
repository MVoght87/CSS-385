/* 
 *  Author:         Michael Voght
 *  Date:           4/21/16
 *  Citations:      Code - Kelvin Sung
 *                  Background - http://eriq.deviantart.com/art/Water-Texture-49283686
 *                  Starfighter -
 *                  EnemyShip -
 *                  Egg -
 *  EggBehavior:    Fires in the direction the Starfighter is facing. If it leaves the
 *                  bounds of the world it is destroyed.
 */

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
