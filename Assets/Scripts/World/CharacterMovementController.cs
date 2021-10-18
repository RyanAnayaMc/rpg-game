using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public enum Direction {
    UP = 0,
    DOWN = 1,
    LEFT = 2,
    RIGHT = 3
}


[RequireComponent(typeof(CharacterController))]
public class CharacterMovementController : MonoBehaviour {
    public static bool isPlayerLocked = false;
    public static Vector3 savedLocation;
    public static bool loadSavedLocation;
    [SerializeField]
    private Animator animator;
    public float speed = 5;
    public float sprintModifier = 0.8f;
    public float jumpForce = 1;
    private Direction direction;
    private CharacterController characterController;
    [SerializeField]
    private HDAdditionalLightData lightData;
    private GameObject flashlight;
    [SerializeField]
    private WorldSFXHandler sfxHandler;
    private bool isJumping = false;
    public float upGravity;
    public float downGravity;
    private bool flashlightOn = true;
    private Light[] lights;

    void Start() {
        characterController = GetComponent<CharacterController>();
        if (lightData != null)
            flashlight = lightData.gameObject;

        if (loadSavedLocation) {
            transform.position = savedLocation;
            loadSavedLocation = false;
		}
    }

    void Update() {
        if (isPlayerLocked) {
            animator.SetFloat("speed", 0);
            return;
        }

        // Check if on ground
        if (characterController.isGrounded)
            isJumping = false;

        // Check for sprint
        float sprint = 0;
        if (Input.GetAxis("Sprint") > 0.01)
            sprint = 1;
        else if (Input.GetAxis("Sprint") < -0.01)
            sprint = -0.5f;

        // Check for lateral movement
        float moveX = isPlayerLocked ? 0 : (Input.GetAxis("Horizontal") * speed) * (1 + sprint * sprintModifier);
        float moveZ = isPlayerLocked ? 0 : (Input.GetAxis("Vertical") * speed) * (1 + sprint * sprintModifier);

        float moveY = 0;

        if (isPlayerLocked) {
            moveY = 0;
		} else {
            if (isJumping) {
                Vector3 characterVelocity = characterController.velocity;
                if (Vector3.Dot(characterVelocity, Vector3.up) < 0)
                    moveY = characterVelocity.y - downGravity * Time.deltaTime;
                else
                    moveY = characterVelocity.y - upGravity * Time.deltaTime;
			} else
                moveY = Physics.gravity.y;
		}

        // Check for jump
        if (characterController.isGrounded && Input.GetButton("Jump")) {
            moveY += jumpForce;
            isJumping = true;
        }


        Vector3 movement = new Vector3(moveX, moveY, moveZ);

        characterController.Move(movement * Time.deltaTime);

        // Play sound effect and animation
        direction = getDirection(characterController.velocity);
        if (sfxHandler != null && characterController.velocity.magnitude > 0.5)
            sfxHandler.PlayWalkSFX();

        float magnitude = characterController.velocity.magnitude;
        animator.SetFloat("speed", magnitude);
        animator.SetInteger("direction", (int) direction);

        if (Input.GetButtonDown("Flashlight"))
            ToggleFlashlight();

        // Rotate flashlight
        if (lightData != null && flashlightOn)
            rotateLight();
    }

    void rotateLight() {
        switch (direction) {
            case Direction.UP:
                flashlight.transform.localPosition = new Vector3(0, 1, 0.6f);
                flashlight.transform.localRotation = Quaternion.Euler(0, 0, 0);
                return;
            case Direction.DOWN:
                flashlight.transform.localPosition = new Vector3(0, 1, 0);
                flashlight.transform.localRotation = Quaternion.Euler(0, 180, 0);
                return;
            case Direction.LEFT:
                flashlight.transform.localPosition = new Vector3(-0.37f, 0.5f, 0.6f);
                flashlight.transform.localRotation = Quaternion.Euler(0, -90, 0);
                return;
            case Direction.RIGHT:
                flashlight.transform.localPosition = new Vector3(0.37f, 0.5f, 0.6f);
                flashlight.transform.localRotation = Quaternion.Euler(0, 90, 0);
                return;

        }
	}

    Direction getDirection(Vector3 velocity) {
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.z) + 0.05) {
            // Movement is either left or right
            return velocity.x > 0 ? Direction.RIGHT : Direction.LEFT;
        } else if (Mathf.Abs(velocity.z) > Mathf.Abs(velocity.x) + 0.05) {
            // Movement is forward or back
            return velocity.z > 0 ? Direction.UP : Direction.DOWN;
        } else
            return direction;
	}
    
    /// <summary>
    /// Turns the flashlight on or off.
    /// </summary>
    public void SetFlashlight(bool setting) {
        flashlightOn = setting;
        
        if (lights == null)
            lights = GetComponentsInChildren<Light>();
        foreach (Light light in lights)
            light.gameObject.SetActive(setting);
	}

    /// <summary>
    /// Toggles the flashlight.
    /// </summary>
    /// <returns>Whether or not the flashlight is on.</returns>
    public bool ToggleFlashlight() {
        SetFlashlight(!flashlightOn);
        return flashlightOn;
	}
}
