using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Unit))]
public class AIComponent : MonoBehaviour
{
    public BehaviourTreeType behaviourTreeType;
    public AIState currentState;

    public Unit pathUnit;
    public AIComponentType type;
    Animator animatorController;
    PlayerController playerController;
    BTContext aiContext;

    private void Awake()
    {
        pathUnit = GetComponent<Unit>();
        animatorController = GetComponent<Animator>();
        playerController = FindObjectOfType<PlayerController>();

        aiContext = new BTContext(this, animatorController, pathUnit, playerController);
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

public enum AIComponentType
{
    PLAYERUNIT,
    WANDERER,
}
