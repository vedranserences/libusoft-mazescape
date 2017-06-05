using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayOn : MonoBehaviour {

    static int instance = 0;
    private static Object obj;

    void Awake() {
        Debug.Log("Music Player Awake " + GetInstanceID());
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name.Contains("Game")) {
            Destroy(obj);
            instance = 0;
        } else {

            if (instance == 0) {
                instance++;
                obj = gameObject;
                GameObject.DontDestroyOnLoad(gameObject);

            } else {
                print("Duplicate self-destructed " + GetInstanceID());
                Destroy(gameObject);
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
