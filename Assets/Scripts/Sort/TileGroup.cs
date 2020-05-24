using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TileGroup
{
    private List<Tile> tiles;
        private int jokerCount;
        public TileGroup()
        {
            tiles = new List<Tile>();
            jokerCount = 0;
        }
        
        public TileGroup(TileGroup tileGroup)
        {
            tiles = new List<Tile>(tileGroup.tiles);
            jokerCount = tileGroup.jokerCount;
        }

        public TileGroup(List<Tile> tiles, int jokerCount)
        {
            this.tiles = new List<Tile>(tiles);
            this.jokerCount = jokerCount;
        }
        
        public void AddJoker(Joker tile) {
            jokerCount++;
            tiles.Add(tile);
        }

        public List<Tile> Tiles
        {
            get => tiles;
            set => tiles = value;
        }

        public int JokerCount
        {
            get => jokerCount;
            set => jokerCount = value;
        }

        public TileGroup OrderByColorThenByNumber()
        {
            tiles = tiles.OrderBy(tile => tile.TileColor).ThenBy(tile => tile.Number).ToList();
            TileGroup tileGroup = new TileGroup(tiles, jokerCount);
            return tileGroup;
        }
        
        public TileGroup OrderByNumberThenByColor()
        {
            tiles = tiles.OrderBy(tile => tile.Number).ThenBy(tile => tile.TileColor).ToList();
            TileGroup tileGroup = new TileGroup(tiles, jokerCount);
            return tileGroup;
        }
        public int GetTileCount()
        {
            return tiles.Count;
        }

        public Tile GetTile(int index)
        {
            return tiles[index];
        }

        public Tile GetLastTile()
        {
            if (tiles.Count == 0)
                return null;
            return tiles[tiles.Count - 1];
        }

        public void AddTile(Tile tile)
        {
            tiles.Add(tile);
        }

        public void RemoveLastTile()
        {
            Tile tile = tiles[tiles.Count - 1];
            if (tile is Joker)
            {
                jokerCount--;
            }
            tiles.RemoveAt(tiles.Count - 1);
        }
        
        public TileGroup RemoveUsedTiles (TileGroup usedTileGroup, Dictionary<int,int> tilePositions)
        {
            TileGroup newTileGroup = new TileGroup(tiles, jokerCount);
            List<Tile> usedTiles = usedTileGroup.tiles;
            for (int i = usedTiles.Count-1; i >= 0; i--)
            {
                if(!(usedTiles[i] is Joker))
                    newTileGroup.tiles.RemoveAt(tilePositions[usedTiles[i].Id]);
            }
            newTileGroup.jokerCount -= usedTileGroup.jokerCount;
            return newTileGroup;
        }

        public override string ToString()
        {
            StringBuilder st = new StringBuilder();
            foreach (Tile tile in tiles)
            {
                st.Append(tile.Number +"  "  + tile.TileColor + "  ");
            }
            st.Append("used joker  : " + jokerCount);
            return st.ToString();
        }

        public void RemoveFirst()
        {
            Tile tile = tiles[0];
            if (tile is Joker)
            {
                jokerCount--;
            }
            tiles.RemoveAt(0);
        }

        public TileColor GetUnUsedColor()
        {
            bool black = false;
            bool blue = false;
            bool red = false;
            bool yellow = false;
            foreach (Tile tile in tiles)
            {
                if (tile.TileColor == TileColor.Black)
                    black = true;
                else if(tile.TileColor == TileColor.Red)
                    red = true;
                else if(tile.TileColor == TileColor.Yellow)
                    yellow = true;
                else if(tile.TileColor == TileColor.Blue)
                    blue = true;
            }
            if (!black)
                return TileColor.Black;
            if (!blue)
                return TileColor.Blue;
            if (!yellow)
                return TileColor.Yellow;
            if (!red)
                return TileColor.Red;
            return TileColor.None;
        }
        
        public override int GetHashCode()
        {
            int hashCode = 1;
            foreach (Tile tile in tiles)
            {
                hashCode = 17 * hashCode + (tile==null ? 0 : tile.GetHashCode());
            }
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return this.GetHashCode().Equals(((TileGroup)obj).GetHashCode());
        }
}
