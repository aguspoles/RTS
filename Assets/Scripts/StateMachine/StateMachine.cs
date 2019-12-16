using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CStateMachine
{
    private int[,] m_finiteStateMachine;
    protected int currentState;

    public CStateMachine(uint statesCount, uint eventsCount)
    {
        m_finiteStateMachine = new int[statesCount, eventsCount];

        for(int i = 0; i < statesCount; i++)
        {
            for (int j = 0; j < eventsCount; j++)
            {
                m_finiteStateMachine[i, j] = -1;
            }
        }
    }

    public void SetRelation(int origin, int destiny, uint ev)
    {
        m_finiteStateMachine[origin, ev] = destiny;
    }

    public void SetEvent(uint ev)
    {
        for (int i = 0; i < m_finiteStateMachine.GetLength(0); i++)
        {
            if(m_finiteStateMachine[i, ev] != -1)
            {
                currentState = m_finiteStateMachine[i, ev];
            }
        }
    }

    public int GetState()
    {
        return currentState;
    }
}
