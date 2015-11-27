using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class Candy : MonoBehaviour {

	public GameObject sphere; 
	public GameObject candyHolder;
	public GameObject selector; 
	string[] candyMat = {"Red", "Orange", "Yellow"}; 
	public string color= ""; 
	public List<Candy> Neighbors= new List<Candy>(); 
	public bool isSelected = false; 
	public bool isMatched= false; 

	public int XCoord {
		get {
			return Mathf.RoundToInt(transform.localPosition.x); 
		}
	}

	public int YCoord {
		get {
			return Mathf.RoundToInt(transform.localPosition.y); 
		}
	}

	// Use this for initialization
	void Start () { 
		createCandy (); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void toggleSelector() {
		isSelected = !isSelected; 
		selector.SetActive (isSelected); 
	}
	public void createCandy() {
		//Destroy(sphere);
		color = candyMat[Random.Range(0,candyMat.Length)]; 
		GameObject candyPrefab = Resources.Load ("Prefabs/" + color) as GameObject;
		sphere = (GameObject) Instantiate(candyPrefab, Vector3.zero, Quaternion.identity);
		sphere.transform.parent = candyHolder.transform;
		sphere.transform.localPosition = Vector3.zero;
		isMatched = false; 
		//Material m = Resources.Load ("Materials/" + color) as Material; 
		//sphere.GetComponent<MeshRenderer> ().material = m; 
		//sphere.renderer.material = m; 

		isMatched = false;
	}
	public void AddNeighbor(Candy ca){
		//if(!Neighbors.Contains(ca))
		Neighbors.Add (ca); 
	}

	public bool isNeighbor(Candy ca) {
		if (Neighbors.Contains (ca)) {
			return true; 
		}
		return false; 
	}
	public void RemoveNeighbor(Candy ca) {
		Neighbors.Remove (ca); 
	}
	void OnMouseDown() {
		if (!GameObject.Find ("Board").GetComponent<Board> ().isSwapping) {
			toggleSelector (); 
			GameObject.Find("Board").GetComponent<Board>().swapCandy(this); 
		}
	}
}
