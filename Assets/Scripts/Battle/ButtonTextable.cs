using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ButtonTextable
{
    /// <summary>
    /// Get the text to display this content on the button
    /// </summary>
    public string GetDescriptionText();

    /// <summary>
    /// Gets the name to display on the button
    /// </summary>
    /// <returns></returns>
    public string GetName();

    /// <summary>
    /// Gets the number to display on the button
    /// </summary>
    public int GetNumber();

    /// <summary>
    /// Get the Sprite to use as the icon for the skill button
    /// </summary>
    public Sprite GetIcon();
}
