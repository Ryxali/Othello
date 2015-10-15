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
    public int boardSize { get { return size; } }
    private GameBoard gameBoard;
    
    public Vector3 normal { get { return transform.forward; } }
    
    

    public MeshRenderer pawn;
    public Transform plate;


    public BasicBoard CreateBoardCopy()
    {
        return gameBoard.Copy();
    }
	void Start () {
        
        plate.GetComponent<MeshRenderer>().material.mainTextureScale = Vector2.one * size / 2;
        plate.transform.localScale = new Vector3(1, 1, 0) * size;
        plate.transform.localPosition += new Vector3(1, 1) * (size / 2 - 0.5f);
        gameBoard = GameBoard.Create(size, transform, pawn.gameObject);
	}

    void Update()
    {
        
    }

    public void Reset()
    {
        gameBoard.Reset();
    }

    public void ResetWithSize(int newSize)
    {
        gameBoard.ResetWithSize(newSize);
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
        return gameBoard.PlacePawnOnTile(owner, x, y);
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
        return gameBoard.IsValidPlacementLocation(owner, x, y);
    }

    public bool CanPlaceAnyMore(Tile.State owner) {
        return gameBoard.CanPlaceAnyMore(owner);
    }

    public Point2D WorldToTilePosition(Vector3 point)
    {
        Vector3 localPoint = point - transform.position;
        Vector3 projPoint = Vector3.ProjectOnPlane(localPoint, normal);
        return new Point2D((int)(projPoint.x+0.5f), (int)(projPoint.y+0.5f));
    }

    public int GetPawnsLeft(Tile.State player)
    {
        return gameBoard.GetPawnsLeft(player);
    }

    
}
