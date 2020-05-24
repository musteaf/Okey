using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTuple
{
    private TileView tileView;
    private Tile tile;

    public TileTuple(TileView tileView, Tile tile)
    {
        this.tileView = tileView;
        this.tile = tile;
    }

    public TileView TileView
    {
        get => tileView;
        set => tileView = value;
    }

    public Tile Tile
    {
        get => tile;
        set => tile = value;
    }
}
