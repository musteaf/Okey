using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileMath
{
    private const int setSize = 13;
    /*Changing setCount will cause the necessity to expand colors variation*/
    private const int setCount = 4;
    public static TileColor IdToColor(int id)
    {
        int colorId = id / setSize;
        if (colorId > (setCount-1))
            colorId -= setCount;
        if (colorId == 0)
        {
            return TileColor.Red;
        }
        else if (colorId == 1)
        {
            return TileColor.Blue;
        }
        else if (colorId == 2)
        {
            return TileColor.Black;
        }
        else
        {
            return TileColor.Yellow;
        }
    }

    public static int IdToNumber(int id)
    {
        return id % setSize;
    }

    public static int NumberAndColorToId(int number, TileColor tileColor)
    {
        return (int) tileColor * setSize + number;
    }

    public static int GetOtherIdOfSameTile(int id)
    {
        int otherId = id;
        int curSet = id / (setSize*setCount);
        if (curSet == 0)
            otherId +=  (setSize*setCount);
        else
            otherId -=  (setSize*setCount);
        return otherId;
    }
}
