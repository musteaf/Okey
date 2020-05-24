using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private static BoardManager instance;
    private List<Tile> tiles;
    private List<Tile> remains;
    private int jokerId;
    private bool custom = true;
    [SerializeField] private TileHolder tileHolder;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CreateBoard();
    }

    public void CreateBoard()
    {
        //custom
        if (custom && tileHolder.isDistributed())
        {
            tiles = new List<Tile>();
            int[] ids = new[] {40, 41, 42, 43, 21, 22, 23, 4, 7, 9, 30, 38, 19, 48};
            TileGenerator tileGenerator = new TileGenerator(ids.ToList(), tiles,60);
            tileGenerator.CreateHand(14);
            tileGenerator.CreateHand(14);
            tileGenerator.CreateHand(14);
            remains = tileGenerator.GetRemains();
            jokerId = tileGenerator.JokerId;
            tileHolder.UpdateBoard(tiles, tileGenerator.Indicator);
            custom = false;
        }else if (tileHolder.isDistributed())
        {
            TileGenerator tileGenerator = new TileGenerator();
            List<Tile> op1 = tileGenerator.CreateHand(14);// opponent
            List<Tile> op2 = tileGenerator.CreateHand(14);// opponent
            List<Tile> op3 = tileGenerator.CreateHand(14);// opponent
            tiles = tileGenerator.CreateHand(14);
            remains = tileGenerator.GetRemains();
            jokerId = tileGenerator.JokerId;
            tileHolder.UpdateBoard(tiles, tileGenerator.Indicator);
            print("JOKER : " + jokerId);
        }
    }

    public void SmartSort()
    {
        if (tileHolder.isDistributed())
        {
            List<TileGroup> tileGroups = TileSort.SortTiles(tiles, jokerId, true, true);
            tileHolder.Sorted(tileGroups);
        }
    }
    
    public void SeqSort()
    {
        if (tileHolder.isDistributed())
        {
            List<TileGroup> tileGroups = TileSort.SortTiles(tiles, jokerId, true, false);
            tileHolder.Sorted(tileGroups);
        }
    }
    
    public void SameNumberSort()
    {
        if (tileHolder.isDistributed())
        {
            List<TileGroup> tileGroups = TileSort.SortTiles(tiles, jokerId, false, true);
            tileHolder.Sorted(tileGroups);
        }
    }
}
