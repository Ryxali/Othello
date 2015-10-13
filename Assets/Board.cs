using UnityEngine;
using System.Collections;

/// <summary>
/// This class contains all Tiles and are responsible
/// for bulk operations while enforcing the rules. The
/// tiles are encapsulated here to prevent foul play.
/// </summary>
public class Board : MonoBehaviour {
    [Range(4, 16), SerializeField]
    private int size = 8;
    private Tile[] tiles;

    private int[] pawnsLeft;
    public TileStates tileStates
    {
        get
        {
            return new TileStates(tiles, size);
        }
    }
    public Vector3 normal { get { return transform.forward; } }
    
    

    public MeshRenderer pawn;
    public Transform plate;

    private readonly Point2D[] moveDirs = new Point2D[] {
        new Point2D(1, 1),
        new Point2D(0, 1),
        new Point2D(-1, 1),
        new Point2D(-1, 0),
        new Point2D(1, 0),
        new Point2D(-1, -1),
        new Point2D(0, -1),
        new Point2D(1, -1)
    };

	void Start () {
        
        plate.GetComponent<MeshRenderer>().material.mainTextureScale = Vector2.one * size / 2;
        plate.transform.localScale = new Vector3(1, 1, 0) * size;
        plate.transform.localPosition += new Vector3(1, 1) * (size / 2 - 0.5f);
        tiles = new Tile[size * size];
        for (int y = 0; y < size; y++ ) {
            for (int x = 0; x < size; x++)
            {
                tiles[x + size * y] = new Tile(transform, x, y, pawn.gameObject);
            }
        }
        pawnsLeft = new int[2];
        Reset();
        
	}

    void Update()
    {
        
    }

    private Tile GetTile(int x, int y)
    {
        return tiles[x + y * size];
    }

    public void Reset()
    {
        pawnsLeft[0] = 32;
        pawnsLeft[1] = 32;
        foreach (Tile t in tiles)
        {
            t.SetState(Tile.State.NONE);
        }
        int mid = size/2;
        GetTile(mid, mid).SetState(Tile.State.PLAYER_0);
        GetTile(mid, mid-1).SetState(Tile.State.PLAYER_1);
        GetTile(mid-1, mid).SetState(Tile.State.PLAYER_1);
        GetTile(mid-1, mid-1).SetState(Tile.State.PLAYER_0);
    }

    public void ResetWithSize(int newSize)
    {
        size = newSize;
        Reset();
    }

    /// <summary>
    /// Helper function
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
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
    public bool PlacePawnOnTile(Tile.State owner, int x, int y)
    {
        if (owner == Tile.State.NONE) Debug.LogError("Player 'NONE' can't place pawns!");
        if (GetTile(x, y).state != Tile.State.NONE) return false;
        bool hasConverted = false;
        Point2D origin = new Point2D(x, y);
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
    public bool IsValidPlacementLocation(Tile.State owner, int x, int y)
    {
        if (owner == Tile.State.NONE) return false;
        Point2D origin = new Point2D(x, y);
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
                return true;
            }
        }
        return false;
    }

    public bool CanPlaceAnyMore(Tile.State owner) {
        if (GetPawnsLeft(owner) <= 0) return false;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (IsValidPlacementLocation(Tile.State.PLAYER_0, x, y))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public Point2D WorldToTilePosition(Vector3 point)
    {
        Vector3 localPoint = point - transform.position;
        Vector3 projPoint = Vector3.ProjectOnPlane(localPoint, normal);
        return new Point2D((int)(projPoint.x+0.5f), (int)(projPoint.y+0.5f));
    }

    public int GetPawnsLeft(Tile.State player)
    {
        if (player == Tile.State.PLAYER_0) return pawnsLeft[0];
        else if (player == Tile.State.PLAYER_1) return pawnsLeft[1];
        return -1;
    }

    private void DecrementPawnsLeft(Tile.State player)
    {
        if (player == Tile.State.PLAYER_0) pawnsLeft[0]--;
        else if (player == Tile.State.PLAYER_1) pawnsLeft[1]--;
    }

    private IEnumerator Ticky()
    {
        foreach(Tile t in tiles)
            t.SetState(Tile.State.PLAYER_0);
        while (true)
        {
            foreach (Tile t in tiles)
            {
                yield return new WaitForSeconds(0.1f);
                t.Flip();
            }
                
            Debug.Log("FLIPPY");
            
        }
    }
}
