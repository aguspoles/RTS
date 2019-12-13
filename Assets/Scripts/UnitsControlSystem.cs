using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsControlSystem : MonoBehaviour
{
    [SerializeField]
    private LayerMask unitsMask;
    private List<Unit> selectedUnits;

    void Start()
    {
        selectedUnits = new List<Unit>();
    }

    public List<Unit> SelectUnits()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, unitsMask))
        {
            Unit unit = hit.collider.GetComponent<Unit>();
            if(unit)
            {
                if(!unit.selected)
                {
                    selectedUnits.Add(unit);
                    unit.selected = true;
                }
                else
                {
                    selectedUnits.Remove(unit);
                    unit.selected = false;
                }
            }
        }
        return selectedUnits;
    }
}
