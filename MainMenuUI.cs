using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {


	public GameObject blackScreenCanvas;
	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = blackScreenCanvas.GetComponent<Animator>();
	}

	/*	Funzione per uscire dall'applicazione	*/
	public void Exit(){
		Debug.Log("APPLICATION QUIT!");
		StartCoroutine (BlackScreen (true));

	}

	/*	Funzione per iniziare la partita	*/
	public void StartGame(){
		StartCoroutine (BlackScreen (false));
	}

	/*	Funzione per mostrae lo schermo nero nel menu principale	*/
	IEnumerator BlackScreen(bool exit){
		animator.ResetTrigger("FadingOut");
		if (exit == false) {
			yield return new WaitForSeconds (1.2f);

			SceneManager.LoadSceneAsync ("GameScene");

			yield return new WaitForSeconds (1);

			yield break;
		} else {

			yield return new WaitForSeconds (1);
			Application.Quit();

			yield break;
		}
	}
}
