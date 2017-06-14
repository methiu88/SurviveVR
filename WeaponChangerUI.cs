using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChangerUI : MonoBehaviour {

	public Camera camera;

	private UIButton assaultRifleButton, shotgunButton;

	private bool stop;

	private float angleInDegrees;
	private Vector3 rotationAxis;

	Quaternion rotationLast; //Valore della rotazione all'update precedente
	Quaternion rotationDelta; //Differenza di rotazione tra quella attuale e quella precedente

	// Use this for initialization
	void Start () {
		camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera> ();
		shotgunButton = gameObject.transform.GetChild (2).GetComponent<UIButton> ();
		assaultRifleButton = gameObject.transform.GetChild (3).GetComponent<UIButton> ();
		stop = false;
	}

	/*	Funzione che blocca il movimento circolare con la vista del giocatore dell'UIChangerCanvas	*/
	public void StopMovement(bool b){
		if (b)
			stop = true;
		else {
			StartCoroutine(ReleaseMovement (b));
		}
	}

	/*	Funzione per rilasciare il blocco e permettere all'interfaccia di muoversi	*/
	IEnumerator ReleaseMovement(bool b){
		yield return new WaitForSeconds (.1f);
		stop = b;
	}

	/*	Funzione per abilitare il pulsante dello Shotgun	*/
	public void EnableAssaultRilfe(){
		assaultRifleButton.gameObject.SetActive (true);
	}

	/*	Funzione per abilitare il pulsante dell'AssaultRifle	*/
	public void EnableShotgun(){
		shotgunButton.gameObject.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		if (!stop) {
			rotationDelta = camera.transform.rotation * Quaternion.Inverse(rotationLast);
			rotationLast = camera.transform.rotation;
			angleInDegrees = rotationDelta.eulerAngles.y;
			Vector3 angularDisplacement = rotationAxis * angleInDegrees * Mathf.Deg2Rad;
			if(angleInDegrees > 0)
				transform.RotateAround (camera.transform.position, Vector3.up, angleInDegrees);
			else
				transform.RotateAround (camera.transform.position, Vector3.up, -angleInDegrees);
		}
	}
}
