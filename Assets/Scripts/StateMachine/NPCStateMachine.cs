using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CNPCStateMachine : CStateMachine
{
    public enum EEvent : uint
    {
        ON_MINE_EXIST = 0,
        ON_MINE_NOT_EXIST = 1,
        ON_ARRIVE_TO_MINE = 2,
        ON_POCKETS_FULL = 3,
        ON_ARRIVE_TO_HOME = 4,
        ON_LEAVE_GOLD = 5,
        COUNT
    }

    public enum EState : uint
    {
        IDLE = 0,
        GO_TO_MINE = 1,
        MINE = 2,
        RETURN_TO_BASE = 3,
        LEAVE_GOLD = 4,
        COUNT
    }

    public CNPCStateMachine() : 
        base((uint)EState.COUNT, (uint)EEvent.COUNT) {

        SetRelation((int)EState.IDLE, (int)EState.GO_TO_MINE, (int)EEvent.ON_MINE_EXIST);
        SetRelation((int)EState.IDLE, (int)EState.IDLE, (int)EEvent.ON_MINE_NOT_EXIST);
        SetRelation((int)EState.GO_TO_MINE, (int)EState.MINE, (int)EEvent.ON_ARRIVE_TO_MINE);
        SetRelation((int)EState.MINE, (int)EState.RETURN_TO_BASE, (int)EEvent.ON_POCKETS_FULL);
        SetRelation((int)EState.RETURN_TO_BASE, (int)EState.LEAVE_GOLD, (int)EEvent.ON_ARRIVE_TO_HOME);
        SetRelation((int)EState.LEAVE_GOLD, (int)EState.IDLE, (int)EEvent.ON_LEAVE_GOLD);

        currentState = (int)EState.IDLE;
    }
}

