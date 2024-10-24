using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObsSection", menuName = "ScriptableObjects/ObsSection")]
public class SectionObsData : SectionData
{
    public List<GameObject> Obstacles;
    internal override BaseSection CreateMono()
    {
        ObsSection section = new ObsSection();

        // Common properties assigned after switch
        section.levelTiles = levelTiles;
        section.SectionEnd = SectionEnd;
        section.SectionStart = SectionStart;
        section.Obstacles = Obstacles;
        return section;
    }
}
