using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedicalKit : Item {

	public float healthAmount = 100f;	// Valore modificabile dall'Inspector

	/*	Specializzazione per un oggetto AssaultRifle del metodo PickUp di Item	*/
	public void PickedUp(){
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().Heal (healthAmount);
		//player.GetComponent<Player> ().Heal (healthAmount);
		Debug.Log ("Medkit Healing...");
		GameObject.FindWithTag ("PickedUpInfo").GetComponent<PickedUpInfoText> ().SetInfoText ("Medikit");
		Invoke ("Destroy", 0);
		Destroy (gameObject);
	}
}
