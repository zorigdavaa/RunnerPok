using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public StatMax Health;
    public Stat Armor;
    public Stat AttackSpeed;
    public Stat BaseDamage;
    public StatInt AddProjCount;
    // Start is called before the first frame update
    void Start()
    {
        // Armor = new Stat(0);
    }
}
