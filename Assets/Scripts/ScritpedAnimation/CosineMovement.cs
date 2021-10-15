using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosineMovement : MonoBehaviour {
    public float movementOffset = 0;
    public float movementMultiplier = 1;
    public float speedMultiplier = 1;

    public bool moveX;
    public bool moveY;
    public bool moveZ;

    private Vector3 originalPosition;

	private void Start() {
        originalPosition = transform.localPosition;
	}

	void Update() {
        float xVal = originalPosition.x + (moveX ? (movementOffset + Mathf.Cos(Time.time * speedMultiplier) * movementMultiplier) : 0);
        float yVal = originalPosition.y + (moveY ? (movementOffset + Mathf.Cos(Time.time * speedMultiplier) * movementMultiplier) : 0);
        float zVal = originalPosition.z + (moveZ ? (movementOffset + Mathf.Cos(Time.time * speedMultiplier) * movementMultiplier) : 0);

        transform.localPosition = new Vector3(xVal, yVal, zVal);
    }
}
