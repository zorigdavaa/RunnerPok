using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public float DefMaxHealth = 100;
    public Stat Health;
    public Stat MaxHealth;
    public Stat Armor;
    // Start is called before the first frame update
    void Start()
    {
        // MaxHealth = new Stat(DefMaxHealth);
        // Health = new Stat(DefMaxHealth);
        // Armor = new Stat(0);
    }
}
