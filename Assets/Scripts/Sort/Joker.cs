using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joker : Tile {
    // only for sort algorithms because it changes the id
    public Joker(int id, TileColor tileColor, int number) : base(id, tileColor, number)
    {
            
    }

    public Joker(int id) : base(id)
    {
            
    }

    public Joker(int number, TileColor tileColor) : base(number, tileColor)
    {
        id = 104;
    }
    
    public override string ToString()
    {
        return "Joker : " + base.ToString();
    }
}
