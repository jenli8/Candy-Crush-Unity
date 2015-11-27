using UnityEngine;
using System.Collections;

public class MouseDrag : MonoBehaviour {

	float distance =10f; 
	
	void OnMouseDrag() {
		Vector3 mousePosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, distance); 
		Vector3 objectPosition = Camera.main.ScreenToWorldPoint (mousePosition); 
		transform.position = objectPosition; 
	}
}
