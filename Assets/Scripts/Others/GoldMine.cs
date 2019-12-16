using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMine : MonoBehaviour {

	public float goldLeft = 100;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(goldLeft <= 0){
			gameObject.SetActive(false);
		}
	}
}
