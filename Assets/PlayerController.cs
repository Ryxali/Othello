using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public Board board;
    private Tile.State curOwner = Tile.State.PLAYER_0;
	// Update is called once per frame
	void Update () {
        if (board == null) return;
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 intersect;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Math3d.LinePlaneIntersection(out intersect, ray.origin, ray.direction, board.normal, board.transform.position))
            {
                
                Point2D tilePos = board.WorldToTilePosition(intersect);
                bool success = board.PlacePawnOnTile(curOwner, tilePos.x, tilePos.y);
                if (success)
                {
                    curOwner = Tile.Other(curOwner);
                }
            }
        }
	}
}
