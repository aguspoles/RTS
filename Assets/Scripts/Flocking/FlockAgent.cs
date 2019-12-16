using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    public Vector3 velocity;
    public Unit pathAgent;
    public float gravity = 10;

    Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; } }

    Collider agentCollider;
    public Collider AgentCollider { get { return agentCollider; } }

    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider>();
        pathAgent = GetComponent<Unit>();
    }

    void Update()
    {
        if(pathAgent.followingPath)
        {
            transform.position += velocity * Time.deltaTime;
        }
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
    }

    public void Initialize(Flock flock)
    {
        agentFlock = flock;
    }

    public void Move(Vector3 target)
    {
        //Find the path and then move
        pathAgent.MoveToPosition(target);
    }
}
