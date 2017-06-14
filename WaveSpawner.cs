using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {

	public enum SpawnState{ SPAWNING, WAITING, COUNTING}	//	stati del WaveSpawner

	// Serializzable permette di modificare queste variabili dall'interno dell'inspector di Unity
	[System.Serializable]
	public class Wave{
		public string name;
		public Transform enemy;
		public int count;
		public float rate;
	}

	public Player player;				// Riferimento all'oggetto Player

	public Wave[] waves;				//	Array su cui sono inserite le ondate
	private int nextWave = 0;			//	Indica il numero di ondata

	public Transform[] spawnPoints;		// Array su cui sono inseriti tutti gli spawnPoint da utilizzare nella fase di spawning

	public float timeBetweenWaves = 5f;	// Tempo tra un'ondata e l'altra
	private float waveCountdown;		// Tempo che manca alla partenza della prossima ondata

	private float searchCountdown = 1f;	//	Tempo per cui il waveSpawner cerca altri nemici all'interno della scena

	private SpawnState state = SpawnState.COUNTING;	//	Viene settato lo stato iniziale dell'azione di spawning

	private AudioSource[] sounds;		//	Array di AudioSource e AudioSource specifici a cui verranno attaccati i
	private AudioSource zombieHorde1, zombieHorde2, waveCompleted;	//	riferimenti ad ogni singolo AudioSource presente nell'array
	private Animation anim;				//	Animazione per gestire i volumi dell'audio di un determinato effetto di spawn

	public GameObject waveTextCanvas;	//	Canvas del punteggio e del numero di ondata
	public GameObject scoreCanvas;		//	su cui vengono scrittiil numero di ondata 
	private InfoText waveText;			//	e il punteggio stesso		
	private bool doneEndGame;			//	variabile che mi definisce quando è possibile distruggere tutti i nemici e gli oggetti
	private Score score;				//	oggetto che farà riferimento al punteggio

	/*	Funzione che assegna i riferimenti agli oggetti pubblici	
	 * 	o che inizializza le variabili o gli oggetti				*/
	void Start(){
		if (spawnPoints.Length == 0) {
			Debug.LogError ("No spawn points referenced.");
		}
		waveCountdown = timeBetweenWaves;
		sounds = GetComponents<AudioSource> ();
		zombieHorde1 = sounds [0];
		zombieHorde2 = sounds [1];
		waveCompleted = sounds [2];
		anim = GetComponent<Animation> ();
		waveText = waveTextCanvas.GetComponent<InfoText> ();
		doneEndGame = false;
		score = scoreCanvas.GetComponent<Score> ();
	}

	/*	Funzione di aggiornamento che viene ripetuta per ogni frame del gioco.
	 * Questa controlla, in base alle azioni del giocatore in quale stato si trova 
	 * l'azione di gioco ed effettua le transizioni da uno all'altro quando è opportuno				*/
	void Update(){
		if (!player.IsPlayerDead ()) {
			
			if (state == SpawnState.WAITING) {
				// Verifica di nemici in vita nell'ondata corrente
				if (!EnemyIsAlive ()) {
					// Inizia un nuovo round
					WaveCompleted ();
					return;

				} else {
					return;
				}
			}

			if (waveCountdown <= 0) {
				if (state != SpawnState.SPAWNING) {
					// Inizio della fase di spawn dell'ondata
					if (Random.Range (0f, 2f) <= 1f) {
						zombieHorde1.Play ();
						anim.Play ();
					}
					else
						zombieHorde2.Play ();
					StartCoroutine (SpawnWave (waves [nextWave]));
				}
			} else {
				waveCountdown -= Time.deltaTime;
			}
		} else {
			if (!doneEndGame) {
				waveText.SetDeathText ();
				doneEndGame = true;
			}
				StartCoroutine (DestroyEnemies ());
				DestroyItems ();
		}
	}

	/*	Funzione che viene chiamata al completamento di un'ondata di nemici	e che 
	 * fa passare all'ondata successiva												*/
	void WaveCompleted(){
		Debug.Log("Wave Completed!");
		waveText.SetCompletedWaveText ();
		waveCompleted.Play ();
		score.IncWave ();

		state = SpawnState.COUNTING;
		waveCountdown = timeBetweenWaves;

		if (nextWave + 1 > waves.Length - 1) {
			nextWave = 0;
			Debug.Log ("ALL WAVES COMPLETE! Looping...");
		} else {
			nextWave++;
		}
	}

	/*	Funzione che controlla se vi soo ancora dei nemici in vita nella scena	*/
	bool EnemyIsAlive(){

		searchCountdown -= Time.deltaTime;
		if (searchCountdown <= 0f) {
			searchCountdown = 1f;
			if (GameObject.FindGameObjectWithTag ("Enemy") == null) {
				return false;
			}
		}
		return true;
	}

	/*	Funzione che avvia lo spawning di un'ondata e che genera i nemici ad intervalli regolari	*/
	IEnumerator SpawnWave(Wave wave){
		Debug.Log ("Spawning Wave : " + wave.name);
		state = SpawnState.SPAWNING;

		// Spawn
		for(int i = 0; i < wave.count; i++){

			SpawnEnemy (wave.enemy);
			yield return new WaitForSeconds (1f / wave.rate);
		}

		state = SpawnState.WAITING;

		yield break;

	}

	/*	Funzione che distrugge tutti i nemici presenti nella scena	*/
	IEnumerator DestroyEnemies(){
		// Destroy
		yield return new WaitForSeconds(1);

		Destroy (GameObject.FindGameObjectWithTag ("Enemy"));

		yield break;
	}

	/*	Funzione che distrugge tutti gli oggetti presenti nella scena	*/
	void DestroyItems(){

		Destroy (GameObject.FindGameObjectWithTag ("Item"));

	}

	/*	Funzione che genera un nemico in uno SpawnPoint all'interno della scena	*/
	void SpawnEnemy(Transform enemy){
		// Spawn Enemy
		Debug.Log("Spawning Enemy: " + enemy.name);

		if (spawnPoints.Length == 0) {
			Debug.LogError ("No spawn points referenced.");
		}

		Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];

		GameObject obj = NewEnemyPoolerScript.current.GetPooledObject();
		if (obj == null) return;
		obj.transform.position = sp.position;
		obj.transform.rotation = sp.rotation;
		obj.SetActive(true);
	}
}
