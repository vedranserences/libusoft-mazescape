using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendData : MonoBehaviour {

    public string address;
    public string port;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TaskOnClick()
  {
      try
      {  
          string ourPostData = "{ \"username\": \"" + SaveUsername.username + "\", \"points\": " + ScoreShower.scoreText.text + ", \"time\": " + TimeShower.timeText.text + "}";
          Dictionary<string,string> headers = new Dictionary<string, string>();
          headers.Add("Content-Type", "application/json");
          //byte[] b = System.Text.Encoding.UTF8.GetBytes();
          byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
          ///POST by IIS hosting...
          if(string.IsNullOrEmpty(address)){
              address="bornaivankovic.com";
          }
          if(string.IsNullOrEmpty(port)){
              port="5003";
          }
          var hostname=address+":"+port;
          Debug.Log(ourPostData);
          WWW api = new WWW("http://"+hostname+"/game", pData, headers);
          ///GET by IIS hosting...
          ///WWW api = new WWW("http://192.168.1.120/si_aoi/api/total?dynamix={\"plan\":\"TESTA02\"");
      }
      catch (UnityException ex) { Debug.Log(ex.Message); }
  }
}
