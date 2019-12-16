using UnityEngine;

public class BTPlayAnimation : BTNode
{
    public override BTResult Execute()
    {
        if (context.pathUnit.followingPath)
        {
            context.animatorController.SetInteger("State", 2);
        }
        else
        {
            context.animatorController.SetInteger("State", 0);
        }

        return BTResult.SUCCESS;
    }
}
