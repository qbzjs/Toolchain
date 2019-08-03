using UnityEngine;
using System.Collections;

public class AlienCreatureAIController : MonoBehaviour {
	public AlienCreatureCharacter alienCharacter;
	public float forwardSpeed=0f;
	public float turnSpeed=0f;
	public float speedChangeTime=0f;
	
	void Start () {
		alienCharacter = GetComponent<AlienCreatureCharacter> ();
	}
	
	void FixedUpdate(){
		speedChangeTime = speedChangeTime - Time.deltaTime;
		if (speedChangeTime < 0f) {
			speedChangeTime=Random.Range(0f,5f);
			forwardSpeed=Random.Range(-1f,1f);
			turnSpeed=Random.Range (-1f,1f);
		}
		alienCharacter.Move (forwardSpeed,turnSpeed);	
	}
}
