using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TextMeshBehavior : MonoBehaviour {

	public float scalingTimer = 0.3f;
	public float LifeTime = 3f;
	private Vector3 endScale;
	// Use this for initialization
	void Start () {
		endScale = new Vector3 (-0.5f, 0.5f, 0.5f);
		transform.LookAt (Camera.main.transform);
		gameObject.transform.DOScale (Vector3.zero, 0.01f);
		gameObject.transform.DOKill (true);
		//gameObject.transform.DOScale (new Vector3 (-0.5f, 0.5f, 0.5f), scalingTimer).OnComplete(()=>{gameObject.transform.DOPunchScale(Vector3.one*1f,0.2f,10).SetEase(Ease.OutBounce);});
		gameObject.transform.DOScale (endScale * 2f, 0.1f).OnComplete (() => {
			gameObject.transform.DOScale (endScale, 0.2f).SetEase (Ease.OutBounce);
		});
		StartCoroutine (DieAfterTime ());

	}

	void Update()
	{
		transform.LookAt (Camera.main.transform);
	}
	IEnumerator DieAfterTime()
	{
		yield return new WaitForSeconds (0.3f);
		gameObject.transform.DOKill (true);
		gameObject.transform.DOScale (Vector3.zero, LifeTime).SetEase(Ease.OutCirc).OnComplete(()=>{Destroy(this.gameObject);});

	}
}
