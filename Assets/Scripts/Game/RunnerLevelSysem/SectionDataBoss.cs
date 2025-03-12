using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossSection", menuName = "ScriptableObjects/BossSection")]
public class SectionDataBoss : SectionDataFight
{
    public EnemyWave Boss;
    public override LevelSection CreateMono()
    {
        // BossSection section = new BossSection();
        GameObject BossSection = new GameObject();
        BossSection section = BossSection.AddComponent<BossSection>();

        // Common properties assigned after switch
        section.levelTiles = levelTiles;
        section.SectionEnd = SectionEnd;
        section.SectionStart = SectionStart;
        section.LevelEnemies = LevelEnemies;
        section.Boss = Boss;
        return section;
    }
}
