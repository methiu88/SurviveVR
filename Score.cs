using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	private const string DISPLAY_TEXT_FORMAT = "{0}";

	private int scorePoints, counterMultiplier;
	private int waveCounter;
	private Text waveText, scoreText;

	// Use this for initialization
	void Start () {
		scorePoints = 0;
		counterMultiplier = 1;
		waveCounter = 1;
		scoreText = gameObject.transform.GetChild (1).GetComponent<Text> ();
		waveText = gameObject.transform.GetChild (3).GetComponent<Text> ();
	}

	/*	Funzione per aggiungere punti al puntegio	*/
	public void AddPoints(int amount){
		scorePoints += counterMultiplier * amount;
		counterMultiplier++;
		UpdateScore ();
	}
		
	/*	Funzione per resettare il moltiplicatore	*/
	public void ResetMultiplier(){
		counterMultiplier = 1;
	}

	/*	Funzione per recuperare il valore del punteggio	*/
	public int GetScore(){
		return scorePoints;
	}

	/*	Funzione per incrementare il numero dell'ondata	*/
	public void IncWave(){
		scorePoints += waveCounter * 100;
		waveCounter += 1;
		UpdateScore ();
		UpdateWave ();
	}

	/*	Funzione di EndGame	*/
	public void EndGame(){
		CheckHighScore ();
	}

	/*	Confronta il punteggio ottenuto con quello salvato e nel caso lo sostituisce	*/
	private void CheckHighScore(){
		if (scorePoints > PlayerPrefs.GetInt ("BestScore")) {
			PlayerPrefs.SetInt ("BestScore", scorePoints);
			PlayerPrefs.SetInt ("WaveNumber", waveCounter);
		}
	}

	/*	Funzione per aggiornare il punteggio nel campo di testo	*/
	private void UpdateScore(){
		scoreText.text = string.Format (DISPLAY_TEXT_FORMAT, scorePoints);
	}

	/*	Funzione per aggiornare il numero di ondata nel campo di testo	*/
	private void UpdateWave(){
		waveText.text = string.Format(DISPLAY_TEXT_FORMAT , waveCounter);
	}
}
