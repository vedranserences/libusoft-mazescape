using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointsAndTime : MonoBehaviour {

	public static float finalTime;
	public static float finalScore;
	private float maxScore=1000;

	private Scene currentScene;

	// Use this for initialization
	void Start () {
		currentScene = SceneManager.GetActiveScene();
	}
	
	// Update is called once per frame
	void Update () {
		finalTime = Time.timeSinceLevelLoad;
		if(currentScene.name.Contains("Easy")){
			finalScore = maxScore* 100 /finalTime;
		} else if (currentScene.name.Contains("Medium")) {
			finalScore = maxScore* 500 /finalTime;
		} else if (currentScene.name.Contains("Hard")) {
			finalScore = maxScore* 5000 /finalTime;
		}
		
	}
}
