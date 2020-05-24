using System;
using System.Collections.Generic;
using System.Linq;

public static class TileSort
{
    public static List<TileGroup> SortTiles(List<Tile> tiles, int jokerId, bool useSeq, bool useSameNumber)
    {
        List<Tile> remains = new List<Tile>();
        List<Tile> useTileAsJoker = new List<Tile>();
        int jokerCount = 0;
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].Id == jokerId || TileMath.GetOtherIdOfSameTile(tiles[i].Id) == jokerId)
            {
                jokerCount++;
                useTileAsJoker.Add(new Tile(tiles[i]));
            }
            else
            {
                remains.Add(new Tile(tiles[i].Id));
            }
        }
        List<TileGroup> result = new List<TileGroup>();
        TileGroup tileGroup = new TileGroup(remains, jokerCount);
        FindSequenceThatHasMinimumRemainingTile(tileGroup, remains.Count + remains.Count,
            new List<TileGroup>(), result, useSeq, useSameNumber);
        List<TileGroup> sortedTiles = new List<TileGroup>();
        for (int i = 0; i < result.Count; i++)
        {
            TileGroup newTileGroup = new TileGroup();
            foreach (Tile tile in result[i].Tiles)
            {
                if (tile is Joker)
                {
                    newTileGroup.AddTile(new Tile(useTileAsJoker[0]));
                    useTileAsJoker.RemoveAt(0);
                }
                else
                {
                    newTileGroup.AddTile(new Tile(tile));
                    remains.Remove(tile);
                }
            }
            sortedTiles.Add(newTileGroup);
        }
        TileGroup remainsTileGroup = new TileGroup();
        for (int i = 0; i < remains.Count; i++)
        {
            remainsTileGroup.AddTile(remains[i]);
        }

        if (remains.Count > 0)
        {
            sortedTiles.Add(remainsTileGroup);
        }
        int n = 0;
        foreach (TileGroup curTileGroup in sortedTiles)
        {
            foreach (Tile t in curTileGroup.Tiles)
            {
                n++;
            }
        }

        if (n != 14)
        {
            throw new Exception("An error occurred when tiles sorted.");
        }
        return sortedTiles;
    }

    private static int FindSequenceThatHasMinimumRemainingTile(TileGroup remains, int minimumLeft,
            List<TileGroup> usedSoFar, List<TileGroup> result, bool useSeq, bool useSameNumber)
        {
            int currMinimumLeft = Math.Min(remains.GetTileCount() + remains.JokerCount, minimumLeft);
            if (currMinimumLeft < minimumLeft)
            {
                result.Clear();
                for (int i = 0; i < usedSoFar.Count; i++)
                {
                    result.Add(new TileGroup(usedSoFar[i]));
                }
                minimumLeft = currMinimumLeft;
            }

            if (useSeq)
            {
                TileGroup remainingForSequential = remains.OrderByColorThenByNumber();
                Dictionary<int, int> tilePositions = CreateDictionary(remainingForSequential.Tiles);
                HashSet<TileGroup> possiblesSequential = new HashSet<TileGroup>();
                GetAllSequentialGroups(remainingForSequential.Tiles, possiblesSequential, remainingForSequential.JokerCount, 0,
                    new TileGroup());
            
                int seqMin = TryAllPossibleGroups(possiblesSequential, usedSoFar, remainingForSequential, tilePositions,
                    minimumLeft, result, useSeq, useSameNumber);
                minimumLeft = Math.Min(seqMin, minimumLeft);
            }
            
            if (useSameNumber)
            {
                TileGroup remainingForSameNumber = remains.OrderByNumberThenByColor();
                HashSet<TileGroup> possiblesSameNumber = new HashSet<TileGroup>();
                List<Tile> remainingForSameNumberTiles = remainingForSameNumber.Tiles;
                HashSet<Tile> uniqTilesForSameNumbers = new HashSet<Tile>(new TileComparerForEqualNumberGroups());
                for (int i = 0; i < remainingForSameNumberTiles.Count; i++)
                {
                    uniqTilesForSameNumbers.Add(remainingForSameNumberTiles[i]);
                }
                Dictionary<int, int> tilePositionsSameNumber = CreateDictionary(remainingForSameNumberTiles);
                List<Tile> orderedUniqTilesForSameNumbers = uniqTilesForSameNumbers.ToList().OrderBy(tile => tile.Number)
                    .ThenBy(tile => tile.TileColor).ToList();
                GetAllSameNumberGroups(orderedUniqTilesForSameNumbers, possiblesSameNumber, remainingForSameNumber.JokerCount,
                    0, new TileGroup());
                int sameMin = TryAllPossibleGroups(possiblesSameNumber, usedSoFar, remainingForSameNumber, tilePositionsSameNumber,
                    minimumLeft, result, useSeq, useSameNumber);
                minimumLeft = Math.Min(sameMin, minimumLeft);
            }
            
            return minimumLeft;
        }

        private static Dictionary<int, int> CreateDictionary(List<Tile> tiles)
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            for (int i = 0; i < tiles.Count; i++)
            {
                dictionary[tiles[i].Id] = i;
            }
            return dictionary;
        }

        private static int TryAllPossibleGroups(HashSet<TileGroup> possibleGroups, List<TileGroup> usedSoFar,
            TileGroup currentTileGroup, Dictionary<int, int> tilePositions, int currentMin, List<TileGroup> result,
            bool useSeq, bool useSameNumber)
        {
            foreach (TileGroup used in possibleGroups)
            {
                usedSoFar.Add(new TileGroup(used));
                TileGroup remains = currentTileGroup.RemoveUsedTiles(used, tilePositions);
                currentMin = Math.Min(FindSequenceThatHasMinimumRemainingTile(remains, currentMin, usedSoFar, result, 
                    useSeq, useSameNumber), currentMin);
                usedSoFar.RemoveAt(usedSoFar.Count - 1);
                if (currentMin == 0)
                    break;
            }
            return currentMin;
        }
        
        private static void GetAllSameNumberGroups(List<Tile> tiles, HashSet<TileGroup> result, int currentJokerCount,
            int currentIndex, TileGroup currentSelected)
        {
            //result 
            if (currentSelected.GetTileCount() == 3)
            {
                TileGroup newSelected = new TileGroup(currentSelected);
                result.Add(newSelected);
            }

            if (currentSelected.GetTileCount() == 4)
            {
                TileGroup newSelected = new TileGroup(currentSelected);
                result.Add(newSelected);
                currentSelected = new TileGroup();
                GetAllSameNumberGroups(tiles, result, currentJokerCount, currentIndex, currentSelected);
            }

            // we selected all tiles, only jokers left
            if (currentIndex >= tiles.Count)
            {
                if (currentJokerCount > 0 && currentSelected.GetUnUsedColor() != TileColor.None && currentSelected.GetTileCount() > 0)
                {
                    Tile lastSelected = currentSelected.GetLastTile();
                    currentSelected.AddJoker(new Joker(lastSelected.Number, currentSelected.GetUnUsedColor()));
                    GetAllSameNumberGroups(tiles, result, currentJokerCount - 1, currentIndex, currentSelected);
                    currentSelected.RemoveLastTile();
                }

                return;
            }

            Tile currentTile = tiles[currentIndex];

            // if numbers are different 
            if (currentSelected.GetTileCount() > 0 && currentTile.Number != currentSelected.GetLastTile().Number)
            {
                //check if we can use joker , we only need joker if we cant use another tile for this group
                if (currentJokerCount > 0 && currentSelected.GetUnUsedColor() != TileColor.None)
                {
                    Tile lastSelected = currentSelected.GetLastTile();
                    currentSelected.AddJoker(new Joker(lastSelected.Number, currentSelected.GetUnUsedColor()));
                    GetAllSameNumberGroups(tiles, result, currentJokerCount - 1, currentIndex, currentSelected);
                    currentSelected.RemoveLastTile();
                }

                currentSelected = new TileGroup();
            }

            // if last one 5 and current 5 
            if (currentSelected.GetTileCount() > 0 && currentTile.Number == currentSelected.GetLastTile().Number)
            {
                currentSelected.AddTile(currentTile);
                GetAllSameNumberGroups(tiles, result, currentJokerCount, currentIndex + 1, currentSelected);
                currentSelected.RemoveLastTile();
            }
            else
            {
                currentSelected = new TileGroup();
                currentSelected.AddTile(currentTile);
                GetAllSameNumberGroups(tiles, result, currentJokerCount, currentIndex + 1, currentSelected);
                currentSelected.RemoveLastTile();
            }
        }

        private static void GetAllSequentialGroups(List<Tile> tiles, HashSet<TileGroup> result, int currentJokerCount,
            int currentIndex, TileGroup currentSelected)
        { 
            //result 
            if (currentSelected.GetTileCount() >= 3)
            {
                TileGroup newSelected = new TileGroup(currentSelected);
                result.Add(newSelected);
                for (int i = 0; i < currentSelected.GetTileCount() - 3; i++)
                {
                    newSelected = new TileGroup(newSelected);
                    newSelected.RemoveFirst();
                    result.Add(newSelected);
                }
            }

            //use joker or not
            Tile lastSelected = currentSelected.GetLastTile();
            int lastSelectedNumber = lastSelected == null ? -1 : lastSelected.Number;
            if (currentJokerCount > 0 && (lastSelectedNumber != 12))
            {
                if (lastSelected == null && currentIndex < tiles.Count && tiles[currentIndex].Number != 0)
                {
                    currentSelected.AddJoker(new Joker(tiles[currentIndex].Number - 1, tiles[currentIndex].TileColor));
                    GetAllSequentialGroups(tiles, result, currentJokerCount - 1, currentIndex, currentSelected);
                    currentSelected.RemoveLastTile();

                    if (tiles[currentIndex].Number != 1 && currentJokerCount == 2)
                    {
                        currentSelected.AddJoker(new Joker(tiles[currentIndex].Number - 1,
                            tiles[currentIndex].TileColor));
                        currentSelected.AddJoker(new Joker(tiles[currentIndex].Number - 2,
                            tiles[currentIndex].TileColor));
                        GetAllSequentialGroups(tiles, result, currentJokerCount - 2, currentIndex, currentSelected);
                        currentSelected.RemoveLastTile();
                        currentSelected.RemoveLastTile();
                    }
                }
                else
                {
                    // do not use unnecessary joker
                    if (lastSelected != null && (currentIndex >= tiles.Count ||
                                                 tiles[currentIndex].Number != lastSelected.Number + 1))
                    {
                        currentSelected.AddJoker(new Joker(lastSelected.Number + 1, lastSelected.TileColor));
                        GetAllSequentialGroups(tiles, result, currentJokerCount - 1, currentIndex, currentSelected);
                        currentSelected.RemoveLastTile();
                    }
                }
            }

            if (currentIndex >= tiles.Count)
                return;

            //normalseq
            Tile currentTile = tiles[currentIndex];


            // if colors are different
            if (currentSelected.GetTileCount() > 0 && currentTile.TileColor != currentSelected.GetLastTile().TileColor)
            {
                currentSelected = new TileGroup();
            }

            // if currentSelected arr empty
            if (currentSelected.GetTileCount() == 0)
            {
                currentSelected.AddTile(currentTile);
                GetAllSequentialGroups(tiles, result, currentJokerCount, currentIndex + 1, currentSelected);
                currentSelected.RemoveLastTile();
                return;
            }

            // if it is same tile
            if (currentSelected.GetTileCount() > 0 && currentTile.Number == currentSelected.GetLastTile().Number)
            {
                GetAllSequentialGroups(tiles, result, currentJokerCount, currentIndex + 1, currentSelected);
                return;
            }

            // if last one 5 and current 6
            if (currentSelected.GetTileCount() > 0 && currentTile.Number - 1 == currentSelected.GetLastTile().Number)
            {
                currentSelected.AddTile(currentTile);
                GetAllSequentialGroups(tiles, result, currentJokerCount, currentIndex + 1, currentSelected);
                currentSelected.RemoveLastTile();
            }
            else
            {
                currentSelected = new TileGroup();
                currentSelected.AddTile(currentTile);
                GetAllSequentialGroups(tiles, result, currentJokerCount, currentIndex + 1, currentSelected);
                currentSelected.RemoveLastTile();
            }
        }
}
