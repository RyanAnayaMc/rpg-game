using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

#pragma warning disable IDE0051

public class Flashlight : MonoBehaviour, ICharExtra {
    // Effect related fields
    [SerializeField]
    private HDAdditionalLightData lightData;
    private GameObject flashlight;
    private bool flashlightOn = false;
    [SerializeField]
    private Light[] lights;
    private Direction direction;
    bool usingFarCamera;

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
        if (usingFarCamera) {
            switch (direction) {
                case Direction.UP:
                    flashlight.transform.localPosition = new Vector3(0, 0.2f, 1.5f);
                    flashlight.transform.localRotation = Quaternion.Euler(-40, 0, 0);
                    return;
                case Direction.DOWN:
                    flashlight.transform.localPosition = new Vector3(0, 0.15f, -0.3f);
                    flashlight.transform.localRotation = Quaternion.Euler(40, 180, 0);
                    return;
                case Direction.LEFT:
                    flashlight.transform.localPosition = new Vector3(-0.03f, 0.15f, 0f);
                    flashlight.transform.localRotation = Quaternion.Euler(0, -90, -40);
                    return;
                case Direction.RIGHT:
                    flashlight.transform.localPosition = new Vector3(0.03f, 0.15f, 0f);
                    flashlight.transform.localRotation = Quaternion.Euler(0, 90, -40);
                    return;
            }
        } else {
            flashlight.transform.localPosition = new Vector3(0, 0.2f, 1.5f);
            flashlight.transform.localRotation = Quaternion.Euler(-20, 0, 0);
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

    public void ReceiveData(Vector3 movement, Direction dir, bool usingFarCam) {
        direction = dir;
        usingFarCamera = usingFarCam;
	}
}
