using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerUnit", menuName = "RPG Element/Unit/Player Unit")]
public class PlayerUnit : Unit {
	public Sprite meleeSprite;
	public Sprite magicSprite;
	public Sprite rangedSprite;
	public bool useBasicSpriteMode;
	public int xp;


}
