using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunItem : Item {

	/*	Specializzazione per un oggetto Shotgun del metodo PickUp di Item	*/
		public void PickedUp(){
		WeaponSwitching weaponHolder = GameObject.FindWithTag("WeaponHolder").GetComponent<WeaponSwitching> ();
		if (weaponHolder.GetFirstTimeShotgun ()) {
			weaponHolder.selectedWeapon = 1;
			weaponHolder.TurnOffFirstTimeShotgun ();
			GameObject.FindWithTag ("WeaponChanger").GetComponent<WeaponChangerUI> ().EnableShotgun();
			weaponHolder.SelectWeapon ();
		}
		weaponHolder.IncWeaponAmmo (1);
		Debug.Log ("Shotgun picked up...");
		GameObject.FindWithTag ("PickedUpInfo").GetComponent<PickedUpInfoText> ().SetInfoText ("Shotgun");
		Destroy (gameObject);
	}
}
