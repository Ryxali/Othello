using UnityEngine;
using System.Collections;

public class GameBoard : BasicBoard {
    private Transform parent;
    private GameObject tilePrefab;
    protected GameBoard(int size, Transform parent, GameObject tilePrefab) : base(size)
    {
        this.parent = parent;
        this.tilePrefab = tilePrefab;
    }

    public static GameBoard Create(int size, Transform parent, GameObject tilePrefab)
    {
        GameBoard b = new GameBoard(size, parent, tilePrefab);
        b.ResetWithSize(size);
        return b;
    }

    protected override BasicTile CreateTile(int x, int y)
    {
        return new Tile(parent, x, y, tilePrefab);
    }
}
