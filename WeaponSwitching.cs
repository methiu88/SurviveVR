using UnityEngine;

public class WeaponSwitching : MonoBehaviour {

	public int selectedWeapon = 0;							// Indice del'arma selezionata
	private bool firstTimeShotgun, firstTimeAssaultRifle;	// Per sapere se un'arma e'stata raccolta per la prima volta
	private AudioSource grabSound, changeSound;							// suono emesso alla raccolta dell'arma e dal cambio arma
	private AudioSource[] sounds;

	/*	Associazione di tutti gli elementi e inzizializzazione variabili	*/
	void Start () {
		firstTimeShotgun = firstTimeAssaultRifle = true;
		sounds = GetComponents<AudioSource> ();
		grabSound = sounds [0];
		changeSound = sounds [1];
		SelectWeapon ();
	}

	/* Funzione che definisce se un'arma può sparare	*/
	public void SetCanShoot(bool b){
		int i = 0;
		foreach (Transform weapon in transform) {
			weapon.GetComponent<Gun> ().canShoot = b;
			i++;
		}
	}

	/*	Funzione che incrementa il nuemro di proiettili in possesso per un'arma specifica	*/
	public void IncWeaponAmmo(int weaponIndex){
		grabSound.Play ();
		int i = 0;
		foreach (Transform weapon in transform) {
			if (i == weaponIndex)
				weapon.GetComponent<Gun> ().IncWeaponAmmo ();
			i++;
		}
	}

	/* Funzione che setta un'arma come quella corrente	*/
	public void SetScelectedWeapon(int index){
		selectedWeapon = index;
		SelectWeapon ();
	}

	/*	Funzione per effettuare una ricarica prima che i colpi del caricatore scendano a zero	*/
	public void ManualReloadWeapon(){
		int i = 0;
		foreach (Transform weapon in transform) {
			if (i == selectedWeapon)
				weapon.GetComponent<Gun> ().ManualReloading();
			i++;
		}
	}

	/*	Funzione per sapere se l'arma selezionata puo' essere ricaricata	*/
	public bool CanReload(){
		int i = 0;
		foreach (Transform weapon in transform) {
			if (i == selectedWeapon) {
				if (weapon.GetComponent<Gun> ().GetCurrentMagazineAmmo () != weapon.GetComponent<Gun> ().maxMagazineAmmo)
					return true;
				else
					return false;
			}
			i++;
		}
		return false; 
	}

	/*	Funzione per recuperare il nuemro di proiettili disponibili dell'arma selezionata 
	 * oltre a quelli del caricatore														*/
	public int GetCurrentWeaponAmmo(){
		int i = 0;
		int ammo = 0;
		foreach (Transform weapon in transform) {
			if (i == selectedWeapon)
				ammo = weapon.GetComponent<Gun> ().GetCurrentWeaponAmmo();
			i++;
		}
		return ammo;
	}

	/*	Funzione per recuperare il numero di proiettili nel caricatore dell'arma selezionata	*/
	public int GetCurrentMagazineAmmo(){
		int i = 0;
		int magazineAmmo = 0;
		foreach (Transform weapon in transform) {
			if (i == selectedWeapon)
				magazineAmmo = weapon.GetComponent<Gun> ().GetCurrentMagazineAmmo();
			i++;
		}
		return magazineAmmo;
	}

	/*	Funzione per selezionara l'arma di indice selectedWeapon	*/
	public void SelectWeapon(){
		
		int i = 0;
		foreach (Transform weapon in transform) {
			if (i == selectedWeapon)
				weapon.gameObject.SetActive (true);
			else
				weapon.gameObject.SetActive (false);
			i++;
		}
		changeSound.Play ();
	}

	/*	Funzione per disabilitare tutte le armi	*/
	public void DisableWeapon(){
		int i = 0;
		foreach (Transform weapon in transform) {
			weapon.gameObject.SetActive (false);
			i++;
		}
	}

	/*	Funzione per sapere se lo Shotgun e' stato utilizzato la prima volta	*/
	public bool GetFirstTimeShotgun(){
		return firstTimeShotgun;
	}

	/*	Funzione per sapere se l'AssaultRifle e' stato utilizzato la prima volta	*/
	public bool GetFirstTimeAssaultRifle(){
		return firstTimeAssaultRifle;
	}

	/*	Funzione per resettare se e' stato utilizzato una volta l'AssaultRifle	*/
		public void TurnOffFirstTimeAsaultRifle(){
		firstTimeAssaultRifle = false;
	}

	/*	Funzione per resettare se e' stato utilizzato una volta lo Shotgun	*/
	public void TurnOffFirstTimeShotgun(){
		firstTimeShotgun = false;
	}
}
