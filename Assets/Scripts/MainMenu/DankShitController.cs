using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple script to set a GameObject as active or not based on
/// the DoDankShit options in settings.
/// </summary>
public class DankShitController : MonoBehaviour {
    void Start() {
        gameObject.SetActive(Settings.INSTANCE.doDankShit);
    }
}
