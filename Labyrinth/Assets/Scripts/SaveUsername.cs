using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveUsername : MonoBehaviour {

	private InputField field;
	public static string username;

	// Use this for initialization
	void Start () {
		field = GetComponent<InputField>();
		field.onValueChanged.AddListener(GetData);
	}

	void GetData(string arg0){
		username = arg0;
		Debug.Log(username);
	}
}
