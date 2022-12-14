using System.Collections;
using System.Collections.Generic; //позволяет работать со списками
using UnityEngine;

public class Map : MonoBehaviour {

	//*****************************
	//ГЛАВНЫЙ СКРИП ИГРЫ(ОН ПРИНИМАЕТ ВСЕ РЕШЕНИЯ)
	public GameObject GMain;
	//*****************************


	public GameObject eLitera, eNumbers, eMain, eMessage;

	//новый обьект в который мы будем копировать данные поля игрока(второе поле для второго кейса в satrt)
	public GameObject CopyMap;

	public bool HideShip = false;

    GameObject[] Litera;  //тут буквы и там преф букв
    GameObject[] Numbers;  //тут цифры
	public GameObject[,] Main;   //а тут квадраты для поля

	int time = 1000, deltatime = 0; //нужно для сообщений
	int MapLen = 10; //тут пока поле 10 на 10 будет
	//надо будет делать чтоб можно было выбрать какой рабер поля


	//структура хранения координат одно-палубного корабля
	public struct TestCor{	public int X, Y; }
	//структура хранения координат всего корабля
	public struct Ship{ public TestCor[] ShipCoord;}
	//Делаем список в котороый заносим все корабле которые расположены на карте
	public List<Ship> ListShip = new List<Ship>();


	//количество кораблей на поле
	public int[] ShipsCount = { 0, 2, 2, 1, 1, 1};
	//тип корабля [* * * * *]-1шт [* * * *]-1шт [* * *]-1шт [* *]-2шт [*]-2шт

	//true если есть корабли
	bool CountShip(){
		//переменная подсчета кораблей
		int Amount = 0;
		//сумма всех значений
		foreach (int Ships in ShipsCount) Amount += Ships;
		//если сумма !=0 то ставим еще
		if (Amount != 0)
			return true;
		//если сумма 0, то не ставим ничего больше
		else
			return false;
	}

	//функция копирования поля 
	public void CopyPole(){
		if (CopyMap != null) {
			//цикл перебора всего поля
			//рисуем по Y
			for (int Y = 0; Y < MapLen; Y++) { 
				//рисуем по X
				for (int X = 0; X < MapLen; X++) {
					CopyMap.GetComponent<Map>().Main[X, Y].GetComponent<BGS>().index = Main[X, Y].GetComponent<BGS>().index;
				}
			}

			//очистка списка от мусора
			CopyMap.GetComponent<Map>().ListShip.Clear();
			//заполнение списка значениями из поля
			CopyMap.GetComponent<Map>().ListShip.AddRange(ListShip); //взятие диапазона из листшипа

		}
	}

	//функция очистки поля
	public void ClearMap(){
	ShipsCount = new int[] {0, 2, 2, 1, 1, 1}; //записываем количесвто кораблей
		ListShip.Clear();//сначала очищение а потом добавление нового
		//цикл отрисовки поля по Y
		for (int Y = 0; Y < MapLen; Y++ ){
			//по X
			for(int X = 0; X < MapLen; X++) {
				Main [X, Y].GetComponent<BGS> ().index = 0;}
		}
	}

	public void RndPlaceShip(){

		ClearMap();
		//номер выбранного корабля
		int SelectShip = 5;//корабль вида [* * * * *] - 1штука
		//координаты по которым ставим корабль
		int X,Y;
		//положение корабля
		int Direction;

		//бесконечный цикл подсчета кораблей
		while(CountShip()){
			//получаем 2 координаты по которым ставится корабль(не игроком)
			X = Random.Range(0, MapLen); //по иксу
			Y =	Random.Range(0, MapLen); //по игрику
			//получаем направление  0 вертик, 1 гориз
			Direction = Random.Range(0,2);

			if (SetShip (SelectShip, Direction, X, Y)) {
				//если корабль поставили, то уменьшаем на один
				ShipsCount [SelectShip]--;
				//если корабли данного типа = 0, то идем в след секцию
				if (ShipsCount[SelectShip]==0){
					//след группа кораблей, сдвиг влево по массиву
					SelectShip--;
				}
			}

		}
	}



