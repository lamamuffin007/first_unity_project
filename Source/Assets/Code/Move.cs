using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

	public GameObject Who = null;
	public int CorX, CorY; //тут координаты
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown(){ //это чето из юнити и оно должно делать чето с нажатиями
	
		if (Who !=null){ //если ссылка есц, то что ты мне сделаешь

			Who.GetComponent<Map>().WhoCliccie(CorX, CorY);

		}

	}
}
