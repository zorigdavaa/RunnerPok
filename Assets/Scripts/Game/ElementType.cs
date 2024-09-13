using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    Normal, Fire, Water, Electric, Earth, Ice
}
public static class ElementExtensions
{
    private static readonly ElementEffectivenessRules effectivenessRules = new ElementEffectivenessRules();

    public static float GetEffectiveMultiplier(this ElementType thisType, ElementType otherType)
    {
        return effectivenessRules.GetEffectiveMultiplier(thisType, otherType);
    }
}
public class ElementEffectivenessRules
{
    private readonly Dictionary<(ElementType, ElementType), float> effectivenessTable;

    public ElementEffectivenessRules()
    {
        effectivenessTable = new Dictionary<(ElementType, ElementType), float>();
        InitializeEffectivenessRules();
    }

    private void AddRule(ElementType attacker, ElementType defender, float multiplier)
    {
        effectivenessTable[(attacker, defender)] = multiplier;
    }

    private void InitializeEffectivenessRules()
    {
        // Add effectiveness rules here
        AddRule(ElementType.Fire, ElementType.Earth, 2.5f);
        AddRule(ElementType.Fire, ElementType.Water, 0.3f);
        AddRule(ElementType.Fire, ElementType.Fire, 0.6f);

        AddRule(ElementType.Water, ElementType.Fire, 2.5f);
        AddRule(ElementType.Water, ElementType.Electric, 0.3f);
        AddRule(ElementType.Water, ElementType.Water, 0.6f);

        AddRule(ElementType.Electric, ElementType.Water, 2.5f);
        AddRule(ElementType.Electric, ElementType.Earth, 0.3f);
        AddRule(ElementType.Electric, ElementType.Electric, 0.6f);

        AddRule(ElementType.Earth, ElementType.Electric, 2.5f);
        AddRule(ElementType.Earth, ElementType.Ice, 0.3f);
        AddRule(ElementType.Earth, ElementType.Earth, 0.6f);

        AddRule(ElementType.Ice, ElementType.Earth, 2.5f);
        AddRule(ElementType.Ice, ElementType.Fire, 0.3f);
        AddRule(ElementType.Ice, ElementType.Ice, 0.6f);
    }

    public float GetEffectiveMultiplier(ElementType attacker, ElementType defender)
    {
        if (effectivenessTable.TryGetValue((attacker, defender), out float multiplier))
        {
            return multiplier;
        }

        // Handle unknown element type combination
        // You may choose to throw an exception, return a default value, or handle it in another way.
        return 1.0f;
    }
}
// Fire is strong against Earth, but weak against Water.
// Water is strong against Fire, but weak against Lightning.
// Lightning is strong against Water, but weak against Earth.
// Earth is strong against Lightning, but weak against Ice.
// Ice is strong against Earth, but weak against Fire.

// Earth: The tower can create earthquakes, spikes or boulders to attack enemies.
// Wind: The tower can create gusts of wind to knock enemies back or create tornadoes to damage them.
// Lightning: The tower can create bolts of lightning to electrocute enemies.
// Ice: The tower can create ice shards or freeze enemies in place.
// Light: The tower can emit a bright light that blinds enemies or create a laser beam that damages them.
// Darkness: The tower can create shadows that slow down enemies or summon dark creatures to attack them.
// Poison: The tower can release toxic gases or venomous creatures to damage enemies.
// Metal: The tower can launch metal projectiles or create sharp metal blades to attack enemies.


// Fire is strong against Plants, but weak against Metal.
// Water is strong against Fire and Earth, but weak against Poison.
// Earth is strong against Electric and Poison, but weak against Ice.
// Electric is strong against Water, but weak against Earth.
// Ice is strong against Fire and Water, but weak against Poison.
// Poison is strong against Earth and Plant, but weak against Electric.
// Metal is strong against Earth, but weak against Fire and Acid.
// Acid is strong against Metal, but weak against Water and Wind.
// Wind is strong against Acid, but weak against Earth and Ice.

// public static float GetEffectiveMultiplier(this ElementType thisType, ElementType otherType)
// {
//     float multiplier = 1;

//     switch (thisType)
//     {
//         case ElementType.Fire:
//             if (otherType == ElementType.Earth)
//             {
//                 multiplier = 2.5f; // effective
//             }
//             else if (otherType == ElementType.Water)
//             {
//                 multiplier = 0.3f; // weak
//             }
//             else if (otherType == ElementType.Fire)
//             {
//                 multiplier = 0.6f; // weak
//             }
//             break;
//         case ElementType.Water:
//             if (otherType == ElementType.Fire)
//             {
//                 multiplier = 2.5f; // effective
//             }
//             else if (otherType == ElementType.Electric)
//             {
//                 multiplier = 0.3f; // weak
//             }
//             else if (otherType == ElementType.Water)
//             {
//                 multiplier = 0.6f; // weak
//             }
//             break;
//         case ElementType.Electric:
//             if (otherType == ElementType.Water)
//             {
//                 multiplier = 2.5f; // effective
//             }
//             else if (otherType == ElementType.Earth)
//             {
//                 multiplier = 0.3f; // weak
//             }
//             else if (otherType == ElementType.Electric)
//             {
//                 multiplier = 0.6f; // weak
//             }
//             break;
//         case ElementType.Earth:
//             if (otherType == ElementType.Electric)
//             {
//                 multiplier = 2.5f; // effective
//             }
//             else if (otherType == ElementType.Ice)
//             {
//                 multiplier = 0.3f; // weak
//             }
//             else if (otherType == ElementType.Earth)
//             {
//                 multiplier = 0.6f; // weak
//             }
//             break;
//         case ElementType.Ice:
//             if (otherType == ElementType.Earth)
//             {
//                 multiplier = 2.5f; // effective
//             }
//             else if (otherType == ElementType.Fire)
//             {
//                 multiplier = 0.3f; // weak
//             }
//             else if (otherType == ElementType.Ice)
//             {
//                 multiplier = 0.6f; // weak
//             }
//             break;
//         // add cases for other element types here
//         default:
//             // handle unknown element type
//             break;
//     }
//     return multiplier;
// }
