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
    private ICharExtra[] extras;

    [SerializeField]
    private WorldSFXHandler sfxHandler;


    [SerializeField] IMovement moveHandler;

    void Start() {
        characterController = GetComponent<CharacterController>();
        moveHandler = GetComponent<IMovement>();
        extras = GetComponents<ICharExtra>();
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

        // Handle extras
        foreach (ICharExtra extra in extras)
            extra.ReceiveData(moveDirection, direction);
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
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.z) + 0.05) {
            // Movement is either left or right
            return velocity.x > 0 ? Direction.RIGHT : Direction.LEFT;
        } else if (Mathf.Abs(velocity.z) > Mathf.Abs(velocity.x) + 0.05) {
            // Movement is forward or back
            return velocity.z > 0 ? Direction.UP : Direction.DOWN;
        } else
            return direction;
	}
    

}
