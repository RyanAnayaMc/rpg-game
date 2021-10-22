using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class Utilities {
	/// <summary>
	/// Identical to Time.deltaTime except it is in miliseconds
	/// </summary>
	public static int FrameTimeMs() {
		return (int) (Time.deltaTime * 1000);
	}

	/// <summary>
	/// Returns an awaitable Task that waits for the next frame
	/// </summary>
	public static async Task UntilNextFrame() {
		await Task.Delay(FrameTimeMs());
	}

	/// <summary>
	/// Waits until the supplied delegate is true.
	/// </summary>
	/// <param name="predicate">The delegate to check.</param>
	/// <param name="updateDelay">The time in miliseconds before the delegate is checked again.</param>
	/// <returns></returns>
	public static async Task WaitUntil(Func<bool> predicate, int updateDelay = 50) {
		while (!predicate()) await Task.Delay(updateDelay);
	}

	/// <summary>
	/// Waits until the supplied delegate is false.
	/// </summary>
	/// <param name="predicate">The delegate to check.</param>
	/// <param name="updateDelay">The time in miliseconds before the delegate is checked again.</param>
	/// <returns></returns>
	public static async Task WaitWhile(Func<bool> predicate, int updateDelay = 50) {
		while (predicate()) await Task.Delay(updateDelay);
	}

	public static async Task DoAfter(int delayMs, Action action) {
		await Task.Delay(delayMs);
		action();
	}
}
