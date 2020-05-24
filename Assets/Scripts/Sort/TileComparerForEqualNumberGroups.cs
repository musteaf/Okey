using System.Collections.Generic;

public class TileComparerForEqualNumberGroups : IEqualityComparer<Tile>
{
    public bool Equals(Tile x, Tile y)
    {
        if (x == null && y == null)
            return true;
        if (x == null || y == null)
            return false;
        return x.Number.Equals(y.Number) && x.TileColor.Equals(y.TileColor);
    }

    public int GetHashCode(Tile obj)
    {
        int hashCode = 1;
        hashCode = 17 * hashCode + (obj.Number);
        hashCode = 17 * hashCode + (obj.TileColor.ToString().GetHashCode());
        return hashCode;
    }
}
