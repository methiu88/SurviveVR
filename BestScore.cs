using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestScore : MonoBehaviour {

	private const string DISPLAY_TEXT_FORMAT = "{0}";

	private Text bestScore, waveNumber;

	/* Best Score nel Panel. Mostra i valore salvati */
	void Start () {
		waveNumber = gameObject.transform.GetChild (3).GetComponent<Text> ();
		bestScore = gameObject.transform.GetChild (5).GetComponent<Text> ();
		waveNumber.text = string.Format(DISPLAY_TEXT_FORMAT, PlayerPrefs.GetInt ("WaveNumber"));
		bestScore.text = string.Format(DISPLAY_TEXT_FORMAT, PlayerPrefs.GetInt ("BestScore"));
	}

}
