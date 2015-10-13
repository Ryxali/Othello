using UnityEngine;
using System.Collections;

public class BasicTile {
    public enum State
    {
        NONE, PLAYER_0, PLAYER_1
    }
    public static State Other(State state)
    {
        if (state == State.PLAYER_0) return State.PLAYER_1;
        else if (state == State.PLAYER_1) return State.PLAYER_0;
        else return State.NONE;
    }
    public State state { get; protected set; }

    public BasicTile()
    {
        state = State.NONE;
    }

    public void Flip()
    {
        if (state == State.NONE) Debug.LogWarning("Cannot flip a NONE tile");
        else if (state == State.PLAYER_0) SetState(State.PLAYER_1);
        else if (state == State.PLAYER_1) SetState(State.PLAYER_0);
    }

    public virtual void SetState(State newState)
    {
        state = newState;
    }
}
