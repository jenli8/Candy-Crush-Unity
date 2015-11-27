using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	public List<Candy> candies= new List<Candy>(); 
	public int gridWidth; 
	public int gridHeight; 
	public GameObject candyPrefab;
	public Candy lastCandy; 
	public Vector3 cand1Start, cand1End, cand2Start, cand2End; 
	public bool isSwapping= false; 
	public bool swapBack;
	public Candy cand1, cand2; 
	public float startTime; 
	public float swapRate=2; 
	public int amountToMatch= 3; 
	public bool isMatched= false;
	public static int score = 0;

	// Use this for initialization
	void Start () {
		for (int i=0; i< gridHeight; i++) {
			for( int x=0; x<gridWidth; x++) {
				GameObject c= Instantiate(candyPrefab, new Vector3(i*2.0f,x,0), Quaternion.identity) as GameObject; 
				candies.Add(c.GetComponent<Candy>()); 
			}
		}

	
	}
	
	// Update is called once per frame
	void Update () {
		if (isMatched) {
			for (int i = 0; i < candies.Count; i++) {
				if (candies [i].isMatched) {
					candies [i].createCandy ();
					candies [i].transform.position = new Vector3 (
						candies [i].transform.position.x,
						candies [i].transform.position.y + 6,
						candies [i].transform.position.z);
					score = score + 100;
				}
			}
			isMatched = false;
		} else if (isSwapping) {
			moveCand (cand1, cand1End, cand1Start); 
			moveCand (cand2, cand2End, cand2Start); 
			if (Vector3.Distance (cand1.transform.position, cand1End) < 0.1f || Vector3.Distance (cand2.transform.position, cand2End) < 0.1f) {
				cand1.transform.position = cand1End; 
				cand2.transform.position = cand2End; 
				lastCandy = null; 
				isSwapping = false; 
				togglePhysics (false); 
				if (!swapBack) {
					cand1.toggleSelector ();  
					cand2.toggleSelector (); 
					checkMatch ();
				} /*else {
					swapBack = false;
				} */
			}
		} else if (!determineBoardState ()) {
			for (int i = 0; i < candies.Count; i++) {
				checkForNearbyMatches (candies [i]);
			}
		}
	}

	public bool determineBoardState() {
		for(int i = 0; i < candies.Count; i++) {
			if (candies[i].transform.localPosition.y > 4) {
				return true;
			}
			else if (candies[i].GetComponent<Rigidbody>().velocity.y > 1f) {
				return true;
			}
		}
		return false;
	}

	public void checkMatch() {
		List<Candy> cand1List = new List<Candy> (); 
		List<Candy> cand2List = new List<Candy> ();
		constructMatchList (cand1.color, cand1, cand1.XCoord, cand1.YCoord, ref cand1List);
		fixCandList (cand1, cand1List); 
		constructMatchList (cand2.color, cand2, cand2.XCoord, cand2.YCoord, ref cand2List); 
		fixCandList (cand2, cand2List); 
		print ("Candies: " + cand1List.Count); 
		if (!isMatched) {
			swapBack = true;
			resetCandies();
		}
	}

	public void resetCandies() {
		cand1Start= cand1.transform.position;
		cand1End= cand2.transform.position; 
		
		cand2Start= cand2.transform.position; 
		cand2End= cand1.transform.position; 
		
		togglePhysics(true); 
		startTime= Time.time; 
		isSwapping= true; 
	}

	public void checkForNearbyMatches(Candy c) {
		List<Candy> candList = new List<Candy> (); 
		constructMatchList (c.color, c, c.XCoord, c.YCoord, ref candList);
		fixCandList (c, candList);
	}

	public void constructMatchList(string color, Candy ca, int XCoord, int YCoord, ref List<Candy> MatchList) {
		if (ca == null) {
			return;
		} 
		else if (ca.color != color) {
			return;
		}
		else if(MatchList.Contains(ca)) {
			return; 
		}

		else{
			MatchList.Add(ca); 
			if (XCoord == ca.XCoord || YCoord == ca.YCoord) {
				foreach(Candy c in ca.Neighbors){
					constructMatchList(color, ca, XCoord, YCoord, ref MatchList); 
				}
			}
		}

	}

	public void fixCandList(Candy ca, List<Candy> listToFix) {

		List<Candy> rows = new List<Candy> (); 
		List<Candy> cols = new List<Candy> ();

		for (int i=0; i<listToFix.Count; i++) {
			if(ca.XCoord == listToFix[i].XCoord) {
				rows.Add(listToFix[i]); 
			}
			if(ca.YCoord == listToFix[i].YCoord) {
				cols.Add(listToFix[i]); 
			}
		}

		if (rows.Count >= amountToMatch) {
			isMatched= true; 
			for(int i=0; i<rows.Count; i++) {
				rows[i].isMatched= true;
			}
		}

		if (cols.Count >= amountToMatch) {
			isMatched= true;
			for(int i=0; i<cols.Count; i++) {
				cols[i].isMatched= true;
			}
		}

	}
	public void moveCand(Candy candyToMove, Vector3 toPos, Vector3 fromPos) {

		Vector3 center = (fromPos + toPos) * 0.5f; 
		center = new Vector3 (0, 0, 0.01f); 
		Vector3 riseRelCenter = fromPos - toPos; 
		Vector3 setRelCenter = toPos - center; 
		float fracComplete = (Time.time - startTime) / swapRate; 
		candyToMove.transform.position = Vector3.Slerp (riseRelCenter, setRelCenter, fracComplete); 
		candyToMove.transform.position += center; 
	}

	public void moveNegCand(Candy candyToMove, Vector3 toPos, Vector3 fromPos){

		Vector3 center = (fromPos + toPos) * 0.5f; 
		center = new Vector3 (0, 0, -.01f); 
		Vector3 riseRelCenter = fromPos - toPos; 
		Vector3 setRelCenter = toPos - center; 
		float fracComplete = (Time.time - startTime) / swapRate; 
		candyToMove.transform.position = Vector3.Slerp (riseRelCenter, setRelCenter, fracComplete); 
		candyToMove.transform.position += center;
	}

	public void togglePhysics(bool isOn) {
		for (int i=0; i<candies.Count; i++) {
			candies[i].GetComponent<Rigidbody>().isKinematic= isOn; 
		}
	}
	public void swapCandy(Candy currentCand) {
		if (lastCandy == null) {
			lastCandy = currentCand; 
		} 
		else if (lastCandy == currentCand) {
			lastCandy= null; 
		}
		else{
			if(lastCandy.isNeighbor(currentCand)){
				cand1Start= lastCandy.transform.position;
				cand1End= currentCand.transform.position; 

				cand2Start= currentCand.transform.position; 
				cand2End= lastCandy.transform.position; 

				togglePhysics(true); 
				startTime= Time.time; 
				cand1= lastCandy; 
				cand2= currentCand; 
				isSwapping= true; 

			}
			else {
				lastCandy.toggleSelector(); 
				lastCandy= currentCand; 
			}
		}
	}
}
