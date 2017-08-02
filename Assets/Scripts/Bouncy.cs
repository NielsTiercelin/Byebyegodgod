using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bouncy : MonoBehaviour
{

	public float bounceForcemin = 2f;
	public float bounceForceMax = 2f;
	public float bounceTime = 2f;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine (BouncyAnim ());
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	IEnumerator BouncyAnim ()
	{
		gameObject.transform.DOKill (true);
		gameObject.transform.DOPunchScale (new Vector3 (bounceForcemin, Random.Range (bounceForcemin, bounceForceMax), bounceForcemin), bounceTime, 3, 2);
		yield return new WaitForSeconds (bounceTime + Random.Range (0f, 0.2f));
		StartCoroutine (BouncyAnim ());
	}
}
