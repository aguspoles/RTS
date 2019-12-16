using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Avoidance")]
public class AvoidanceBehavior : FlockBehavior
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //if no neighbors, return no adjustment
        if (context.Count == 0)
            return Vector3.zero;

        //add all points together and average
        Vector2 avoidanceMove = Vector2.zero;
        int nAvoid = 0;
        foreach (Transform item in context)
        {
            Vector2 item2D = new Vector2(item.position.x, item.position.z);
            Vector2 this2D = new Vector2(agent.transform.position.x, agent.transform.position.z);
            if (Vector2.SqrMagnitude(item2D - this2D) < flock.SquareAvoidanceRadius)
            {
                nAvoid++;
                avoidanceMove += (this2D - item2D);
            }
        }
        if (nAvoid > 0)
            avoidanceMove /= nAvoid;

        return new Vector3(avoidanceMove.x, agent.transform.position.y, avoidanceMove.y);
    }
}
