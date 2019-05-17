using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	private TurnController controller;
	public float minZoom;
	public float maxZoom;
	public float zoomSpeed;

	private Vector3 minBounds;
	private Vector3 maxBounds;
	private Vector3 relativePos;
	private Vector3 centerPos;

	private Vector3 velocity;
	public float smoothTime = 0.5f;

	private bool lookAround = false;

	private Camera cam;

	private List<GameObject> characters;

	// Use this for initialization
	void Start () {
		controller = TurnController.GetInstance();
		controller.NotifyCharactersChange += UpdateCharacters;
		Debug.Log("Camera registered to TurnController");
		characters = controller.GetActiveCharacters();
		relativePos = Vector3.zero;
		centerPos = Vector3.zero;
		cam = GetComponentInChildren<Camera>();
		//TODO event to add and remove enemies and allies
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (Input.GetMouseButtonDown(2))
		{
			lookAround = !lookAround;
		}
		SetBounds();
		SetCenterPos();
		if (lookAround)
		{
			MoveCamera();
		}
		else
		{
			FollowCharacters();
		}
		Zoom();
	}

	private void MoveCamera()
	{
		float motionX = Input.GetAxis("Mouse X");
		float motionZ = Input.GetAxis("Mouse Y");
		//TODO y

		//Vector3 reset = transform.position;
		transform.Translate(motionX, 0, motionZ, Space.Self);
		Vector3 relative = transform.position;

        if (relative.x < minBounds.x)
        {
            transform.position = new Vector3(minBounds.x, relative.y, relative.z);
        }
        else if (relative.x > maxBounds.x)
        {
            transform.position = new Vector3(maxBounds.x, relative.y, relative.z);
        }

        if (relative.y < minBounds.y)
        {
            transform.position = new Vector3(relative.x, minBounds.y, relative.z);
        }
        else if (relative.y > maxBounds.y)
        {
            transform.position = new Vector3(relative.x, maxBounds.y, relative.z);
        }

        if (relative.z < minBounds.z)
        {
            transform.position = new Vector3(relative.x, relative.y, minBounds.z);
        }
        else if (relative.z > maxBounds.z)
        {
            transform.position = new Vector3(relative.x, relative.y, maxBounds.z);
        }


        /*if (relative.x < minBounds.x || relative.x > maxBounds.x || relative.z < minBounds.z || relative.z > maxBounds.z)
		{
			transform.position = reset;
		}
        */
		relativePos = transform.position - centerPos;
    }

	void FollowCharacters()
	{
		Vector3 newPosition = centerPos + relativePos;
		if (newPosition.x < minBounds.x) newPosition.x = minBounds.x;
		if (newPosition.x > maxBounds.x) newPosition.x = maxBounds.x;
		if (newPosition.z < minBounds.z) newPosition.z = minBounds.z;
		if (newPosition.z > maxBounds.z) newPosition.z = maxBounds.z;
        if (newPosition.y < minBounds.y) newPosition.y = minBounds.y;
        if (newPosition.y > maxBounds.y) newPosition.y = maxBounds.y;
        //relativePos = transform.position - centerPos;
        transform.position = newPosition;
		//Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
		//TODO calculate offset to fit in all characters
	}

	void Zoom()//TODO move the camera backwards, so each character is visible
	{
		float zoomChange = Input.GetAxis("Mouse ScrollWheel");
		if (zoomChange != 0)
		{
			cam.fieldOfView -= zoomChange * zoomSpeed;
			if (cam.fieldOfView < minZoom) cam.fieldOfView = minZoom;
			if (cam.fieldOfView > maxZoom) cam.fieldOfView = maxZoom;
		}
	}

	public void SetCenterPos()
	{
		//Maybe we should not use a new bounds each frame
		Bounds bounds = new Bounds();

		foreach (GameObject character in characters)
		{
			bounds.Encapsulate(character.transform.position);
		}
		centerPos = bounds.center;
	}

	public void UpdateCharacters(List<GameObject> newCharacters)
	{
		Debug.Log("Characters updated");
		characters = newCharacters;
	}

	public void SetBounds()
	{
		Vector3 firstPos = characters[0].transform.position;
		float minx = firstPos.x, maxx = firstPos.x;
		float miny = firstPos.y, maxy = firstPos.y;
		float minz = firstPos.z, maxz = firstPos.z;
		foreach (GameObject character in characters)
		{
			Vector3 pos = character.transform.position;
			if (pos.x < minx) minx = pos.x;
			if (pos.y < miny) miny = pos.y;
			if (pos.z < minz) minz = pos.z;

			if (pos.x > maxx) maxx = pos.x;
			if (pos.y > maxy) maxy = pos.y;
			if (pos.z > maxz) maxz = pos.z;
		}
		minBounds = new Vector3(minx, miny, minz);
		maxBounds = new Vector3(maxx, maxy, maxz);
	}
	
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(minBounds, new Vector3(minBounds.x, minBounds.y, maxBounds.z));
		Gizmos.DrawLine(minBounds, new Vector3(maxBounds.x, minBounds.y, minBounds.z));
		Gizmos.DrawLine(maxBounds, new Vector3(minBounds.x, minBounds.y, maxBounds.z));
		Gizmos.DrawLine(maxBounds, new Vector3(maxBounds.x, minBounds.y, minBounds.z));
		Gizmos.DrawSphere(transform.position, 0.1f);
	}
}
