using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ShootMania : MonoBehaviour
{
	public GameObject Crosshair;
	public Image CrosshairImage;
	public float hitForce;
	public Camera cam;
	public GameObject BloodSplatter;

	[Header ("Crosshair Colors")]
	public Color canShootColor;
	public Color cannotShootColor;

	public float hitReloadTime = 0.2f;
	public float missReloadTime = 1.5f;
	public bool canShoot = true;

	[Header ("Feedback")]
	public float freezeFrameTime = 0.05f;
	private EnvironmentFeedbackManager enviroFeedbackManager;
	// Use this for initialization
	void Start ()
	{
		CrosshairImage.DOKill (true);
		CrosshairImage.DOColor (canShootColor, 0.1f);
		enviroFeedbackManager = GameObject.Find ("MANAGERS").GetComponent<EnvironmentFeedbackManager> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Shooting ();
		/*if (Input.GetKeyDown (KeyCode.A)) {
			CrosshairImage.DOKill (true);
			CrosshairImage.DOColor (cannotShootColor, 0.1f);
		}*/ // Why is this here ?
	}


	IEnumerator ReloadAfterHit ()
	{
		
		Crosshair.transform.DOKill (true);
		Crosshair.transform.DOPunchScale (Vector3.one * 1f, 0.1f, 10, 1f);
		Crosshair.transform.DORotate (Vector3.forward * -360f, hitReloadTime, RotateMode.FastBeyond360);
		CrosshairImage.DOColor (cannotShootColor, 0.1f);
		CrosshairImage.DOKill (true);
		//Debug.Log ("HIIT");
		yield return new WaitForSeconds (hitReloadTime);
		canShoot = true;
		CrosshairImage.DOKill (true);
		CrosshairImage.DOColor (canShootColor, 0.1f);
	}

	IEnumerator ReloadAfterMiss ()
	{
		Crosshair.transform.DOKill (true);
		Crosshair.transform.DOPunchScale (Vector3.one * 0.1f, 0.1f, 10, 1f);
		Crosshair.transform.DORotate (Vector3.forward * -360f, missReloadTime, RotateMode.FastBeyond360);
		CrosshairImage.DOKill (true);
		CrosshairImage.DOColor (cannotShootColor, 0.1f);
		//Debug.Log ("MISSS");
		yield return new WaitForSeconds (missReloadTime);
		canShoot = true;
		CrosshairImage.DOKill (true);
		CrosshairImage.DOColor (canShootColor, 0.1f);
	}

	/*IEnumerator FreezeFrame ()
	{
		Time.timeScale = 0f;
		yield return new WaitForSecondsRealtime (freezeFrameTime);
		Time.timeScale = 1f;
	}*/

	IEnumerator FOVKick()
	{
		float timerLerpe = 0.1f;
		float baseFOV;
		baseFOV = cam.fieldOfView;
		cam.DOFieldOfView (baseFOV - 3f, timerLerpe);
		yield return new WaitForSeconds (timerLerpe);
		cam.DOFieldOfView (baseFOV + 10f, timerLerpe);
		yield return new WaitForSeconds (timerLerpe);
		cam.DOFieldOfView (baseFOV, 0.2f).SetEase(Ease.OutBounce);
	}

	void Shooting ()
	{
		if (Input.GetMouseButtonDown (0) && canShoot) {
			canShoot = false;



			RaycastHit hit;
			Ray ray = Camera.main.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0f));
			if (Physics.Raycast (ray, out hit)) {

				// QUAND LE TIR HIT UN ENNEMI
				if (hit.collider.gameObject.CompareTag ("Enemy")) {
					cam.DOKill (true);
					cam.DOShakeRotation (hitReloadTime, 5f, 50); // Screenshake
					StartCoroutine (ReloadAfterHit ()); // Fast Reload
					Destroy (hit.transform.gameObject);
					StartCoroutine (FOVKick ());// FOV Kick
					enviroFeedbackManager.StartAllFeedbacks (hitReloadTime);// SkyBlink, Freezeframe, radial etc
					//StartCoroutine (FreezeFrame ());//Freeze Frame maintenant lancée dans le script SkyBlink = EnviroFeedbackManager


				} else {
					cam.DOKill (true);
					cam.DOShakeRotation (0.1f, 5f, 50); // Screenshake
					StartCoroutine (ReloadAfterMiss ()); // Slow Reload
					//StartCoroutine (FOVKick ());
				}
				if (hit.rigidbody != null) {
					Instantiate (BloodSplatter, hit.transform.position, Quaternion.identity); // Gib FX
					hit.rigidbody.AddForce (ray.direction * hitForce);

				}

			} else {
				StartCoroutine (ReloadAfterMiss ());
				cam.DOKill (true);
				//cam.DOShakePosition (0.2f, 3f, 1);
				cam.DOShakeRotation (0.1f, 5f, 50);
			}

		}
	}
}
