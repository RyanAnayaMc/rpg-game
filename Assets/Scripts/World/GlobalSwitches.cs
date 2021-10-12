using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSwitches {
	private Dictionary<string, bool> switches;
	private GlobalSwitches _instance;

	/// <summary>
	/// The game's GlobalSwitches instance.
	/// </summary>
	public GlobalSwitches INSTANCE {
		get {
			if (_instance is null)
				_instance = new GlobalSwitches();
			return _instance;
		}
	}

	private GlobalSwitches() {
		switches = new Dictionary<string, bool>();
	}

	/// <summary>
	/// Retreives the value for the supplied switch. Alternative notation:
	/// GlobalSwitches.INSTANCE[name]
	/// </summary>
	/// <param name="name">The name of the switch to get.</param>
	/// <returns>The value of the switch.</returns>
	/// <exception cref="NonExistentSwitchException">If there is no switch with the given name.</exception>
	public bool GetSwitch(string name) {
		if (!switches.ContainsKey(name))
			throw new NonExistentSwitchException(name);
		else
			return switches[name];
	}

	public bool this[string name] {
		get { return GetSwitch(name); }
		set {
			if (switches.ContainsKey(name))
				SetSwitch(name, value);
			else
				CreateSwitch(name, value);
		}
	}

	/// <summary>
	/// Sets a switch to a given value. Alternative notation:
	/// GlobalSwitches.INSTANCE[name] = value
	/// </summary>
	/// <param name="name">The name of the switch.</param>
	/// <param name="value">The value to assign to it.</param>
	/// <exception cref="NonExistentSwitchException">If there is no switch with the given name.</exception>
	public void SetSwitch(string name, bool value) {
		if (!switches.ContainsKey(name))
			throw new NonExistentSwitchException(name);
		else
			switches[name] = value;
	}

	/// <summary>
	/// Toggles the value of a switch.
	/// </summary>
	/// <param name="name">The name of the switch.</param>
	/// <exception cref="NonExistentSwitchException">If there is no switch with the given name.</exception>
	public void ToggleSwitch(string name) {
		if (!switches.ContainsKey(name))
			throw new NonExistentSwitchException(name);
		else
			switches[name] = !switches[name];
	}

	/// <summary>
	/// Adds a switch to this GlobalSwitches instance. Alternative notation:
	/// GlobalSwitches.INSTANCE[name] = value
	/// </summary>
	/// <param name="name">The name of the switch to add.</param>
	/// <param name="defaultValue">The default value of the switch.</param>
	/// <exception cref="SwitchAlreadyExistsException">If a switch with the given name exists already.</exception>
	public void CreateSwitch(string name, bool defaultValue = false) {
		if (switches.ContainsKey(name))
			throw new SwitchAlreadyExistsException(name);
		switches.Add(name, defaultValue);
	}
}

class NonExistentSwitchException : Exception {
	public NonExistentSwitchException(string name) {
		Console.WriteLine("Switch \"" + name + "\" does not exist.");
	}
}

class SwitchAlreadyExistsException : Exception {
	public SwitchAlreadyExistsException(string name) {
		Console.WriteLine("Switch \"" + name + "\" already exists.");
	}
}