using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour {
    public UnityEvent interaction;
    private bool isInRange;

    void Update() {
        if (isInRange && !InputMovement.IsPlayerLocked() && Input.GetButtonDown("Interact")) {
            interaction.Invoke();
        }
    }

	public void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            isInRange = true;
            OnScreenControlsUI.UpdateState(ControlState.MapInteract);
        }
	}

	public void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            isInRange = false;
            OnScreenControlsUI.UpdateState(ControlState.Map);
        }
    }
}
