using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Flashlight : MonoBehaviour, ICharExtra {
    // Effect related fields
    [SerializeField]
    private HDAdditionalLightData lightData;
    private GameObject flashlight;
    private bool flashlightOn = true;
    [SerializeField]
    private Light[] lights;
    private Direction direction;

    // Start is called before the first frame update
    void Start() {
        if (lightData != null)
            flashlight = lightData.gameObject;
    }

    // Update is called once per frame
    void Update() {
        // Turn on flashlight if player pressed button
        if (Input.GetButtonDown("Flashlight"))
            ToggleFlashlight();

        // Rotate flashlight
        if (lightData != null && flashlightOn)
            RotateLight();
    }

    /// <summary>
    /// Rotates the player's flashlight based on the current direction
    /// </summary>
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

    /// <summary>
    /// Turns the flashlight on or off.
    /// </summary>
    public void SetFlashlight(bool setting) {
        Debug.Log("setflashlight");
        flashlightOn = setting;

        foreach (Light light in lights)
            light.gameObject.SetActive(setting);
    }

    /// <summary>
    /// Toggles the flashlight.
    /// </summary>
    /// <returns>Whether or not the flashlight is on.</returns>
    public bool ToggleFlashlight() {
        Debug.Log("toggleflashlight");
        SetFlashlight(!flashlightOn);
        return flashlightOn;
    }

    public void ReceiveData(Vector3 movement, Direction dir) {
        direction = dir;
	}
}
