using UnityEngine;
using System.Collections;

public class AlienCreatureUserController : MonoBehaviour {
	public AlienCreatureCharacter alienCharacter;
	
	void Start () {
		alienCharacter = GetComponent<AlienCreatureCharacter> ();	
	}
	
	void FixedUpdate(){
		if (Input.GetButtonDown ("Fire1")) {
			alienCharacter.Attack();
		}

		if (Input.GetButtonDown ("Jump")) {
			alienCharacter.Jump();
		}

		if (Input.GetKey (KeyCode.H)) {
			alienCharacter.Hit();
		}

		if (Input.GetKey (KeyCode.N)) {
			alienCharacter.Down();
		}

		if (Input.GetKey (KeyCode.U)) {
			alienCharacter.StandUp();
		}

		float v = Input.GetAxis ("Vertical");
		float h = Input.GetAxis ("Horizontal");
		alienCharacter.Move (v,h);	
	}
}
