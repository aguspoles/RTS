using UnityEngine;
using System.Collections;

public class PlayerUnit : PlayerUnitBase
{
    float fitness = 0;

    protected override void OnReset()
    {
        fitness = 1;
    }

    protected override void OnThink(float dt) 
	{
        Vector3 dirToCollectable = GetDirToCollectable(collectable);

        inputs[0] = dirToCollectable.x;
        inputs[1] = dirToCollectable.z;
        inputs[2] = transform.forward.x;
        inputs[3] = transform.forward.z;

        // Think!!! 
        float[] output = brain.Synapsis(inputs);

        //Vector3 nextpos = new Vector3(output[0], output[1], output[2]);
        SetTarget(output[0], output[1], dt);
	}
    
    protected override void OnTakeCollectable(GameObject mine)
    {
        if (grid.NodeFromWorldPoint(mine.transform.position).movementPenalty < 50)
        {
            fitness *= 2;
            genome.fitness = fitness;
        }
    }
}
