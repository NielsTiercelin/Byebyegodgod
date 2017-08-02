using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour {

	public GameObject[] Floors;
	private int i = 0;
	// Use this for initialization
	void Awake () {
		i = Random.Range (0, Floors.Length);
		Instantiate (Floors [i], Vector3.zero, transform.rotation);
	}

}
