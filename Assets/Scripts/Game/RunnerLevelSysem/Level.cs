using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ZPackage;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour
{
    public int SecIDX = 0;
    public int SecTileIDx = 0;
    public List<LevelSection> LevelObjects;
    public Player player; // Reference to the player's transform
    public Vector3 nextSpawnPosition; // Position to spawn the next tile
    bool HasNextSection => LevelObjects.Count - 1 > SecIDX;

    public Tile PlayerBeingTile;
    public SpeedUp speedUpPF;
    public Tile BaseTilePf;
    [SerializeField] LevelSection CurSection;
    public float HealthMultiplier = 1;
    public float DamageMultiplier = 1;
    public void Start()
    {
        // BaseTilePf = 
        nextSpawnPosition = transform.position;
        SpawnedTiles = new List<Tile>();
        player = Z.Player;
        // StartNewSection();
        
    }

    public void Update()
    {
        // if (isNearEndofLand && CurSectionHasTile)
        if (CurSection == null)
        {
            bool isNearEndofLand = player.transform.position.z > nextSpawnPosition.z - 70;
            if (isNearEndofLand)
            {
                SpawnTile(BaseTilePf);
            }
        }
        else
        {
            CurSection.UpdateSection(this);

        }

    }

    private void NextSection(object caller, EventArgs args)
    {
        CurSection.Oncomplete -= NextSection;
        if (HasNextSection)
        {
            SecIDX++;
            StartNewSection();
        }
        else
        {
            CurSection = null;
            Tile tile = SpawnTile(BaseTilePf);
            tile.OnTileEnter += LevelComplete;

        }
    }

    private void LevelComplete(object sender, EventArgs e)
    {
        Tile tile = sender as Tile;
        tile.OnTileEnter -= LevelComplete;
        Z.GM.LevelComplete(this, 0);
    }

    public void StartNewSection()
    {
        SecTileIDx = 0;
        BeforSectionTiles.Clear();
        BeforSectionTiles = new List<Tile>(SpawnedTiles);
        CurSection = LevelObjects[SecIDX];
        if (SecIDX > 0)
        {
            LevelSection PrevSection = null;
            PrevSection = LevelObjects[SecIDX - 1];
            if (PrevSection.SectionType == SectionType.Fight)
            {
                InsSpeedUp();
            }

            // Vector3 NearPlayerPos = GetNearPlayerPos(BeforSectionTiles);
        }
        else
        {
            InsSpeedUp();
        }
        CurSection.StartSection(this);
        CurSection.Oncomplete += NextSection;
        print("Start Of From Level " + CurSection.SectionType + " " + CurSection.name);
    }

    private void InsSpeedUp()
    {
        Vector3 pos = player.transform.position + Vector3.forward * 10;
        pos.x = 0;
        SpeedUp sp = Instantiate(speedUpPF, pos, Quaternion.identity, transform);
        Destroy(sp, 10);
    }

    public Tile lastSpawnedTile;
    public List<Tile> SpawnedTiles;
    public List<Tile> BeforSectionTiles;
    public Tile SpawnTile(Tile tilePrefab)
    {
        Tile tile = Instantiate(tilePrefab, nextSpawnPosition, Quaternion.identity, transform);
        if (SpawnedTiles.Count > 0)
        {
            lastSpawnedTile.SetNextTile(tile);
            lastSpawnedTile.BeforeTile = SpawnedTiles[SpawnedTiles.Count - 1];
        }
        else if (BeforSectionTiles.Count > 0)
        {
            lastSpawnedTile.BeforeTile = BeforSectionTiles[BeforSectionTiles.Count - 1];
        }
        lastSpawnedTile = tile;
        SpawnedTiles.Add(tile);
        // nextSpawnPosition += spawnDistance;
        nextSpawnPosition = tile.end.position;
        if (SpawnedTiles.Count > 8)
        {
            Destroy(SpawnedTiles[0].gameObject);
            SpawnedTiles.RemoveAt(0);
        }
        return tile;
    }

    internal void DestSelf()
    {
        for (int i = SpawnedTiles.Count - 1; i >= 0; i--)
        {
            Destroy(SpawnedTiles[i].gameObject, i);
        }

    }
}