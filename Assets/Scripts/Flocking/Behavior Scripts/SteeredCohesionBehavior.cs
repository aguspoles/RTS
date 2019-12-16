using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Steered Cohesion")]
public class SteeredCohesionBehavior : FlockBehavior
{

    Vector3 currentVelocity;
    public float agentSmoothTime = 0.5f;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //if no neighbors, return no adjustment
        if (context.Count == 0)
            return Vector3.zero;

        //add all points together and average
        Vector2 cohesionMove = Vector2.zero;
        foreach (Transform item in context)
        {
            Vector2 item2D = new Vector2(item.position.x, item.position.z);
            cohesionMove += item2D;
        }
        cohesionMove /= context.Count;

        //create offset from agent position
        Vector2 this2D = new Vector2(agent.transform.position.x, agent.transform.position.z);
        cohesionMove -= this2D;
        Vector3 target = new Vector3(cohesionMove.x, agent.transform.position.y, cohesionMove.y);
        Vector3 ret = Vector3.SmoothDamp(agent.transform.forward, target, ref currentVelocity, agentSmoothTime);
        return ret;
    }
}
