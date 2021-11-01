using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for different ways to move around a character.
/// </summary>
public interface IMovement {
	/// <summary>
	/// Gets the direction to move the character this frame.
	/// </summary>
	/// <param name="currentVelocity">The character's current velocity.</param>
	/// <param name="isGrounded">Whether or not the character is currently on the ground.</param>
	/// <returns>The direction to move this frame.</returns>
	public Vector3 GetMovement(Vector3 currentVelocity, bool isGrounded);
}
