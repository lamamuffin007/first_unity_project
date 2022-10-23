using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Satrt : MonoBehaviour {



	//флаг кто ходит в данный момент
	//если true - игрок
	//если false - комп
	bool WhoMove = true;


	//режим игры
	//0 - главное меню
	//1 - редактирование карты
	//2 - играем в игру
	//3-4 - результат игры
	//5 - настрйоки
	public int GameMode = 0;
	public GameObject PlayerMap, AiMap, Player;


	//описание вызова графического интерфейса в юнити
	void OnGUI(){

		//кнопки по центру экрана (расчет)
		float CentreScreenX = Screen.width / 2; //по иксу
		float CentreScreenY = Screen.height / 2; // по игрику
		Rect LocationButton;
		Camera cam;
		//получение игрового поля
		Map PlayerMapControl = PlayerMap.GetComponent<Map>();

		//свич для переключения режима игры
		switch (GameMode){

		case 0: //главное меню
			//получение компонента камеры
			cam = GetComponent<Camera> ();
			//задаем дальность камеры
			cam.orthographicSize = 8;
			//задаем координаты для камеры
			this.transform.position = new Vector3 (0, 0, -10);
			//создаем прямоугольник для кнопок
			LocationButton = new Rect (new Vector2 (CentreScreenX - 150, CentreScreenY - 50), new Vector2 (300, 200));
			//рисование прямоугольника ^^^
			GUI.Box (LocationButton, "Морской бой");
	
			//кнопка запуска игры
			LocationButton = new Rect (new Vector2 (CentreScreenX - 100, CentreScreenY - 30 ), new Vector2 (200, 30));
			if (GUI.Button (LocationButton, "Старт")) //если нажал то начало игры
				GameMode = 1;

			//кнопка запуска игры
			LocationButton = new Rect (new Vector2 (CentreScreenX - 100, CentreScreenY + 20), new Vector2 (200, 30));
			if (GUI.Button (LocationButton, "Рекорды")) //если нажал то начало игры
				GameMode = 6;

			//кнопка настроек
			LocationButton = new Rect (new Vector2 (CentreScreenX - 100, CentreScreenY + 60), new Vector2 (200, 30));
			if (GUI.Button (LocationButton, "Настройки")) //если нажал то начало игры
				GameMode = 5;
			
			//кнопка выхода
			LocationButton = new Rect (new Vector2 (CentreScreenX - 100, CentreScreenY + 100), new Vector2 (200, 30));
			if (GUI.Button (LocationButton, "Выход")) //если нажал то выход из игры
				Application.Quit();

			break;
		case 1: //редактирование карты
			//получение компонента камеры
			cam = GetComponent<Camera> ();
			//задаем дальность камеры
			cam.orthographicSize = 8;

			//задаем координаты для камеры
			this.transform.position = new Vector3 (30, 0, -10);

			LocationButton = new Rect (new Vector2 (CentreScreenX - 680, CentreScreenY - 400), new Vector2 (200, 80));
			//вернуться в меню
			if (GUI.Button(LocationButton, "Вернуться в меню")){ PlayerMapControl.ClearMap(); GameMode = 0; }

			//кнопка вызова генератора кораблей
			LocationButton = new Rect (new Vector2 (CentreScreenX - 680, CentreScreenY - 240), new Vector2 (200, 80));
			//если кнопка нажата - вызов генера
			if (GUI.Button(LocationButton, "Разместить флот"))
				//размещаем флот
				PlayerMapControl.RndPlaceShip();
			if (PlayerMapControl.AliveShip() == 18){
				//переходим в игру
				LocationButton = new Rect (new Vector2 (CentreScreenX + 400, CentreScreenY + 240), new Vector2 (300, 150));
				if (GUI.Button(LocationButton, "Старт")) 
				{
					GameMode = 2;
					PlayerMap.GetComponent<Map> ().CopyPole ();//копируем поле 
					AiMap.GetComponent<Map> ().RndPlaceShip ();//рандомное поля для комплюдатера
				};

			}

			break;
		case 2: //поле с игрой
			//задаем координаты для камеры
			this.transform.position = new Vector3 (70, 0, -10);
			//получение компонента камеры
			cam = GetComponent<Camera> ();
			//задаем дальность камеры
			cam.orthographicSize = 9;

			LocationButton = new Rect (new Vector2 (CentreScreenX - 100, 50), new Vector2 (200, 80));
			//вернуться в меню
			if (GUI.Button(LocationButton, "Вернуться в главное меню")){ PlayerMapControl.ClearMap(); GameMode = 0; }


			break;
		case 3: //победа
			//задаем координаты для камеры
			this.transform.position = new Vector3 (30, 40, -10);
			LocationButton = new Rect (new Vector2 (CentreScreenX - 100, 100), new Vector2 (200, 80));
			//вернуться в меню
			if (GUI.Button(LocationButton, "Вернуться в главное меню")){ PlayerMapControl.ClearMap(); GameMode = 0; }
			break;
		case 4: // ты лось
			//получение компонента камеры
			cam = GetComponent<Camera> ();
			//задаем дальность камеры
			cam.orthographicSize = 8;

			//задаем координаты для камеры
			this.transform.position = new Vector3 (30, 20, -10);

			LocationButton = new Rect (new Vector2 (CentreScreenX - 100, 100), new Vector2 (200, 80));
			//вернуться в меню
			if (GUI.Button(LocationButton, "Вернуться в главное меню")){ PlayerMapControl.ClearMap(); GameMode = 0; }

			break;
		case 5: //настройки
			//получение компонента камеры
			cam = GetComponent<Camera> ();
			//задаем дальность камеры
			cam.orthographicSize = 8;
			//задаем координаты для камеры
			this.transform.position = new Vector3 (0, -25, -10);

			LocationButton = new Rect (new Vector2 (CentreScreenX - 100, 50), new Vector2 (200, 80));
			//вернуться в меню
			if (GUI.Button(LocationButton, "Вернуться в главное меню")){ PlayerMapControl.ClearMap(); GameMode = 0; }

			break;
		case 6://рейтинг
			//получение компонента камеры
			cam = GetComponent<Camera> ();
			//задаем дальность камеры
			cam.orthographicSize = 8;
			//задаем координаты для камеры
			this.transform.position = new Vector3 (-30, 0, -10);

			LocationButton = new Rect (new Vector2 (CentreScreenX - 100, 50), new Vector2 (200, 80));
			//вернуться в меню
			if (GUI.Button(LocationButton, "Вернуться в главное меню")){ PlayerMapControl.ClearMap(); GameMode = 0; }

			break;

		}
	}

	//функция выбивания кораблей игрока для ai
	Map.TestCor Kill(){

		Map.TestCor XY;
		XY.X = -1; XY.Y = -1;
		//перебор всех кораблей и смотрим палубы
		foreach(Map.Ship Test in Player.GetComponent<Map>().ListShip){
			//перебор палуб на попадание
			foreach(Map.TestCor Paluba in Test.ShipCoord){
				//смотрим индекс палубы
				int index = Player.GetComponent<Map>().GetIndexBlock(Paluba.X, Paluba.Y); 
				if (index == 1)	//если индекс = 1
								//возвращаем координаты палубы
					return Paluba;
			}
		}
		return XY; //если перебрали и нет ни одной подходящей то -1 -1 вернем

	}

	int ShootCount = 0;

	//базовый AI
	void AI(){
		//можем ли мы ходить
		if (!WhoMove) {
			//если палуб больше половины то играем по простому
			//стреляем на угад
			//получаем X
			int ShootX = Random.Range(0, 9);
			//получаем Y
			int ShootY = Random.Range(0, 9);

			//раздел в котором смотрим сколько игрок убил кораблей.палуб
			int Ai_Ship = AiMap.GetComponent<Map>().AliveShip();
			if (Ai_Ship < 6) {
				if (ShootCount == 0) {
					//убивает ваще все и всех
					//стреляем по палубе
					Map.TestCor XY = Kill();

					if ((XY.X >= 0) && (XY.Y >= 0)) {

						ShootX = XY.X;
						ShootY = XY.Y;

					}
					ShootCount++;
				} else {
				
					ShootCount = 0;
				}
			}

			//проверяем если мы попали, то ход за собой иначе отдаем ход игроку
			WhoMove = !Player.GetComponent<Map>().Shoot(ShootX, ShootY);

		}

	}

	public void UserClick(int X, int Y){
		//Debug.Log ("Click");

		//если не выиграл и ходит игрок то ходим
		if(WhoMove){
			//ходит игрок, если попал то функция вернут правду и ходит игрок
			//если промахнулся то ход за компом
			WhoMove = AiMap.GetComponent<Map>().Shoot(X, Y);


		}


	}
	void TestWhoWin(){
		//проверка сколько палуб у AI
		int Ai_Ship = AiMap.GetComponent<Map>().AliveShip();
		//сколько палуб у игрока
		int P_Ship = Player.GetComponent<Map>().AliveShip();

		//AI лось если
		if (Ai_Ship == 0) GameMode = 3;

		//Игрок лось если
		if (P_Ship == 0) GameMode = 4;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//если играем то проверяем проверку на выигрыш
		if(GameMode == 2){
			TestWhoWin ();
			//если ход ПК, то ходим
			AI();

		}

	}
}
