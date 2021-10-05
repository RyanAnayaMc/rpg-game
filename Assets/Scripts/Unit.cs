using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string name;
    public int level;
    public int cHP; // current hp
    public int maxHP; // maximum hp
    public int str; // strength
    public int mag; // magic
    public int dex; // dexterity
    public int def; // defense
    public int res; // resistance
    public int arm; // armor
    public int agi; // agility

    public bool TakeDamage(int damage)
    {
        cHP -= damage;
        return cHP <= 0;
    }

    public void Heal(int heal)
    {
        cHP += heal;
        if (cHP > maxHP)
            cHP = maxHP;
    }
}
