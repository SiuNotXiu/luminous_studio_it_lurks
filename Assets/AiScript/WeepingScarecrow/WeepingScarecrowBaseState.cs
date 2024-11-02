using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeepingScarecrowBaseState
{
    public abstract void EnterState(WeepingScarecrowManager weepingScarecrow);

    public abstract void UpdateState(WeepingScarecrowManager weepingScarecrow);

    public abstract void ExitState(WeepingScarecrowManager weepingScarecrow);
}
