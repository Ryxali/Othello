using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;

class Test
{
    private int Dohickey()
    {
        return 3;
    }
}
public class BoardProspector
{
    Script script;
    private LuaBoard currentState;
    Tile.State owner;
    public Point2D chosenLocation { get; private set; }
    public BoardProspector(Tile.State owner)
    {
        this.owner = owner;
        
        
        //script.LoadFile(Application.dataPath + "/");
        
        
        /*script.DoString(@"
            function foo()
                unity.print('goodie')
            end
        
        ");*/
        
        //script.DoFile(Application.dataPath + "/Resources/MoonSharp/Scripts/BasicAI");

        //RegisterEvents();
    }

    public IEnumerator LoadScript(string aiScriptName)
    {
        script = LuaMachine.Create();
        RegisterVars();
        RegisterFunctions();
        Debug.Log(Application.streamingAssetsPath + "/AiScripts/" + aiScriptName + ".lua");
        WWW loader = new WWW("file://" + Application.streamingAssetsPath + "/AiScripts/" + aiScriptName + ".lua");
        yield return loader;

        if (loader.error != null)
        {
            Debug.LogError(loader.error);
            script.DoFile(Application.dataPath + "/Resources/MoonSharp/Scripts/BasicAI");
        }
        else
        {
            script.DoString(loader.text);
        }
    }
    

    private void SetBoardState(BasicBoard board)
    {
        currentState = LuaBoard.Create(board);
        UserData.RegisterType<LuaBoard>();
        UserData.RegisterType<BasicBoard.Score>();
        script.Globals["ai", "currentBoard"] = UserData.Create(currentState);
    }

    

    private void RegisterVars()
    {
        Table boardTable = new Table(script);
        boardTable["owner"] = owner;
        script.Globals["ai"] = boardTable;
        
    }
    private void RegisterFunctions()
    {
        /*Table boardTable = new Table(script);
        boardTable["canIPlaceAnyMore"] = (System.Func<bool>)CanIPlaceAnyMore;
        boardTable["getTileState"] = (System.Func<int, int, int>)GetTileState;
        boardTable["canPlaceAt"] = (System.Func<int, int, bool>)IsValidPlacementLocation;
        boardTable["getOwnPawnsLeft"] = (System.Func<int>)GetOwnPawnsLeft;
        boardTable["getOpponentPawnsLeft"] = (System.Func<int>)GetOpponentPawnsLeft;
        boardTable["getBoardSize"] = (System.Func<int>)GetBoardSize;
        script.Globals["board"] = boardTable;*/
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
        if (script.Globals[functionName] == null) return null;
        return script.Call(script.Globals[functionName]);
    }

    public void StartProspectingTimer(float time)
    {
        
    }
    /// <summary>
    /// Starts an asynchronous prospecting session. It will continue using
    /// the information stored within itself until aborted.
    /// </summary>
    /// <returns></returns>
    public IEnumerator BeginProspectingSession(BasicBoard board)
    {
        SetBoardState(board);
        yield return new WaitForSeconds(1.0f);
        DynValue val = CallLuaFunction("onCalculateBestMove");
        if(val == null) 
        {
            Debug.LogWarning("onCalculateBestMove not implemented in script!");
        }
        int x = 0;
        int y = 0;
        
        if (val.Type == DataType.Table)
        {
            
            //
            if (val.Table.Get("x").Type == DataType.Number)
            {
                x = (int)val.Table.Get("x").Number;
            }
            else if (val.Table.Get(1).Type == DataType.Number)
            {
                x = (int)val.Table.Get(1).Number;
            }
             
            
            //
            if (val.Table.Get("y").Type == DataType.Number)
            {
                y = (int)val.Table.Get("y").Number;
            }
            else if (val.Table.Get(2).Type == DataType.Number)
            {
                y = (int)val.Table.Get(2).Number;
            }
             
            
        }
        chosenLocation = new Point2D(x, y);
        
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
        Tile.State state = currentState.GetTileState(x-1, y-1);
        if(state == owner)
            return 1;
        else if (state == Tile.Other(owner))
            return -1;
        else return 0;
    }

    private int GetBoardSize()
    {
        if (currentState == null)
        {
            Debug.LogWarning("You are attempting to access the board from script before initializing!");
            return 0;
        }
        return currentState.size;
    }

}
