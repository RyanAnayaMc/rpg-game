using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BalloonAnimation {
	EXCLAMATION
}

public class BalloonAnimations {
	private static BalloonAnimations _instance;

	public static BalloonAnimations INSTANCE {
		get {
			if (_instance is null)
				_instance = new BalloonAnimations();

			return _instance;
		}
	}

	private Dictionary<BalloonAnimation, GameObject> balloonAnimations;

	#region Prefab Fields
	private GameObject _exclamation;
	#endregion

	private BalloonAnimations() {
		// Get resources from project
		_exclamation = Resources.Load<GameObject>(Paths.EXCLAMATION_PATH);

		// Add them to the dictionary
		balloonAnimations = new Dictionary<BalloonAnimation, GameObject>() {
			{ BalloonAnimation.EXCLAMATION, _exclamation }
		};
	}

	/// <summary>
	/// Performs a balloon animation on the target. Destroys the animation object after teh given time.
	/// </summary>
	/// <param name="animation">The animation to play.</param>
	/// <param name="target">The target transform to display the animation on.</param>
	/// <param name="durationMs">
	/// The duration to keep the animation on screen in miliseconds. If negative, it will remain
	/// on screen until manually destroyed (check return value for object to destroy).
	/// </param>
	/// <returns>The GameObject of the animation.</returns>
	public GameObject DoAnimationOn(BalloonAnimation animation, Transform target, int durationMs = 2000, float offsetX = 0, float offsetY = 0, float offsetZ = 0) {
		GameObject obj = GameObject.Instantiate(balloonAnimations[animation], target);
		obj.transform.localPosition = new Vector3(offsetX, offsetY, offsetZ);
		if (durationMs >= 0)
			_ = Utilities.DoAfter(durationMs, () => GameObject.Destroy(obj));
		return obj;
	}
}
