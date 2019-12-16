using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior : MonoBehaviour {

    public float goldPocketSize = 50;
    public float goldInPocket = 0;
    public float miningStep = 1;

    private GameObject m_home;
    private GameObject[] m_mines;
    private CNPCStateMachine m_NPCStateMachine;
    private Unit unit;

    // Use this for initialization
    void Start () {
        m_home = GameObject.FindGameObjectWithTag("Base");
        m_mines = GameObject.FindGameObjectsWithTag("Mine");
        m_NPCStateMachine = new CNPCStateMachine();
        unit = GetComponent<Unit>();
    }

    // Update is called once per frame
    void Update () {
       
        switch (m_NPCStateMachine.GetState())
        {
            case (int)CNPCStateMachine.EState.IDLE:
                Idle();
                break;
            case (int)CNPCStateMachine.EState.GO_TO_MINE:
                GoToMine();
                break;
            case (int)CNPCStateMachine.EState.MINE:
                Mine();
                break;
            case (int)CNPCStateMachine.EState.RETURN_TO_BASE:
                ReturnToBase();
                break;
            case (int)CNPCStateMachine.EState.LEAVE_GOLD:
                LeaveGold();
                break;
            default:
                break;
        }
    }

    private GameObject MineWithGold()
    {
        foreach (GameObject mine in m_mines)
        {
            GoldMine goldMine = mine.GetComponent<GoldMine>();
            if (goldMine.goldLeft > 0)
            {
                return mine;
            }
        }
        return null;
    }

    private void Idle()
    {
        transform.RotateAround(transform.position, Vector3.up, 90 * Time.deltaTime);
        if (MineWithGold())
        {
            m_NPCStateMachine.SetEvent((int)CNPCStateMachine.EEvent.ON_MINE_EXIST);
        }
        else
        {
            m_NPCStateMachine.SetEvent((int)CNPCStateMachine.EEvent.ON_MINE_NOT_EXIST);
        }
    }

    private void GoToMine()
    {
        GameObject mine = MineWithGold();
        if (mine)
        {
            unit.MoveToPosition(mine.transform.position, Vector3.zero);
            if(unit.targetReached)
                m_NPCStateMachine.SetEvent((int)CNPCStateMachine.EEvent.ON_ARRIVE_TO_MINE);
        }
        //Debug.Log("Going to mine...");
    }

    private void Mine()
    {
        transform.RotateAround(transform.position, Vector3.up, 90 * Time.deltaTime);
        GameObject mine = MineWithGold();
        if (mine)
        {
            float amountMined = miningStep * Time.deltaTime;
            goldInPocket += amountMined;
            if (goldInPocket >= goldPocketSize)
            {
                goldInPocket = goldPocketSize;
                mine.GetComponent<GoldMine>().goldLeft -= goldPocketSize;
                m_NPCStateMachine.SetEvent((int)CNPCStateMachine.EEvent.ON_POCKETS_FULL);
            }
        }
        //Debug.Log("Mining...");
    }

    private void ReturnToBase()
    {
        unit.MoveToPosition(m_home.transform.position, Vector3.zero);
        if (unit.targetReached)
        {
            m_NPCStateMachine.SetEvent((int)CNPCStateMachine.EEvent.ON_ARRIVE_TO_HOME);
        }
        //Debug.Log("Returning to base...");
    }

    private void LeaveGold()
    {
        goldInPocket = 0;
        m_NPCStateMachine.SetEvent((int)CNPCStateMachine.EEvent.ON_LEAVE_GOLD);
        //Debug.Log("Leaving gold...");
    }

}