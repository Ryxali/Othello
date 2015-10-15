using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter.Interop;

/// <summary>
/// Contains the rules exclusively
/// </summary>
[MoonSharp.Interpreter.MoonSharpUserData]
public class BasicBoard {
    public int size { get; private set; }
    protected BasicTile[] tiles;
    private int[] pawnsLeft;
    private static readonly Point2D[] moveDirs = new Point2D[] {
        new Point2D(1, 1),
        new Point2D(0, 1),
        new Point2D(-1, 1),
        new Point2D(-1, 0),
        new Point2D(1, 0),
        new Point2D(-1, -1),
        new Point2D(0, -1),
        new Point2D(1, -1)
    };
    [MoonSharp.Interpreter.MoonSharpUserData]
    public class Score
    {
        public Score(int p0, int p1, int none)
        {
            player0 = p0;
            player1 = p1;
            this.none = none;
        }
        public int this[BasicTile.State state]
        {

            get
            {
                if (state == BasicTile.State.PLAYER_0) return player0;
                if (state == BasicTile.State.PLAYER_1) return player1;
                return none;
            }

        }
        public int this[int state]
        {

            get
            {
                if (state == 1) return player0;
                if (state == 2) return player1;
                return none;
            }

        }

        private int none;
        private int player0;
        private int player1;
    }

    protected BasicBoard(int size = 8)
    {
        this.size = size;
        pawnsLeft = new int[2];
    }
    [MoonSharpVisible(false)]
    public static BasicBoard Create(int size = 8)
    {
        BasicBoard b = new BasicBoard(size);
        b.ResetWithSize(8);
        return b;
    }
    [MoonSharpVisible(false)]
    public virtual BasicBoard Copy()
    {
        BasicBoard b = Create(size);
        for (int i = 0; i < size * size; i++ )
        {
            b.tiles[i].SetState(tiles[i].state);
        }
        return b;
    }
    [MoonSharpVisible(false)]
    protected virtual BasicTile CreateTile(int x, int y)
    {
        return new BasicTile();
    }

    private BasicTile GetTile(int x, int y)
    {
        return tiles[x + y * size];
    }
    public BasicTile.State GetTileState(int x, int y)
    {
        return GetTile(x, y).state;
    }
    [MoonSharpVisible(false)]
    public virtual void Reset()
    {
        pawnsLeft[0] = 32;
        pawnsLeft[1] = 32;
        foreach (BasicTile t in tiles)
        {
            t.SetState(BasicTile.State.NONE);
        }
        int mid = size / 2;
        GetTile(mid, mid).SetState(BasicTile.State.PLAYER_0);
        GetTile(mid, mid - 1).SetState(BasicTile.State.PLAYER_1);
        GetTile(mid - 1, mid).SetState(BasicTile.State.PLAYER_1);
        GetTile(mid - 1, mid - 1).SetState(BasicTile.State.PLAYER_0);
    }
    [MoonSharpVisible(false)]
    public void ResetWithSize(int newSize)
    {
        size = newSize;
        tiles = new BasicTile[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                tiles[x + size * y] = CreateTile(x, y);//new Tile(transform, x, y, pawn.gameObject);
            }
        }
        Reset();
    }
    
    /// <summary>
    /// Helper function
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    [MoonSharpVisible(true)]
    private bool InBounds(Point2D point)
    {
        return 0 <= point.x && point.x < size && 0 <= point.y && point.y < size;
    }

    /// <summary>
    /// Places a Pawn on the specified tile should the rules permit it.
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    [MoonSharpVisible(true)]
    public bool PlacePawnOnTile(Tile.State owner, int x, int y)
    {
        if (owner == Tile.State.NONE) Debug.LogError("Player 'NONE' can't place pawns!");
        if (GetTile(x, y).state != Tile.State.NONE) return false;
        bool hasConverted = false;
        Point2D origin = new Point2D(x, y);
        if (GetTile(origin.x, origin.y).state != Tile.State.NONE) return false;
        foreach (Point2D moveDir in moveDirs)
        {
            Point2D iter = new Point2D(origin) + moveDir;
            int converted = 0;
            bool inBounds;
            while ((inBounds = InBounds(iter)) && GetTile(iter.x, iter.y).state == Tile.Other(owner))
            {
                converted++;
                iter += moveDir;
            }
            if (inBounds && converted > 0 && GetTile(iter.x, iter.y).state == owner)
            {
                hasConverted = true;
                iter -= moveDir;
                while (iter != origin)
                {

                    GetTile(iter.x, iter.y).Flip();
                    iter -= moveDir;
                }
                GetTile(origin.x, origin.y).SetState(owner);
            }
        }
        return hasConverted;
    }

    /// <summary>
    /// Checks whether the owner player can place a pawn
    /// on the given tile.
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    [MoonSharpVisible(true)]
    public bool IsValidPlacementLocation(Tile.State owner, int x, int y)
    {
        if (owner == Tile.State.NONE) return false;

        Point2D origin = new Point2D(x, y);
        if (GetTile(origin.x, origin.y).state != Tile.State.NONE) return false;
        foreach (Point2D moveDir in moveDirs)
        {
            Point2D iter = new Point2D(origin) + moveDir;
            int converted = 0;
            bool inBounds = InBounds(iter);
            while (inBounds && GetTile(iter.x, iter.y).state == Tile.Other(owner))
            {
                converted++;
                iter += moveDir;
                inBounds = InBounds(iter);
            }
            if (inBounds && converted > 0 && GetTile(iter.x, iter.y).state == owner)
            {
                return true;
            }
        }

        return false;
    }
    [MoonSharpVisible(true)]
    public bool CanPlaceAnyMore(Tile.State owner)
    {
        if (GetPawnsLeft(owner) <= 0) return false;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (IsValidPlacementLocation(owner, x, y))
                {
                    return true;
                }
            }
        }
        return false;
    }
    [MoonSharpVisible(true)]
    public int GetPawnsLeft(Tile.State player)
    {
        if (player == Tile.State.PLAYER_0) return pawnsLeft[0];
        else if (player == Tile.State.PLAYER_1) return pawnsLeft[1];
        return -1;
    }
    [MoonSharpVisible(false)]
    private void DecrementPawnsLeft(Tile.State player)
    {
        if (player == Tile.State.PLAYER_0) pawnsLeft[0]--;
        else if (player == Tile.State.PLAYER_1) pawnsLeft[1]--;
    }
    [MoonSharpVisible(true)]
    public Score GetScore()
    {
        int p0 = 0;
        int p1 = 0;
        int none = 0;
        for (int i = 0; i < size * size ; i++)
        {
            BasicTile.State s = tiles[i].state;
            if (s == BasicTile.State.PLAYER_0) p0++;
            else if (s == BasicTile.State.PLAYER_1) p1++;
            else none++;
        }
        return new Score(p0, p1, none);
    }

}
