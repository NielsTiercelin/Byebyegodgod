using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Colorful;

public class EnvironmentFeedbackManager : MonoBehaviour {

	public Color skyBlinkColor;
	public Color LevelBlinkColor;
	private Color normalSkyColor;
	public float skyBlinkingTime;

	[Header("FreezeFrame")]
	public float freezeFrameTime = 0.04f;

	//[Header("CameraFX Feedbacks")]
	private RadialBlur radialBlur;


	private Camera cam;
	private GameObject[] LevelFeedbackProps;
	public GameObject[] LevelFeedbackBlink;

	// Use this for initialization
	void Start () {
		normalSkyColor = Camera.main.backgroundColor;
		LevelFeedbackProps = GameObject.FindGameObjectsWithTag("EnvironmentFeedback");
		LevelFeedbackBlink = GameObject.FindGameObjectsWithTag("EnvironmentBlink");
		cam = Camera.main;
		radialBlur = cam.GetComponent<RadialBlur> ();
	}


	public void StartAllFeedbacks(float timerr)
	{
		StartCoroutine (LevelBlink (timerr));
		StartCoroutine (LevelPunchScale (timerr));
		StartCoroutine (SkyBlink(timerr));
		RadialBlurring (timerr);
	}

	void RadialBlurring(float timerr)
	{
		DOTween.To (() => radialBlur.Strength, x => radialBlur.Strength = x, 0.26f, timerr / 2).OnComplete(()=>{ DOTween.To (() => radialBlur.Strength, x => radialBlur.Strength = x, 0, timerr / 2);});
	}

	IEnumerator FreezeFrame (float timerr)
	{
		//Lancé à l'apex du LevelPunchScale
		Time.timeScale = 0f;
		yield return new WaitForSecondsRealtime (freezeFrameTime);
		Time.timeScale = 1f;
	}


	public IEnumerator SkyBlink(float timerr)
	{
		Camera.main.DOColor (skyBlinkColor, timerr / 2);
		yield return new WaitForSeconds (timerr / 2);
		Camera.main.DOColor (normalSkyColor, timerr / 2);
	}



	IEnumerator LevelPunchScale(float timerr)
	{
		foreach (GameObject lvl in LevelFeedbackProps)
		{
			Vector3 baseScale = lvl.transform.localScale;
			lvl.transform.DOScale(baseScale +(Vector3.up * Random.Range (5F, 37f) + (Vector3.right + Vector3.forward) * Random.Range (.2f, 1.2f)),timerr/2).OnComplete(()=> {
				StartCoroutine (FreezeFrame (timerr));
				lvl.transform.DOScale (baseScale, timerr / 2);
			});
		
			//lvl.transform.DOPunchScale (Vector3.up * Random.Range (0.2F, 30f) + (Vector3.right + Vector3.forward) * Random.Range (.2f, 1.2f), timerr, 10);
		}
		yield return null;
	}


	IEnumerator LevelBlink(float timerr)
	{
		foreach (GameObject lvl in LevelFeedbackBlink)
		{
			Material mati;
			mati = lvl.GetComponent<Renderer> ().material;
			Color baseMatColor = mati.GetColor("_EmissionColor");
			mati.DOKill (true);
			mati.DOColor (LevelBlinkColor,"_EmissionColor", timerr / 2).OnComplete(()=>{mati.DOColor (baseMatColor,"_EmissionColor", timerr / 2);});
			//yield return new WaitForSeconds (timerr / 2);
			//mati.DOColor (baseMatColor, timerr / 2);
		}
		yield return null;
	}

}
