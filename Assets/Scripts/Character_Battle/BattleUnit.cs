using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleUnit : MonoBehaviour
{
    public Unit unit;
    public GameObject effectRenderer;

    public void Start()
    {
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = unit.unitSprite;
        spriteRenderer.sortingOrder = 10;
    }

    // Makes the unit take damage. Returns true if the unit died, false otherwise
    public bool TakeDamage(int damage)
    {
        unit.cHP -= damage;
        bool isDead = unit.cHP <= 0;

        if (isDead)
            unit.cHP = 0;

        return isDead;
    }

    // Heals the unit's HP. cHP cannot exceed maxHP
    public void Heal(int heal)
    {
        unit.cHP += heal;
        if (unit.cHP > unit.maxHP)
            unit.cHP = unit.maxHP;
    }
}
