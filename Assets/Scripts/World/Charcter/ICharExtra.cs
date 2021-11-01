using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharExtra {
    /// <summary>
    /// Feeds this item the character's current movement and direction.
    /// </summary>
    /// <param name="movement">The character's movement this frame.</param>
    /// <param name="dir">The direction the character is moving.</param>
    public void ReceiveData(Vector3 movement, Direction dir);
}
