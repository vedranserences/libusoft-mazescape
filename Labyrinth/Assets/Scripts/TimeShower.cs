using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimeShower : MonoBehaviour {

	public static Text timeText;

	void Start(){
		timeText = GetComponent<Text>();
		timeText.text = PointsAndTime.finalTime.ToString("n2");
	}
	
}
