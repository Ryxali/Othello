using MoonSharp.Interpreter;
using UnityEngine;
public class LuaMachine {
    /// <summary>
    /// Creates a Script object and adds a predefined
    /// set of functions and values related to unity
    /// to it.
    /// </summary>
    /// <returns></returns>
    public static Script Create()
    {
        Script s = new Script();
        RegisterFunctions(s);
        return s;
    }

    /// <summary>
    /// Defines a set of functions that lua can call
    /// </summary>
    /// <param name="s"></param>
    private static void RegisterFunctions(Script s)
    {
        //script.Globals["printy"] = (System.Func<int>)soo;
        Table uni = new Table(s);
        uni["print"] = (System.Action<string>)Print;
        uni["printErr"] = (System.Action<string>)PrintErr;
        uni["printWarn"] = (System.Action<string>)PrintWarn;
        uni["time"] = (System.Func<float>)GetTime;
        s.Globals["unity"] = uni;
    }

    private static float GetTime() {
        return Time.time;
    }

    private static void Print(string msg)
    {
        Debug.Log(msg);
    }

    private static void PrintErr(string msg)
    {
        Debug.LogError(msg);
    }
    private static void PrintWarn(string msg)
    {
        Debug.LogError(msg);
    }
}
