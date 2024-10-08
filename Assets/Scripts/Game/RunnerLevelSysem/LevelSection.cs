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

}
public enum SectionType
{
    None, Obstacle, Fight, Collect
}

