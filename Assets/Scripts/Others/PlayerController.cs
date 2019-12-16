using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private List<Unit> myUnits;
    private Flock myFlock;
    [SerializeField]
    private LayerMask walkableMask;
    [SerializeField]
    private LayerMask unitsMask;

    void Start()
    {
        myUnits = new List<Unit>();
        myFlock = GetComponent<Flock>();
    }

    void Update()
    {
        //Select units and add them to flock
        if(Input.GetMouseButtonDown(0))
        {
            SelectUnits();
            List<FlockAgent> agents = new List<FlockAgent>();
            foreach (Unit u in myUnits)
            {
                agents.Add(u.GetComponent<FlockAgent>());
            }
            myFlock.SetAgents(agents);
        }

        //Move flock
        if(Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, walkableMask))
            {
                myFlock.MoveFlock(hit.point);
            }
        }
    }

    public LayerMask GetWalakableMask()
    {
        return walkableMask;
    }

    public LayerMask GetUnitsMask()
    {
        return unitsMask;
    }

    public void SelectUnits()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, unitsMask))
        {
            Unit unit = hit.collider.GetComponent<Unit>();
            if (unit)
            {
                if (!unit.selected)
                {
                    myUnits.Add(unit);
                    unit.selected = true;
                }
                else
                {
                    myUnits.Remove(unit);
                    unit.selected = false;
                }
            }
        }
    }
}
