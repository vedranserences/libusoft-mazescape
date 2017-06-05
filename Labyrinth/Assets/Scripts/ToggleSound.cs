using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSound : MonoBehaviour {

	public GameObject audioOn;
	public GameObject audioOff;

	private AudioSource music;

	// Use this for initialization
	void Start () {
		music = FindObjectOfType<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void toggleOnAndOff(){
		if(PlayerPrefs.GetInt("Muted", 0) == 0){
			audioOn.SetActive(false);
			audioOff.SetActive(true);
			PlayerPrefs.SetInt("Muted",1);
			AudioListener.volume = 0;
			//music.Stop();
		}else{
			audioOff.SetActive(false);
			audioOn.SetActive(true);
			PlayerPrefs.SetInt("Muted", 0);
			AudioListener.volume = 1f;
			//music.Play();
		}
	}
}
