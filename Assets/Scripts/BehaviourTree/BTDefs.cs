using System.Reflection;

public static class BTDefs
{
    public const string MOVEMENT_STATE = "MoveState";

    public static void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}


public enum MovementState
{
    IDLE,
    WALK,
    RUN,
}

public enum BTResult
{
    SUCCESS,
    FAILURE,
    XRUNNING_DO_NOT_USE // LEAF NODES SHOULD NEVER RETURN RUNNING! BEHAVIOUR TREE IS NOT MULTITHREADED
}

//Add new types to the bottom of this enum, before COUNT
//Failing to do so will reorder the enum and mess up the values you set in the scene!!
public enum BehaviourTreeType
{
    BASIC,
    //Add stuff here
    COUNT,
}

public enum HasOp
{
    PATH,
    PATH_TO_TARGET,
    TARGET
}

public enum IsOp
{
    IDLE,
    HOSTILE,
    WANDERING,
}

public enum PathType
{
    TARGET,
    RANDOM
}
