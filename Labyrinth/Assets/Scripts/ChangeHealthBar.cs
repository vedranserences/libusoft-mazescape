using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeHealthBar : MonoBehaviour {

    private int spriteIndex = 4;
    public Sprite fullHeart;
    public Sprite emptyHeart;



    public void ChangeHBar(int increment) {
        GameObject heart = GameObject.FindGameObjectWithTag("Health"+spriteIndex);
        Image image = heart.GetComponent<Image>();
        if(increment > 0){
            image.sprite = fullHeart;
        }else{
            image.sprite = emptyHeart;
        }
        spriteIndex += increment;
        if(spriteIndex<0){
            GameObject.Find("SceneManager").GetComponent<ChangeScene>().GoToScene("EndScene_01");
        }
    }
}
