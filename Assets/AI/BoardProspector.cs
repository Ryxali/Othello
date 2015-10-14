using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;
public class BoardProspector
{
    Script script;
    private BasicBoard currentState;
    Tile.State owner;
    public BoardProspector(Tile.State owner)
    {
        this.owner = owner;
        script = LuaMachine.Create();
        RegisterVars();
        RegisterFunctions();
        
        //script.LoadFile(Application.dataPath + "/");
        
        
        script.DoString(@"
            function foo()
                unity.print('goodie')
            end
        
        ");

        //RegisterEvents();
    }

    private void SetBoardState(BasicBoard board)
    {
        currentState = board.Copy();
        //script.Globals["board", "foo"] = null;
    }

    

    private void RegisterVars()
    {
        
        
    }
    private void RegisterFunctions()
    {
        Table boardTable = new Table(script);
        boardTable["canIPlaceAnyMore"] = (System.Func<bool>)CanIPlaceAnyMore;
        boardTable["getTileState"] = (System.Func<int, int, int>)GetTileState;
        boardTable["canPlaceAt"] = (System.Func<int, int, bool>)IsValidPlacementLocation;
        boardTable["getOwnPawnsLeft"] = (System.Func<int>)GetOwnPawnsLeft;
        boardTable["getOpponentPawnsLeft"] = (System.Func<int>)GetOpponentPawnsLeft;
        script.Globals["board"] = boardTable;
    }

    private void RegisterEvents()
    {
        if (script.Globals["onSift"] == null)
        {
            //script.Globals["onSift"] = (System.Func<int>)soo;
            script.LoadFunction("");
        }
        
    }

    private DynValue CallLuaFunction(string functionName)
    {
        return script.Call(script.Globals["foo"]);
    }

    public void StartProspectingTimer(float time)
    {
        
    }
    /// <summary>
    /// Starts an asynchronous prospecting session. It will continue using
    /// the information stored within itself until aborted.
    /// </summary>
    /// <returns></returns>
    public IEnumerator BeginProspectingSession()
    {
        Debug.Log("1");
        yield return null;
        Debug.Log("2");
    }
    // Callback functions to access the board.
    private bool CanIPlaceAnyMore()
    {
        if (currentState == null)
        {
            Debug.LogWarning("You are attempting to access the board from script before initializing!");
            return false;
        }
        else
        {
            return currentState.CanPlaceAnyMore(owner);
        }
        
    }
    private int GetOwnPawnsLeft()
    {
        if (currentState == null)
        {
            Debug.LogWarning("You are attempting to access the board from script before initializing!");
            return 0;
        }
        return currentState.GetPawnsLeft(owner);
    }
    private int GetOpponentPawnsLeft()
    {
        if (currentState == null)
        {
            Debug.LogWarning("You are attempting to access the board from script before initializing!");
            return 0;
        }
        return currentState.GetPawnsLeft(Tile.Other(owner));
    }

    private bool IsValidPlacementLocation(int x, int y)
    {
        if (currentState == null)
        {
            Debug.LogWarning("You are attempting to access the board from script before initializing!");
            return false;
        }
        return currentState.IsValidPlacementLocation(owner, x, y);
    }

    /// <summary>
    /// Tile states are represented as ints in lua for simplicity's sake.
    /// -1 = opponent
    /// 1 = owner
    /// 0 = none or undefined.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private int GetTileState(int x, int y)
    {
        if (currentState == null)
        {
            Debug.LogWarning("You are attempting to access the board from script before initializing!");
            return 0;
        }
        Tile.State state = currentState.GetTileState(x, y);
        if(state == owner)
            return 1;
        else if (state == Tile.Other(owner))
            return -1;
        else return 0;
    }
}
