using UnityEngine;

public class BTFindPath : BTNode
{
    public PathType pathType;

    public float minWanderDistance = 10;
    public float maxWanderDistance = 15;

    public override BTResult Execute()
    {
        Unit unit = context.contextOwner.pathUnit;
        switch (pathType)
        {
            case PathType.TARGET:
                break;
            case PathType.RANDOM:
                /*Vector3 randomPosition = Random.insideUnitSphere * 10;
                if (Physics.Raycast(randomPosition, Vector3.up, 100, PlayerController.GetInstance().GetWalakableMask()))
                {
                    unit.MoveToPosition(randomPosition);
                }*/
                break;
            default:
                break;
        }

        return context.pathUnit.followingPath ? BTResult.SUCCESS : BTResult.FAILURE;
       
    }

}
