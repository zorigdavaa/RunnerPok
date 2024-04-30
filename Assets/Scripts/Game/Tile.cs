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
    public EventHandler OnTileEnter;

    internal void SetNextTile(Tile tile)
    {
        Nexttile = tile;
        OnNextTileSet?.Invoke(tile, EventArgs.Empty);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            // print(true);
            OnTileEnter?.Invoke(this, EventArgs.Empty);
            transform.parent.GetComponent<Level>().PlayerBeingTile = this;
        }
    }
}
