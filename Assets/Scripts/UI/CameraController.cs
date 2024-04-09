using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	[SerializeField] private Camera myCamera;

	[SerializeField] private float moveSpeed = 5.0f;
	[SerializeField] private float smoothSpeed = 2.0f;

	[SerializeField] private float zoomSpeed = 10.0f;

	private Vector3 targetPosition;
	private float targetOrthoSize;

	private void Start() {
		
		targetOrthoSize = myCamera.orthographicSize;
	}

	void Update() {

		// movement
        Vector3 dirVector = Vector3.zero;

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            dirVector.x = -1;
        }
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			dirVector.x = 1;
		}
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
			dirVector.y = 1;
		}
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
			dirVector.y = -1;
		}

		targetPosition = transform.position + dirVector * moveSpeed * Time.deltaTime;

		
		// zooming in

		if(Input.GetKey(KeyCode.Z)) {
			targetOrthoSize = targetOrthoSize - zoomSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.X)) {
			targetOrthoSize = targetOrthoSize + zoomSpeed * Time.deltaTime;
		}


	}

	private void LateUpdate() {
			
		transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

		myCamera.orthographicSize = Mathf.Lerp(myCamera.orthographicSize, targetOrthoSize, smoothSpeed * Time.deltaTime);
	}
}
