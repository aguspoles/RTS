﻿using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BTContext
{
    public AIComponent contextOwner;
    public Animator animatorController;
    public Unit pathUnit;
    public PlayerController playerController;
#if UNITY_EDITOR
    public List<string> behaviourHistory = new List<string>();
#endif //UNITY EDITOR

    public BTContext(AIComponent owner, Animator animatorController, Unit pathUnit, PlayerController controller)
    {
        contextOwner = owner;
        this.animatorController = animatorController;
        this.pathUnit = pathUnit;
        this.playerController = controller;
    }
}
