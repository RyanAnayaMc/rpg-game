using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that holds variables across the whole program.
/// </summary>
public class GlobalVariables {
    private Dictionary<string, int> variables;
    private GlobalVariables _instance;

    /// <summary>
    /// The game's GlobalVariables instance.
    /// </summary>
    public GlobalVariables INSTANCE {
        get {
            if (_instance is null)
                _instance = new GlobalVariables();
            return _instance;
		}
	}

    private GlobalVariables() {
        variables = new Dictionary<string, int>();
	}

    public int this[string name] {
        get { return GetVariable(name); }
        set {
            if (variables.ContainsKey(name))
                SetVariable(name, value);
            else
                CreateVariable(name, value);
		}
	}

    /// <summary>
    /// Gets the value of a given variable. Alternative notation:
    /// GlobalVariables.INSTANCE[name]
    /// </summary>
    /// <param name="name">The name of the variable to get the value of.</param>
    /// <returns>The value of the variable.</returns>
    /// <exception cref="NonExistentVariableException">If a variable with the given name does not exist.</exception>
    public int GetVariable(string name) {
        if (!variables.ContainsKey(name))
            throw new NonExistentVariableException(name);
        else
            return variables[name];
	}

    /// <summary>
    /// Sets the value of a given variable. Alternative notation:
    /// GlobalVariables.INSTANCE[name] = value
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">The new value of the variable.</param>
    /// <exception cref="NonExistentVariableException">If a variable with the given name does not exist.</exception>
    public void SetVariable(string name, int value) {
        if (!variables.ContainsKey(name))
            throw new NonExistentVariableException(name);
        else
            variables[name] = value;
	}

    /// <summary>
    /// Adds a new variable to the GlobalVariables instance. Alternative notation:
    /// GlobalVariables.INSTANCE[name] = value
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <param name="value">The value of the variable.</param>
    /// <exception cref="VariableAlreadyExistsException">If a variable with the given name already exists.</exception>
    public void CreateVariable(string name, int value = 0) {
        if (variables.ContainsKey(name))
            throw new VariableAlreadyExistsException(name);
        else
            variables.Add(name, value);
	}
}

class NonExistentVariableException : Exception {
    public NonExistentVariableException(string name) {
        Console.WriteLine("Variable \"" + name + "\" does not exist.");
    }
}

class VariableAlreadyExistsException : Exception {
    public VariableAlreadyExistsException(string name) {
        Console.WriteLine("Variable \"" + name + "\" already exists.");
    }
}