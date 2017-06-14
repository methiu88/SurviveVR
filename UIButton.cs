using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour {

	/*	Funzione relativa ai pulsanti delle armi sul WeaponChangerUI	*/

	private AudioSource[] sounds;
	private AudioSource sfxEnter;
	private AudioSource sfxClick;
	public WeaponSwitching weaponHolder;
	public int newWeaponIndex;

	// Use this for initialization
	void Start () {
		sounds = GetComponents<AudioSource> ();
		sfxEnter = sounds [0];
		sfxClick = sounds [1];
	}

	/*	Funzione per cambiare l'arma attuale con quella rappresentante dal pulsante	*/
	public void SelectWeapon(){
		weaponHolder.selectedWeapon = newWeaponIndex;
		weaponHolder.SelectWeapon ();
	}

	/*	Funzione per ricaricare l'arma	*/
	public void ReloadWeapon(){
		weaponHolder.ManualReloadWeapon ();
	}

	/*	Funzione che riproduce il suono quando il cursore passa sopra al pulsante	*/
	public void PlayEnterSound(){
		sfxEnter.Play ();
	}

	/*	Funzione che riproduce il suono quando viene premuto il pulsante	*/
	public void PlayClickSound(){
		sfxClick.Play ();
	}
}
