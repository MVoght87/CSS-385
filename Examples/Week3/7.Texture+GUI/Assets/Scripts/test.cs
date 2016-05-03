using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	private int CountOnHeap;
	private bool triggered;
	// Use this for initialization
	void Start () {
		CountOnHeap = 0;
		triggered = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!triggered) {
			StartCoroutine (timer ());
			triggered = true;
		}
	}

	IEnumerator FireFunction(int c) {
		int localCount = 1;
		for (int i = 0; i<3; i++) {
			Debug.Log ("FireFunction: GlobalCount= " + c + " localCount= " + localCount);
			localCount++;
			yield return new WaitForSeconds(3f);
		}
	}

	IEnumerator timer() {
		StartCoroutine(FireFunction(CountOnHeap));
		CountOnHeap++;
		yield return new WaitForSeconds(1f);
		StartCoroutine(FireFunction(CountOnHeap));
	}
}
