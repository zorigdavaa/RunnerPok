using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

// [CreateAssetMenu(fileName = "ObstacleSection", menuName = "ScriptableObjects/ObstacleSection")]
[Serializable]
public class LevelSection : BaseSection
{
    public void InitializefromData(SectionData data)
    {
        levelTiles = data.levelTiles;
        SectionEnd = data.SectionEnd;
        SectionStart = data.SectionStart;
        VisualPrefab = data.VisualPrefab;
    }
    public Tile GetEnteringTile()
    {
        return transform.GetChild(0).GetComponent<Tile>();
    }
}
public enum SectionType
{
    None, Obstacle, Fight, Collect, Choose, BossFight, Normal, Complex
}

