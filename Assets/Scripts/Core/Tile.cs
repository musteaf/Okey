using System;

public class Tile
{
    protected int id;
    private TileColor tileColor;
    private int number;
    private bool isItJoker;
    public Tile(int id, TileColor tileColor, int number)
    {
        this.id = id;
        this.tileColor = tileColor;
        this.number = number;
        NumberRangeCheck();
    }
    
    public Tile(int id)
    {
        this.id = id;
        number = TileMath.IdToNumber(id);
        tileColor = TileMath.IdToColor(id);
        NumberRangeCheck();
    }

    public Tile(Tile tile)
    {
        this.id = tile.id;
        this.number = tile.number;
        this.tileColor = tile.tileColor;
    }

    protected Tile(int number, TileColor tileColor)
    {
        this.number = number;
        this.tileColor = tileColor;
        NumberRangeCheck();
    }

    public void NumberRangeCheck()
    {
        if(number > 12 || number < 0) 
            throw new Exception(" Number should be between 0 and 11");
        // We need to create handler to send analytics.
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }
        return this.id.Equals(((Tile)obj).Id);
    }

    public override int GetHashCode()
    {
        return this.id.GetHashCode();
    }

    public int Id
    {
        get => id;
        set => id = value;
    }

    public TileColor TileColor
    {
        get => tileColor;
        set => tileColor = value;
    }

    public int Number
    {
        get => number;
        set => number = value;
    }

    public bool IsItJoker
    {
        get => isItJoker;
        set => isItJoker = value;
    }

    public override string ToString()
    {
        return "Tile ID : " + id + "  Tile Number : " + number + "  Tile Color : "+ tileColor;
    }
}
