using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour {

	public GameObject 	healthbar, //блок хранения внешнего виа
						Map; //функция получения от поля сколько палуб живо


	//панель отображения количества живых палуб на поле
	GameObject[] barhealth = new GameObject[18];

	//функция создания панели здоровья
	void Createbarhealth(){
	//получение точки в которой будет создано поле
		Vector3 GetPositionScreen = this.transform.position;
		//смещение относительно точки создания поля из за уменьшения картинки для здоровья
		float DX = 0.5f;

		//цикл создания 18 блоков
		for (int i = 0; i < 18; i++){
			//создание 1 ячейки здоровья
			barhealth[i] = Instantiate(healthbar) as GameObject;
			//задаем позицию
			barhealth[i].transform.position = GetPositionScreen;
			//смещение на указанное расстояние
			GetPositionScreen.x += DX;
		}
	}

	//функция обновления поля
	void RefreshHealth(){
		int l = 0; //количество живых кораблей

		//обнуление всей полоски здоровья
		for (int i = 0; i < 18; i++) barhealth[i].GetComponent<BGS>().index = 0;
		//получение из поля сколько живых кораблей-палуб кораблей
		if (Map !=null) l = Map.GetComponent<Map>().AliveShip();
		//запись количества живых палуб или кораблей в полоску здоровья
		for (int i = 0; i < l; i++) barhealth[i].GetComponent<BGS>().index = 1;
	}


	// Use this for initialization
	void Start () {
		if (healthbar != null) Createbarhealth();
	}
	
	// Update is called once per frame
	void Update () {
		if ((Map != null) && (healthbar != null))	RefreshHealth ();
	}
}
