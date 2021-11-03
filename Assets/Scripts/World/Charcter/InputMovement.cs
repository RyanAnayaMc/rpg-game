using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMovement : MonoBehaviour, IMovement {
    /// <summary>
    /// Whether or not the player is able to move.
    /// </summary>
    private static bool isPlayerLocked;

    /// <summary>
    /// The normal walking speed of the player.
    /// </summary>
    [SerializeField] private float speed = 5f;

    /// <summary>
    /// The speed multiplier to apply when the player is sprinting.
    /// </summary>
    [SerializeField] private float sprintModifier = 0.8f;

    /// <summary>
    /// The gravity to apply to the character while ascending.
    /// </summary>
    [SerializeField] private float upGravity = 40f;


    /// <summary>
    /// The gravity to apply to the character while descending.
    /// </summary>
    [SerializeField] private float downGravity = 50f;

    /// <summary>
    /// The vertical force to apply when the player jumps.
    /// </summary>
    [SerializeField] private float jumpForce = 22f;

    /// <summary>
    /// The saved location of the player from the map.
    /// </summary>
    private static Vector3 savedLocation;

    /// <summary>
    /// The loaded location of the player.
    /// </summary>
    private static bool loadSavedLocation;

    private bool isJumping;

    public Vector3 GetMovement(Vector3 currentVelocity, bool isGrounded, bool usingFarCamera) {
        if (loadSavedLocation) {
            transform.position = savedLocation;
            loadSavedLocation = false;
		}

        // If player is locked, just return zeroes
        if (isPlayerLocked)
            return new Vector3(0, 0, 0);

        // Get effective sprint modifier
        float sprint = 0;
        if (Input.GetAxis("Sprint") > 0.01)
            sprint = 1;
        else if (Input.GetAxis("Sprint") < -0.01)
            sprint = -0.5f;

        // Get lateral movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // If player is jumping then apply the movement as a delta to current velocity
        if (isJumping) {
            moveX = currentVelocity.x - moveX * Time.deltaTime * 0.5f;
            moveZ = currentVelocity.z - moveZ * Time.deltaTime * 0.5f;
        }

        // Normalize lateral movement
        float moveXZ = Mathf.Sqrt(moveX * moveX + moveZ * moveZ);
        moveX /= moveXZ;
        moveZ /= moveXZ;

        // Apply real speed modifier
        float realSpeedModifier = (1 + sprint * sprintModifier) * speed;
        moveX *= realSpeedModifier;
        moveZ *= realSpeedModifier;

        // Player is definitely not jumping if they are grounded
        if (isGrounded) isJumping = false;

        // Get vertical movement
        float moveY;
        if (isJumping) {
            Vector3 characterVelocity = currentVelocity;

            if (Vector3.Dot(characterVelocity, Vector3.up) < 0)
                // If player is moving down then apply downward gravity
                moveY = characterVelocity.y - downGravity * Time.deltaTime;
            else
                // If player is moving upward then apply upward gravity
                moveY = characterVelocity.y - upGravity * Time.deltaTime;
        } else
            // Just use default gravity is player is not jumping
            moveY = Physics.gravity.y;

        // Check for jump
        if (isGrounded && Input.GetButton("Jump")) {
            moveY += jumpForce;
            isJumping = true;
        }

        // Ensure that values are numbers
        if (float.IsNaN(moveX))
            moveX = 0;
        if (float.IsNaN(moveZ))
            moveZ = 0;

        return new Vector3(moveX, moveY, moveZ);
    }

    public static void SaveLocation() {
        loadSavedLocation = true;
        savedLocation = GameObject.FindGameObjectWithTag("Player").transform.position;
	}

    /// <summary>
    /// Locks the player's current position.
    /// </summary>
    public static void LockPlayer() {
        isPlayerLocked = true;
	}

    /// <summary>
    /// Unlocks the player's current position.
    /// </summary>
    public static void UnlockPlayer() {
        isPlayerLocked = false;
	}
    
    /// <summary>
    /// Returns whether or not the player is locked.
    /// </summary>
    /// <returns></returns>
    public static bool IsPlayerLocked() {
        return isPlayerLocked;
	}
}
