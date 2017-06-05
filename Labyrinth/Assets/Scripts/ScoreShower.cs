using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreShower : MonoBehaviour {

	public static Text scoreText;

	void Start(){
		scoreText = GetComponent<Text>();
		scoreText.text = PointsAndTime.finalScore.ToString("F");
	}
	
}
