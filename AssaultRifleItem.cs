using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifleItem : Item {

	/*	Specializzazione per un oggetto AssaultRifle del metodo PickUp di Item	*/
	public void PickedUp(){
		WeaponSwitching weaponHolder = GameObject.FindWithTag("WeaponHolder").GetComponent<WeaponSwitching> ();
		if (weaponHolder.GetFirstTimeAssaultRifle ()) {
			weaponHolder.selectedWeapon = 2;
			weaponHolder.TurnOffFirstTimeAsaultRifle ();
			GameObject.FindWithTag ("WeaponChanger").GetComponent<WeaponChangerUI> ().EnableAssaultRilfe ();
			weaponHolder.SelectWeapon ();
		}
		weaponHolder.IncWeaponAmmo (2);
		Debug.Log ("AssaultRifle picked up...");
		GameObject.FindWithTag ("PickedUpInfo").GetComponent<PickedUpInfoText> ().SetInfoText ("Assault Rifle");
		Destroy (gameObject);
	}
}
