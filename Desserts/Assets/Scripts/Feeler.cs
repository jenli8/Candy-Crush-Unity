using UnityEngine;
using System.Collections;

public class Feeler : MonoBehaviour {

	public Candy Owner; 

	void OnTriggerEnter(Collider c) {
		if (c.tag == "Candy") {
			Owner.AddNeighbor(c.GetComponent<Candy>()); 
		}
	}

	void OnTriggerExit(Collider c) {
		if (c.tag == "Candy") {
			Owner.RemoveNeighbor (c.GetComponent<Candy> ()); 
		}
	}
}
