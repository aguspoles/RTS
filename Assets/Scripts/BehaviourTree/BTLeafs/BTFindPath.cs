using UnityEngine;

public class BTFindPath : BTNode
{
    public PathType pathType;

    public float minWanderDistance = 10;
    public float maxWanderDistance = 15;

    public override BTResult Execute()
    {
        switch (pathType)
        {
            case PathType.TARGET:
                break;
            case PathType.RANDOM:
                Vector3 randomPosition = Random.insideUnitSphere * Random.Range(minWanderDistance, maxWanderDistance);
                if (Physics.Raycast(randomPosition, -Vector3.up, 100, context.playerController.GetWalakableMask()))
                {
                    context.pathUnit.MoveToPosition(randomPosition, Vector3.zero);
                }
                break;
            default:
                break;
        }

        return context.pathUnit.followingPath ? BTResult.SUCCESS : BTResult.FAILURE;
       
    }

}
