using System.Collections.Generic;
using UnityEngine;

public class lvlRandomizer : MonoBehaviour
{
    Level TargetLevel;
    [SerializeField] List<SecPattern> secPatterns= new List<SecPattern>();
    [SerializeField] List<Tile> NormalTiles = new List<Tile>();
    [SerializeField] List<Enemy> FightEnemies = new List<Enemy>();
    [SerializeField] List<SecDataChoose> ChooseSections = new List<SecDataChoose>();
    void Awake()
    {
        TargetLevel = GetComponent<Level>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