	//щас сделаем чтоб поле крафилось
	void CreateMap(){

		Vector3 St = transform.position; //берем из юнити штуку для расчета координат
		float XX = St.x + 1; //тут вправо для букв
		float YY = St.y - 1; //тут вниз для цифр

		Litera = new GameObject[MapLen];
		Numbers = new GameObject[MapLen];

		//тут мы делаем говно для букв в ряд и цифр
		for (int Word = 0; Word < MapLen; Word++){
			Litera [Word] = Instantiate (eLitera);
			Litera [Word].transform.position = new Vector3 (XX, St.y, St.z); //Вектор три для координат хэ у и зэд
			Litera [Word].GetComponent<BGS>().index = Word;
			XX++;

			Numbers [Word] = Instantiate (eNumbers);
			Numbers [Word].transform.position = new Vector3 (St.x, YY, St.z);
			Numbers [Word].GetComponent<BGS> ().index = Word;
			YY--;

		}

		XX = St.x + 1;
		YY = St.y - 1;

		Main = new GameObject[MapLen,MapLen]; //поле с размером из MapLen

		//тут цикл для поля с квадратами и т.д
		for(int Y = 0; Y < MapLen; Y++){ //вместо 10 потом поставить свое значение когда сделаю функцию изменения размера поля

			for(int X = 0; X < MapLen; X++){

				Main [X, Y] = Instantiate (eMain);
				Main [X, Y].GetComponent<BGS> ().index = 0;

				Main [X, Y].GetComponent<BGS> ().HideBGS = HideShip;

				Main [X, Y].transform.position = new Vector3 (XX, YY, St.z);
				if (HideShip)
				Main [X, Y].GetComponent<Move> ().Who = this.gameObject;
				Main [X, Y].GetComponent<Move> ().CorX = X;
				Main [X, Y].GetComponent<Move> ().CorY = Y;



				XX++;
			}
			XX = St.x + 1;
			YY--;
		}

	}
	bool Test(int X, int Y){

		if((X > -1)&&(X < MapLen)&&(Y > -1)&&(Y < MapLen)){ //тут проверка для клеток поля для корабля

			//щас проверка на корабли вокруг 
			// 0 0 0
			// 0 1 0
			// 0 0 0
			int[] XX = new int[9], YY = new int[9] ;

			XX[0] = X + 1;  		XX[1] = X;  		XX[2] = X - 1;
			YY[0] = Y + 1;  		YY[1] = Y + 1;  	YY[2] = Y + 1;
			//--------------------------------------------------------
			XX[3] = X + 1;  		XX[4] = X; 			XX[5] = X - 1;
			YY[3] = Y;  			YY[4] = Y;  		YY[5] = Y;
			//--------------------------------------------------------
			XX[6] = X + 1;  		XX[7] = X;  		XX[8] = X - 1;
			YY[6] = Y - 1;  		YY[7] = Y - 1;  	YY[8] = Y - 1;

			//опять цикл для внешних чтоб нельзя было снаружи поставить

			for(int i = 0; i < 9; i++){

				if((XX[i] > -1)&&(XX[i] < MapLen)&&(YY[i] > -1)&&(YY[i] < MapLen)){
					if(Main[XX[i], YY[i]].GetComponent<BGS>().index !=0) return false;
																			}
									}
			return true;
		}

		return false;
	}


	//проверка на постановку кораблей вверх и влеправо
	//ShipType - тип корабля [* * * * *]-1шт [* * * *]-1шт [* * *]-1шт [* *]-2шт [*]-2шт
	//XP YP проверка позиции по осям
	//X Y координаты начальные



	TestCor[] TestShipPos(int ShipType, int XP, int YP, int X, int Y)
	{
		//массив для результата
		TestCor[] Result = new TestCor[ShipType];
		//идем в указанную сторону
		for(int i = 0; i < ShipType; i++){
			if (Test (X, Y)) {
				//запоминаем результат
				Result[i].X = X; //ну тут хэ
				Result[i].Y = Y; // а тут у
			} else
				//остановка проверки
				return null;
			//смещение проверки
			X += XP; 
			Y += YP;
			
		}
		return Result;
	}

	TestCor[] TestShip(int ShipType, int Dir, int X, int Y){
	
		TestCor[] ResultCor = new TestCor[ShipType];
		if (Test(X, Y)) {
			switch (Dir) {

			case 0:
				ResultCor = TestShipPos(ShipType, 1, 0, X, Y);
				if(ResultCor == null) ResultCor = TestShipPos(ShipType, -1, 0, X, Y);;
				break;
			case 1:
				ResultCor = TestShipPos(ShipType, 0, 1, X, Y);
				if(ResultCor == null) ResultCor = TestShipPos(ShipType, 0, -1, X, Y);;
				break;
			}
			return ResultCor;
		}

		return null;
	}
	//ставим корабли
	bool SetShip(int ShipType, int Dir, int X, int Y){
		TestCor[] i = TestShip (ShipType, Dir, X, Y); //взять координату для ставки корабля
		//ставим корабль
		if (i != null){
			//если в списке есть координа то да
			foreach(TestCor T in i){
				Main [T.X, T.Y].GetComponent<BGS> ().index = 1;
			} 

			Ship Deck;//сохраняем координаты корабля
			Deck.ShipCoord = i;
			ListShip.Add (Deck);//добавление в список как один большой корабль

			return true; //поставили корабль
		}	return false; //не смогли поставить корабль

	}

