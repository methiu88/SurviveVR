using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour {

	private bool isHitted, isDead, isInDanger;		// Variabili per sapere se il giocatore e' stato colpit, e' morto o ha poca salute
	private float currentHealth, maxHealth;			// Salute corrente e massima del giocatore

	public Slider healthBar;						// Slider per mostrare la barra dell'energia al giocatore	

	private AudioSource[] sounds;										// ArrayList di AudioSource per avere riferimento di tutti i suoni
	private AudioSource playerHit1, playerHit2, die, heal, danger;		// attaccati al player e assegnare i riferimenti agli oggetti istanziati

	public GameObject gameOverUI;					// Oggetto che rappresenta lo Script GameOverUI 
	public GameObject dieScreen;					// Oggetto per effettuare le transizioni di buio sulla scena
	public GameObject scoreCanvas;					// Canvas delle transizioni di buio sulla scena
	public GameObject damageHealCanvas;				// Canvas degli effetti di cura e danno

	private WeaponSwitching weaponHolder;			// Istanza dello script WeaponSwitching che gestice le armi equipagiate
	private Score score;							// Istanza di Score
	private Animator damageHealAnim;
	private WeaponChangerUI weaponChangerCanvas;

	void Awake(){
		sounds = GetComponents<AudioSource> ();
		playerHit1 = sounds [0];
		playerHit2 = sounds [1];
		die = sounds [2];
		heal = sounds [3];
		danger = sounds [4];
		weaponHolder = gameObject.transform.GetChild (0).GetChild (0).GetComponent<WeaponSwitching> ();
		weaponChangerCanvas = gameObject.transform.GetChild (1).GetComponent<WeaponChangerUI> ();
		score = scoreCanvas.GetComponent<Score> ();
	}

	// Use this for initialization
	void Start () {
		isHitted = isDead = false;
		currentHealth = maxHealth = 100f;
		Debug.Log("Salute del giocatore = " + currentHealth);
		healthBar.value = CalculateHealth ();
		damageHealAnim = damageHealCanvas.GetComponent<Animator> ();
		isInDanger = false;
	}

	/*	Funzione che implementa il danneggiamento del giocatore	*/
	public IEnumerator Hit(float damage) {
		GetComponent<CapsuleCollider> ().enabled = false;
		isHitted = true;
		score.ResetMultiplier ();
		yield return new WaitForSeconds (.7f);
		damageHealAnim.SetTrigger ("Damage");
		currentHealth -= damage;
		Debug.Log ("Salute del PLAYER = " + currentHealth);
		if (currentHealth <= 40 && !isInDanger) {
			danger.Play ();
			isInDanger = true;
		}
		if (currentHealth <= 0f) {
			currentHealth = 0f;
			isDead = true;
		}

		healthBar.value = CalculateHealth ();
		if (isDead) {
			danger.Stop ();
			die.Play ();
			EndGame ();
		} else {
			if (Random.Range (0f, 2f) <= 1f)
				playerHit1.Play ();
			else
				playerHit2.Play ();
			Debug.Log ("Giocatore colpito! Danno = " + damage);
			Debug.Log ("Salute = " + currentHealth);

			yield return new WaitForSeconds (1.5f);
			isHitted = false;
			GetComponent<CapsuleCollider> ().enabled = true;
		}

		yield break;
	}

	/*	Funzione per la fine della partita e che fa partire la schermata di morte	*/
	public void EndGame(){
		Debug.Log ("GAME OVER!");
		score.EndGame ();
		weaponChangerCanvas.gameObject.SetActive(false);
		weaponHolder.gameObject.SetActive (false);
		StartCoroutine (DieScreen ());
	}

	/*	Funzione che implementa la scheramata rossa della morte del giocatore, disabilita le armi
	 * ed abilita il pannello di GameOver															*/
	IEnumerator DieScreen(){
		Debug.Log ("RedScreen starting!");
		dieScreen.SetActive (true);
		yield return new WaitForSeconds (1.5f);
		weaponHolder.DisableWeapon ();
		yield return new WaitForSeconds (1.8f);
		dieScreen.SetActive (false);
		gameOverUI.SetActive (true);

		yield break;
	}

	/*	funzione che implementa la cura del giocatore	*/
	public void Heal(float amount){
		heal.Play ();
		damageHealAnim.SetTrigger("Heal");
		currentHealth += amount;
		if (currentHealth > maxHealth)
			currentHealth = maxHealth;
		isInDanger = false;
		danger.Stop ();
		healthBar.value = CalculateHealth ();
		Debug.Log ("Player healed of " + amount);
	}

	/*	Funziona che ritorna se il giocatore e' morto	*/
	public bool IsPlayerDead(){
		return isDead;
	}

	/*	Funzione che rimette in vita il giocatore. Mai utilizzata nell'applicazione 
	 * 	a causa della struttura attualmente implementata delle meccaniche	*/
	public void setAlive(){
		isDead = false;
		currentHealth = maxHealth;
		healthBar.value = CalculateHealth ();
	}

	/*	Funzione per effettuare il calcolo della salute e l'aggiornamento della barra della salute	*/
	float CalculateHealth(){
		return currentHealth / maxHealth;
	}
}