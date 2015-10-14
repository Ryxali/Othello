using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;

public class AIController : Controller {
    private BoardProspector prospector;
    private MonoBehaviour coroutiner;
    public AIController(Tile.State owner, Board board, MonoBehaviour coroutiner)
        : base(owner, board)
    {
     
        prospector = new BoardProspector(owner);
        this.coroutiner = coroutiner;
    }

    public override IEnumerator OnGameStart()
    {
        
        yield return null;
    }

    public override IEnumerator TakeTurn()
    {

        yield return coroutiner.StartCoroutine(prospector.BeginProspectingSession(board.CreateBoardCopy()));
        if (!board.PlacePawnOnTile(owner, prospector.chosenLocation.x, prospector.chosenLocation.y))
        {
            bool searching = true;
            for (int x = 0; x < board.boardSize && searching; x++)
            {
                for (int y = 0; y < board.boardSize && searching; y++)
                {
                    searching = !board.PlacePawnOnTile(owner, x, y);
                }
            }
        }
    }
    

    
}
