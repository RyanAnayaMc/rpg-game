using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour {
    public UnityEvent interaction;
    private Collider interactCollider;
    private bool isInRange;

    // Start is called before the first frame update
    void Start() {
        interactCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update() {
        if (isInRange && !CharacterMovementController.isPlayerLocked && Input.GetButtonDown("Interact")) {
            interaction.Invoke();
            Debug.Log("Invoking");
        }
    }

	public void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
            isInRange = true;
	}

	public void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player"))
            isInRange = false;
	}
}
