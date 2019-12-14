using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private UnitsControlSystem unitSystem;
    private List<Unit> myUnits;
    private Flock myFlock;
    [SerializeField]
    private LayerMask walkableMask;

    void Start()
    {
        unitSystem = FindObjectOfType<UnitsControlSystem>();
        myUnits = new List<Unit>();
        myFlock = FindObjectOfType<Flock>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            myUnits = unitSystem.SelectUnits();
            List<FlockAgent> agents = new List<FlockAgent>();
            foreach (Unit u in myUnits)
            {
                agents.Add(u.GetComponent<FlockAgent>());
            }
            myFlock.SetAgents(agents);
        }


        if(Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, walkableMask))
            {
                myFlock.MoveFlock(hit.point);
                Debug.Log(hit.point);
            }
        }
    }
}
