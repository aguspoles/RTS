using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Composite")]
public class CompositeBehavior : FlockBehavior
{
    public FlockBehavior[] behaviors;
    public float[] weights;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //handle data mismatch
        if (weights.Length != behaviors.Length)
        {
            Debug.LogError("Data mismatch in " + name, this);
            return Vector3.zero;
        }

        //set up move
        Vector2 move = Vector2.zero;

        //iterate through behaviors
        for (int i = 0; i < behaviors.Length; i++)
        {
            Vector3 partialMove = behaviors[i].CalculateMove(agent, context, flock);
            Vector2 partialMove2D = new Vector2(partialMove.x, partialMove.z) * weights[i];

            if (partialMove2D != Vector2.zero)
            {
                if (partialMove2D.sqrMagnitude > weights[i] * weights[i])
                {
                    partialMove2D.Normalize();
                    partialMove2D *= weights[i];
                }

                move += partialMove2D;

            }
        }

        return new Vector3(move.x, agent.transform.position.y, move.y);


    }
}
