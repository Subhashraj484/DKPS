using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour {

	[Range(10,60)]
	public int zoom = 20;
	[Header("Sight size in height")]
	public int height = 40;
	[Header("Sight size in width")]
	public int width = 40;

	private int smoothZoom = 10;
	private int normal = 60;
	private float isZoomed = 0;

	void Update()
	{
		if (Input.GetMouseButton (1))
		{
	
			isZoomed = 1;
		}
		if (isZoomed == 1) {
			GetComponent<Camera> ().fieldOfView = Mathf.Lerp (GetComponent<Camera> ().fieldOfView, zoom, Time.deltaTime * smoothZoom); 
		}
		if (Input.GetMouseButtonUp (1))
		{

			isZoomed = 0;
		}
		if(isZoomed == 0)
		{
			GetComponent<Camera> ().fieldOfView = Mathf.Lerp (GetComponent<Camera> ().fieldOfView, normal, Time.deltaTime * smoothZoom);
		}
	}
}
