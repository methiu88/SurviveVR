using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;
	public GameObject gameOverUI;

	void Awake(){
		if (gm == null) {
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster> ();
		}
	}
		
	// Use this for initialization
	void Start () {
		
	}

	/*	Funzione per far partire la schermata di EndGame	*/
	public void EndGame(){
		Debug.Log ("GAME OVER!");
		gameOverUI.SetActive (true);
	}
}
