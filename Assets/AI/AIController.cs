using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;

public class AIController : Controller {
    private BoardProspector prospector;
    private MonoBehaviour coroutiner;
    public AIController(Tile.State owner, Board board, MonoBehaviour coroutiner)
        : base(owner, board)
    {
        prospector = new BoardProspector(board);
        this.coroutiner = coroutiner;
    }

    public override IEnumerator OnGameStart()
    {
        coroutiner.StartCoroutine(prospector.BeginProspectingSession());
        yield return null;
    }

    public override IEnumerator TakeTurn()
    {
        
        throw new System.NotImplementedException();
    }
    

    
}
