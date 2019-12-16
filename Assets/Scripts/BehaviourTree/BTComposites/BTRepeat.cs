using UnityEngine;
using XNode;

public class BTRepeat : BTNode
{
    [Input] public BTResult inResult;
    public int repeatCount = 2;
    int currentRepeatCount = 0;

    BehaviourTreeManager behaviourTreeManager;

    public override void OnStart()
    {
        behaviourTreeManager = BehaviourTreeManager.GetInstance();
        currentRepeatCount = repeatCount;
    }

    public override BTResult Execute()
    {
        //First Entry
        if (currentRepeatCount == 0)
        {
            currentRepeatCount = repeatCount;
        }

        NodePort inPort = GetPort("inResult");
        if (inPort != null && inPort.GetConnections().Count != 0)
        {
            NodePort connection = inPort.GetConnection(0);

            if (connection != null)
            {
                BTResult result = (BTResult)connection.GetOutputValue();
                --currentRepeatCount;
                
                if (currentRepeatCount == 0)
                {
                    BehaviourTreeRuntimeData.RemoveRunningNode(context, this);
                }
                else if (result == BTResult.XRUNNING_DO_NOT_USE)
                {
                    return BTResult.XRUNNING_DO_NOT_USE;
                }
                else
                {
                    BehaviourTreeRuntimeData.AddRunningNode(context, this);
                    return BTResult.XRUNNING_DO_NOT_USE;
                }
            }
        }
        return BTResult.SUCCESS;
    }
}
