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

	/// <summary>
	/// Performs an Action after a delay.
	/// </summary>
	/// <param name="delayMs">The delay time in miliseconds.</param>
	/// <param name="action">The action to perform.</param>
	public static async Task DoAfter(int delayMs, Action action) {
		await Task.Delay(delayMs);
		action();
	}

	/// <summary>
	/// Returns a Color based on given RGBA. Automatically clamps given values.
	/// </summary>
	/// <param name="r">The red value to the color (0-255).</param>
	/// <param name="g">The green value to the color (0-255)</param>
	/// <param name="b">The blue value to the color (0-255).</param>
	/// <param name="a">The alpha value to the color (0-255).</param>
	/// <returns>The color based on the given RGBA values.</returns>
	public static Color RGB(int r, int g, int b, int a = 255) {
		float rf = Mathf.Clamp01((float) r / 255);
		float gf = Mathf.Clamp01((float) g / 255);
		float bf = Mathf.Clamp01((float) b / 255);
		float af = Mathf.Clamp01((float) a / 255);

		return new Color(rf, gf, bf, af);
	}
}
