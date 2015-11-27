using UnityEngine;
using System.Collections;

public class candyGrid : MonoBehaviour {
	public GameObject[] sweet; 
	public GameObject candies; 
	float spacing= 3; 

	// Use this for initialization
	void Start () {
		for (int i=0; i< 3; i++) {
			for(int j=0;j<3; j++) {
				int randNum= Random.Range (0,3);
				candies= GameObject.Instantiate(sweet[randNum]); 
				candies.transform.position= new Vector3(i*2.0f, j*2.0f, spacing); 
			}
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
