using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private UnitsControlSystem unitSystem;
    private List<Unit> myUnits;
    [SerializeField]
    private LayerMask walkableMask;

    void Start()
    {
        unitSystem = FindObjectOfType<UnitsControlSystem>();
        myUnits = new List<Unit>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            myUnits = unitSystem.SelectUnits();
        }
        if(Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, walkableMask))
            {
                foreach(Unit u in myUnits)
                {
                    u.MoveToPosition(hit.point);
                }
            }
        }
    }
}
