using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGS : MonoBehaviour {

    public Sprite[] imgs; //массив с буквами
    public int index = 0; //штука для выбора букв из массива

	public bool HideBGS = false;

    void ChangeImg()    { //смена картинки

        if (imgs.Length > index) {

	if ((HideBGS) && (index == 1))	GetComponent<SpriteRenderer>().sprite = imgs[0];
			else
            //тут мы берем компонени из юнити
            GetComponent<SpriteRenderer>().sprite = imgs[index];
        						}
    					}



	// Use this for initialization
	void Start () {

        ChangeImg();
					}
	
	// Update is called once per frame
	void Update () {
        ChangeImg();
   				 }
}
