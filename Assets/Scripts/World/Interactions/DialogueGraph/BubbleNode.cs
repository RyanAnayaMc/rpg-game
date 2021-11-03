using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BubbleNode : BaseNode {
	[Input] public int entry;
	[Output] public int exit;
	public BalloonAnimation animation;
	public int durationMs;
	public bool waitForAnimationToFinish;
	public int targetObjectIndex;
	public float offsetX;
	public float offsetY;
	public float offsetZ;

    public override string GetString() {
		return "BubbleNode/" + animation.ToString() + "/" + durationMs + "/" + waitForAnimationToFinish.ToString();
	}
}
