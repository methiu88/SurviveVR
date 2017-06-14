using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickedUpInfoText : MonoBehaviour {

	private const string DISPLAY_INFO_TEXT_FORMAT = "Picked Up {0}";	// Formato di testo predefinito per scrivere su testo
	private Text infoText;		// Oggetto di testo a cui viene associato il riferimento al componente di testo da cambiare
	private Animator anim;		// Animatore a cui viene associato il componente animatore che gestisce le animazioni per il testo

	// Use this for initialization
	void Start () {
		infoText = gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
		anim = gameObject.GetComponent<Animator> ();
	}

	/*	Per mostrare a schermo il testo sullo score	*/
	public void SetScoreText(){
		infoText.text = "SCORE";
		anim.SetTrigger ("Show");
	}

	/*	Per mostrare il testo nel formato scelto	*/
	public void SetInfoText(string name){
		infoText.text = string.Format (DISPLAY_INFO_TEXT_FORMAT, name);
		anim.SetTrigger ("Show");
	}

	// Update is called once per frame
	void Update () {

	}
}
