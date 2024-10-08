using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SectionData", menuName = "ScriptableObjects/SectionData")]
public class SectionData : ScriptableObject
{
    public SectionType Type;
    public List<Tile> levelTiles;
    public Tile SectionEnd;
    public Tile SectionStart;

    internal virtual BaseSection CreateMono()
    {
        LevelSection section;

        switch (Type)
        {
            case SectionType.Collect:
                section = new CollectSection();
                break;
            default:
                section = new LevelSection();
                break;
        }

        // Common properties assigned after switch
        section.levelTiles = levelTiles;
        section.SectionEnd = SectionEnd;
        section.SectionStart = SectionStart;

        return section;
    }
}
