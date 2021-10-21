using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities {
	public static int FrameTimeMs() {
		return (int) ((1 / Time.deltaTime) * 1000);
	}
}
