using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    public Board board;
    public Controller player0;
    public Controller player1;

    void Start()
    {
        MoonSharp.Interpreter.UserData.RegisterAssembly();
        player0 = new PlayerController(Tile.State.PLAYER_0, board);
        player1 = new AIController(Tile.State.PLAYER_1, board, this);//new PlayerController(Tile.State.PLAYER_1, board);
        
        StartCoroutine(StartSession());
    }

    IEnumerator StartSession()
    {
        yield return null;
        yield return StartCoroutine(player0.OnGameStart());
        yield return StartCoroutine(player1.OnGameStart());
        bool running = true;
        while (running)
        {
            if (board.CanPlaceAnyMore(player0.owner))
            {
                yield return StartCoroutine(player0.TakeTurn());
                if (board.CanPlaceAnyMore(player1.owner))
                {
                    yield return StartCoroutine(player1.TakeTurn());
                }
                else
                {
                    running = false;
                }
            }
            else
            {
                running = false;
            }
        }
        Debug.Log("End of match");
        Debug.Log("Score:");
        int p0 = 0;
        int p1 = 0;

        //Debug.Log("Player 1: " + player0.board.)
    }
}
