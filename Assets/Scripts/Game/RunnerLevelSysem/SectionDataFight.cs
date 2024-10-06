using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FightSection", menuName = "ScriptableObjects/FightSection")]
public class SectionDataFight : SectionData
{
    public List<EnemyWave> LevelEnemies;
    internal override LevelSection CreateMono()
    {
        FightSection section = new FightSection();

        // Common properties assigned after switch
        section.levelTiles = levelTiles;
        section.SectionEnd = SectionEnd;
        section.SectionStart = SectionStart;
        section.LevelEnemies = LevelEnemies;
        return section;
    }
}
