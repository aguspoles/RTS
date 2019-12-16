using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockBehavior behavior;
    List<FlockAgent> agents = new List<FlockAgent>();

    [Range(10, 500)]
    public int startingCount = 250;
    const float AgentDensity = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 10f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
    }
    
    void Update()
    {
        UpdateAgentsVelocity();
    }

    public void MoveFlock(Vector3 target)
    {
        foreach (FlockAgent agent in agents)
        {
            agent.Move(target);
        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);
        foreach (Collider c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }

    public void SetAgents(List<FlockAgent> agents)
    {
        this.agents = agents;
        foreach(FlockAgent a in this.agents)
        {
            a.Initialize(this);
        }
    }

    public List<FlockAgent> GetAgents()
    {
        return this.agents;
    }

    public void UpdateAgentsVelocity()
    {
        //Update velocity of each agent
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);
            //FOR DEMO ONLY
            agent.GetComponentInChildren<MeshRenderer>().material.color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

            Vector3 velocity = behavior.CalculateMove(agent, context, this);
            velocity *= driveFactor;
            if (velocity.sqrMagnitude > squareMaxSpeed)
            {
                velocity = velocity.normalized * maxSpeed;
            }
            agent.velocity = velocity;
        }
    }

}
