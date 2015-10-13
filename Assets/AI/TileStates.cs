using UnityEngine;
using System.Collections;

public class TileStates
{
    private Tile.State[] states;
    private int size;
    public Tile.State this[int x, int y]
    {
        get
        {
            return states[x + size * y];
        }
        set
        {
            states[x + size * y] = value;
        }
    }
    public TileStates(Tile[] tiles, int size)
    {
        this.size = size;
        states = new Tile.State[size * size];
        for (int i = 0; i < size; i++)
        {
            states[i] = tiles[i].state;
        }
    }
}