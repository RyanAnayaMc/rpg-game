using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ControlState {
    None,
    Map,
    MapInteract,
    Dialogue,
    Menu
}

#pragma warning disable IDE0051

public class OnScreenControlsUI : MonoBehaviour {
    public static ControlState state { get { return _state; } }
    private static ControlState _state = ControlState.Map;
    private static Dictionary<ControlState, string> controlsText;

    [SerializeField] private TMP_Text textUI;

	private void Awake() {
		if (controlsText is null) {
            controlsText = new Dictionary<ControlState, string>() {
                { ControlState.None, "" },
                { ControlState.Map, "[Tab] Menu    [Q] Flashlight     [E] Camera    [WASD] Move    [Space] Jump" },
                { ControlState.MapInteract, "[Tab] Menu    [Q] Flashlight     [E] Camera    [WASD] Move    [Space] Jump     [F] Interact"},
                { ControlState.Dialogue, "[F] Next" },
                { ControlState.Menu, "[Tab] Close Menu    [Mouse] Navigate    [Left Click] Select" }
            };
		}
	}

	private void Update() {
        textUI.text = controlsText[_state];
	}

	/// <summary>
	/// Updates the current controls state.
	/// </summary>
	/// <param name="newState">The new state for the controls.</param>
	public static void UpdateState(ControlState newState) {
        _state = newState;
	}
}
