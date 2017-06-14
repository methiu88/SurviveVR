using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	/*	Oggetto generico da raccogliere	*/

	private Animation anim;

	// Use this for initialization
	void Start () {
		//create references
		anim = GetComponent<Animation> ();
		anim.Play ();
		Destroy (gameObject, 10f);
	}

	/*	Funzione dell'effetto di racolta di un oggetto interattivo	*/
	public void PickedUp(){
		Debug.Log ("PickedUp generic Item!");
		Destroy (gameObject);
	}

	/*	Funzione per disabilitare o abilitare lo sparo dell'arma	*/
	public void SetCanShoot(bool b){
		GameObject.FindWithTag ("WeaponHolder").GetComponent<WeaponSwitching> ().SetCanShoot (b);
	}
}
