using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
[MoonSharpUserData]
public class LuaBoard : BasicBoard {

    protected LuaBoard(int size)
        : base(size)
    {
        

    }
    [MoonSharpVisible(false)]
    public static LuaBoard Create(BasicBoard board)
    {
        int size = board.size;
        LuaBoard b = new LuaBoard(size);
        b.ResetWithSize(size);
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                b.tiles[x + y * size].SetState(board.GetTileState(x, y));
            }
        }
        //MoonSharp.Interpreter.
        return b;
    }

    [MoonSharpVisible(true)]
    public LuaBoard Copy()
    {
        return Create(this);
    }
    
}
