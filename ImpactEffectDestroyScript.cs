using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactEffectDestroyScript : MonoBehaviour {

	/*	Effetto particellare creato con lo sparo sugli oggetti colpiti	*/
	void OnEnable(){

		Invoke ("Destroy", 0.1f);
	}

	/*	Attivazione dell'effetto al posto dell'istanziazione	*/
	void Destroy(){
		gameObject.SetActive (false);
	}

	/*	Disattivazione dell'effetto al posto della distruzione	*/
	void OnDisable(){
		CancelInvoke ();
	}
}
