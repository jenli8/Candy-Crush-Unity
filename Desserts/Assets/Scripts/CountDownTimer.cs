using UnityEngine;
using System.Collections;

public class CountDownTimer : MonoBehaviour {
	
	float timeR = 200;
	int points;
	
	// Use this for initialization
	void Start () {
		SetText ();
		points = 0;
	}
	
	// Update is called once per frame
	void Update () {
		timeR -= Time.deltaTime;
		points = Board.score;
		SetText ();
	}
	
	void SetText()
	{
		gameObject.GetComponent<TextMesh>().text = "Points: " + points + " Time Remaining: " + timeR.ToString();
	}
}
