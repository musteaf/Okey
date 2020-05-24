using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// I am not sure about naming, it can be HandGenerator but it generates all hands and remain tiles also generates joker 
public class TileGenerator
{
    private List<int> allSet;
    private int jokerId;
    private Tile indicator;
    public TileGenerator()
    {
        allSet = new List<int>();
        for (int i = 0; i < 104; i++)
        {
            allSet.Add(i);
        }
        indicator = RollJoker();
        int jokerNumber = (indicator.Number + 1) % 13;
        jokerId = TileMath.NumberAndColorToId(jokerNumber, indicator.TileColor);
    }

    public TileGenerator(List<int> ids, List<Tile> firstHand, int customIndicatorId)
    {
        //withCustomIds
        allSet = new List<int>();
        for (int i = 0; i < 104; i++)
        {
            allSet.Add(i);
        }

        indicator = CustomJoker(customIndicatorId);
        int jokerNumber = (indicator.Number + 1) % 13;
        jokerId = TileMath.NumberAndColorToId(jokerNumber, indicator.TileColor);
        
        for (int i = 0; i < ids.Count; i++)
        {
            int selectedIndex = ids[i];
            Tile currentTile = new Tile(allSet[selectedIndex], TileMath.IdToColor(allSet[selectedIndex]),
                TileMath.IdToNumber(allSet[selectedIndex]));
            firstHand.Add(currentTile);
            if (currentTile.Id == jokerId || TileMath.GetOtherIdOfSameTile(jokerId) == currentTile.Id)
            {
                currentTile.IsItJoker = true;
            }
        }
       
        ids.Add(customIndicatorId);
        ids.Sort();
        for (int i = ids.Count-1; i >= 0; i--)
        {
            allSet.RemoveAt(ids[i]);
        }
    }

    public int JokerId
    {
        get => jokerId;
        set => jokerId = value;
    }

    public List<Tile> CreateHand(int tileCount)
    {
        List<Tile> currentSet = new List<Tile>();
        for (int i = 0; i < tileCount; i++)
        {
            int selectedIndex = Random.Range(0, allSet.Count);
            Tile currentTile = new Tile(allSet[selectedIndex], TileMath.IdToColor(allSet[selectedIndex]),
                TileMath.IdToNumber(allSet[selectedIndex]));
            currentSet.Add(currentTile);
            if (currentTile.Id == jokerId || TileMath.GetOtherIdOfSameTile(jokerId) == currentTile.Id)
            {
                currentTile.IsItJoker = true;
            }
            allSet.RemoveAt(selectedIndex);
        }
        return currentSet;
    }

    public Tile RollJoker()
    {
        int selectedIndex = Random.Range(0, allSet.Count);
        Tile currentTile = new Tile(allSet[selectedIndex], TileMath.IdToColor(allSet[selectedIndex]),
            TileMath.IdToNumber(allSet[selectedIndex]));
        allSet.RemoveAt(selectedIndex);
        return currentTile;
    }
    
    public Tile CustomJoker(int id)
    {
        int selectedIndex = id;
        Tile currentTile = new Tile(allSet[selectedIndex], TileMath.IdToColor(allSet[selectedIndex]),
            TileMath.IdToNumber(allSet[selectedIndex]));
        return currentTile;
    }

    public List<Tile> GetRemains()
    {
        
       
        List<Tile> remains = new List<Tile>();
        for (int i = 0; i < allSet.Count; i++)
        {
            Tile currentTile = new Tile(allSet[i], TileMath.IdToColor(allSet[i]), TileMath.IdToNumber(allSet[i]));
            remains.Add(currentTile);
        }
        allSet.Clear();
        return remains;
    }

    public Tile Indicator
    {
        get => indicator;
        set => indicator = value;
    }
}
