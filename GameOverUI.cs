using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {

	public Player player;
	public GameObject blackScreen;		//	Oggetto che implementa la schermata nera
	public GameObject fadingCanvas;		//	Canvas che implementa la schermata nera
	private Animator fadingAnimator;	//	Animatore per le transizioni di buio

	// Use this for initialization
	void Start () {
		fadingAnimator = fadingCanvas.GetComponent<Animator> ();
	}

	/*	Funzione per uscire e tornare al menu principale	*/
	public void BackMenu(){
		Debug.Log("Back Main Menu!");
		StartCoroutine (BlackScreen (true));
	}

	/*	Funzione per ricominciare a giocare	*/
	public void Retry(){
		StartCoroutine (BlackScreen (false));
	}

	/*	Funzione per far partire la scermata nera e ricominciare la partita o 
	 * tonare alla schermata principale in base al pulsante premuto				*/
	IEnumerator BlackScreen(bool exit){
		GetComponent<Animator> ().SetTrigger ("ButtonClicked");
		fadingAnimator.ResetTrigger ("FadingOut");

		if (exit == false) {
			blackScreen.SetActive (true);
			yield return new WaitForSeconds (1.2f);

			SceneManager.LoadSceneAsync (SceneManager.GetActiveScene().buildIndex);

			yield return new WaitForSeconds (1);

			blackScreen.SetActive (false);

			yield break;
		} else {
			blackScreen.SetActive (true);

			yield return new WaitForSeconds (1);

			SceneManager.LoadSceneAsync ("MainMenu");

			yield break;
		}
	}
}
