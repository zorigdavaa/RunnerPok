using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionDataBoss : SectionDataFight
{
    public EnemyWave Boss;
    internal override LevelSection CreateMono()
    {
        BossSection section = new BossSection();

        // Common properties assigned after switch
        section.levelTiles = levelTiles;
        section.SectionEnd = SectionEnd;
        section.SectionStart = SectionStart;
        section.LevelEnemies = LevelEnemies;
        section.Boss = Boss;
        return section;
    }
}
