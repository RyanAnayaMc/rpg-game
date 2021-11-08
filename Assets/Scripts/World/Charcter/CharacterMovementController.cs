using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

#pragma warning disable IDE0051

public enum Direction {
    UP = 0,
    DOWN = 1,
    LEFT = 2,
    RIGHT = 3
}


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(IMovement))]
public class CharacterMovementController : MonoBehaviour {
    // Animation related fields
    [SerializeField]
    private Animator animator;
    private Direction direction;
    private CharacterController characterController;
    private ICharExtra[] extras;

    [SerializeField]
    private WorldSFXHandler sfxHandler;


    private IMovement moveHandler;
    [SerializeField] private Camera closeCamera;
    [SerializeField] private Camera farCamera;
    [SerializeField] private float camSpeedMultiplier = 5f;
    private float closeCamRotation = 0;
    private bool usingFarCamera = true;

    void Start() {
        characterController = GetComponent<CharacterController>();
        moveHandler = GetComponent<IMovement>();
        extras = GetComponents<ICharExtra>();
    }

	private void Update() {
        // Check if player toggled camera
        if (Input.GetButtonDown("Camera"))
            ToggleCamera();
    }

	private void FixedUpdate() {
        // Make sure right camera is being used
        closeCamera.gameObject.SetActive(!usingFarCamera);
        farCamera.gameObject.SetActive(usingFarCamera);

        // Get movement direction
        Vector3 moveDirection = moveHandler.GetMovement(characterController.velocity, characterController.isGrounded, usingFarCamera, !usingFarCamera);

        // Set animator parameter
        animator.SetFloat("speed", moveDirection.magnitude);

        // Move the character
        DoMovement(moveDirection);

        // Play sound effect and animation
        PlayWalkSoundAndAnimation();

        // Handle extras
        foreach (ICharExtra extra in extras)
            extra.ReceiveData(moveDirection, direction, usingFarCamera);
    }

    private void ToggleCamera() {
        if (usingFarCamera) {
            // Transition to close camera
            closeCamera.transform.parent.rotation = Quaternion.Euler(20, 0, 0);
            closeCamRotation = 0;
            usingFarCamera = false;
		} else {
            // Transition to far camera
            closeCamera.transform.parent.rotation = Quaternion.Euler(40, 0, 0);
            usingFarCamera = true;
		}
	}

    /// <summary>
    /// Performs the character's movement.
    /// </summary>
    /// <param name="moveDirection">The direction to move the player.</param>
    private void DoMovement(Vector3 moveDirection) {
        if (usingFarCamera)
            DoMovementFarCamera(moveDirection);
        else
            DoMovementCloseCamera(moveDirection);
	}

    /// <summary>
    /// Moves the character if the far camera is being used.
    /// </summary>
    /// <param name="moveDirection">The direction to move the player.</param>
    private void DoMovementFarCamera(Vector3 moveDirection) {
        characterController.Move(moveDirection * Time.deltaTime);
    }

    /// <summary>
    /// Moves the character if the close camera is used. Automatically performs camera rotation.
    /// </summary>
    /// <param name="moveDirection">The direction to move the player.</param>
    private void DoMovementCloseCamera(Vector3 moveDirection) {
        // X direction movement maps to camera rotation about the Y axis
        Quaternion camRotation = closeCamera.transform.parent.rotation;

        float rX = camRotation.x;
        float rY = closeCamRotation + moveDirection.x * Time.deltaTime * camSpeedMultiplier;
        float rZ = camRotation.z;

        closeCamRotation = rY;

        camRotation = Quaternion.Euler(rX, rY, rZ);
        closeCamera.transform.parent.rotation = camRotation;

        // Save vertical movement variable
        float moveY = moveDirection.y;

        // Take forward and back direction of movement
        moveDirection = new Vector3(0, 0, Mathf.Sqrt(moveDirection.x * moveDirection.x + moveDirection.z * moveDirection.z));
        Debug.Log(moveDirection.ToString());

        // Rotate move vector to match camera rotation
        moveDirection = Quaternion.Euler(0, rY, 0) * moveDirection;
        Debug.Log(moveDirection);

        // Add vertical movement back
        moveDirection.y = moveY;

        // Move the player
        characterController.Move(moveDirection * Time.deltaTime);
    }

    /// <summary>
    /// Plays the walking sound effect and animation if character is walking.
    /// </summary>
    private void PlayWalkSoundAndAnimation() {
        // Play sound effect and animation
        direction = GetDirection(characterController.velocity);
        if (sfxHandler != null && characterController.velocity.magnitude > 0.5)
            sfxHandler.PlayWalkSFX();

        float magnitude = characterController.velocity.magnitude;
        animator.SetFloat("speed", magnitude);
        animator.SetInteger("direction", (int) direction);
    }

    Direction GetDirection(Vector3 velocity) {
        if (usingFarCamera) {
            if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.z) + 0.05) {
                // Movement is either left or right
                return velocity.x > 0 ? Direction.RIGHT : Direction.LEFT;
            } else if (Mathf.Abs(velocity.z) > Mathf.Abs(velocity.x) + 0.05) {
                // Movement is forward or back
                return velocity.z > 0 ? Direction.UP : Direction.DOWN;
            } else
                return direction;
        } else
            return Direction.UP;
	}
    

}
