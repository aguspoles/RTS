using UnityEngine;

public class AIGeneralDebugView : AIDebugView
{
    protected override string GetDebugText()
    {
        string viewText = "";

        AIComponent owningAgent = activeViewContext.owningContext.contextOwner;
        viewText += "Agent Name: " + owningAgent.name +"\n";
        viewText += "Tree Type: " + owningAgent.behaviourTreeType.ToString() + "\n";
        viewText += "AI State: " + owningAgent.currentState.ToString() + "\n";
        viewText += "Target: " + owningAgent.pathUnit.target + "\n";

        switch (owningAgent.currentState)
        {
            case AIState.IDLE:
                debugStyle.normal.textColor = Color.green;
                break;
            case AIState.MOVING:
                debugStyle.normal.textColor = Color.yellow;
                break;
            case AIState.HOSTILE:
                debugStyle.normal.textColor = Color.red;
                break;
            default:
                break;
        }

        return viewText;
    }
}
