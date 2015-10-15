using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;

public class AIController : Controller {
    private BoardProspector prospector;
    private MonoBehaviour coroutiner;
    private string scriptName;
    public AIController(Tile.State owner, Board board, MonoBehaviour coroutiner, string scriptName)
        : base(owner, board)
    {
     
        prospector = new BoardProspector(owner);
        this.scriptName = scriptName;
        this.coroutiner = coroutiner;
    }


    public IEnumerator LoadAIScript(string scriptName) {
        this.scriptName = scriptName;
        yield return coroutiner.StartCoroutine(prospector.LoadScript(scriptName));
    }

    public override IEnumerator OnGameStart()
    {
        
        yield return coroutiner.StartCoroutine(LoadAIScript(scriptName));
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
