using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gun : MonoBehaviour {

	private const string DISPLAY_TEXT_FORMAT = "{0} / {1}";	// Formato di testo predefinito per scrivere del testo

	/*	Parametri di default delle armi che poi vengono sovrascritti da ogni 
	 * 	tipologia di arma direttamente nell'Inspector							*/
	public float damage = 10f;
	public float range = 100f;
	public float fireRate = 15f;
	public float impactForce = 30f;
	public bool unlimited = false;
	public bool canShoot;
	public int maxMagazineAmmo = 10;
	private int currentMagazineAmmo;
	public int maxWeaponAmmo;
	private int currentWeaponAmmo;
	public float reloadTime = 1f;
	public int gunShots = 1;

	/* Variabili per la ricarica */
	private bool isReloading = false;
	private bool manualReloading = false;

	public Camera fpsCam;						// Camera del giocatore da cui viene emanato il Raycast nel momento in cui si spara
	public ParticleSystem muzzleFlash;			// Effetto particellare quando si spara
	public GameObject impactEffect;				// Oggetto a cui e' associato un effetto sull'oggetto colpito
	public Animator animator;					// Animatore associato all'arma per le animazioni nelle fasi di ricarica

	/*	Parametri inerenti alla cadenza di fuoco e al tempo minimo da aspettare dopo aver interagito con un oggetto	*/
	private float nextTimeToFire = 0f;
	private float nextTimeToFireAfterInteraction = 0f;

	private Text ammoField;						// Oggetto Text a cui e' associato il component Text dell'arma su cui mostrare le munizioni
	private AudioSource[] sounds;				// Array degli effetti audio attaccati all'arma per
	private AudioSource shot, reload;			// la ricarica e lo sparo

	/*	Riferimento alle tre tipologie di oggetti che possono essere raccolti	*/
	MedicalKit medkit;
	AssaultRifleItem rifle;
	ShotgunItem shotgun;

	/*	Associazione dei vari riferimenti agli oggetti	*/
	void Awake(){
		sounds = GetComponents<AudioSource> ();
		shot = sounds [0];
		reload = sounds [1];
		ammoField = gameObject.transform.GetChild (0).gameObject.GetComponent<Text>();
	}

	/*	Inizializzazione delle variabili e del testo delle munizioni mostrato sull'arma	*/
	void Start(){
		currentMagazineAmmo = maxMagazineAmmo;
		currentWeaponAmmo = maxMagazineAmmo;
		if(unlimited) ammoField.text = string.Format (DISPLAY_TEXT_FORMAT, currentMagazineAmmo, "∞");
		else ammoField.text = string.Format (DISPLAY_TEXT_FORMAT, currentMagazineAmmo, currentWeaponAmmo);
		canShoot = true;
	}

	/*	Funzione che viene eseguita tutte le volte che viene attivata un oggetto di tipo Gun	*/
	void OnEnable(){
		isReloading = false;
		animator.SetBool ("Reloading", false);
		nextTimeToFireAfterInteraction = Time.time + .2f;
		if(unlimited) ammoField.text = string.Format (DISPLAY_TEXT_FORMAT, currentMagazineAmmo, "∞");
		else ammoField.text = string.Format (DISPLAY_TEXT_FORMAT, currentMagazineAmmo, currentWeaponAmmo);
	}

	/*	Funzione che recupera il numero di proiettili disponibili nel caricatore	*/
	public int GetCurrentMagazineAmmo(){
		return currentMagazineAmmo;
	}

	/*	Funzione che recupera il numero di proiettili disponibili oltre a quelli del caricatore	*/
	public int GetCurrentWeaponAmmo(){
		return currentWeaponAmmo;
	}

	/*	Funzione per attivare la ricarica manuale	*/
	public void ManualReloading(){
		if (currentMagazineAmmo < maxMagazineAmmo && (currentWeaponAmmo > 0 || unlimited))
			manualReloading = true;
	}

	/*	Funzione per effettuare un colpo di arma da fuoco	*/
	void Shoot(){

		//play the gun shot sound and gun animation
		shot.Play();
		if(!isReloading)
			GetComponent<Animation>().Play ();

		muzzleFlash.Play ();

		currentMagazineAmmo--;

		if(unlimited) ammoField.text = string.Format (DISPLAY_TEXT_FORMAT, currentMagazineAmmo, "∞");
		else ammoField.text = string.Format (DISPLAY_TEXT_FORMAT, currentMagazineAmmo, currentWeaponAmmo); 

		if (gunShots > 1) {
			for (int i = 0; i < gunShots; i++) {
				RaycastHit hit;

				Ray ray = new Ray (fpsCam.transform.position, fpsCam.transform.forward);

				Vector3 v3T = ray.direction;
				v3T.x += Random.Range (-.3f, .3f);
				v3T.y += Random.Range (-.01f, .01f);
				//v3T.z += Random.Range (-1.0f, 1.0f);
				ray.direction = v3T;

				if (Physics.Raycast (ray, out hit, range)) {
					Zombie enemy = hit.transform.GetComponent<Zombie> ();

					if (enemy != null) {
						enemy.TakeDamage (damage);
						Debug.Log ("Nemico " + enemy.name + " colpito! Danno : " + damage);
					}
					if (hit.rigidbody != null) {
						hit.rigidbody.AddForce (-hit.normal * impactForce);
					}
					// Per inserire l'effetto di impatto utilizzando l'Object Pool Pattern
					GameObject obj = NewObjectPoolerScript.current.GetPooledObject();
					if (obj == null) return;
					obj.transform.position = hit.point;
					obj.transform.rotation = Quaternion.LookRotation (hit.normal);
					obj.SetActive(true);
				}
			}
		}else{
			RaycastHit hit;
			if (Physics.Raycast (fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) {

				//Debug.Log (hit.transform.name);

				Zombie enemy = hit.transform.GetComponent<Zombie> ();

				if (enemy != null) {
					enemy.TakeDamage (damage);
					Debug.Log ("Nemico " + enemy.name + " colpito! Danno : " + damage);
				}
				if (hit.rigidbody != null) {
					hit.rigidbody.AddForce (-hit.normal * impactForce);
				}
				// Per inserire l'effetto di impatto utilizzando l'Object Pool Pattern
				GameObject obj = NewObjectPoolerScript.current.GetPooledObject();
				if (obj == null) return;
				obj.transform.position = hit.point;
				obj.transform.rotation = Quaternion.LookRotation (hit.normal);
				obj.SetActive(true);
			}
		}
	}

	void Update(){

		if (isReloading)
			return;

		if(currentMagazineAmmo <= 0 || manualReloading){
			StartCoroutine(Reload());
			return;
		}
		
		if (Input.GetButton ("Fire1")) {
			//Debug.Log ("Gun : canShoot = " + canShoot);
			if (!canShoot) {
				RaycastHit rayHit;
				Physics.Raycast (fpsCam.transform.position, fpsCam.transform.forward, out rayHit, range);
				rifle = rayHit.transform.GetComponent<AssaultRifleItem> ();
				shotgun = rayHit.transform.GetComponent<ShotgunItem> ();
				medkit = rayHit.transform.GetComponent<MedicalKit> ();

				if (medkit != null) {
					nextTimeToFireAfterInteraction = Time.time + .1f;
				} else if (rayHit.transform.GetComponent<AssaultRifleItem> () != null) {
					nextTimeToFireAfterInteraction = Time.time + .1f;
				} else if (rayHit.transform.GetComponent<ShotgunItem> () != null) {
					nextTimeToFireAfterInteraction = Time.time + .1f;
				}
			}else{
				if (Time.time >= nextTimeToFire && Time.time >= nextTimeToFireAfterInteraction) {
					nextTimeToFire = Time.time + 1f / fireRate;
					Shoot ();
				}
			}
		}
	}

	/*	Funzione che ritorna se un'arma sta ricaricando	*/
	public bool IsReloading(){
		return isReloading;
	}

	/*	Funzione che incrementa i proiettili disponibili di un caricatore	*/
	public void IncWeaponAmmo(){
		currentWeaponAmmo += maxMagazineAmmo;
		if (currentWeaponAmmo > maxWeaponAmmo)
			currentWeaponAmmo = maxWeaponAmmo;
		ammoField.text = string.Format (DISPLAY_TEXT_FORMAT, currentMagazineAmmo, currentWeaponAmmo);
	}

	/*	Funzione che effettua la ricarica	*/
	IEnumerator Reload(){
		if ((currentWeaponAmmo > 0 || unlimited)) {
			if (currentMagazineAmmo == maxMagazineAmmo) {
				manualReloading = false;
				yield break;
			}
			
			isReloading = true;
			//Debug.Log ("Reloading...");

			animator.SetBool ("Reloading", true);
			reload.Play ();

			yield return new WaitForSeconds (reloadTime - .25f);
			animator.SetBool ("Reloading", false);
			yield return new WaitForSeconds (.25f);	// così posso sparare solo quando l'arma si rialza

			if (currentWeaponAmmo >= (maxMagazineAmmo - currentMagazineAmmo)) {
				currentWeaponAmmo -= (maxMagazineAmmo - currentMagazineAmmo);
				currentMagazineAmmo = maxMagazineAmmo;
			} else {
				if (unlimited)
					currentMagazineAmmo = maxMagazineAmmo;
				else {
					currentMagazineAmmo += currentWeaponAmmo;
					currentWeaponAmmo = 0;
				}
			}
			
			if (currentWeaponAmmo < 0)
				currentWeaponAmmo = 0;

			if(unlimited) ammoField.text = string.Format (DISPLAY_TEXT_FORMAT, currentMagazineAmmo, "∞");
			else ammoField.text = string.Format (DISPLAY_TEXT_FORMAT, currentMagazineAmmo, currentWeaponAmmo);
			isReloading = false;
			manualReloading = false;
			reload.Stop ();
		} else {
			if (currentMagazineAmmo == 0) {
				//Debug.Log ("No more weapon Ammo!");
				ammoField.text = "NO Ammo";
				animator.SetBool ("Reloading", true);
				yield break;
			}
		}
	}

}