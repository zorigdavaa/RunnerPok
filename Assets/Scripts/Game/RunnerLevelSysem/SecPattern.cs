using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern", menuName = "ScriptableObjects/SectionPattern", order = 1)]
public class SecPattern : ScriptableObject
{
    public List<SectionType> sectionTypes = new List<SectionType>();
}
