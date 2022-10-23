using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestS : MonoBehaviour {

	public GameObject MyMap;

	void OnGUI(){
		Rect LocationButton;
		LocationButton = new Rect (new Vector2(10, 10), new Vector2(200, 40));
		if(GUI.Button(LocationButton,"Сгенерировать карту")) MyMap.GetComponent<Map>().RndPlaceShip();
	
		LocationButton = new Rect (new Vector2(10, 50), new Vector2(200, 40));
		if(GUI.Button(LocationButton,"Скопировать карту в")) MyMap.GetComponent<Map>().CopyPole();
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
