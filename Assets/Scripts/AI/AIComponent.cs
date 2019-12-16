using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Unit))]
public class AIComponent : MonoBehaviour
{
    public BehaviourTreeType behaviourTreeType;
    public AIState currentState;

    public Unit pathUnit;
    Animator animatorController;
    BTContext aiContext;

    private void Awake()
    {
        pathUnit = GetComponent<Unit>();
        animatorController = GetComponent<Animator>();

        aiContext = new BTContext(this, animatorController, pathUnit);
    }

    private void Start()
    {
        BehaviourTreeRuntimeData.RegisterAgentContext(behaviourTreeType, aiContext);
    }

    void Update()
    {
        if (pathUnit.followingPath)
            currentState = AIState.MOVING;
        else
            currentState = AIState.IDLE;
    }
}
