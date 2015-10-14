using UnityEngine;
using System.Collections;

public class PlayerController : Controller {


    public PlayerController(Tile.State owner, Board board) : base(owner, board)
    {

    }
    public override IEnumerator OnGameStart()
    {
        yield break;
    }

    public override IEnumerator TakeTurn()
    {
        if (board == null)
        {
            Debug.LogError("Board not set!");
            yield break;
        }
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 intersect;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Math3d.LinePlaneIntersection(out intersect, ray.origin, ray.direction, board.normal, board.transform.position))
                {
                    
                    Point2D tilePos = board.WorldToTilePosition(intersect);
                    Debug.Log(tilePos.ToString());
                    bool success = board.PlacePawnOnTile(owner, tilePos.x, tilePos.y);
                    if (success)
                    {
                        yield break;
                    }
                }
            }
            yield return null;
        }
        
    }
}
