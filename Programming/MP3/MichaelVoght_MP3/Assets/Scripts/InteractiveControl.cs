/* 
 *  Author:             Michael Voght
 *  Date:               4/21/16
 *  Citations:          Code - Kelvin Sung
 *                      Background - http://eriq.deviantart.com/art/Water-Texture-49283686
 *                      Starfighter -
 *                      EnemyShip -
 *                      Egg -
 *  InteractiveControl: Controls the Starfighter. Has a projectile attached to it that
 *                      fires Eggs at a 0.1 sec RoF. Uses Gamemanager to keep itself in
 *                      world bounds
 */

using UnityEngine;	
using System.Collections;

public class InteractiveControl : MonoBehaviour {

	public GameObject mProjectile = null;

	#region user control references
	private float kHeroSpeed = 20f;
	private float kHeroRotateSpeed = 90/2f; // 90-degrees in 2 seconds
    private float kFireDelta = 0.1f;
    private float mLastFire;
    private GlobalBehavior mglobalBehavior;

	#endregion
	// Use this for initialization
	void Start () {
        // initialize projectile spawning
		if (null == mProjectile)
			mProjectile = Resources.Load ("Prefabs/Egg") as GameObject;
        mLastFire = Time.time + kFireDelta;
        mglobalBehavior = GameObject.Find("GameManager").GetComponent<GlobalBehavior>();
    }
	
	// Update is called once per frame
	void Update () {
        #region user movement control
        GlobalBehavior.WorldBoundStatus status =
            mglobalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);

        switch(status)
        {
            case GlobalBehavior.WorldBoundStatus.CollideBottom:
                transform.position += new Vector3(0.0f, 0.1f, 0.0f);
                break;
            case GlobalBehavior.WorldBoundStatus.CollideTop:
                transform.position += new Vector3(0.0f, -0.1f, 0.0f);
                break;
            case GlobalBehavior.WorldBoundStatus.CollideLeft:
                transform.position += new Vector3(0.1f, 0.0f, 0.0f);
                break;
            case GlobalBehavior.WorldBoundStatus.CollideRight:
                transform.position += new Vector3(-0.1f, 0.0f, 0.0f);
                break;
            default:
                transform.position += Input.GetAxis("Vertical") * transform.up * (kHeroSpeed * Time.smoothDeltaTime);
                transform.Rotate(Vector3.forward, -1f * Input.GetAxis("Horizontal") * (kHeroRotateSpeed * Time.smoothDeltaTime));
                break;
        }

        #endregion

        if (Input.GetAxis ("Fire1") > 0f && Time.time > mLastFire) { // this is Left-Control
            mLastFire = Time.time + kFireDelta;
			GameObject e = Instantiate(mProjectile) as GameObject;
			EggBehavior egg = e.GetComponent<EggBehavior>(); // Shows how to get the script from GameObject
			if (null != egg) {
				e.transform.position = transform.position;
				egg.SetForwardDirection(transform.up);
			}
		}
	}
}
