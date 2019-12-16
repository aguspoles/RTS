using UnityEngine;

public class BTHas : BTNode
{
    public HasOp operation;
    public float destinationTolerance = 1;

    public override BTResult Execute()
    {
        BTResult result = BTResult.FAILURE;
        switch (operation)
        {
            case HasOp.PATH:
                result = context.pathUnit.followingPath ? BTResult.SUCCESS : BTResult.FAILURE;
                break;
            case HasOp.PATH_TO_TARGET:
                if (!context.pathUnit.followingPath)
                {
                    result = BTResult.FAILURE;
                }
                else if (context.pathUnit.followingPath && PathIsWithinToleranceToTarget())
                {
                    result = BTResult.SUCCESS;
                }
                else
                {
                    context.pathUnit.ResetPath();
                    result = BTResult.FAILURE;
                }
                break;
            case HasOp.TARGET:
                result = context.contextOwner.pathUnit.target != Unit.InvalidVector3 ? BTResult.SUCCESS : BTResult.FAILURE;
                break;
        }
        return result;
    }

    private bool PathIsWithinToleranceToTarget()
    {
        return (context.contextOwner.pathUnit.target - context.contextOwner.pathUnit.transform.position).sqrMagnitude < destinationTolerance * destinationTolerance;
    }
}
