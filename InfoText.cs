using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoText : MonoBehaviour {

	private const string DISPLAY_COMPLETED_WAVE_TEXT_FORMAT = "Wave {0} Completed";	// Formato di testo predefinito per scrivere del testo
	private const string DISPLAY_STARTING_WAVE_TEXT_FORMAT = "Starting Wave {0}";	// Formato di testo predefinito per scrivere del testo
	private Text infoText;	// Oggetto di testo a cui viene associato il riferimento al componente di testo da cambiare
	private Animator anim;	// Animatore a cui viene associato il componente animatore che gestisce le animazioni per il testo
	private int waveCounter;
	// Use this for initialization
	void Start () {
		infoText = gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
		anim = gameObject.GetComponent<Animator> ();
		waveCounter = 1;
	}

	/*	Funzione per mostrare a schermo il testo dell'ondata in partenza. Questo metodo non viene utilizzato per non
	*	rimpire di testo l'utente																					*/
	public void SetStartingWaveText(int waveNumber){
		infoText.text = string.Format (DISPLAY_STARTING_WAVE_TEXT_FORMAT, waveNumber);
		anim.SetTrigger ("WaveCompleted");
	}

	/*	Funzione per mostrare a schermo il testo dell'ondata completata	*/	
	public void SetCompletedWaveText(){
		infoText.text = string.Format (DISPLAY_COMPLETED_WAVE_TEXT_FORMAT, waveCounter);
		anim.SetTrigger ("WaveCompleted");
		waveCounter++;
	}

	/*	Funzione per mostrare a schermo il messaggio di GAMEOVER	*/
	public void SetDeathText(){
		infoText.text = "GAME OVER";
		anim.SetTrigger ("WaveCompleted");
	}
}
