using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {
	
    private enum EnemyState
    {
        Normal,
        Run,
        Stun
    }

    private GlobalBehavior mGlobalBehavior;
    private EnemyState mCurrentState;
    private bool mEnemyMovement;
    private SpriteRenderer mSprite;
    private int mStunCount = 0;
    private int kMaxStunCount = 2;
    private float kStunDelta = 5f;
    private float mStunTimer;
    private GameObject mHero;

    [SerializeField]
    private Sprite[] mCurrentSprite;
    
    public float mSpeed;
	public float mTowardsCenter = 0.5f;  
		// what is the change of enemy flying towards the world center after colliding with world bound
		// 0: no control
		// 1: always towards the world center, no randomness
		
	// Use this for initialization
	void Start () {
        mHero = GameObject.Find("Starfighter");
        mSprite = GetComponent<SpriteRenderer>();
        mGlobalBehavior = GameObject.Find("GameManager").GetComponent<GlobalBehavior>();
        mCurrentState = EnemyState.Normal;
        mEnemyMovement = true;
        mSpeed = Random.Range(20f, 40f);
        NewDirection();
    }

	// Update is called once per frame
	void Update () {
        switch(mCurrentState)
        {
            case EnemyState.Normal:
                UpdateNormalState();
                break;
            case EnemyState.Run:
                UpdateRunState();
                break;
            case EnemyState.Stun:
                UpdateStunState();
                break;
        }
	}

    private void UpdateNormalState()
    {
        if(mEnemyMovement)
        {
            transform.position += (mSpeed * Time.smoothDeltaTime) * transform.up;

            GlobalBehavior.WorldBoundStatus status = mGlobalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);

            if (status != GlobalBehavior.WorldBoundStatus.Inside)
            {
                // Debug.Log("collided position: " + this.transform.position);
                NewDirection();
            }
        }
        else
        {
            mSpeed = Random.Range(20f, 40f);
        }

        if(getDistance() < 30f && isFacing())
        {
            mSprite.sprite = mCurrentSprite[1];
            mCurrentState = EnemyState.Run;
        }
    }

    private void UpdateRunState()
    {
        Vector3 moveDirection = transform.position - mHero.transform.position;
        moveDirection.Normalize();
        transform.up = moveDirection;

        transform.position += (mSpeed * Time.smoothDeltaTime) * transform.up;

        GlobalBehavior.WorldBoundStatus status = mGlobalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);

        if (status != GlobalBehavior.WorldBoundStatus.Inside)
        {
            // Debug.Log("collided position: " + this.transform.position);
            NewDirection();
        }

        if (getDistance() > 30f)
        {
            mSprite.sprite = mCurrentSprite[0];
            mCurrentState = EnemyState.Normal;
        }
    }

    private void UpdateStunState()
    {
        if(Time.time < mStunTimer)
        {
            transform.Rotate(Vector3.forward, 1f * (9 * Time.smoothDeltaTime));
        }
        else
        {
            mSprite.sprite = mCurrentSprite[0];
            mCurrentState = EnemyState.Normal;
        }

    }

    private float getDistance()
    {
        return Vector3.Distance(transform.position, mHero.transform.position);
    }

    private bool isFacing()
    {
        Vector3 targetDir = transform.position - mHero.transform.position;
        Vector3 forward = mHero.transform.up;
        float angle = Vector3.Angle(targetDir, forward);
        if(angle < 10f)
        {
            Debug.Log("looking at");
            return true;
        }

        return false;
    }

	// New direction will be something randomly within +- 45-degrees away from the direction
	// towards the center of the world
	//
	// To find an angle within +-45 degree of a direction: 
	//     1. consider the simplist case of 45-degree above or below the x-direction
	//	   2. we compute random.X: a randomly generate x-value between +1 and -1
	//     3. To ensure within 45 degrees, we simply need to make sure generating a y-value that is within the (-random.X to +random.X) range
	//     4. Now a direction towards the (random.X, random.Y) is guaranteed to be within 45-degrees from x-direction
	// Apply the above logic, only now:
	//		X-direciton is V (from current direciton towards the world center)
	//		Y-direciton is (V.y, -V.x)
	//
	// Lastly, 45-degree is nice because X=Y, we can do this for any angle that is less than 90-degree
	private void NewDirection() {

		// we want to move towards the center of the world
		Vector2 v = mGlobalBehavior.WorldCenter - new Vector2(transform.position.x, transform.position.y);  
				// this is vector that will take us back to world center
		v.Normalize();
		Vector2 vn = new Vector2(v.y, -v.x); // this is a direciotn that is perpendicular to V

		float useV = 1.0f - Mathf.Clamp(mTowardsCenter, 0.01f, 1.0f);
		float tanSpread = Mathf.Tan( useV * Mathf.PI / 2.0f );

		float randomX = Random.Range(0f, 1f);
		float yRange = tanSpread * randomX;
		float randomY = Random.Range (-yRange, yRange);

		Vector2 newDir = randomX * v + randomY * vn;
		newDir.Normalize();
		transform.up = newDir;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log ("Hit!");

		// Only care if hitting an Egg (vs. hitting another Enemy!
		if (other.gameObject.name == "Egg(Clone)") {
			Destroy(other.gameObject);
            if (mStunCount < kMaxStunCount)
            {
                mStunCount++;
                mSprite.sprite = mCurrentSprite[2];
                mCurrentState = EnemyState.Stun;
                mStunTimer = Time.time + kStunDelta;
            }
            else
            {
                Destroy(this.gameObject);
            }
		}
	}
	
}
