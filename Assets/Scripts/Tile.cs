using UnityEngine;
using System.Collections;



public class Tile : BasicTile
{
    
    

    private Transform parent;
    public int x { get; private set; }
    public int y { get; private set; }
    
    public Color color { get {
        switch (state)
        {
            case State.NONE:
                return Color.gray;
            case State.PLAYER_0:
                return Color.red;
            case State.PLAYER_1:
                return Color.black;
            default:
                return Color.green;
        }
    } }
    private GameObject tilePrefab;
    private GameObject tile = null;

    public Tile(Transform parent, int x, int y, GameObject tilePrefab)
    {
        this.parent = parent;
        this.x = x;
        this.y = y;
        this.tilePrefab = tilePrefab;
    }



    public override void SetState(BasicTile.State newState)
    {
        State previous = state;
        base.SetState(newState);
        if (newState == State.NONE)
        {
            if (tile != null) tile.SetActive(false);
        }
        else
        {
            if (previous != state)
            {

                if (tile == null)
                {
                    tile = GameObject.Instantiate<GameObject>(tilePrefab);
                    tile.transform.parent = parent;
                    tile.transform.localPosition = new Vector3(x, y);
                }
                else if (!tile.activeSelf) tile.SetActive(true);
                tile.GetComponent<MeshRenderer>().material.color = color;
            }
        }
    }
}