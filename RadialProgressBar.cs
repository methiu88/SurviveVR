using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialProgressBar : MonoBehaviour {

	/*	Barra di progressione della ricarica che si mostra all'utente durante la ricarica dell'arma	*/

	public Transform loadingBar, textIndicator, textLoading;
	[SerializeField] private float currentAmount;
	[SerializeField] private float speed;
	public Gun gun;

	// Use this for initialization
	void Start () {
		speed = 100/gun.reloadTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (gun.IsReloading ()) {
			if (currentAmount < 100) {
				currentAmount += speed * Time.deltaTime;
				textIndicator.GetComponent<Text> ().text = ((int)currentAmount).ToString () + "%";
				textLoading.gameObject.SetActive (true);
			} else {
				textLoading.gameObject.SetActive (false);
				textIndicator.GetComponent<Text> ().text = "DONE!";
			}
			loadingBar.GetComponent<Image> ().fillAmount = currentAmount / 100;
		} else{
			currentAmount = 0;
			loadingBar.GetComponent<Image> ().fillAmount = 0;
		}
	}
}