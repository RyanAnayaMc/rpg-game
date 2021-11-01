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
[RequireComponent(typeof(IMovement))]
public class CharacterMovementController : MonoBehaviour {
    // Animation related fields
    [SerializeField]
    private Animator animator;
    private Direction direction;
    private CharacterController characterController;

    // Effect related fields
    [SerializeField]
    private HDAdditionalLightData lightData;
    private GameObject flashlight;
    [SerializeField]
    private WorldSFXHandler sfxHandler;
    private bool flashlightOn = true;
    private Light[] lights;

    [SerializeField] IMovement moveHandler;

    void Start() {
        characterController = GetComponent<CharacterController>();
        moveHandler = GetComponent<IMovement>();
        if (lightData != null)
            flashlight = lightData.gameObject;
    }

	private void FixedUpdate() {
        // Get movement direction
        Vector3 moveDirection = moveHandler.GetMovement(characterController.velocity, characterController.isGrounded);

        // Set animator parameter
        animator.SetFloat("speed", moveDirection.magnitude);

        // Move the character
        characterController.Move(moveDirection * Time.deltaTime);

        // Play sound effect and animation
        PlayWalkSoundAndAnimation();

        // Handle character flashlight
        if (Input.GetButtonDown("Flashlight"))
            ToggleFlashlight();

        // Rotate flashlight
        if (lightData != null && flashlightOn)
            RotateLight();
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

    void RotateLight() {
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

    Direction GetDirection(Vector3 velocity) {
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
