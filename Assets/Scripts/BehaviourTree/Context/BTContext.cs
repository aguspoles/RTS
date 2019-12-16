using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class BTContext
{
    public AIComponent contextOwner;
    public Animator animatorController;
    public Unit pathUnit;
#if UNITY_EDITOR
    public List<string> behaviourHistory = new List<string>();
#endif //UNITY EDITOR

    public BTContext(AIComponent owner, Animator animatorController, Unit pathUnit)
    {
        contextOwner = owner;
        this.animatorController = animatorController;
        this.pathUnit = pathUnit;
    }
}
