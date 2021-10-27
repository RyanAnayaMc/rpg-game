using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IMenuWindow {

	/// <summary>
	/// Returns whether or not the menu window is open.
	/// </summary>
	public bool IsOpen();

	/// <summary>
	/// Opens the menu asyncronously.
	/// </summary>
	public Task Open();

	/// <summary>
	/// Closes the menu asyncronously.
	/// </summary>
	public Task Close();
}
