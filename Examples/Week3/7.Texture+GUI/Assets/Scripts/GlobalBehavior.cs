using UnityEngine;
using System.Collections;

public class GlobalBehavior : MonoBehaviour {
	
	#region World Bound support
	private Bounds mWorldBound;  // this is the world bound
	private Vector2 mWorldMin;	// Better support 2D interactions
	private Vector2 mWorldMax;
	private Vector2 mWorldCenter;
	private Camera mMainCamera;
	#endregion

	#region  support runtime enemy creation
	// to support time ...
	private float mPreEnemySpawnTime = -1f; // 
	private const float kEnemySpawnInterval = 1.0f; // in seconds
	
	// spwaning enemy ...
	public GameObject mEnemyToSpawn = null;
	#endregion
	
	// Use this for initialization
	void Start () {

		#region world bound support
		mMainCamera = Camera.main;
		mWorldBound = new Bounds(Vector3.zero, Vector3.one);
		UpdateWorldWindowBound();
		#endregion

		#region initialize enemy spawning
		if (null == mEnemyToSpawn) 
			mEnemyToSpawn = Resources.Load("Prefabs/Enemy") as GameObject;
		#endregion
	}
	
	// Update is called once per frame
	void Update () {
		SpawnAnEnemy();	

		if (Input.GetAxis("Jump") > 0) {
            Debug.Log("Jump pressed");
			// To change texture for an object
			GameObject hero = GameObject.Find("Hero");
			SpriteRenderer r = hero.GetComponent<SpriteRenderer>();
			r.sprite = Resources.Load("Textures/Egg", typeof(Sprite)) as Sprite;

            // To echo text to a defined GUIText
            GameObject echoText = GameObject.Find("EchoText");
            UnityEngine.UI.Text gui = echoText.GetComponent<UnityEngine.UI.Text> ();
			gui.text = "Texture Changed";
		}
	}
	
	#region Game Window World size bound support
	public enum WorldBoundStatus {
		CollideTop,
		CollideLeft,
		CollideRight,
		CollideBottom,
		Outside,
		Inside
	};
	
	/// <summary>
	/// This function must be called anytime the MainCamera is moved, or changed in size
	/// </summary>
	public void UpdateWorldWindowBound()
	{
		// get the main 
		if (null != mMainCamera) {
			float maxY = mMainCamera.orthographicSize;
			float maxX = mMainCamera.orthographicSize * mMainCamera.aspect;
			float sizeX = 2 * maxX;
			float sizeY = 2 * maxY;
			float sizeZ = Mathf.Abs(mMainCamera.farClipPlane - mMainCamera.nearClipPlane);
			
			// Make sure z-component is always zero
			Vector3 c = mMainCamera.transform.position;
			c.z = 0.0f;
			mWorldBound.center = c;
			mWorldBound.size = new Vector3(sizeX, sizeY, sizeZ);

			mWorldCenter = new Vector2(c.x, c.y);
			mWorldMin = new Vector2(mWorldBound.min.x, mWorldBound.min.y);
			mWorldMax = new Vector2(mWorldBound.max.x, mWorldBound.max.y);
		}
	}
	
	public Vector2 WorldCenter { get { return mWorldCenter; } }
	public Vector2 WorldMin { get { return mWorldMin; }} 
	public Vector2 WorldMax { get { return mWorldMax; }}
	
	public WorldBoundStatus ObjectCollideWorldBound(Bounds objBound)
	{
		WorldBoundStatus status = WorldBoundStatus.Inside;
		
		if (mWorldBound.Intersects(objBound)) {
			if (objBound.max.x > mWorldBound.max.x)
				status = WorldBoundStatus.CollideRight;
			else if (objBound.min.x < mWorldBound.min.x)
				status = WorldBoundStatus.CollideLeft;
			else if (objBound.max.y > mWorldBound.max.y)
				status = WorldBoundStatus.CollideTop;
			else if (objBound.min.y < mWorldBound.min.y)
				status = WorldBoundStatus.CollideBottom;
			else if ( (objBound.min.z < mWorldBound.min.z) || (objBound.max.z > mWorldBound.max.z))
				status = WorldBoundStatus.Outside;
		} else 
			status = WorldBoundStatus.Outside;
		return status;
		
	}
	#endregion 

	#region enemy spawning support
	private void SpawnAnEnemy()
	{
		if ((Time.realtimeSinceStartup - mPreEnemySpawnTime) > kEnemySpawnInterval) {
			GameObject e = (GameObject) Instantiate(mEnemyToSpawn);
			mPreEnemySpawnTime = Time.realtimeSinceStartup;
			// Debug.Log("New enemy at: " + mPreEnemySpawnTime.ToString());
		}
	}
	#endregion
}