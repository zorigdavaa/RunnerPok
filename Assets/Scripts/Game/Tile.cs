using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public Tile Nexttile;
    public Tile BeforeTile;
    public EventHandler OnNextTileSet;

    internal void SetNextTile(Tile tile)
    {
        Nexttile = tile;
        OnNextTileSet?.Invoke(tile, EventArgs.Empty);
    }
}
