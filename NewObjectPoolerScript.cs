using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewObjectPoolerScript : MonoBehaviour {

	public static NewObjectPoolerScript current;
	public GameObject pooledObject;
	public int pooledAmount = 20;
	public bool willGrow = true;

	public List<GameObject> pooledObjects;

	void Awake(){
		current = this;
	}

	// Use this for initialization
	void Start () {

		pooledObjects = new List<GameObject> ();
		for (int i = 0; i < pooledAmount; i++) {
			GameObject obj = (GameObject)Instantiate (pooledObject);
			obj.SetActive (false);
			pooledObjects.Add (obj);
		}
	}

	/*	Funzione per recuperare l'oggetto disabilitato da utilizzare	*/
	public GameObject GetPooledObject(){
		for(int i = 0; i < pooledObjects.Count; i++){
			if (!pooledObjects [i].activeInHierarchy) {
				return pooledObjects [i];
			}
		}
		if (willGrow) {
			GameObject obj = (GameObject)Instantiate (pooledObject);
			pooledObjects.Add(obj);
			return obj;
		}
		return null;
	}
}
