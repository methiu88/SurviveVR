using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {
	// transform dell'obiettivo del navmesh. Corrisponde a quelo del gioatore
	private Transform goal;
	private UnityEngine.AI.NavMeshAgent agent;
	private bool isAttacking, isDead, hittingPlayer;
	private float attackDistance, damage;
	private Animation anim;
	private Player p;
	public float health = 20f;

	/*	Riferimenti agli oggetti da raccogliere che possono essre intercettati dal Raycast	*/
	public MedicalKit medkit;
	public ShotgunItem shotgun;
	public AssaultRifleItem assaultRifle;

	/*	Valori relativi alle percentuali di rilascio degli oggetti e dei punti all'uccisione	*/
	private float dropMed = .1f;
	private float dropShotgun = .3f;
	private float dropAssaultRifle = .5f;
	private int points = 100;

	private AudioSource[] sounds;
	private AudioSource zombieAttack1, zombieAttack2, zombieDeath1, zombieDeath2, zombieHurt;

	/*	Funzione che viene eseguita per prima. Le assegnazioni possono essere effettuate anche in 
	*	Awake oltre che in Start																	*/
	void Awake(){
		anim = GetComponent<Animation> ();
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		sounds = GetComponents<AudioSource> ();
		zombieAttack1 = sounds [0];
		zombieAttack2 = sounds [1];
		zombieDeath1 = sounds [2];
		zombieDeath2 = sounds [3];
		zombieHurt = sounds [4];
	}

	// Use this for initialization
	void Start () {
		goal = Camera.main.transform;
		// destinazione del navmesh agent uguale alla posizione della maincamera
		agent.destination = goal.position;
		attackDistance = 3f;
		damage = 20f;
		isAttacking = isDead = hittingPlayer = false;
		// partenza dell'animazione
		anim.Play ("walk");
	}

	/*	Funzione per assegnare il danno al nemico	*/
	public void TakeDamage(float amount){
		health -= amount;
		if (Random.Range (0, 3) == 1)
			zombieHurt.Play ();

		if (health <= 0f) {
			Die ();
			Debug.Log ("Zombie morto");
		}
	}

	/*	Funzione che effettua tutte le operazioni alla morte del nemico	*/
	void Die(){
		isDead = true;
		// disabilito capsule collider per non far avvenire collisioni multiple
		GetComponent<CapsuleCollider> ().enabled = false;
		// blocco della posizione dello zombie a quella corrente
		agent.destination = gameObject.transform.position;
		// blocco dell'animazione del movimento
		anim.Play ("back_fall");
		float ran = Random.Range (0, 3f);
		if (ran <= 1f)
			zombieDeath1.Play ();
		else if(ran <= 2f)
			zombieDeath2.Play ();

		//	Distruzione dello Zombie in 6 secondi
		Invoke("Destroy", 6f);

		//	Drop degli oggetti
		float dropChance = Random.Range(0f, 3f);
		if (dropChance < dropMed) {
			Instantiate (medkit, gameObject.transform.position, gameObject.transform.rotation);
			Debug.Log ("Medkit dropped!");
		}else if (dropChance < dropShotgun) {
			Instantiate (shotgun, gameObject.transform.position, gameObject.transform.rotation);
			Debug.Log ("Shotgun dropped!");
		}else if (dropChance < dropAssaultRifle) {
			Instantiate (assaultRifle, gameObject.transform.position, gameObject.transform.rotation);
			Debug.Log ("AssaultRifle dropped!");
		}
		AddPoints ();
	}

	/*	Funzione per inizializzare l'oggetto alla sua abilitazione,	
		E' relativa agli elementi che sono già stati disabilitati almeno una volta*/
	void OnEnable(){
		health = 20f;
		isAttacking = isDead = hittingPlayer = false;
		goal = Camera.main.transform;
		GetComponent<CapsuleCollider> ().enabled = true;
		agent.destination = goal.position;
		anim.Play ("walk");
	}

	/*	Funzione che simula la distruzione ma in realtà disabilita l'oggetto	*/
	void Destroy(){
		gameObject.SetActive (false);
	}

	/*	Funzione che cancella l'invocazione effettuata precedentemente
		alla disabilitazione dell'oggetto	*/
	void OnDisable(){
		CancelInvoke ();
	}

	void Update () {
		if (isDead == false) {
			goal = Camera.main.transform;
			float distance = Vector3.Distance (goal.position, transform.position);
			if (distance < attackDistance) {
				//stop the zombie from moving forward by setting its destination to it's current position
				agent.destination = gameObject.transform.position;
				// stop dell'animazione del movimento
				isAttacking = true;
				anim.Play ("attack");

			} else {
				isAttacking = false;
				anim.Play ("walk");
				agent.destination = goal.position;
			}
			if (hittingPlayer && isAttacking) {
				StartCoroutine(PlayAttackSound ());
				hittingPlayer = false;
				p.StartCoroutine (p.Hit(damage));
			}
		}
	}

	/*	Funzione che riproduce in maniera casuale un suono di attacco	*/
	IEnumerator PlayAttackSound(){
		yield return new WaitForSeconds (.2f);
		float random = Random.Range (0f, 3f);
		if (random <= 1f)
			zombieAttack1.Play ();
		else if(random <= 2f)
			zombieAttack2.Play ();
		yield return new WaitForSeconds (1f);
	}

	/*	Funzione che aggiunge i punti relativi al nemico allo Score	*/
	private void AddPoints(){
		GameObject.FindWithTag ("Score").GetComponent<Score> ().AddPoints (points);
	}

	/*	Funzione per bloccare sulla posizione il nemico	*/
	public void Freeze(){
		anim.Stop ();
		agent.destination = gameObject.transform.position;
	}

	/*	Funzione che viene eseguita quando si verifica una collisione tra il nemico e un oggetto
	*	Rigidbody																					*/
	void OnTriggerEnter (Collider col)
	{
		//Debug.Log ("Toccato " + col.gameObject.tag + " ; isAttacking " + isAttacking);
		if (col.tag == "Player") {
			p = col.GetComponent<Player> ();
			hittingPlayer = true;
		} else {
			hittingPlayer = false;
		}
	}
}