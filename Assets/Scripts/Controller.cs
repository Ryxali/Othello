using UnityEngine;
using System.Collections;

public abstract class Controller {
    public Board board;
    public Tile.State owner = Tile.State.NONE;

    public Controller(Tile.State owner, Board board)
    {
        this.board = board;
        this.owner = owner;
    }


    public abstract IEnumerator OnGameStart();
    public abstract IEnumerator TakeTurn();
}
