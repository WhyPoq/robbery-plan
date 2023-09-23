using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;

public class AIStopLerp : AILerp
{
    public UnityEvent reached = new();

    public override void OnTargetReached()
    {
        base.OnTargetReached();
        reached.Invoke();
    }
}
