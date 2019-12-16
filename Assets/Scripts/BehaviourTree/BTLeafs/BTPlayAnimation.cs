using UnityEngine;

public class BTPlayAnimation : BTNode
{
    public int AnimationNumber = 0;
    public override BTResult Execute()
    {
        context.animatorController.SetInteger("State", AnimationNumber);

        return BTResult.SUCCESS;
    }
}