	//функция получения параметра блока
	public int GetIndexBlock(int X, int Y){

		return Main [X, Y].GetComponent<BGS> ().index;
	}


	//Проверка на попадание/промазывание по кораблю
	public bool Shoot(int X, int Y){
		//обнуление статуса - сообщения о попадании и тд
		if (eMessage != null)
		eMessage.GetComponent<BGS>().index = 0;


		int MapSelect = Main [X, Y].GetComponent<BGS> ().index;//номер картинки
		bool Result = false;
		switch(MapSelect){
		case 0: //промазал
			Main [X, Y].GetComponent<BGS> ().index = 3;
			Result = false;
			//если промазал
			if (eMessage != null) eMessage.GetComponent<BGS>().index = 3;
			break;
		case 1: //попавсь
			Main [X, Y].GetComponent<BGS> ().index = 2;
			Result = true;

			if (TestShoot (X, Y)) {
				//сообщение об убийстве корабля
				eMessage.GetComponent<BGS>().index = 1;
			}
			else {
				//сообщение о попадании в корабль
				if (eMessage != null) eMessage.GetComponent<BGS>().index = 2;
			};

			break;

		} return Result;
	
	}

	bool TestShoot(int X, int Y){ //функция проверка выстрела
		bool Result = false;
		//перебор кораблей в списке и смотрим куда попали
		foreach(Ship Test in ListShip){
			//перебор палуб и проверка попадания в нее
			foreach(TestCor Paluba in Test.ShipCoord){
				//сравнивание координат выстрела с координатами палубы
				if ((Paluba.X == X)&&(Paluba.Y == Y)){
					//обьявление переменной для подсчета количества попаданий
					int CountKill = 0;
					//если игрок попал по кораблю то сколько палуб разрушено в корабле
					foreach (TestCor KillPaluba in Test.ShipCoord) {
						//проверка чего записано в поле по данным координатам
						int TestBlock = Main[KillPaluba.X, KillPaluba.Y].GetComponent<BGS>().index;
						//если там цифра 2, то там палуба мертвая
						if (TestBlock == 2)CountKill++;
					}
					//если количество попаданий = количество палуб -> уничтожен корабль
					if(CountKill == Test.ShipCoord.Length) Result = true; //убит - правда
					else Result = false; // если еще не убит - ложь
					//завершение цикла и возвращение результата
					return Result;
				}
			}
		}

		return Result;
	}

    // Use this for initialization
    void Start () {
		CreateMap();
		if (HideShip) RndPlaceShip();
	}
	
	// Update is called once per frame
	void Update () {
		//сокрываем по прохождении времени сообщения о попаданиях-промахах
		deltatime++;
		if (deltatime > time) {
			if (eMessage != null) eMessage.GetComponent<BGS>().index = 0;
			deltatime = 0;
		}
		
	}

	//штука для нажатия по полю, нужна для Move.cs

	public void WhoCliccie(int X, int Y){

		//if (Test(X, Y)) Main [X, Y].GetComponent<BGS> ().index = 1;
		//SetShip(5, 1, X,Y);
		//Shoot(X, Y);

		//сообщаем главному скрипту что игрок тынул на блок
		if(GMain != null) GMain.GetComponent<Satrt>().UserClick(X, Y);
	}

	//функция возвращения количесвта живых кораблей на карте
	public int AliveShip(){
		int countAlive = 0; //переменная в которой считаем живые корабли

		//перебор кораблей и смотрим сколько жиавых
		foreach(Ship Test in ListShip) {
			//перебор палуб корабля и жив ли он
			foreach(TestCor Paluba in Test.ShipCoord){
				//смотрим что с палубой
				int TestBlock = Main[Paluba.X, Paluba.Y].GetComponent<BGS>().index;
				//если там цифра 1 то палуба живая
				if (TestBlock == 1) countAlive++;
			}
		}

		//возвращаем найденные
		return countAlive;
	}

}
