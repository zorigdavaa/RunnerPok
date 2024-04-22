using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSection", menuName = "ScriptableObjects/LvlSection")]
public class LevelSection : ScriptableObject
{
    public SectionType SectionType;
    public EventHandler Oncomplete;
    public List<Tile> levelTiles;
    public Tile SectionEnd;
    public Tile SectionStart;

}
public enum SectionType
{
    None, Obstacle, Fight
}
